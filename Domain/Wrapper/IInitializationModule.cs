using GameSharp.Domain.Commands;
using GameSharp.Domain.Queries;

namespace GameSharp.Domain.Wrapper
{
    public interface IInitializationModule
    {
        public Command<bool> Initialize { get; }
        public Query<bool> IsInitialized { get; }

    }
}