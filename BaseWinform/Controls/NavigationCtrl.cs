using BaseWinform.Interfaces;
using BaseWinform.Services;

namespace BaseWinform.Controls
{
    public partial class NavigationCtrl : BaseCtrl, INavigationCtrl
    {
        public NavigationService? NavigationService { get; set; }

        public NavigationCtrl() : base()
        {
            InitializeComponent();
        }
    }
}
