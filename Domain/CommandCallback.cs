using System;
using System.Collections.Generic;
using GameSharp.Domain.Models;
using UnityEngine;

namespace GameSharp.Domain
{
    public class CommandCallback<T>
    {
        private readonly Dictionary<string, Action<ActionResponse<T>>> _callbacks = new();

        public void AddCallback(string guid, Action<ActionResponse<T>> action)
        {
            if (_callbacks.ContainsKey(guid) == true)
                throw new ArgumentException();
            
            _callbacks.Add(guid, action);
        }

        public void RemoveCallback(string guid)
        {
            if (_callbacks.ContainsKey(guid) == false)
                return;
            
            _callbacks.Remove(guid);
        }
        
        public void SendCallbacks(string jsonResponse, out string id)
        {
            var response = JsonUtility.FromJson<ActionResponse<T>>(jsonResponse);
            id = response.Id;
            
            if (response.IsValid() == false)
                return;
            
            if (_callbacks.ContainsKey(response.Id) == false)
                return;

            _callbacks[response.Id]?.Invoke(response);
        }
    }
}