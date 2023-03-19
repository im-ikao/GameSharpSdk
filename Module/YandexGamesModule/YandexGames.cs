using GameSharp.Module.YandexGamesModule.Application;
using GameSharp.Domain;
using GameSharp.Domain.Wrapper;

namespace GameSharp.Module.YandexGamesModule
{
    public class YandexGames : IModule
    {
        public IAdvertModule Advert => new AdvertModule();
        public IAccountModule Account => new AccountModule();
        public IInitializationModule Initialization => new InitializationModule();
    }
}