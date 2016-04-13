using System;
using System.Windows.Forms;

namespace FrontEnd.UserControl.Winform.State
{
    public class EtatSelection : EtatBase
    {
        /// <summary>
        /// Constructeur par paramètre
        /// </summary>
        /// <param name="fenGL">
        /// L'éditeur
        /// </param>
        public EtatSelection(IntegratedOpenGl editeur)
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
        /// Cette fonction termine la sélection des objets
        /// </summary>
        /// <param name="e">
        /// L'évènement relié au relâchement du bouton gauche de la souris
        /// </param>
        public override void SourisGaucheRelachee(MouseEventArgs e)
        {
            var ajout = ((Control.ModifierKeys & Keys.Control) != 0 );
            var nbSel = 0;            
            if (!EstClic)
            {
                FonctionsNatives.terminerRectangle(PositionXDebut, PositionYDebut, e.X, e.Y);
                nbSel = FonctionsNatives.selectionner((PositionXDebut + e.X) / 2, (PositionYDebut + e.Y) / 2, Math.Max(Math.Abs(PositionXDebut - e.X), 2),  Math.Max(Math.Abs(PositionYDebut - e.Y), 2), ajout);
                EstClic = true;
            }
            else
            {
                nbSel = FonctionsNatives.selectionner(e.X, e.Y, 3, 3, ajout);
            }
            Fenetre.AfficherPanelEditionObjet(nbSel == 1);
            if (nbSel == 0)
                Fenetre.DesactiverBoutons(false);
            else
                Fenetre.DesactiverBoutons(true);
            if(nbSel==1)
                Fenetre.MettreAJourParametres();
            Fenetre.AssignerBoutonSupprimer(nbSel != 0);
            Fenetre.MettreAJour();

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
            base.SourisDeplacee(o,e);
        }
    }
}
