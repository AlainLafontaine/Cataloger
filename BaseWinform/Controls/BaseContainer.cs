using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseWinform.Controls
{
    public partial class BaseContainer : DevExpress.XtraEditors.GroupControl
    {
        public BaseContainer() 
        {
            InitializeComponent();

            this.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.Appearance.BorderColor = Color.Transparent;
            this.Text = " ";
        }
    }
}
