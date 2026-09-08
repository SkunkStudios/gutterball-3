mergeInto(LibraryManager.library,
    {
        /**
         * Init
         * _appId           : Y8 application ID
         * _gameId          : Y8 ads game ID (empty string = no ads)
         * _preloadAdBreaks : 'on' | 'auto'
         * _autoLogin       : int 1/0
         * _sound           : 'on' | 'off'
         */
        Init: function (_appId, _gameId, _preloadAdBreaks, _autoLogin, _sound) {
            var appId = UTF8ToString(_appId);
            var gameId = UTF8ToString(_gameId);
            var preloadAdBreaks = UTF8ToString(_preloadAdBreaks);
            var autoLogin = !!_autoLogin;
            var sound = UTF8ToString(_sound);

            // ── Module-level ad guard state ────────────────────────────────────
            window._y8AdInFlight = false;
            window._y8PausedByAd = false;

            function onSdkReady() {
                var y8Sdk = y8.sdk();
                window._y8Sdk = y8Sdk;

                // ── Auth callback ──────────────────────────────────────────────
                y8Sdk.onAuth(function (user, error) {
                    if (error) {
                        var errPayload = {
                            message: (error && error.message) ? error.message : String(error),
                            code: (error && error.code) ? error.code : 0
                        };
                        SendMessage('Y8_Root', 'AuthCallbackError', JSON.stringify(errPayload));
                        return;
                    }

                    var payload = {
                        status: user ? 'connected' : 'not_connected',
                        user: user || {}
                    };
                    SendMessage('Y8_Root', 'AuthCallbackResponse', JSON.stringify(payload));
                });

                // ── SDK init ───────────────────────────────────────────────────
                var appConfig = {
                    appId: appId,
                    autoLogin: autoLogin
                };

                var adConfig;
                if (gameId) {
                    adConfig = {
                        gameId: gameId,
                        preloadAdBreaks: preloadAdBreaks,
                        sound: sound,
                        onReady: function () {
                            console.log('[Y8] adConfig.onReady — ads ready');
                        }
                    };
                }

                y8Sdk.init(appConfig, adConfig);
                SendMessage('Y8_Root', 'CallbackReady');
            }

            window.addEventListener('y8sdk.ready', onSdkReady, { once: true });

            // Handle race-condition: SDK already loaded before listener registered
            if (window.y8 && window.y8.emitReadyEvent) {
                window.y8.emitReadyEvent();
            }

            // ── Load SDK 2.0 ───────────────────────────────────────────────────
            (function (d, s, id) {
                if (d.getElementById(id)) { return; }
                var js = d.createElement(s);
                js.id = id;
                js.src = 'https://cdn.y8.com/minimal-sdk/2-0/y8.min.js';
                js.async = true;
                d.head.appendChild(js);
            }(document, 'script', 'y8-jssdk'));
        },

        Call: function (_id, _request, _jsonData) {
            var id = _id;
            var request = UTF8ToString(_request);
            var jsonStr = _jsonData ? UTF8ToString(_jsonData) : '';
            var jsonData = (jsonStr && jsonStr.length > 2) ? JSON.parse(jsonStr) : {};

            var sdk = window._y8Sdk;
            if (!sdk) {
                console.warn('[Y8] SDK not ready for request: ' + request);
                SendMessage('Y8_Root', 'CallbackResponse', request + '[' + id + ']={}');
                return;
            }

            function respond(payload) {
                SendMessage('Y8_Root', 'CallbackResponse',
                    request + '[' + id + ']=' + JSON.stringify(payload));
            }

            function respondError(err) {
                var msg = (err && err.message) ? err.message : String(err);
                respond({ success: false, errormessage: msg });
            }

            switch (request) {
                // ── Auth ──────────────────────────────────────────────────────
                case 'login':
                    sdk.login();
                    // Result arrives via onAuth → AuthCallbackResponse / AuthCallbackError
                    break;

                case 'logout':
                    sdk.logout();
                    // Result arrives via onAuth → AuthCallbackResponse / AuthCallbackError
                    // respond({ success: true });
                    break;

                case 'autoLogin':
                    var user = sdk.getUser ? sdk.getUser() : null;
                    respond(user
                        ? { status: 'connected', user: user }
                        : { status: 'not_connected' });
                    break;

                case 'getToken':
                    {
                        var token = sdk.getToken ? sdk.getToken() : null;
                        respond({ token: token || null });
                        break;
                    }

                case 'refreshToken':
                    sdk.refreshToken()
                        .then(function (token) {
                            respond({ token: token || null });
                        })
                        .catch(function (e) {
                            console.error('[Y8] refreshToken error:', e);
                            respond({ token: null });
                        });
                    break;

                case 'getUser':
                    {
                        var user = sdk.getUser ? sdk.getUser() : null;
                        respond(user ? { user: user } : { user: null });
                        break;
                    }

                case 'reloadUser':
                    sdk.reloadUser()
                        .then(function (user) {
                            // onAuth will also fire, but we respond here so TryCallAsync
                            // resolves with the fresh user directly on the awaiting call.
                            respond(user
                                ? { status: 'connected', user: user }
                                : { status: 'not_connected' });
                        })
                        .catch(function (e) {
                            console.error('[Y8] reloadUser error:', e);
                            respond({ status: 'not_connected' });
                        });
                    break;

                // ── Ads ───────────────────────────────────────────────────────
                //
                // showAd handles ALL ad types (start/pause/next/browse/reward).
                // jsonData.type drives the behaviour.
                //
                // Ad event flow:
                //   beforeAd     → SendMessage AdPauseGame  (pause your game)
                //   afterAd      → resumeOnce               (interstitial path)
                //   beforeReward → showAdFn()               (required gate for reward)
                //   adBreakDone  → SendMessage CallbackResponse with full info
                //                  + resumeOnce fallback
                //
                // Only TWO SendMessage events exist: AdPauseGame and AdResumeGame.
                // Everything else (viewed/dismissed/noFill) is encoded in the
                // adBreakDone info object returned to C# via CallbackResponse,
                // which then populates AdBreakInfo.Status enum.
                // ─────────────────────────────────────────────────────────────
                case 'showAd':
                    {
                        var adType = jsonData.type || 'start';
                        var adName = jsonData.name || adType + '-game';

                        // ── Guards ─────────────────────────────────────────────
                        if (window._y8AdInFlight) {
                            console.warn('[Y8] Ad request ignored — ad already in flight');
                            respond({
                                breakType: adType,
                                breakFormat: adType,
                                breakStatus: 'other',
                                breakName: adName
                            });
                            break;
                        }

                        window._y8AdInFlight = true;
                        window._y8PausedByAd = false;

                        // Fires exactly once per ad break regardless of which
                        // callback triggers the resume
                        var _resumed = false;
                        function resumeOnce(from) {
                            if (_resumed) return;
                            _resumed = true;
                            window._y8AdInFlight = false;
                            console.log('[Y8] resumeOnce from:', from);
                            if (window._y8PausedByAd) {
                                window._y8PausedByAd = false;
                                SendMessage('Y8_Root', 'AdResumeGame', '');
                            }
                        }

                        var payload = {
                            type: adType,
                            name: adName,

                            beforeAd: function () {
                                window._y8PausedByAd = true;
                                console.log('[Y8] beforeAd:', adType);
                                SendMessage('Y8_Root', 'AdPauseGame', '');
                            },

                            afterAd: function () {
                                console.log('[Y8] afterAd:', adType);
                                resumeOnce('afterAd');
                            },

                            // Required for reward type — without this the ad never shows
                            beforeReward: function (showAdFn) {
                                console.log('[Y8] beforeReward — calling showAdFn()');
                                showAdFn();
                            },

                            // adViewed and adDismissed are NOT sent as separate SendMessage
                            // calls. Their outcome is already encoded in adBreakDone's
                            // breakStatus ("viewed" / "dismissed") which C# maps to the
                            // AdBreakStatus enum. No extra events needed.
                            adViewed: function () {
                                console.log('[Y8] adViewed');
                            },

                            adDismissed: function () {
                                console.log('[Y8] adDismissed');
                            },

                            // Always fires at end of every ad break.
                            // This is the single point where C# is notified of the outcome.
                            adBreakDone: function (info) {
                                var safeInfo = info || {};
                                console.log('[Y8] adBreakDone:', adType, safeInfo);

                                // Fallback resume if afterAd never fired
                                resumeOnce('adBreakDone');

                                // Resolve the C# TryCallAsync with the full break info.
                                // C# will parse breakStatus into AdBreakStatus enum.
                                respond({
                                    breakType: safeInfo.breakType || adType,
                                    breakFormat: safeInfo.breakFormat || adType,
                                    breakStatus: safeInfo.breakStatus || 'other',
                                    breakName: safeInfo.breakName || adName
                                });
                            }
                        };

                        sdk.showAd(payload)
                            .catch(function (e) {
                                console.error('[Y8] showAd error:', e);
                                resumeOnce('catch');
                                // Return an error-status AdBreakInfo so C# can handle it cleanly
                                respond({
                                    breakType: adType,
                                    breakFormat: adType,
                                    breakStatus: 'error',
                                    breakName: adName
                                });
                            });
                        break;
                    }

                // ── Achievements ──────────────────────────────────────────────
                case 'getAchievements':
                    sdk.getAchievements()
                        .then(function (list) { respond({ achievements: list }); })
                        .catch(respondError);
                    break;

                case 'showAchievements':
                    sdk.showAchievements()
                        .then(function () { respond({ success: true }); })
                        .catch(respondError);
                    break;

                case 'awardAchievement':
                    sdk.awardAchievement({
                        achievement: jsonData.achievement,
                        achievementKey: jsonData.achievementkey,
                        overwrite: jsonData.overwrite || false,
                        allowDuplicates: jsonData.allowduplicates || false
                    })
                        .then(function () { respond({ success: true, errormessage: '' }); })
                        .catch(function (e) { respond({ success: false, errormessage: e.message || String(e) }); });
                    break;

                // ── Leaderboards ──────────────────────────────────────────────
                case 'getLeaderboards':
                    sdk.getLeaderboards()
                        .then(function (tables) { respond({ tables: tables }); })
                        .catch(respondError);
                    break;

                case 'getLeaderboardScores':
                    sdk.getLeaderboardScores({
                        table: jsonData.table,
                        page: jsonData.page || 1,
                        perPage: jsonData.perPage || 20,
                        mode: jsonData.mode || 'alltime',
                        highest: jsonData.highest !== undefined ? jsonData.highest : true,
                        playerId: jsonData.playerid || null
                    })
                        .then(function (scores) { respond(scores); })
                        .catch(respondError);
                    break;

                case 'showLeaderboard':
                    sdk.showLeaderboard({
                        table: jsonData.table,
                        mode: jsonData.mode || 'alltime',
                        highest: jsonData.highest !== undefined ? jsonData.highest : true,
                        useMilli: jsonData.useMilli || false
                    })
                        .then(function () { respond({ success: true }); })
                        .catch(respondError);
                    break;

                case 'saveLeaderboardScore':
                    sdk.saveLeaderboardScore({
                        table: jsonData.table,
                        points: jsonData.points,
                        allowDuplicates: jsonData.allowduplicates || false,
                        highest: jsonData.highest !== undefined ? jsonData.highest : true,
                        playerName: jsonData.playername || null
                    })
                        .then(function () { respond({ success: true, errormessage: '' }); })
                        .catch(function (e) { respond({ success: false, errormessage: e.message || String(e) }); });
                    break;

                // ── Online Saves ──────────────────────────────────────────────
                case 'saveData':
                    sdk.saveData({ key: jsonData.key, value: jsonData.value, retries: true })
                        .then(function () { respond({ success: true, key: jsonData.key }); })
                        .catch(function (e) {
                            respond({
                                success: false, key: jsonData.key,
                                errormessage: e.message || String(e)
                            });
                        });
                    break;

                case 'loadData':
                    sdk.loadData({ key: jsonData.key })
                        .then(function (val) { respond({ key: jsonData.key, value: val || '', error: '' }); })
                        .catch(function (e) {
                            respond({
                                key: jsonData.key, value: '',
                                error: e.message || String(e)
                            });
                        });
                    break;

                case 'removeData':
                    sdk.removeData({ key: jsonData.key })
                        .then(function () { respond({ success: true, key: jsonData.key }); })
                        .catch(function (e) {
                            respond({
                                success: false, key: jsonData.key,
                                errormessage: e.message || String(e)
                            });
                        });
                    break;

                // ── App Image ──────────────────────────────────────────────────
                case 'submitImage':
                    sdk.submitImage({ picture: jsonData.data })
                        .then(function (imageUrl) { respond({ success: true, imageUrl: imageUrl, message: '' }); })
                        .catch(function (e) {
                            respond({
                                success: false, imageUrl: '',
                                message: e.message || String(e)
                            });
                        });
                    break;

                // ── Profile ────────────────────────────────────────────────────
                case 'openProfile':
                    sdk.openProfile()
                        .then(function () { respond({ success: true }); })
                        .catch(respondError);
                    break;

                case 'isBlacklisted':
                    sdk.isBlacklisted()
                        .then(function (blacklisted) {
                            respond(blacklisted);   // raw bool — C# parses with bool.TryParse
                        })
                        .catch(function (e) {
                            console.error('[Y8] isBlacklisted error:', e);
                            // Fail safe: if the check errors, treat as NOT blacklisted
                            respond(false);
                        });
                    break;

                case 'getPlatformLocale':
                    sdk.getPlatformLocale()
                        .then(function (locale) {
                            respond({ locale: locale });
                        })
                        .catch(function (e) {
                            console.error('[Y8] getPlatformLocale error:', e);
                            // Fallback to 'en' — matches SDK behaviour for unrecognised subdomains
                            respond({ locale: 'en' });
                        });
                    break;


                default:
                    console.warn('[Y8] Unknown request: ' + request);
                    respond({});
                    break;
            }
        }
    });