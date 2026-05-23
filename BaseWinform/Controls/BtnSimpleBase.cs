using System.ComponentModel;
using DevExpress.XtraEditors;

namespace BaseWinform.Controls
{
    public enum TypeBtnKind
    {
        none,

        // Pleins
        btn_primary,
        btn_secondary,
        btn_success,
        btn_danger,
        btn_warning,
        btn_info,
        btn_light,
        btn_dark,

        // Outline
        btn_outline_primary,
        btn_outline_secondary,
        btn_outline_success,
        btn_outline_danger,
        btn_outline_warning,
        btn_outline_info,
        btn_outline_light,
        btn_outline_dark
    }

    public partial class BtnSimpleBase : SimpleButton
    {

        private TypeBtnKind _typeBtn = TypeBtnKind.none;

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Design")]
        [Description("Type du bouton (style Bootstrap)")]
        public TypeBtnKind TypeBtn
        {
            get => _typeBtn;
            set
            {
                if (_typeBtn != value)
                {
                    _typeBtn = value;
                    ApplyBootstrapStyle();
                    Invalidate(); // Redessiner si nécessaire
                }
            }
        }

        public BtnSimpleBase()
        {
            InitializeComponent();
            ApplyBootstrapStyle();
        }

        private void ApplyBootstrapStyle()
        {
            // On force un rendu custom
            this.LookAndFeel.UseDefaultLookAndFeel = false;

            // Réinitialise les apparences
            this.Appearance.Reset();
            this.AppearanceHovered.Reset();
            this.AppearancePressed.Reset();
            this.AppearanceDisabled.Reset();

            // Style de bordure plat
            this.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;

            // Rayon (coins arrondis) façon Bootstrap
            this.Appearance.BorderColor = Color.Transparent; // On gére la bordure plus bas
            this.Appearance.Options.UseBorderColor = true;

            // Palette Bootstrap (v5)
            var primary = Color.FromArgb(13, 110, 253);     // #0d6efd
            var secondary = Color.FromArgb(108, 117, 125);  // #6c757d
            var success = Color.FromArgb(25, 135, 84);      // #198754
            var danger = Color.FromArgb(220, 53, 69);       // #dc3545
            var warning = Color.FromArgb(255, 193, 7);      // #ffc107
            var info = Color.FromArgb(13, 202, 240);        // #0dcaf0
            var light = Color.FromArgb(248, 249, 250);      // #f8f9fa
            var dark = Color.FromArgb(33, 37, 41);          // #212529

            // Par défaut
            Color bg = SystemColors.Control;
            Color fg = SystemColors.ControlText;
            Color br = Color.Transparent;
            bool isOutline = false;

            switch (TypeBtn)
            {
                // ***** STYLES PLEINS *****
                case TypeBtnKind.btn_primary:
                    bg = primary; 
                    fg = Color.White; 
                    br = primary; 
                    break;

                case TypeBtnKind.btn_secondary:
                    bg = secondary; 
                    fg = Color.White; 
                    br = secondary; 
                    break;

                case TypeBtnKind.btn_success:
                    bg = success; 
                    fg = Color.White; 
                    br = success; 
                    break;

                case TypeBtnKind.btn_danger:
                    bg = danger; 
                    fg = Color.White; 
                    br = danger; 
                    break;

                case TypeBtnKind.btn_warning:
                    bg = warning; 
                    fg = Color.Black; 
                    br = warning; 
                    break;

                case TypeBtnKind.btn_info:
                    bg = info; 
                    fg = Color.White; 
                    br = info; 
                    break;

                case TypeBtnKind.btn_light:
                    bg = light; 
                    fg = Color.Black; 
                    br = light; 
                    break;

                case TypeBtnKind.btn_dark:
                    bg = dark; 
                    fg = Color.White; 
                    br = dark; 
                    break;

                // ***** STYLES OUTLINE *****
                case TypeBtnKind.btn_outline_primary:
                    isOutline = true; 
                    bg = SystemColors.Control;
                    fg = primary; 
                    br = primary; 
                    break;

                case TypeBtnKind.btn_outline_secondary:
                    isOutline = true;
                    bg = SystemColors.Control;
                    fg = Color.Black; 
                    br = secondary; 
                    break;

                case TypeBtnKind.btn_outline_success:
                    isOutline = true; 
                    bg = SystemColors.Control;
                    fg = success; 
                    br = success; 
                    break;

                case TypeBtnKind.btn_outline_danger:
                    isOutline = true; 
                    bg = SystemColors.Control;
                    fg = danger; 
                    br = danger; 
                    break;

                case TypeBtnKind.btn_outline_warning:
                    isOutline = true; 
                    bg = SystemColors.Control;
                    fg = warning; 
                    br = warning; 
                    break;

                case TypeBtnKind.btn_outline_info:
                    isOutline = true; 
                    bg = SystemColors.Control;
                    fg = info; 
                    br = info; 
                    break;

                case TypeBtnKind.btn_outline_light:
                    isOutline = true; 
                    bg = SystemColors.Control;
                    fg = light; 
                    br = light; 
                    break;

                case TypeBtnKind.btn_outline_dark:
                    isOutline = true; 
                    bg = SystemColors.Control;
                    fg = dark; 
                    br = dark; 
                    break;

                case TypeBtnKind.none:
                default:
                    bg = SystemColors.Control; 
                    fg = SystemColors.ControlText; 
                    br = Color.Transparent; 
                    break;
            }

            // Applique état Normal
            this.Appearance.BackColor = bg;
            this.Appearance.ForeColor = fg;
            this.Appearance.BorderColor = br;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseForeColor = true;
            this.Appearance.Options.UseBorderColor = true;

            // ***** états hover / pressed *****
            if (!isOutline)
            {
                // Styles PLEINS: hover = légére assombrie, pressed = plus sombre
                var hoverBg = Darken(bg, 0.08f);
                var pressedBg = Darken(bg, 0.15f);

                this.AppearanceHovered.BackColor = hoverBg;
                this.AppearanceHovered.ForeColor = fg;
                this.AppearanceHovered.BorderColor = hoverBg;

                this.AppearancePressed.BackColor = pressedBg;
                this.AppearancePressed.ForeColor = fg;
                this.AppearancePressed.BorderColor = pressedBg;
            }
            else
            {
                // Styles OUTLINE:
                // - Normal: fond transparent, texte et bordure = couleur
                // - Hover: fond rempli avec la couleur, texte blanc (sauf light: texte noir)
                // - Pressed: encore un peu plus sombre

                var filledFg = (TypeBtn == TypeBtnKind.btn_outline_light) ? Color.Black : Color.White;

                var hoverBg = br;                   // rempli avec la couleur
                var pressedBg = Darken(br, 0.12f);  // un peu plus sombre

                this.AppearanceHovered.BackColor = hoverBg;
                this.AppearanceHovered.ForeColor = filledFg;
                this.AppearanceHovered.BorderColor = hoverBg;

                this.AppearancePressed.BackColor = pressedBg;
                this.AppearancePressed.ForeColor = filledFg;
                this.AppearancePressed.BorderColor = pressedBg;

                // Pour un rendu outline plus net, on peut forcer un fond vraiment transparent :
                this.Appearance.Options.UseBackColor = true;
            }

            // Désactivé (approx Bootstrap : assombrir et griser)
            var disabledBg = isOutline ? Color.Transparent : Lighten(bg, 0.50f);
            var disabledFg = Color.FromArgb(128, 128, 128);
            var disabledBr = isOutline ? Lighten(br, 0.50f) : Lighten(br, 0.50f);

            this.AppearanceDisabled.BackColor = disabledBg;
            this.AppearanceDisabled.ForeColor = disabledFg;
            this.AppearanceDisabled.BorderColor = disabledBr;
            this.AppearanceDisabled.Options.UseBackColor = true;
            this.AppearanceDisabled.Options.UseForeColor = true;
            this.AppearanceDisabled.Options.UseBorderColor = true;

            this.Appearance.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        }

        private static Color Darken(Color c, float amount)
        {
            // amount: 0..1
            int r = (int)Math.Round(c.R * (1 - amount));
            int g = (int)Math.Round(c.G * (1 - amount));
            int b = (int)Math.Round(c.B * (1 - amount));
            return Color.FromArgb(
                Math.Clamp(r, 0, 255),
                Math.Clamp(g, 0, 255),
                Math.Clamp(b, 0, 255)
            );
        }

        private static Color Lighten(Color c, float amount)
        {
            int r = (int)Math.Round(c.R + (255 - c.R) * amount);
            int g = (int)Math.Round(c.G + (255 - c.G) * amount);
            int b = (int)Math.Round(c.B + (255 - c.B) * amount);
            return Color.FromArgb(
                Math.Clamp(r, 0, 255),
                Math.Clamp(g, 0, 255),
                Math.Clamp(b, 0, 255)
            );
        }
    }
}