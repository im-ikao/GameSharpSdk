using GameSharp.Domain;
using GameSharp.Domain.Wrapper;
using GameSharp.Module.YandexGamesModule;

namespace GameSharp
{
    public static class GameSharp
    {
        private static IModule _platform = new YandexGames();

        public static IAdvertModule Advert => _platform.Advert;
        public static IAccountModule Account => _platform.Account;
        public static IInitializationModule Initialization => _platform.Initialization;
    }
}