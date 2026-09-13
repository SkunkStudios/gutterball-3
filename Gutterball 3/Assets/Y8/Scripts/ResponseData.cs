using System;

namespace Y8API
{
    // ─── Auth ─────────────────────────────────────────────────────────────────

    [Serializable]
    public class Y8User
    {
        public string pid = null;
        public string nickname = null;
        public string first_name = null;
        public string gender = null;
        public string language = null;
        public string locale = null;
        public string dob = null;
        public string access_token = null;
        public int level = 0;
        public Avatars avatars = null;
    }

    [Serializable]
    public class Avatars
    {
        public string thumb_url = null;
        public string thumb_secure_url = null;
        public string medium_url = null;
        public string medium_secure_url = null;
        public string large_url = null;
        public string large_secure_url = null;
    }

    [Serializable]
    public class AuthError
    {
        public string message = "";
        public int code = 0;
    }

    /// <summary>
    /// Token returned by y8Sdk.getToken().
    /// null when not logged in.
    /// Most SDK methods handle tokens internally — you rarely need this directly.
    /// </summary>
    [Serializable]
    public class Y8Token
    {
        public string access_token = null;
        public string refresh_token = null;

        /// <summary>Always "bearer".</summary>
        public string token_type = null;
        public string scope = null;

        /// <summary>Lifetime in seconds (e.g. 31 536 000 = 1 year).</summary>
        public int expires_in = 0;

        /// <summary>Unix timestamp of when the token was created.</summary>
        public int created_at = 0;
    }

    // ─── Ads ──────────────────────────────────────────────────────────────────

    /// <summary>
    /// Ad break type passed to showAd().
    /// Matches the new SDK's type strings exactly via .ToString().
    /// </summary>
    public enum AdType
    {
        /// <summary>Game loaded, before play starts. Most common slot.</summary>
        start,

        /// <summary>Player paused the game manually.</summary>
        pause,

        /// <summary>Between levels or rounds.</summary>
        next,

        /// <summary>Player browsing menus or options.</summary>
        browse,

        /// <summary>Rewarded ad — player opts in for a reward.</summary>
        reward
    }

    /// <summary>
    /// What happened at the end of an ad break.
    /// Populated from adBreakDone info.breakStatus.
    /// </summary>
    public enum AdBreakStatus
    {
        /// <summary>Ad played to completion. For rewarded ads: grant the reward.</summary>
        Viewed,

        /// <summary>Player closed the rewarded ad before it finished. Do NOT grant reward.</summary>
        Dismissed,

        /// <summary>
        /// No ad was available (frequencyCapped / noAdPreloaded / other).
        /// Tell the player to try again later.
        /// </summary>
        NoFill,

        /// <summary>SDK error or Promise rejection during showAd().</summary>
        Error
    }

    /// <summary>
    /// Returned by ShowAdAsync() and ShowRewardedAdAsync().
    /// Contains the full adBreakDone payload plus a typed Status enum.
    /// </summary>
    [Serializable]
    public class AdBreakInfo
    {
        // ── Raw fields from adBreakDone info object ────────────────────────
        public string breakType = "";
        public string breakFormat = "";
        public string breakStatus = "";
        public string breakName = "";

        // ── Typed status — set by C# after deserialisation ─────────────────
        /// <summary>
        /// Typed result of the ad break. Use this instead of comparing breakStatus strings.
        /// </summary>
        public AdBreakStatus Status = AdBreakStatus.Error;

        /// <summary>Convenience: true only when a rewarded ad was fully watched.</summary>
        public bool WasViewed => Status == AdBreakStatus.Viewed;

        /// <summary>
        /// Parses the raw breakStatus string from the SDK into the typed Status enum.
        /// Called once after JsonUtility.FromJson().
        /// </summary>
        public void ResolveStatus()
        {
            Status = breakStatus switch
            {
                "viewed" => AdBreakStatus.Viewed,
                "dismissed" => AdBreakStatus.Dismissed,
                "frequencyCapped" or "noAdPreloaded" or "other" => AdBreakStatus.NoFill,
                _ => AdBreakStatus.Error,
            };
        }
    }

    // ─── Achievements ─────────────────────────────────────────────────────────

    [Serializable]
    public class AchievementSave
    {
        public bool success = false;
        public string errormessage = "";
    }

    [Serializable]
    public class AchievementsData
    {
        public Achievement[] achievements;
    }

    [Serializable]
    public class Achievement
    {
        public string achievementid = null;
        public string achievement = null;
        public string description = null;
        public string achievementkey = null;
        public string icon = null;
        public string difficulty = null;
        public bool secret = false;
        public int awarded = 0;
        public string game = null;
        public string link = null;
        public PlayerAchievement player = null;
    }

    [Serializable]
    public class PlayerAchievement
    {
        public int date = 0;
        public string rdate = null;
        public string playername = null;
        public string playerid = null;
    }

    // ─── Leaderboards ─────────────────────────────────────────────────────────

    [Serializable]
    public class ScoreTables
    {
        public string[] tables;
    }

    [Serializable]
    public class ScoreTable
    {
        public Score[] items;
        public int page = 1;
        public int perPage = 10;
        public int totalPages = 1;
    }

    [Serializable]
    public class Score
    {
        public string table = null;
        public string playerid = null;
        public string playername = null;
        public int points = 0;
        public int rank = 0;
        public int date = 0;
        public string rdate = null;
        public string scoreid = null;
    }

    [Serializable]
    public class ScoreSave
    {
        public bool success = false;
        public string errormessage = "";
    }

    // ─── Online Saves ──────────────────────────────────────────────────────────

    [Serializable]
    public class SetData
    {
        public bool success = false;
        public string key = "";
    }

    [Serializable]
    public class GetData
    {
        public string key = "";
        public string value = "";
        public string error = "";
    }

    // ─── App Image ────────────────────────────────────────────────────────────

    [Serializable]
    public class SavedScreenshot
    {
        public bool success = false;
        public string imageUrl = "";
        public string message = "";
    }

    // ─── Shared ───────────────────────────────────────────────────────────────

    [Serializable]
    public class JsResponse<T>
    {
        public bool IsSuccess { get; }
        public T Data { get; }

        public JsResponse(bool isSuccess, T data)
        {
            IsSuccess = isSuccess;
            Data = data;
        }
    }

    public class Empty { }

    /// <summary>
    /// Returned by GetPlatformLocaleAsync().
    /// locale is a two-letter code e.g. "en", "fr", "de", "ja".
    /// Returns "en" for unrecognised subdomains (including www).
    /// Does not require login.
    /// </summary>
    [Serializable]
    public class PlatformLocale
    {
        public string locale = "en";
    }
}
