using System.Windows.Forms;

namespace FrontEnd.UserControl.Winform.State
{
    class EtatDeplacement:EtatBase
    {
        /// <summary>
        /// Constructeur par paramètre
        /// </summary>
        /// <param name="fenGL">
        /// L'éditeur
        /// </param>
        public EtatDeplacement(IntegratedOpenGl editeur)
        {
            Fenetre = editeur;
        }

        /// <summary>
        /// public override void sourisGaucheRelachee(MouseEventArgs e)
        /// 
        /// Cette fonction met à jour la position de la souris
        /// </summary>
        /// <param name="e">
        /// L'évènement relié au relâchement du bouton gauche de la souris
        /// </param>
        public override void SourisGaucheRelachee(MouseEventArgs e)
        {
            Fenetre.MettreAJourPosition();
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
            PositionXDebut = e.X;
            PositionYDebut = e.Y;
            PositionXDernier = e.X;
            PositionYDernier = e.Y;
        }

        /// <summary>
        /// public override void sourisDeplacee(object o, MouseEventArgs e)
        /// 
        /// Cette fonction déplace la sélection
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e">
        /// L'évènement relié au déplacement de la souris
        /// </param>
        public override void SourisDeplacee(object o, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FonctionsNatives.deplacerSelection(e.X, PositionXDernier, e.Y, PositionYDernier, false);
                PositionYDernier = e.Y;
                PositionXDernier = e.X;
                Fenetre.MettreAJour();
            }

            base.SourisDeplacee(o, e);

        }
        
    }
}
