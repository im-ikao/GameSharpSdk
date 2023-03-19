namespace GameSharp.Domain.Queries
{
    public abstract class Query<T>
    {
        public abstract bool IsSupported();
        public abstract T Ask();
        protected abstract T AskProcess();
        public abstract Query<T> ThrowIfNotSupported();
    }
}