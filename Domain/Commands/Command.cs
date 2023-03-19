using System;
using GameSharp.Domain.Models;

namespace GameSharp.Domain.Commands
{
    public abstract class Command<T>
    {
        public abstract bool IsSupported();
        public abstract void Execute();
        protected abstract void ExecuteProcess();
        public abstract Command<T> AddCallback(ActionResult result, Action<T> callback);
        public abstract Command<T> RemoveCallback(ActionResult result, Action<T> callback);
        public abstract Command<T> SetForceResultData(ActionResult result, T data);
        public abstract Command<T> ThrowIfNotSupported();
    }
}