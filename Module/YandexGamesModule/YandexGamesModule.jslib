const library = {
  
    $GameSharpSdk: {
   
        ActionResult: {
            None: 0,
            Success: 1,
            Failed: 2,
            Close: 3,
            Reward: 4,
            Offline: 5,
            NotInitialized: 6
        },
        
        InvokeCallback: function (id, content, result, callbackPtr) 
        {
            var data = new Object();
            data.Id = id;
            data.Result = result;
            data.Content = JSON.stringify(content);
            
            var dataMessage = JSON.stringify(data);
            const unmanagedStringPtr = GameSharpSdk.AllocateUnmanagedString(dataMessage);
            
            dynCall('vi', callbackPtr, [unmanagedStringPtr]);
            _free(unmanagedStringPtr);
        },
        
        AllocateUnmanagedString: function (string) 
        {
              const stringBufferSize = lengthBytesUTF8(string) + 1;
              const stringBufferPtr = _malloc(stringBufferSize);
              stringToUTF8(string, stringBufferPtr, stringBufferSize);
              return stringBufferPtr;
        },
    },
    
    $YandexGamesModule: {
        
        isInitialized: false,
        isAuthenticated: false,
        isInitializeCalled: false,
        sdk: undefined,
        playerAccount: undefined,
    
        // Internal JavaScript calls.
        // Initialization calls start.
        Initialize: function (guid, callback) {

              if (YandexGamesModule.isInitializeCalled) {
                return;
              }

              YandexGamesModule.isInitializeCalled = true;
        
              const sdkScript = document.createElement('script');
              sdkScript.src = 'https://yandex.ru/games/sdk/v2';
              document.head.appendChild(sdkScript);
        
              sdkScript.onload = function () {
                window['YaGames'].init().then(function (sdk) {
                  YandexGamesModule.sdk = sdk;
                  YandexGamesModule.isInitialized = true;
                  GameSharpSdk.InvokeCallback(guid, true, GameSharpSdk.ActionResult.Success, callback);
                });
              }
            },
        
        ThrowIfSdkNotInitialized: function (guid, callback) {
          if (!YandexGamesModule.isInitialized) {
            console.error('SDK is not initialized. Invoke Initialize() and wait for it to finish.');
            GameSharpSdk.InvokeCallback(guid, true, GameSharpSdk.ActionResult.NotInitialized, callback);
          }
        },
        
        ThrowIfNotAuthenticated: function (guid, callback) {
          if (!YandexGamesModule.isAuthorized) {
            console.error('is need Authorize');
            GameSharpSdk.InvokeCallback(guid, false, GameSharpSdk.ActionResult.Failed, callback);
            return true;
          }
          return false;
        },
        
        Authenticate: function (guid, callback) {
              if (YandexGamesModule.isAuthorized) {
                console.error('Already authorized.');
                GameSharpSdk.InvokeCallback(guid, true, GameSharpSdk.ActionResult.Success, callback);
                return;
              }
        
              YandexGamesModule.sdk.auth.openAuthDialog().then(function () {
                YandexGamesModule.sdk.getPlayer({ scopes: false }).then(function (playerAccount) {
                  YandexGamesModule.isAuthorized = true;
                  YandexGamesModule.playerAccount = playerAccount;
                  GameSharpSdk.InvokeCallback(guid, true, GameSharpSdk.ActionResult.Success, callback);
                }).catch(function (error) {
                  console.error('authorize failed to update playerAccount. Assuming authorization failed. Error was: ' + error.message);
                  GameSharpSdk.InvokeCallback(guid, true, GameSharpSdk.ActionResult.Failed, callback);
                });
              }).catch(function (error) {
                  console.error('error was:' + error.message);
                  GameSharpSdk.InvokeCallback(guid, true, GameSharpSdk.ActionResult.Failed, callback);
              });
            },
            
        HasProfileAccess: function (guid, callback) {
              if (!YandexGamesModule.isAuthorized) {
                console.error('getPlayerAccountHasPersonalProfileDataPermission requires authorization. Assuming profile data permissions were not granted.');
                return false;
              }
        
              var publicNamePermission = undefined;
              if ('_personalInfo' in YandexGamesModule.playerAccount && 'scopePermissions' in YandexGamesModule.playerAccount._personalInfo) {
                publicNamePermission = YandexGamesModule.playerAccount._personalInfo.scopePermissions.public_name;
              }
        
              switch (publicNamePermission) {
                case 'forbid':
                  return false;
                case 'not_set':
                  return false;
                case 'allow':
                  return true;
                default:
                  console.error('Unexpected response from Yandex. Assuming profile data permissions were not granted. playerAccount = '
                    + JSON.stringify(YandexGamesModule.playerAccount));
                  return false;
              }
            },
        
        RequestPermissions: function (guid, callback) {
              if (YandexGamesModule.ThrowIfNotAuthenticated(guid, callback)) {
                console.error('playerAccountRequestPersonalProfileDataPermission requires authorization. Assuming profile data permissions were not granted.');
                return;
              }
        
              YandexGamesModule.sdk.getPlayer({ scopes: true }).then(function (playerAccount) {
                YandexGamesModule.playerAccount = playerAccount;
        
                if (YandexGamesModule.HasProfileAccess(guid, callback)) {
                  GameSharpSdk.InvokeCallback(guid, true, GameSharpSdk.ActionResult.Success, callback);
                } else {
                  console.error('User has refused the permission request');
                  GameSharpSdk.InvokeCallback(guid, "User has refused the permission request.", GameSharpSdk.ActionResult.Failed, callback);
                }
              }).catch(function (error) {
                  GameSharpSdk.InvokeCallback(guid, false, GameSharpSdk.ActionResult.Failed, callback);
              });
            },
    
        RequestProfile: function (guid, callback) {
              if (YandexGamesModule.ThrowIfNotAuthenticated(guid, callback)) {
                console.error('playerAccountRequestPersonalProfileDataPermission requires authorization. Assuming profile data permissions were not granted.');
                return;
              }
              YandexGamesModule.sdk.getPlayer({ scopes: false }).then(function (playerAccount) {
                YandexGamesModule.playerAccount = playerAccount;
                console.log(JSON.stringify(playerAccount._personalInfo));
                const profileDataJson = JSON.stringify(playerAccount._personalInfo);
                GameSharpSdk.InvokeCallback(guid, profileDataJson, GameSharpSdk.ActionResult.Success, callback);
              }).catch(function (error) {
                GameSharpSdk.InvokeCallback(guid, false, GameSharpSdk.ActionResult.Failed, callback);
              });
            },
        // Initialization calls end.
        // Advert calls start.
        ShowInterstitialAdvert: function (guid, callback) {
            YandexGamesModule.sdk.adv.showFullscreenAdv({
                    callbacks: {
                      onOpen: function () {
                        GameSharpSdk.InvokeCallback(guid, true, GameSharpSdk.ActionResult.Success, callback);
                      },
                      onClose: function (wasShown) {
                        GameSharpSdk.InvokeCallback(guid, wasShown, GameSharpSdk.ActionResult.Close, callback);
                      },
                      onError: function (error) {
                        GameSharpSdk.InvokeCallback(guid, false, GameSharpSdk.ActionResult.Failed, callback);
                      },
                      onOffline: function () {
                        GameSharpSdk.InvokeCallback(guid, false, GameSharpSdk.ActionResult.Offline, callback);
                      },
                    }
            });
        },
            
        ShowRewardAdvert: function (guid, callback) {
            YandexGamesModule.sdk.adv.showRewardedVideo({
                    callbacks: {
                      onOpen: function () {
                        GameSharpSdk.InvokeCallback(guid, true, GameSharpSdk.ActionResult.Success, callback);
                      },
                      onRewarded: function () {
                        GameSharpSdk.InvokeCallback(guid, false, GameSharpSdk.ActionResult.Reward, callback);
                      },
                      onClose: function () {
                        GameSharpSdk.InvokeCallback(guid, wasShown, GameSharpSdk.ActionResult.Close, callback);
                      },
                      onError: function (error) {
                        GameSharpSdk.InvokeCallback(guid, false, GameSharpSdk.ActionResult.Failed, callback);
                      },
                    }
            });
        },
        
        ShowStickyBanner: function (guid, callback) {
            YandexGamesModule.sdk.adv.showBannerAdv();
        },
        
        HideStickyBanner: function (guid, callback) {
            YandexGamesModule.sdk.adv.hideBannerAdv();
        },
        // Advert calls end.
    },

    // External C# calls.
    // Advert calls start.
    InitializeInvoke: function (guid, callback) {
        YandexGamesModule.Initialize(UTF8ToString(guid), callback);
    },
        
    // Advert calls.
    ShowInterstitialAdvertInvoke: function (guid, callback) {
        YandexGamesModule.ThrowIfSdkNotInitialized(UTF8ToString(guid), callback);
        YandexGamesModule.ShowInterstitialAdvert(UTF8ToString(guid), callback);
    },
        
    ShowRewardAdvertInvoke: function (guid, callback) {
        YandexGamesModule.ThrowIfSdkNotInitialized(UTF8ToString(guid), callback);
        YandexGamesModule.ShowRewardAdvert(UTF8ToString(guid), callback);
    },
    
    ShowStickyBannerInvoke: function (guid, callback) {
        YandexGamesModule.ThrowIfSdkNotInitialized(UTF8ToString(guid), callback);
        YandexGamesModule.ShowStickyBanner(UTF8ToString(guid), callback);
    },
    
    HideStickyBannerInvoke: function (guid, callback) {
        YandexGamesModule.ThrowIfSdkNotInitialized(UTF8ToString(guid), callback);
        YandexGamesModule.HideStickyBanner(UTF8ToString(guid), callback);
    },
    // Advert calls end.
    // Initialization calls start.
    IsInitializedAsk: function (guid, callback) {
        return YandexGamesModule.isInitialized;   
    },
    
    AuthenticateInvoke: function (guid, callback) {
        YandexGamesModule.ThrowIfSdkNotInitialized(UTF8ToString(guid), callback);
        YandexGamesModule.Authenticate(UTF8ToString(guid), callback);
    },
    
    IsAuthenticatedAsk: function (guid, callback) {
        YandexGamesModule.ThrowIfSdkNotInitialized(UTF8ToString(guid), callback);
        return YandexGamesModule.isAuthenticated;
    },
    
    HasProfileAccessAsk: function (guid, callback) {
        YandexGamesModule.ThrowIfSdkNotInitialized(UTF8ToString(guid), callback);
        YandexGamesModule.HasProfileAccess(UTF8ToString(guid), callback);
    },
    
    RequestPermissions: function (guid, callback) {
        YandexGamesModule.ThrowIfSdkNotInitialized(UTF8ToString(guid), callback);
        YandexGamesModule.RequestPermissions(UTF8ToString(guid), callback);
    },
    
    RequestProfile: function (guid, callback) {
        YandexGamesModule.ThrowIfSdkNotInitialized(UTF8ToString(guid), callback);
        YandexGamesModule.RequestProfile(UTF8ToString(guid), callback);
    },
    // Initialization calls end.
}

autoAddDeps(library, '$GameSharpSdk');
autoAddDeps(library, '$YandexGamesModule');
mergeInto(LibraryManager.library, library);
