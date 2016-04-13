using System;
using System.IO;
using System.Windows.Forms;
using FrontEnd.Core;
using FrontEnd.ProfileHelper;

namespace FrontEnd.UserControl.Winform
{
  

    public partial class Enregistrer : Form
    {
        private string _nomFichier = "";
        private const string FichierDefaut = "zoneJeuDefaut.xml";

        public Enregistrer()
        {
            InitializeComponent();
        }

        private void non_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void oui_Click(object sender, EventArgs e)
        {
            var enregistrerSous = new SaveFileDialog
            {
                Title = "Enregistrer sous",
                DefaultExt = "xml",
                Filter = "Fichiers xml (*.xml)|*.xml|Tous les fichiers (*.*)|*.*",
                CheckPathExists = true,
                InitialDirectory = Path.Combine(ZoneHelper.GetZonePath(), @"")
            };

            if (enregistrerSous.ShowDialog() == DialogResult.OK)
            {
                _nomFichier = enregistrerSous.FileName;
                if (!_nomFichier.Contains(FichierDefaut))
                {
                    ZoneHelper.Save(_nomFichier);
                }
                else
                    MessageBox.Show("Vous ne pouvez pas modifier le fichier XML par défaut");
            }

            Dispose();
        }
    }
}
