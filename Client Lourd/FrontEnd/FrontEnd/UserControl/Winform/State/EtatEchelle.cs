using System.Windows.Forms;

namespace FrontEnd.UserControl.Winform.State
{
    class EtatEchelle:EtatBase
    {
        /// <summary>
        /// Constructeur par paramètre
        /// </summary>
        /// <param name="fenGL">
        /// L'éditeur
        /// </param>
        public EtatEchelle(IntegratedOpenGl editeur)
        {
            Fenetre = editeur;
        }

        /// <summary>
        /// public override void sourisGaucheRelachee(MouseEventArgs e)
        /// 
        /// Cette fonction met à jour le redimensionnement de la sélection
        /// </summary>
        /// <param name="e">
        /// L'évènement relié au relâchement du bouton gauche de la souris
        /// </param>
        public override void SourisGaucheRelachee(MouseEventArgs e)
        {
            Fenetre.MettreAJourScale();
            base.SourisGaucheRelachee(e);
        }

        /// <summary>
        /// public override void sourisGaucheEnfoncee(MouseEventArgs e)
        /// 
        /// Cette fonction assigne les positions de la souris
        /// </summary>
        /// <param name="e">
        /// L'évènement relié à l'enfoncement du bouton gauche de la souris
        /// </param>
        public override void SourisGaucheEnfoncee(MouseEventArgs e)
        {
            PositionYDebut = e.Y;
            PositionYDernier = e.Y;
        }

        /// <summary>
        /// public override void sourisDeplacee(object o, MouseEventArgs e)
        /// 
        /// Cette fonction effectue le redimensionnement de la sélection selon le déplacement de la souris
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e">
        /// L'évènement relié au déplacement de la souris
        /// </param>
        public override void SourisDeplacee(object o, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FonctionsNatives.redimensionnerSelection(PositionYDernier, e.Y);
                PositionYDernier = e.Y;
                Fenetre.MettreAJour();
            }

            base.SourisDeplacee(o, e);

        }

    }
}
