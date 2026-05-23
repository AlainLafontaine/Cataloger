using BaseWinform.Controls;

namespace Cataloger.Controls
{
    public partial class LeftSideToolbar : BaseCtrl
    {
        public LeftSideToolbar()
        {
            InitializeComponent();
            customBorder.Top.Visible = true;
            customBorder.Right.Visible = true;
        }

        private void LeftSideToolbar_Load(object sender, EventArgs e)
        {

        }
    }
}
