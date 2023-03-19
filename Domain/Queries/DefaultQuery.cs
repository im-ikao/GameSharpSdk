using System;

namespace GameSharp.Domain.Queries
{
    public abstract class DefaultQuery<T> : Query<T>
    {
        private bool _throwIfNotSupported = false;

        public override T Ask()
        {
            if (_throwIfNotSupported == true && IsSupported() == false)
                throw new NotSupportedException();
            
            if (IsSupported() == false)
                return default;
            
            return AskProcess();
        }

        public override Query<T> ThrowIfNotSupported()
        {
            _throwIfNotSupported = true;
            return this;
        }
    }
}