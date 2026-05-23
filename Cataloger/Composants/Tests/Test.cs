using BaseWinform.Attributes;
using Cataloger.Views;
using Cataloger.Composantes;

namespace Cataloger.Composants.Tests
{
    [WinformURL("Tests")]
    public partial class Test : CatalogerComposante, ITestView
    {
        public Test()
        {
            InitializeComponent();
        }
    }
}
