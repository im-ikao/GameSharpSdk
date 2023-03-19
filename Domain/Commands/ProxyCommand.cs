using System;
using System.Collections.Generic;
using GameSharp.Domain.Commands;
using GameSharp.Domain.Models;

namespace DefaultNamespace.Domain.Commands
{
    public class ProxyCommand<T> : DefaultCommand<T>
    {
        private readonly Action<string, Action<ActionResponse<T>>> _action;
        private readonly bool _isSupported = false;
        
        public ProxyCommand(Action<string, Action<ActionResponse<T>>> action, bool isSupported) : base(new Dictionary<ActionResult, List<Action<T>>>
        {
            { ActionResult.Failed, new List<Action<T>>() },
            { ActionResult.Success, new List<Action<T>>() },
            { ActionResult.Reward, new List<Action<T>>() },
            { ActionResult.Close, new List<Action<T>>() },
            { ActionResult.Offline, new List<Action<T>>() }
        })
        {
            _action = action;
            _isSupported = isSupported;
        }

        public override bool IsSupported()
        {
            return _isSupported;
        }

        protected override void ExecuteProcess()
        {
            if (_action == null)
                return;
            
            _action(_id, GetCallback);
        }
    }
}