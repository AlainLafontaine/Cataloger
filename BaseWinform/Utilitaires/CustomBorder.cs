using DevExpress.XtraEditors;

namespace BaseWinform.Utilitaires
{
    public class CustomBorder
    {
        private enum PositionBorder { Left, Top, Right, Bottom }

        private SeparatorControl? _top = null;
        private SeparatorControl? _left = null;
        private SeparatorControl? _right = null;
        private SeparatorControl? _bottom = null;

        private Control parent;

        public SeparatorControl Top { 
            get {
                if (_top == null) _top = CreeBorder(PositionBorder.Top);
                return _top;
            } 
        }

        public SeparatorControl Right
        {
            get
            {
                if (_right == null) _right = CreeBorder(PositionBorder.Right);
                return _right;
            }
        }

        public SeparatorControl Bottom
        {
            get
            {
                if (_bottom == null) _bottom = CreeBorder(PositionBorder.Bottom);
                return _bottom;
            }
        }

        public SeparatorControl Left
        {
            get
            {
                if (_left == null) _left = CreeBorder(PositionBorder.Left);
                return _left;
            }
        }
        
        public CustomBorder(Control parent)
        {
            this.parent = parent;
        }

        private SeparatorControl CreeBorder(
            PositionBorder position
        )
        {
            SeparatorControl separatorCtrl = new SeparatorControl();

            switch (position)
            {
                case PositionBorder.Left:
                    separatorCtrl.Name = "leftSeparator";
                    separatorCtrl.Dock = DockStyle.Left;
                    separatorCtrl.LineOrientation = Orientation.Vertical;
                    separatorCtrl.Size = new Size(2, 100);
                    break;

                case PositionBorder.Top:
                    separatorCtrl.Name = "topSeparator";
                    separatorCtrl.Dock = DockStyle.Top;
                    separatorCtrl.LineOrientation = Orientation.Horizontal;
                    separatorCtrl.Size = new Size(100, 2);
                    break;

                case PositionBorder.Right:
                    separatorCtrl.Name = "rightSeparator";
                    separatorCtrl.Dock = DockStyle.Right;
                    separatorCtrl.LineOrientation = Orientation.Vertical;
                    separatorCtrl.Size = new Size(2, 100);
                    break;

                case PositionBorder.Bottom:
                    separatorCtrl.Name = "bottomSeparator";
                    separatorCtrl.Dock = DockStyle.Bottom;
                    separatorCtrl.LineOrientation = Orientation.Horizontal;
                    separatorCtrl.Size = new Size(100, 2);
                    break;
            }

            separatorCtrl.LineThickness = 2;
            separatorCtrl.Margin = new Padding(0);
            separatorCtrl.Padding = new Padding(0);
            separatorCtrl.Location = new Point(0, 0);
            //separatorCtrl.BackColor = SystemColors.ActiveBorder;
            //separatorCtrl.Visible = false;
            
            parent.Controls.Add(separatorCtrl);

            return separatorCtrl;
        }
    }
}
