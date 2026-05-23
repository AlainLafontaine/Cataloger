using BaseWinform.Entites;
using DevExpress.Utils;
using DevExpress.Utils.Win;
using DevExpress.XtraEditors;
using Zzz.App.Core;

namespace BaseWinform.Utilitaires
{
    public sealed class WinformNotifierManager
    {
        public enum TypeMessage { }
        
        private readonly Form mainForm;
        private readonly List<FlyoutPanel> active = new();
        private readonly int width;
        private readonly int height;
        private readonly int spacing;
        private readonly int autoHideMs;

        public WinformNotifierManager(Form mainForm, int width = 420, int height = 100, int spacing = 8, int autoHideMs = 4000)
        {
            this.mainForm = mainForm;
            this.width = width;
            this.height = height;
            this.spacing = Math.Max(0, spacing);
            this.autoHideMs = Math.Max(0, autoHideMs);

            // Repositionner les flyouts si le Form est déplacé/redimensionné
            mainForm.Move += (_, __) => RepositionAll();
            mainForm.SizeChanged += (_, __) => RepositionAll();
        }

        public void Show(WinformActionMessage message, string? title = null)
        {
            // Créer un FlyoutPanel distinct par notification
            var flyout = new FlyoutPanel
            {
                OwnerControl = mainForm,
                Size = new Size(width, height),
            };

            // Contenu (simple) : un panneau + 2 labels
            var panel = new PanelControl { Dock = DockStyle.Fill, BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder };
            var lblTitle = new LabelControl
            {
                Dock = DockStyle.Top,
                Padding = new Padding(12, 10, 12, 0),
                Appearance = { Font = new Font("Tahoma", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0) },
            };
            var lblMsg = new LabelControl
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12, 6, 12, 12),
                Appearance = { Font = new Font("Tahoma", 14.25F) },
                AutoEllipsis = true,
                AllowHtmlString = true,
                AutoSizeMode = LabelAutoSizeMode.None,
                Size = new Size(200, 0),
                Text = message.msg
            };

            lblMsg.Appearance.TextOptions.WordWrap = WordWrap.Wrap;

            panel.Controls.Add(lblMsg);
            panel.Controls.Add(lblTitle);
            flyout.Controls.Add(panel);

            // Options d’ancrage/animation
            flyout.Options.AnchorType = PopupToolWindowAnchor.Manual;               // relatif au Form
            flyout.Options.AnimationType = PopupToolWindowAnimation.Slide;          // slide depuis le bas
            flyout.Options.CloseOnOuterClick = true;                                // optionnel

            var success = Color.FromArgb(25, 135, 84);      // #198754
            var danger = Color.FromArgb(220, 53, 69);       // #dc3545
            var warning = Color.FromArgb(255, 193, 7);      // #ffc107
            var info = Color.FromArgb(13, 202, 240);        // #0dcaf0

            switch (message.type)
            {
                case ConstantesNoyau.ActionMsgType.danger:
                    lblTitle.Text = title ?? "Notification: Danger";
                    flyout.BackColor = danger;
                    flyout.ForeColor = Color.White;
                    break;

                case ConstantesNoyau.ActionMsgType.info:
                    lblTitle.Text = title ?? "Notification: Information";
                    flyout.BackColor = info;
                    flyout.ForeColor = Color.White;
                    break;

                case ConstantesNoyau.ActionMsgType.success:
                    lblTitle.Text = title ?? "Notification: Succès";
                    flyout.BackColor = success;
                    flyout.ForeColor = Color.White;
                    break;

                case ConstantesNoyau.ActionMsgType.warning:
                    lblTitle.Text = title ?? "Notification: Avertissement";
                    flyout.BackColor = warning;
                    flyout.ForeColor = Color.Black;
                    break;
            }
            
            // Timer d’auto-fermeture
            var timer = new System.Windows.Forms.Timer { Interval = autoHideMs };

            timer.Tick += (_, __) =>
            {
                timer.Stop();
                timer.Dispose();
                flyout.HidePopup();
            };

            // Nettoyage et empilement dynamiques
            flyout.Hidden += (_, __) =>
            {
                flyout.Dispose();
                active.Remove(flyout);
                RepositionAll();
            };

            // Ajouter le contrôle dans la collecgtion
            active.Add(flyout);
            
            // Repositionner tout l’empilement (y compris ce nouveau)
            RepositionAll();
            
            // Afficher le message
            flyout.ShowPopup();

            // Démarer le timer
            if (autoHideMs > 0) timer.Start();
        }

        private void RepositionAll()
        {
            // Rectangle client du Form en coordonnées écran (gère DPI/bordures)
            //var rcClientOnScreen = mainForm.RectangleToScreen(mainForm.ClientRectangle);
            var rcClientOnScreen = mainForm.ClientRectangle;

            // On positionne de bas en haut : la dernière notifiée reste en bas, les précédentes montent
            int currentBottom = rcClientOnScreen.Bottom - spacing;

            foreach (var fly in active.OrderByDescending(f => f.Top)) // l'ordre visuel n’importe pas, on va tout recaler
            {
                var x = rcClientOnScreen.Right - width - spacing;
                var y = currentBottom - height;
                fly.Options.Location = new Point(x, y);
                fly.SetBounds(x, y, width, height);
                currentBottom = y - spacing;
            }
        }

        public void Dispose()
        {
            foreach (var f in active.ToList())
            {
                try { f.HidePopup(); } catch { /* ignore */ }
                f.Dispose();
            }
            active.Clear();

            
            if (mainForm != null)
            {
                mainForm.Move -= (_, __) => RepositionAll();       // NB: lambdas anonymes ne peuvent pas être détachées ainsi;
                mainForm.SizeChanged -= (_, __) => RepositionAll(); // si besoin, stocker des handlers nommés pour les détacher proprement.
            }
        }
    }
}