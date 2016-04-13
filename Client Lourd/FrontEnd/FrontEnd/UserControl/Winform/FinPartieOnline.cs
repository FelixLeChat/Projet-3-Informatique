using System;
using System.Windows.Forms;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Game.Wrap;

namespace FrontEnd.UserControl.Winform
{
    public partial class FinPartieOnline : Form
    {

        readonly IntegratedOpenGl _fenetre;

        /// <summary>
        /// public FinPartie(fenetreOpenGL f, bool gagne)
        /// 
        /// Constructeur
        /// </summary>
        /// <param name="f"> la fenetre OpenGL </param>
        /// <param name="gagne"> si la aprtie est gagnée </param>
        public FinPartieOnline(IntegratedOpenGl f, bool gagne)
        {
            InitializeComponent();
            _fenetre = f;
            ControlBox = false;
            if (!gagne)
            {
                label1.Hide();
                label2.Show();
            }
            else
            {
                label1.Show();
                label2.Hide();
            }
        }

        /// <summary>
        /// private void recommencer_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction contrôle le bouton "Recommencer"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void recommencer_Click(object sender, EventArgs e)
        {
            _fenetre.AssignerEstTerminee(false);
            NativeFunction.arreterSons();
            EventManager.Instance.Notice(new EndGameEvent(EndGameType.Dead, false));
            NativeFunction.demarrerPartie();
            Dispose();
        }

        /// <summary>
        /// private void menuPrincipal_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction contrôle le bouton "Menu principal"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuPrincipal_Click(object sender, EventArgs e)
        {
            _fenetre.Hide();
            Dispose();
        }


    }
}
