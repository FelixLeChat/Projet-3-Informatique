using System;
using System.Windows.Forms;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Game.Wrap;

namespace FrontEnd.UserControl.Winform
{
    public partial class FinPartie : Form
    {

        readonly IntegratedOpenGl _fenetre;

        /// <summary>
        /// public FinPartie(fenetreOpenGL f, bool gagne)
        /// 
        /// Constructeur
        /// </summary>
        /// <param name="f"> la fenetre OpenGL </param>
        /// <param name="gagne"> si la aprtie est gagnée </param>
        public FinPartie(IntegratedOpenGl f, bool gagne)
        {
            InitializeComponent();
            _fenetre = f;
            ControlBox = false;
            if (!gagne)
            {
                label1.Hide();
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
            NativeFunction.arreterSons();
            // Todo: vérifier transition
            //Program.afficherMenuPrincipal();
            EventManager.Instance.Notice(new EndGameEvent(EndGameType.Dead));
            Dispose();
        }


    }
}
