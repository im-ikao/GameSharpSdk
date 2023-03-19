using System;
using System.Runtime.InteropServices;
using AOT;
using GameSharp.Domain;
using GameSharp.Domain.Models;
using GameSharp.Domain.Models.Entities;

namespace GameSharp.Module.YandexGamesModule.Domain
{
    public static class AccountProxy
    {
        private static readonly CommandCallback<string> _stringCallback = new();
        private static readonly CommandCallback<Profile> _profileCallback = new();

        public static bool IsAuthenticated() => IsAuthenticatedAsk();
        
        public static bool HasProfileAccess() => HasProfileAccessAsk();
        
        public static void Authenticate(string guid, Action<ActionResponse<string>> callback)
        {
            _stringCallback.AddCallback(guid, callback);
            AuthenticateInvoke(guid, OnReceiveCallback);
        }
        
        public static void RequestPermissions(string guid, Action<ActionResponse<string>> callback)
        {
            _stringCallback.AddCallback(guid, callback);
            RequestPermissions(guid, OnReceiveCallback);
        }
        
        public static void RequestProfile(string guid, Action<ActionResponse<Profile>> callback)
        {
            _profileCallback.AddCallback(guid, callback);
            RequestProfile(guid, OnReceiveCallback);
        }

        [DllImport("__Internal")]
        private static extern bool IsAuthenticatedAsk();
        
        [DllImport("__Internal")]
        private static extern bool HasProfileAccessAsk();
        
        [DllImport("__Internal")]
        private static extern bool AuthenticateInvoke(string guid, Action<string> callback);
        
        [DllImport("__Internal")]
        private static extern bool RequestPermissions(string guid, Action<string> callback);
        
        [DllImport("__Internal")]
        private static extern bool RequestProfile(string guid, Action<string> callback);
        
        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnReceiveCallback(string data)
        {
            _stringCallback.SendCallbacks(data, out var guid);
            _stringCallback.RemoveCallback(guid);
            
            _profileCallback.SendCallbacks(data, out guid);
            _profileCallback.RemoveCallback(guid);
        }
    }
}