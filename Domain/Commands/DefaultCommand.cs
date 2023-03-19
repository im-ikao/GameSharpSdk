using System;
using System.Collections.Generic;
using GameSharp.Domain.Models;

namespace GameSharp.Domain.Commands
{
    public abstract class DefaultCommand<T> : Command<T>
    {
        protected readonly string _id = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("/", "_")
            .Replace("+", "-")
            .Substring(0, 8);
        
        private bool _throwIfNotSupported = false;
        
        private bool _isForceResult = false;
        private ActionResult _forceResult = ActionResult.None;
        private T _forceResultData;

        private readonly Dictionary<ActionResult, List<Action<T>>> _callbacks;
        protected DefaultCommand(Dictionary<ActionResult, List<Action<T>>> callbacks)
        {
            _callbacks = callbacks;
        }

        public override void Execute()
        {
            if (_throwIfNotSupported == true && IsSupported() == false)
                throw new NotSupportedException();
            
            if (_isForceResult == true)
            {
                SendCallbacks(_forceResult, _forceResultData);
                return;
            }
            
            if (IsSupported() == false)
                return;

            ExecuteProcess();
        }

        public override Command<T> AddCallback(ActionResult result, Action<T> callback)
        {
            if (_callbacks.ContainsKey(result) == false)
                throw new ArgumentException();
            
            _callbacks[result].Add(callback);
            return this;
        }

        public override Command<T> RemoveCallback(ActionResult result, Action<T> callback)
        {
            if (_callbacks.ContainsKey(result) == false)
                throw new ArgumentException();
            
            if (_callbacks[result].Contains(callback) == false)
                throw new ArgumentException();

            _callbacks[result].Remove(callback);
            return this;
        }

        public override Command<T> SetForceResultData(ActionResult result, T data)
        {
            if (_callbacks.ContainsKey(result) == false)
                throw new ArgumentException();
            
            _isForceResult = true;
            _forceResult = result;
            _forceResultData = data; 
            return this;
        }

        private void SendCallbacks(ActionResult result, T data)
        {
            if (_isForceResult == true)
            {
                result = _forceResult;
                data = _forceResultData;
            }
            
            if (_callbacks.ContainsKey(result) == false)
                throw new ArgumentException();

            foreach (var callback in _callbacks[result])
                callback?.Invoke(data);
        }

        protected void GetCallback(ActionResponse<T> response)
        {
            SendCallbacks(response.Result, response.Content);
        }
        
        public override Command<T> ThrowIfNotSupported()
        {
            _throwIfNotSupported = true;
            return this;
        }
    }
}