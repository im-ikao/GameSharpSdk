using DefaultNamespace.Domain.Commands;
using GameSharp.Domain.Commands;
using GameSharp.Domain.Models.Entities;
using GameSharp.Domain.Queries;
using GameSharp.Domain.Wrapper;
using GameSharp.Module.YandexGamesModule.Domain;

namespace GameSharp.Module.YandexGamesModule.Application
{
    public class AccountModule : IAccountModule
    {
        public Query<bool> IsAuthenticated => new ProxyQuery<bool>(AccountProxy.IsAuthenticated, true);
        public Query<bool> HasProfileAccess => new ProxyQuery<bool>(AccountProxy.HasProfileAccess, true);
        public Command<string> Authenticate => new ProxyCommand<string>(AccountProxy.Authenticate, true);
        public Command<string> RequestPermissions => new ProxyCommand<string>(AccountProxy.RequestPermissions, true);
        public Command<Profile> RequestProfile => new ProxyCommand<Profile>(AccountProxy.RequestProfile, true);
    }
}