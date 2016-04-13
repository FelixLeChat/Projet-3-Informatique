using System.Windows.Forms;

namespace FrontEnd.UserControl.Winform.State
{
    class EtatRotation : EtatBase
    {
        /// <summary>
        /// Constrcuteur par paramètre
        /// </summary>
        /// <param name="fenGL">
        /// L'éditeur
        /// </param>
        public EtatRotation(IntegratedOpenGl editeur)
        {
            Fenetre = editeur;
        }

        /// <summary>
        /// public override void sourisGaucheRelachee(MouseEventArgs e)
        /// 
        /// Cette fonction met à jour la rotation de la sélection
        /// </summary>
        /// <param name="e">
        /// L'évènement relié au relâchement du bouton gauche de la souris
        /// </param>
        public override void SourisGaucheRelachee(MouseEventArgs e)
        {
            Fenetre.MettreAJourRotation();
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
            FonctionsNatives.assignerCentreSelection();
        }

        /// <summary>
        /// public override void sourisDeplacee(object o, MouseEventArgs e)
        /// 
        /// Cette fonction effectue la rotation de l'objet selon le déplacement de l'objet en y
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e">
        /// L'évènement reliée au déplacement de la souris
        /// </param>
        public override void SourisDeplacee(object o, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FonctionsNatives.rotaterSelection(PositionYDernier, e.Y);
                PositionYDernier = e.Y;
                Fenetre.MettreAJour();
            }

            base.SourisDeplacee(o, e);

        }

    }
}
