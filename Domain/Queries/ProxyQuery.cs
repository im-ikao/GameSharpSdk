using System;

namespace GameSharp.Domain.Queries
{
    public class ProxyQuery<T> : DefaultQuery<T>
    {
        private readonly Func<T>  _action;
        private readonly bool _isSupported = false;
        
        public ProxyQuery(Func<T> action, bool isSupported) : base()
        {
            _action = action;
            _isSupported = isSupported;
        }

        public override bool IsSupported()
        {
            return _isSupported;
        }

        protected override T AskProcess()
        {
            if (_action == null)
                throw new NullReferenceException();

            return _action();
        }
    }
}