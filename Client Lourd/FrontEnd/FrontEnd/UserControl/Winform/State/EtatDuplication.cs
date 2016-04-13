using System.Windows.Forms;

namespace FrontEnd.UserControl.Winform.State
{
    class EtatDuplication:EtatBase
    {
        // Si la duplicatione est en cours
        bool _duplicationEnCours;

        /// <summary>
        /// Constructeur par paramètre
        /// </summary>
        /// <param name="edit">
        /// L'éditeur
        /// </param>
        public EtatDuplication(IntegratedOpenGl edit)
        {
            Fenetre = edit;
            _duplicationEnCours = false;
        }

        /// <summary>
        /// public override void sourisGaucheEnfoncee(MouseEventArgs e)
        /// 
        /// Cette fonction termine la duplication de l'objet
        /// </summary>
        /// <param name="e">
        /// L'évènement relié à l'enfoncement du bouton gauche de la souris
        /// </param>
        public override void SourisGaucheEnfoncee(MouseEventArgs e)
        {
            if (_duplicationEnCours)
            {
                FonctionsNatives.finirDuplication();
                _duplicationEnCours = false;
                Fenetre.MettreAJour();
            }
            SourisDroiteEnfoncee(e);
             
        }

        /// <summary>
        /// public override void sourisDeplacee(object o, MouseEventArgs e)
        /// 
        /// Cette fonction déplace l'étampe de duplication
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e">
        /// L'évènement reliée au déplacement de la souris
        /// </param>
        public override void SourisDeplacee(object o, MouseEventArgs e)
        {
            if (!_duplicationEnCours)
            {
                PositionXDebut = e.X;
                PositionYDebut = e.Y;
                PositionXDernier = e.X;
                PositionYDernier = e.Y;
                _duplicationEnCours = FonctionsNatives.initialiserDuplication(e.X, e.Y);
                Fenetre.MettreAJour();
            }
            else
            {
                FonctionsNatives.deplacerSelection(e.X, PositionXDernier, e.Y, PositionYDernier, true);
                PositionYDernier = e.Y;
                PositionXDernier = e.X;
                Fenetre.MettreAJour();
            }

            base.SourisDeplacee(o, e);

        }
        
    }
    
}
