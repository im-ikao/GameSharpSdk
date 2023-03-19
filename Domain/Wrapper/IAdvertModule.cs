using GameSharp.Domain.Commands;

namespace GameSharp.Domain.Wrapper
{
    public interface IAdvertModule
    {
        public Command<bool> ShowInterstitialAdvert { get; }
        public Command<bool> ShowRewardAdvert { get; }
        public Command<bool> ShowStickyBanner { get; }
        public Command<bool> HideStickyBanner { get; }
    }
}