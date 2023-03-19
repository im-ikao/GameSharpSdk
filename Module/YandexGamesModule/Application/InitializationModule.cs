using DefaultNamespace.Domain.Commands;
using GameSharp.Domain.Commands;
using GameSharp.Domain.Queries;
using GameSharp.Domain.Wrapper;
using GameSharp.Module.YandexGamesModule.Domain;

namespace GameSharp.Module.YandexGamesModule.Application
{
    public class InitializationModule : IInitializationModule
    {
        public Command<bool> Initialize => new ProxyCommand<bool>(InitializationProxy.Initialize, true);
        public Query<bool> IsInitialized => new ProxyQuery<bool>(InitializationProxy.IsInitialized, true);
    }
}