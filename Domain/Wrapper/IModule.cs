using GameSharp.Domain.Wrapper;

namespace GameSharp.Domain
{
    public interface IModule
    {
        public IAdvertModule Advert { get; }
        public IAccountModule Account { get; }
        public IInitializationModule Initialization { get; }
    }
}