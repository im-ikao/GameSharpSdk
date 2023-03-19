using GameSharp.Domain.Commands;
using GameSharp.Domain.Models.Entities;
using GameSharp.Domain.Queries;

namespace GameSharp.Domain.Wrapper
{
    public interface IAccountModule
    {
        public Query<bool> IsAuthenticated { get; }
        public Query<bool> HasProfileAccess { get; }
        public Command<string> Authenticate { get; }
        public Command<string> RequestPermissions { get; }
        public Command<Profile> RequestProfile { get; }

    }
}