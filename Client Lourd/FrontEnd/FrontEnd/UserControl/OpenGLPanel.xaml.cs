using System;
using System.Windows.Forms;
using FrontEnd.Core;
using FrontEnd.UserControl.Winform;

namespace FrontEnd.UserControl
{
    /// <summary>
    /// Interaction logic for OpenGLPanel.xaml
    /// </summary>
    public partial class OpenGlPanel
    {
        private IntegratedOpenGl _integratedOpenGl;

        public IntegratedOpenGl.Mode CurrentMode => _integratedOpenGl.CurrentMode;

        public OpenGlPanel()
        {
            InitializeComponent();
            CreateWinformPanel();
        }

        /// <summary>
        /// Create the winform user control and attach it to this wpf control
        /// </summary>
        private void CreateWinformPanel()
        {
            _integratedOpenGl = new IntegratedOpenGl();
            _integratedOpenGl.FormBorderStyle = FormBorderStyle.Sizable;
            _integratedOpenGl.Show();

            // set label reference
            PointJoueur1Label = _integratedOpenGl.Player1PointLabel;
            PointJoueur2Label = _integratedOpenGl.Player2PointLabel;
            PointJoueur3Label = _integratedOpenGl.Player3PointLabel;
            PointJoueur4Label = _integratedOpenGl.Player4PointLabel;

            BallJoueur1Label = _integratedOpenGl.Player1BallLabel;
            BallJoueur2Label = _integratedOpenGl.Player2BallLabel;
            BallJoueur3Label = _integratedOpenGl.Player3BallLabel;
            BallJoueur4Label = _integratedOpenGl.Player4BallLabel;

            BallsPanel = _integratedOpenGl.ballPanel;
            PointsPanel = _integratedOpenGl.pointPanel;
        }

        public Label PointJoueur1Label { get; set; }
        public Label PointJoueur2Label { get; set; }
        public Label PointJoueur3Label { get; set; }
        public Label PointJoueur4Label { get; set; }


        public Label BallJoueur1Label { get; set; }
        public Label BallJoueur2Label { get; set; }
        public Label BallJoueur3Label { get; set; }
        public Label BallJoueur4Label { get; set; }

        public Panel BallsPanel { get; set; }
        public Panel PointsPanel { get; set; }

        public void HideWinform()
        {
            _integratedOpenGl.Hide();
        }

        public void Dispose()
        {
            if (_integratedOpenGl != null)
            {
                try
                {
                    _integratedOpenGl.Dispose();
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Select the mode in which the opengl windows should run
        /// </summary>
        /// <param name="mode"></param>
        public void SelectMode(IntegratedOpenGl.Mode mode)
        {
            if (_integratedOpenGl == null)
            {
                throw new NullReferenceException("The winform integrated open gl user control is not created");
            }
            if (mode != IntegratedOpenGl.Mode.ModeTest && mode != IntegratedOpenGl.Mode.ModeEditeur)
            {
                PointsPanel.Visible = true;
                BallsPanel.Visible = true;
            }
            else
            {
                PointsPanel.Visible = false;
                BallsPanel.Visible = false;
            }
            _integratedOpenGl.AssignerMode(mode);
            _integratedOpenGl.AssignerEstTerminee(false);
            _integratedOpenGl.Show();
            _integratedOpenGl.MettreAJour();
        }

        public void Run(double elapsedTime)
        {
            _integratedOpenGl?.Animer(elapsedTime);
        }
    }
}
