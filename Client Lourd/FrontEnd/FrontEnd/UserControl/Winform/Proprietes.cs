using System;
using System.Windows.Forms;
using FrontEnd.Core;

namespace FrontEnd.UserControl.Winform
{
    public partial class Proprietes : Form
    {
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Proprietes()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  private void button2_Click(object sender, EventArgs e)
        ///  
        /// Cette fonction représente le bouton "Annuler" de la fenêtre de propriétés.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur le bouton "Annuler"
        /// </param>
        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        /// <summary>
        ///  private void button1_Click(object sender, EventArgs e)
        ///  
        /// Cette fonction représente le bouton "OK" de la fenêtre de propriétés.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur le bouton "OK"
        /// </param>
        private void button1_Click(object sender, EventArgs e)
        {
            NativeFunction.mettreAJourProprietes((int)numericUpDown1.Value, (int)numericUpDown2.Value, (int)numericUpDown3.Value, (int)numericUpDown5.Value, (int)numericUpDown4.Value, (int)numericUpDown6.Value);
            Dispose();
        }

    }

    
}
