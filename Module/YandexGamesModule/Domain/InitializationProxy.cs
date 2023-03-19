using System;
using System.Runtime.InteropServices;
using AOT;
using GameSharp.Domain;
using GameSharp.Domain.Models;

namespace GameSharp.Module.YandexGamesModule.Domain
{
    public class InitializationProxy
    {
        private static readonly CommandCallback<bool> _callback = new();
        
        public static bool IsInitialized() => IsInitializedAsk();
        
        public static void Initialize(string guid, Action<ActionResponse<bool>> callback)
        {
            _callback.AddCallback(guid, callback);
            InitializeInvoke(guid, OnReceiveCallback);
        }
        
        [DllImport("__Internal")]
        private static extern bool IsInitializedAsk();
        
        [DllImport("__Internal")]
        private static extern bool InitializeInvoke(string guid, Action<string> callback);
        
        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnReceiveCallback(string data)
        {
            _callback.SendCallbacks(data, out var guid);
            _callback.RemoveCallback(guid);
        }
    }
}