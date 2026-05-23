using BaseWinform.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cataloger.Controls
{
    public partial class StatutBar : BaseCtrl
    {
        public StatutBar()
        {
            InitializeComponent();
            customBorder.Top.Visible = true;
        }
    }
}
