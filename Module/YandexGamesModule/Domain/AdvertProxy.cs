using System;
using System.Runtime.InteropServices;
using AOT;
using GameSharp.Domain;
using GameSharp.Domain.Models;

namespace GameSharp.Module.YandexGamesModule.Domain
{
    public static class AdvertProxy
    {
        private static readonly CommandCallback<bool> _callback = new();

        public static void ShowInterstitialAdvert(string guid, Action<ActionResponse<bool>> callback)
        {
            _callback.AddCallback(guid, callback);
            ShowInterstitialAdvertInvoke(guid, OnReceiveCallback);
        }
        
        public static void ShowRewardAdvert(string guid, Action<ActionResponse<bool>> callback)
        {
            _callback.AddCallback(guid, callback);
            ShowRewardAdvertInvoke(guid, OnReceiveCallback);
        }
        
        public static void ShowStickyBanner(string guid, Action<ActionResponse<bool>> callback)
        {
            _callback.AddCallback(guid, callback);
            ShowStickyBannerInvoke(guid, OnReceiveCallback);
        }
        
        public static void HideStickyBanner(string guid, Action<ActionResponse<bool>> callback)
        {
            _callback.AddCallback(guid, callback);
            HideStickyBannerInvoke(guid, OnReceiveCallback);
        }
        
        [DllImport("__Internal")]
        private static extern bool ShowInterstitialAdvertInvoke(string guid, Action<string> callback);
        
        [DllImport("__Internal")]
        private static extern bool ShowRewardAdvertInvoke(string guid, Action<string> callback);
        
        [DllImport("__Internal")]
        private static extern bool ShowStickyBannerInvoke(string guid, Action<string> callback);
        
        [DllImport("__Internal")]
        private static extern bool HideStickyBannerInvoke(string guid, Action<string> callback);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnReceiveCallback(string data)
        {
            _callback.SendCallbacks(data, out var guid);
            _callback.RemoveCallback(guid);
        }
    }
}