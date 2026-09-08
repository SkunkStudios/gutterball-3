using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;
#if !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace Y8API
{
    // ── Inspector-constrained option enums ────────────────────────────────────

    /// <summary>'on' = preload immediately. 'auto' = SDK decides.</summary>
    public enum PreloadAdBreaks
    {
        on,
        auto
    }

    /// <summary>'on' = ads may play audio. 'off' = muted.</summary>
    public enum AdSound
    {
        on,
        off
    }

    // ─────────────────────────────────────────────────────────────────────────

    public class Y8 : MonoBehaviour
    {
        public static Y8 Instance { get; private set; }

        private static bool isReady = false;

        // ── Inspector fields ──────────────────────────────────────────────────

        [Header("ENTER APP ID HERE")]
        public string AppId = "";

        [Header("ENTER GAME ID FOR ADS HERE (optional)")]
        public string GameId = "";

        [Header("SDK Init Options")]
        [Tooltip("true = SDK silently checks for an existing session on startup.")]
        public bool AutoLogin = true;

        [Tooltip("'on' = preload ad creative immediately.\n'auto' = SDK decides.")]
        public PreloadAdBreaks PreloadAdBreaks = PreloadAdBreaks.on;

        [Tooltip("'on' = ads may play audio.\n'off' = ads are muted.")]
        public AdSound Sound = AdSound.on;

        [SerializeField]
        private bool showDebugMessages;

        // ── Ad events ─────────────────────────────────────────────────────────
        //
        // Only two events exist. Everything else (viewed / dismissed / noFill)
        // is returned via the AdBreakInfo.Status enum on the Task return value.
        //
        // Usage:
        //   Y8.Instance.OnAdPauseGame  += () => Time.timeScale = 0f;
        //   Y8.Instance.OnAdResumeGame += () => Time.timeScale = 1f;

        /// <summary>
        /// Fired by JS beforeAd. Pause your game audio and logic here.
        /// </summary>
        public event Action OnAdPauseGame;

        /// <summary>
        /// Fired by JS resumeOnce (via afterAd or adBreakDone fallback).
        /// Resume your game audio and logic here.
        /// </summary>
        public event Action OnAdResumeGame;

        // ── Auth error event ──────────────────────────────────────────────────

        /// <summary>
        /// Fired when onAuth receives an error (popup blocked, iframe fail, etc.).
        /// LoginAsync / AutoLoginAsync will also return IsSuccess=false.
        /// </summary>
        public event Action<AuthError> OnAuthError;

        // ── Internal state ────────────────────────────────────────────────────

        private int id = 10000;
        private readonly string calleeName = "Y8_Root";
        private readonly Dictionary<int, object> callIdToResponse = new();
        private Y8User currentUser;
        private Y8Token currentToken;

        // ── Unity lifecycle ───────────────────────────────────────────────────

        private void Awake()
        {
            if (Instance == null)
            {
                isReady = false;
                Instance = this;
                gameObject.name = calleeName;
                transform.SetParent(null);

                AppId = AppId.Trim();
                GameId = GameId.Trim();

                Init(
                    AppId,
                    GameId,
                    PreloadAdBreaks.ToString(),
                    AutoLogin ? 1 : 0,
                    Sound.ToString()
                );

                DontDestroyOnLoad(Instance.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // ── JS bindings / Editor stubs ────────────────────────────────────────

#if UNITY_EDITOR || !UNITY_WEBGL
        private static void Init(
            string appId,
            string gameId,
            string preloadAdBreaks,
            int autoLogin,
            string sound
        )
        {
#if !UNITY_WEBGL
            Debug.LogWarning($"Y8 API calls do not work for {Application.platform}!");
            return;
#endif
            if (string.IsNullOrEmpty(appId))
            {
                Debug.LogError(
                    "AppId is not set on Y8Root! Get yours: https://account.y8.com/applications"
                );
            }

            Debug.Log(
                $"[Y8] Init (editor stub) appId={appId} gameId={gameId} "
                    + $"preload={preloadAdBreaks} autoLogin={autoLogin} sound={sound}"
            );
            isReady = true;
        }

        private static void Call(int _id, string _request, string _jsonData) =>
            Debug.Log($"[Y8 editor stub] [{_id}] '{_request}' data={_jsonData}");
#else
        [DllImport("__Internal")]
        private static extern void Init(
            string _appId,
            string _gameId,
            string _preloadAdBreaks,
            int _autoLogin,
            string _sound
        );

        [DllImport("__Internal")]
        private static extern void Call(int _id, string _request, string _jsonData);
#endif

        // ── Auth ──────────────────────────────────────────────────────────────

        /// <summary>
        /// Checks login status. autoLogin during Init handles silent auth;
        /// call this to get the current user state.
        /// </summary>
        public async Task<JsResponse<Y8User>> AutoLoginAsync() =>
            await TryCallAsync<Y8User>("autoLogin", null);

        /// <summary>
        /// Opens the Y8 login popup / silent iframe auth.
        /// Returns IsSuccess=false if popup is blocked or iframe fails.
        /// </summary>
        public async Task<JsResponse<Y8User>> LoginAsync() =>
            await TryCallAsync<Y8User>("login", null);

        /// <summary>Logs out and clears local user state.</summary>
        public async Task LogoutAsync()
        {
            currentUser = null;
            currentToken = null;
            await TryCallAsync<Empty>("logout", null);
        }

        /// <summary>
        /// Asks JS for the user the SDK currently holds synchronously via
        /// sdk.getUser(). Useful when you want to refresh the cached user
        /// without triggering a login popup.
        ///
        /// Returns IsSuccess=true + the user when a session exists, or
        /// IsSuccess=false + null when no session is active.
        /// </summary>
        public async Task<JsResponse<Y8User>> GetUserAsync() =>
            await TryCallAsync<Y8User>("getUser", null);

        /// <summary>
        /// Returns the cached user object without any JS round-trip.
        /// This is the C# equivalent of the synchronous y8Sdk.getUser() call.
        /// Returns null when no session is active.
        /// The value is kept up-to-date by LoginAsync / AutoLoginAsync /
        /// GetUserAsync and cleared by LogoutAsync.
        /// </summary>
        public Y8User GetUser() => currentUser;

        /// <summary>
        /// Fetches the latest user data from the server, updates the local cache,
        /// and re-triggers the onAuth callback with the fresh user.
        ///
        /// Mirrors: y8Sdk.reloadUser().then(user => { ... })
        ///
        /// Returns IsSuccess=true + the refreshed Y8User when a session exists.
        /// Returns IsSuccess=false + null when no session is active or on error.
        ///
        /// Note: onAuth will ALSO fire during this call (updating currentUser a
        /// second time via AuthCallbackResponse). Both paths produce the same data
        /// so this is safe — the awaited return value is always from the direct
        /// reloadUser response.
        /// </summary>
        public async Task<JsResponse<Y8User>> ReloadUserAsync() =>
            await TryCallAsync<Y8User>("reloadUser", null);

        /// <summary>
        /// Asks JS for the token the SDK currently holds synchronously via
        /// sdk.getToken(). Refreshes the cached token without triggering a
        /// login popup.
        ///
        /// Returns IsSuccess=true + the token when a session exists, or
        /// IsSuccess=false + null when not logged in.
        /// </summary>
        public async Task<JsResponse<Y8Token>> GetTokenAsync() =>
            await TryCallAsync<Y8Token>("getToken", null);

        /// <summary>
        /// Returns the cached token object without any JS round-trip.
        /// This is the C# equivalent of the synchronous y8Sdk.getToken() call.
        /// Returns null when not logged in.
        /// Kept up-to-date by GetTokenAsync() and cleared by LogoutAsync().
        /// </summary>
        public Y8Token GetToken() => currentToken;

        /// <summary>
        /// Exchanges the current refresh token for a new access token and updates
        /// the local token cache.
        ///
        /// Mirrors: y8Sdk.refreshToken().then(token => { ... })
        ///
        /// Returns IsSuccess=true + the new Y8Token on success.
        /// Returns IsSuccess=false + null if not logged in, the refresh token has
        /// expired, or the server returns an error.
        ///
        /// Note: Unlike ReloadUserAsync(), this does NOT trigger onAuth — only the
        /// token is updated. currentUser is left unchanged.
        /// </summary>
        public async Task<JsResponse<Y8Token>> RefreshTokenAsync() =>
            await TryCallAsync<Y8Token>("refreshToken", null);

        // ── Ads ───────────────────────────────────────────────────────────────

        /// <summary>
        /// Shows an ad break of the given type and waits for it to complete.
        ///
        /// Subscribe to OnAdPauseGame / OnAdResumeGame to pause/resume your game.
        /// Inspect the returned AdBreakInfo.Status for the outcome:
        ///
        ///   AdBreakStatus.Viewed    → rewarded ad fully watched, grant reward
        ///   AdBreakStatus.Dismissed → rewarded ad skipped, do NOT grant reward
        ///   AdBreakStatus.NoFill    → no ad available, show "try later" message
        ///   AdBreakStatus.Error     → SDK error, handle gracefully
        ///
        /// Usage — interstitial:
        ///   await Y8.Instance.ShowAdAsync(AdType.start);
        ///
        /// Usage — rewarded:
        ///   AdBreakInfo info = await Y8.Instance.ShowAdAsync(AdType.reward);
        ///   if (info.IsSuccess && info.Data.WasViewed) GrantReward();
        /// </summary>
        /// <param name="type">
        /// Ad break type. Defaults to start (game loaded, before play).
        /// Use AdType.reward for rewarded ads.
        /// </param>
        /// <param name="name">Optional tracking name shown in the Y8 dashboard.</param>
        public async Task<JsResponse<AdBreakInfo>> ShowAdAsync(
            AdType type = AdType.start,
            string name = ""
        )
        {
            // if (Screen.fullScreen)
            // {
            //     TryDebugLog("Fullscreen detected – skipping ad");
            //     return new JsResponse<AdBreakInfo>(false, MakeAdBreakInfo(type, "other", name));
            // }

            if (string.IsNullOrEmpty(GameId))
            {
                TryDebugLog("GameId not set – skipping ad");
                return new JsResponse<AdBreakInfo>(false, MakeAdBreakInfo(type, "other", name));
            }

            string adName = string.IsNullOrEmpty(name) ? type.ToString() + "-game" : name;

            KeyValuePair<string, IConvertible>[] json =
            {
                new("type", type.ToString()),
                new("name", adName)
            };

            return await TryCallAsync<AdBreakInfo>("showAd", json);
        }

        // Helper to build a local AdBreakInfo when skipping before calling JS
        private static AdBreakInfo MakeAdBreakInfo(AdType type, string status, string name)
        {
            AdBreakInfo info =
                new()
                {
                    breakType = type.ToString(),
                    breakFormat = type.ToString(),
                    breakStatus = status,
                    breakName = name
                };
            info.ResolveStatus();
            return info;
        }

        // ── Achievements ──────────────────────────────────────────────────────

        /// <summary>Opens the achievements modal dialog.</summary>
        public async Task ShowAchievementsAsync() =>
            await TryCallAsync<Empty>("showAchievements", null);

        /// <summary>
        /// Returns all achievements for this app.
        /// If logged in, includes player unlock status.
        /// </summary>
        public async Task<JsResponse<AchievementsData>> GetAchievements() =>
            await TryCallAsync<AchievementsData>("getAchievements", null);

        /// <summary>
        /// Unlocks an achievement for the current player. Requires login.
        /// </summary>
        /// <param name="achievement">Title — must exactly match the dashboard.</param>
        /// <param name="achievementkey">Key — must exactly match the dashboard.</param>
        /// <param name="overwrite">Allow re-unlocking the same achievement.</param>
        /// <param name="allowduplicates">Allow multiple unlock entries.</param>
        public async Task<JsResponse<AchievementSave>> AwardAchievementAsync(
            string achievement,
            string achievementkey,
            bool overwrite = false,
            bool allowduplicates = false
        )
        {
            if (!IsLoggedIn())
            {
                TryDebugLog("Player is not logged in! Can't use AwardAchievement");
                return new JsResponse<AchievementSave>(false, default);
            }

            KeyValuePair<string, IConvertible>[] json =
            {
                new("achievement", achievement),
                new("achievementkey", achievementkey),
                new("overwrite", overwrite),
                new("allowduplicates", allowduplicates)
            };

            return await TryCallAsync<AchievementSave>("awardAchievement", json);
        }

        // ── Leaderboards ──────────────────────────────────────────────────────

        /// <summary>Returns the leaderboard table names for this app.</summary>
        public async Task<JsResponse<ScoreTables>> GetLeaderboardsAsync() =>
            await TryCallAsync<ScoreTables>("getLeaderboards", null);

        /// <summary>Returns score data for a custom leaderboard display.</summary>
        public async Task<JsResponse<ScoreTable>> GetLeaderboardScoresAsync(
            string table,
            string mode = "alltime",
            int perPage = 20,
            int page = 1,
            bool highest = true,
            string playerid = ""
        )
        {
            List<KeyValuePair<string, IConvertible>> json =
                new()
                {
                    new("table", table),
                    new("mode", mode),
                    new("perPage", perPage),
                    new("page", page),
                    new("highest", highest)
                };

            if (!string.IsNullOrEmpty(playerid))
            {
                json.Add(new KeyValuePair<string, IConvertible>("playerid", playerid));
            }

            return await TryCallAsync<ScoreTable>("getLeaderboardScores", json.ToArray());
        }

        /// <summary>Opens the built-in leaderboard modal dialog.</summary>
        public async Task ShowLeaderboardAsync(
            string tableTitle,
            string mode = "alltime",
            bool highest = true,
            bool useMilli = false
        )
        {
            List<KeyValuePair<string, IConvertible>> json =
                new() { new("table", tableTitle), new("mode", mode), new("highest", highest) };

            if (useMilli)
            {
                json.Add(new KeyValuePair<string, IConvertible>("useMilli", useMilli));
            }

            await TryCallAsync<Empty>("showLeaderboard", json.ToArray());
        }

        /// <summary>
        /// Saves a score for the current player. Requires login.
        /// </summary>
        public async Task<JsResponse<ScoreSave>> SaveLeaderboardScoreAsync(
            string table,
            int points,
            bool allowduplicates = false,
            bool highest = true
        )
        {
            if (!IsLoggedIn())
            {
                TryDebugLog("Player is not logged in! Can't use SaveLeaderboardScore");
                return new JsResponse<ScoreSave>(false, default);
            }

            List<KeyValuePair<string, IConvertible>> json =
                new()
                {
                    new("table", table),
                    new("points", points),
                    new("allowduplicates", allowduplicates),
                    new("highest", highest),
                    new("playername", Nickname())
                };

            return await TryCallAsync<ScoreSave>("saveLeaderboardScore", json.ToArray());
        }

        // ── Online Saves ──────────────────────────────────────────────────────

        /// <summary>
        /// Saves a string value under the given key. Requires login.
        /// Max value size: 30 KB.
        /// </summary>
        public async Task<JsResponse<SetData>> SaveDataAsync(string key, string value)
        {
            if (!IsLoggedIn())
            {
                TryDebugLog("Player is not logged in! Can't use SaveData");
                return new JsResponse<SetData>(false, default);
            }

            KeyValuePair<string, IConvertible>[] json = { new("key", key), new("value", value) };

            return await TryCallAsync<SetData>("saveData", json);
        }

        /// <summary>Serialises data to JSON and saves it. Requires login.</summary>
        public async Task<JsResponse<SetData>> SaveDataAsync<T>(string key, T data)
            where T : class
        {
            string stringData = JsonUtility.ToJson(data).Replace("\"", "\'");
            return await SaveDataAsync(key, stringData);
        }

        /// <summary>Loads and deserialises a previously saved object. Requires login.</summary>
        public async Task<JsResponse<T>> LoadDataAsync<T>(string key)
            where T : class
        {
            JsResponse<GetData> raw = await LoadDataAsync(key);
            T data = null;

            if (raw.IsSuccess && raw.Data != null && !string.IsNullOrEmpty(raw.Data.value))
            {
                data = JsonUtility.FromJson<T>(raw.Data.value.Replace("\'", "\""));
            }

            return new JsResponse<T>(raw.IsSuccess, data);
        }

        /// <summary>Retrieves the raw string value for a key. Requires login.</summary>
        public async Task<JsResponse<GetData>> LoadDataAsync(string key)
        {
            if (!IsLoggedIn())
            {
                TryDebugLog("Player is not logged in! Can't use LoadData");
                return new JsResponse<GetData>(false, default);
            }

            KeyValuePair<string, IConvertible>[] json = { new("key", key) };

            return await TryCallAsync<GetData>("loadData", json);
        }

        /// <summary>Removes a key/value pair from online saves. Requires login.</summary>
        public async Task<JsResponse<SetData>> RemoveDataAsync(string key)
        {
            if (!IsLoggedIn())
            {
                TryDebugLog("Player is not logged in! Can't use ClearData");
                return new JsResponse<SetData>(false, default);
            }

            KeyValuePair<string, IConvertible>[] json = { new("key", key) };

            return await TryCallAsync<SetData>("removeData", json);
        }

        // ── App Image ─────────────────────────────────────────────────────────

        /// <summary>
        /// Submits a screenshot to the player's Y8 profile. Requires login.
        /// Use a coroutine with WaitForEndOfFrame when capturing the screen.
        /// </summary>
        public async Task<JsResponse<SavedScreenshot>> SubmitImageAsync(Texture2D screenshotTexture)
        {
            if (!IsLoggedIn())
            {
                TryDebugLog("Player is not logged in! Can't submit Image");
                return new JsResponse<SavedScreenshot>(false, default);
            }

            byte[] bytes = screenshotTexture.EncodeToJPG();
            string dataUrl = $"data:image/jpeg;base64,{Convert.ToBase64String(bytes)}";

            KeyValuePair<string, IConvertible>[] json = { new("data", dataUrl) };

            return await TryCallAsync<SavedScreenshot>("submitImage", json);
        }

        // ── Profile ───────────────────────────────────────────────────────────

        /// <summary>
        /// Opens the current player's Y8 profile in a new tab. Requires login.
        /// </summary>
        public async Task OpenProfileAsync()
        {
            if (!IsLoggedIn())
            {
                TryDebugLog("Player is not logged in! Can't open profile");
                return;
            }

            await TryCallAsync<Empty>("openProfile", null);
        }

        // ── Quick-access helpers ──────────────────────────────────────────────

        /// <summary>true if the user is currently authenticated.</summary>
        public bool IsLoggedIn() => currentUser != null && !string.IsNullOrEmpty(currentUser.pid);

        /// <summary>The current access token, or empty string.</summary>
        public string SessionToken() => currentUser?.access_token ?? string.Empty;

        /// <summary>The player's PID, or empty string.</summary>
        public string PID() => currentUser?.pid ?? string.Empty;

        /// <summary>The player's first name, or empty string.</summary>
        public string FirstName() => currentUser?.first_name ?? string.Empty;

        /// <summary>The player's nickname, or empty string.</summary>
        public string Nickname() => currentUser?.nickname ?? string.Empty;

        /// <summary>The player's date of birth (Y-M-D), or empty string.</summary>
        public string DateOfBirth() => currentUser?.dob ?? string.Empty;

        /// <summary>The player's gender, or empty string.</summary>
        public string Gender() => currentUser?.gender ?? string.Empty;

        /// <summary>The player's language setting, or system culture.</summary>
        public string Language() =>
            !string.IsNullOrEmpty(currentUser?.language)
                ? currentUser.language
                : CultureInfo.CurrentCulture.Name;

        /// <summary>The player's locale, or system culture.</summary>
        public string Locale() =>
            !string.IsNullOrEmpty(currentUser?.locale)
                ? currentUser.locale
                : CultureInfo.CurrentCulture.Name;

        // ── Internal async machinery ──────────────────────────────────────────

        private async Task<JsResponse<T>> TryCallAsync<T>(
            string requestName,
            KeyValuePair<string, IConvertible>[] kvPairs
        )
        {
            if (!isReady)
            {
                TryDebugLog("SDK is not ready");
                return new JsResponse<T>(false, default);
            }

            if (Application.isEditor)
            {
                TryDebugLog($"Fake editor response for: \"{requestName}\"");
                return new JsResponse<T>(false, default);
            }

            id++;
            int callId = id;

            string json = ConvertListToJson(kvPairs);
            TryDebugLog($"JS call [{callId}] with JSON = {json}");
            Call(callId, requestName, json);

            while (!callIdToResponse.ContainsKey(callId))
            {
                await Task.Yield();
            }

            object response = callIdToResponse[callId];
            callIdToResponse.Remove(callId);
            return (JsResponse<T>)response;
        }

        private static string ConvertListToJson(KeyValuePair<string, IConvertible>[] kvList)
        {
            if (kvList == null)
            {
                return "";
            }

            string s = "{ ";
            for (int i = 0, l = kvList.Length; i < l; i++)
            {
                s += $"\"{kvList[i].Key}\":";

                if (kvList[i].Value is string sv)
                {
                    s += $"\"{sv}\"";
                }
                else if (kvList[i].Value is bool bv)
                {
                    s += bv.ToString().ToLower();
                }
                else
                {
                    s += kvList[i].Value;
                }

                if (i + 1 < l)
                {
                    s += ",";
                }
            }
            s += " }";
            return s;
        }

        // ── JS → C# callbacks ─────────────────────────────────────────────────

        /// <summary>Called by JS when the SDK has finished initializing.</summary>
        public void CallbackReady()
        {
            TryDebugLog("Y8 SDK 2.0 is ready.");
            isReady = true;
        }

        /// <summary>Called by JS when onAuth fires with a valid user or not_connected.</summary>
        public void AuthCallbackResponse(string responseJson)
        {
            int authCallId = id;
            TryDebugLog($"AuthResponse from JS: {responseJson}");

            AuthPayload payload = JsonUtility.FromJson<AuthPayload>(responseJson);
            Debug.Log(payload.status);
            Debug.Log(payload.user);

            bool ok = payload?.user != null && !string.IsNullOrEmpty(payload.user.pid);
            currentUser = ok ? payload.user : null;

            if (currentUser == null)
            {
                callIdToResponse.Add(authCallId, new JsResponse<Empty>(ok, default));
                return;
            }

            callIdToResponse.Add(authCallId, new JsResponse<Y8User>(ok, currentUser));
        }

        /// <summary>
        /// Called by JS when onAuth fires with an error.
        /// Fires OnAuthError event and resolves the pending LoginAsync with failure.
        /// </summary>
        public void AuthCallbackError(string errorJson)
        {
            TryDebugLog($"AuthCallbackError from JS: {errorJson}");

            AuthError err =
                JsonUtility.FromJson<AuthError>(errorJson) ?? new AuthError { message = errorJson };

            OnAuthError?.Invoke(err);

            // Resolve the pending TryCallAsync (login / autoLogin) with failure
            callIdToResponse.Add(id, new JsResponse<Y8User>(false, null));
        }

        // ── Ad lifecycle callbacks (only pause/resume) ────────────────────────

        /// <summary>JS beforeAd — pause game audio/logic.</summary>
        public void AdPauseGame(string _)
        {
            TryDebugLog("AdPauseGame");
            OnAdPauseGame?.Invoke();
        }

        /// <summary>JS resumeOnce — resume game audio/logic.</summary>
        public void AdResumeGame(string _)
        {
            TryDebugLog("AdResumeGame");
            OnAdResumeGame?.Invoke();
        }

        /// <summary>
        /// Returns the locale code of the platform the game is running on
        /// (e.g. "fr", "de", "ja").
        /// Derived from the platform subdomain detected via postMessage during init.
        /// Returns "en" for unrecognised subdomains (including www).
        /// Does not require login.
        /// </summary>
        public async Task<JsResponse<PlatformLocale>> GetPlatformLocaleAsync()
        {
            return await TryCallAsync<PlatformLocale>("getPlatformLocale", null);
        }

        /// <summary>
        /// Checks whether the current domain is on the Y8 blacklist.
        /// Does not require login.
        /// The protection list is fetched once and cached for subsequent calls.
        /// Rejects on network or server errors — treated as false (not blacklisted) by default.
        /// </summary>
        /// <returns>true if the current domain is blacklisted.</returns>
        public async Task<JsResponse<bool>> IsBlacklistedAsync()
        {
            return await TryCallAsync<bool>("isBlacklisted", null);
        }

        /// <summary>
        /// Called by JS for every non-auth response.
        /// Protocol: request[id]=json
        /// </summary>
        public void CallbackResponse(string responseString)
        {
            int ob = responseString.IndexOf('[');
            int cb = responseString.IndexOf(']');
            string req = responseString.Substring(0, ob);
            int _id = int.Parse(responseString.Substring(ob + 1, cb - ob - 1));
            string data = responseString.Substring(cb + 2);

            TryDebugLog($"Response from JS: {req}[{_id}] = '{data}'");

            object response;

            switch (req)
            {
                // show_ad returns AdBreakInfo with a typed Status enum
                case "showAd":
                {
                    AdBreakInfo info = JsonUtility.FromJson<AdBreakInfo>(data) ?? new AdBreakInfo();
                    info.ResolveStatus(); // populate the AdBreakStatus enum from raw breakStatus string
                    response = new JsResponse<AdBreakInfo>(true, info);
                    break;
                }
                case "awardAchievement":
                {
                    AchievementSave d = JsonUtility.FromJson<AchievementSave>(data);
                    response = new JsResponse<AchievementSave>(d.success, d);
                    break;
                }
                case "getAchievements":
                {
                    AchievementsData d = JsonUtility.FromJson<AchievementsData>(data);
                    response = new JsResponse<AchievementsData>(d?.achievements != null, d);
                    break;
                }
                case "saveLeaderboardScore":
                {
                    ScoreSave d = JsonUtility.FromJson<ScoreSave>(data);
                    response = new JsResponse<ScoreSave>(d.success, d);
                    break;
                }
                case "saveData":
                case "removeData":
                {
                    SetData d = JsonUtility.FromJson<SetData>(data);
                    response = new JsResponse<SetData>(d.success, d);
                    break;
                }
                case "loadData":
                {
                    GetData d = JsonUtility.FromJson<GetData>(data);
                    response = new JsResponse<GetData>(string.IsNullOrEmpty(d.error), d);
                    break;
                }
                case "getLeaderboardScores":
                {
                    ScoreTable d = JsonUtility.FromJson<ScoreTable>(data);
                    response = new JsResponse<ScoreTable>(d?.items != null, d);
                    break;
                }
                case "getLeaderboards":
                {
                    ScoreTables d = JsonUtility.FromJson<ScoreTables>(data);
                    response = new JsResponse<ScoreTables>(d?.tables != null, d);
                    break;
                }
                case "autoLogin":
                {
                    AuthPayload p = JsonUtility.FromJson<AuthPayload>(data);
                    bool ok = p?.user != null && !string.IsNullOrEmpty(p.user.pid);
                    if (ok)
                    {
                        currentUser = p.user;
                    }

                    response = new JsResponse<Y8User>(ok, ok ? currentUser : null);
                    break;
                }
                case "reloadUser":
                {
                    AuthPayload p = JsonUtility.FromJson<AuthPayload>(data);
                    bool ok = p?.user != null && !string.IsNullOrEmpty(p.user.pid);
                    if (ok)
                    {
                        currentUser = p.user;
                    }
                    else
                    {
                        currentUser = null;
                    }

                    response = new JsResponse<Y8User>(ok, ok ? currentUser : null);
                    break;
                }
                case "getUser":
                {
                    GetUserPayload p = JsonUtility.FromJson<GetUserPayload>(data);
                    bool ok = p?.user != null && !string.IsNullOrEmpty(p.user.pid);
                    if (ok)
                    {
                        currentUser = p.user;
                    }

                    response = new JsResponse<Y8User>(ok, ok ? currentUser : null);
                    break;
                }
                case "getToken":
                {
                    TokenPayload p = JsonUtility.FromJson<TokenPayload>(data);
                    bool ok = p?.token != null && !string.IsNullOrEmpty(p.token.access_token);
                    if (ok)
                    {
                        currentToken = p.token;
                    }
                    else
                    {
                        currentToken = null;
                    }

                    response = new JsResponse<Y8Token>(ok, currentToken);
                    break;
                }
                case "refreshToken":
                {
                    TokenPayload p = JsonUtility.FromJson<TokenPayload>(data);
                    bool ok = p?.token != null && !string.IsNullOrEmpty(p.token.access_token);
                    if (ok)
                    {
                        currentToken = p.token;
                    }
                    else
                    {
                        currentToken = null;
                    }

                    response = new JsResponse<Y8Token>(ok, currentToken);
                    break;
                }
                case "submitImage":
                {
                    SavedScreenshot d = JsonUtility.FromJson<SavedScreenshot>(data);
                    response = new JsResponse<SavedScreenshot>(d.success, d);
                    break;
                }
                // case "logout":
                case "showLeaderboard":
                case "showAchievements":
                case "openProfile":
                    response = new JsResponse<Empty>(true, default);
                    break;

                case "isBlacklisted":
                    bool.TryParse(data, out bool isBlacklisted);
                    response = new JsResponse<bool>(true, isBlacklisted);
                    break;

                case "getPlatformLocale":
                    PlatformLocale platformLocale = JsonUtility.FromJson<PlatformLocale>(data);
                    response = new JsResponse<PlatformLocale>(
                        !string.IsNullOrEmpty(platformLocale.locale),
                        platformLocale
                    );
                    break;

                default:
                    TryDebugLog($"Unhandled request type: {req}");
                    response = new JsResponse<Empty>(false, default);
                    break;
            }

            callIdToResponse.Add(_id, response);
        }

        private void TryDebugLog(object message)
        {
            if (showDebugMessages)
            {
                Debug.Log($"[Y8] {message}");
            }
        }

        // ── Inner types ───────────────────────────────────────────────────────

        [Serializable]
        private class AuthPayload
        {
            public string status = "";
            public Y8User user = null;
        }

        [Serializable]
        private class GetUserPayload
        {
            public Y8User user = null;
        }

        [Serializable]
        private class TokenPayload
        {
            public Y8Token token = null;
        }
    }
}
