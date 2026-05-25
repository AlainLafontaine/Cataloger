using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cataloger.Composantes;
using Cataloger.Views;

namespace Cataloger.Composants
{
    public partial class Category : CatalogerChildComposante, ICategoryView
    {
        public Category()
        {
            InitializeComponent();
        }
    }
}
