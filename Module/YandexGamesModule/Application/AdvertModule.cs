using DefaultNamespace.Domain.Commands;
using GameSharp.Domain.Commands;
using GameSharp.Domain.Wrapper;
using GameSharp.Module.YandexGamesModule.Domain;

namespace GameSharp.Module.YandexGamesModule.Application
{
    public class AdvertModule : IAdvertModule
    {
        public Command<bool> ShowInterstitialAdvert => new ProxyCommand<bool>(AdvertProxy.ShowInterstitialAdvert, true);
        public Command<bool> ShowRewardAdvert => new ProxyCommand<bool>(AdvertProxy.ShowRewardAdvert, true);
        public Command<bool> ShowStickyBanner => new ProxyCommand<bool>(AdvertProxy.ShowStickyBanner, true);
        public Command<bool> HideStickyBanner => new ProxyCommand<bool>(AdvertProxy.HideStickyBanner, true);
    }
}