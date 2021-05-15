using RuriLib.ViewModels;

namespace OpenBullet.ViewModels
{
    public class SilverZoneViewModel : ViewModelBase
    {
        private string supportersBadge;
        public string SupportersBadge
        {
            get => supportersBadge;
            set { supportersBadge = value; OnPropertyChanged(); }
        }

        private string verifiedMarketBadge;
        public string VerifiedMarketBadge
        {
            get => verifiedMarketBadge;
            set { verifiedMarketBadge = value; OnPropertyChanged(); }
        }

    }
}
