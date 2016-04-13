using System;
using System.Windows.Forms;

namespace FrontEnd.UserControl.Winform.State
{
    public class EtatZoom : EtatBase
    {
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public EtatZoom() { }

        /// <summary>
        /// Constructeur par paramètre
        /// </summary>
        /// <param name="fenGL">
        /// L'éditeur
        /// </param>
        public EtatZoom(IntegratedOpenGl editeur)
        {
            Fenetre = editeur;
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
            PositionYDernier = e.Y;
            PositionXDernier = e.X;
            EstClic = true;
        }

        /// <summary>
        /// public override void sourisGaucheRelachee(MouseEventArgs e)
        /// 
        /// Cette fonction termine le rectangle élastique de zoom
        /// </summary>
        /// <param name="e">
        /// L'évènement relié au relâchement du bouton gauche de la souris
        /// </param>
        public override void SourisGaucheRelachee(MouseEventArgs e)
        {
            if (!EstClic)
            {
                FonctionsNatives.terminerRectangle(PositionXDebut, PositionYDebut, e.X, e.Y);
                EstClic = true;
                if ((Control.ModifierKeys & Keys.Alt) != 0)
                    FonctionsNatives.zoomOutRect(PositionXDebut, PositionYDebut, e.X, e.Y);
                else
                    FonctionsNatives.zoomInRect(PositionXDebut, PositionYDebut, e.X, e.Y);
                Fenetre.MettreAJour();
            }
        }

        /// <summary>
        /// public override void sourisDeplacee(object o, MouseEventArgs e)
        /// 
        /// Cette fonction met à jour le rectangle élastique selon le déplacement de la souris
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e">
        /// L'évènement relié au déplacement de la souris
        /// </param>
        public override void SourisDeplacee(object o, MouseEventArgs e)
        {
            base.SourisDeplacee(o, e);
            if (EstClic && e.Button == MouseButtons.Left)
            {
                if (Math.Abs(PositionXDebut - e.X) >= 3 || Math.Abs(PositionYDebut - e.Y) >= 3)
                {
                    EstClic = false;
                    FonctionsNatives.initRectangle(PositionXDebut, PositionYDebut);
                }
            }
            else if (!EstClic)
            {
                if (PositionXDernier != e.X || PositionYDernier != e.Y)
                {
                    FonctionsNatives.miseAJourRectangle(PositionXDebut, PositionYDebut, PositionXDernier, PositionYDernier, e.X, e.Y);
                    PositionXDernier = e.X;
                    PositionYDernier = e.Y;
                }

            }


        }
    }
}
