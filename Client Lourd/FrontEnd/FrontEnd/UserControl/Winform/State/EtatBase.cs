using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace FrontEnd.UserControl.Winform.State
{
    public class EtatBase
    {
        // positions x et y de départ
        protected int PositionXDebut, PositionYDebut;
        // positions x et y finales
        protected int PositionXDernier, PositionYDernier;
        // vérification du clic
        protected bool EstClic = true;
        // la fenêtre
        protected IntegratedOpenGl Fenetre;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public EtatBase()
        {       }

        /// <summary>
        /// Constructeur par paramètre
        /// </summary>
        /// <param name="fenGL">
        /// L'éditeur 
        /// </param>
        public EtatBase(IntegratedOpenGl editeur)
        {
            Fenetre = editeur;
        }

        /// <summary>
        /// public virtual void toucheEnfoncee(KeyEventArgs e)
        /// 
        /// Cette fonction vérifie quelle touche du clavier est enfoncée
        /// </summary>
        /// <param name="e"> 
        /// L'évènement relié à l'enfoncement de la touche du clavier
        /// </param>
        public virtual void ToucheEnfoncee(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case (Keys.Left):
                    FonctionsNatives.deplacerCamera(0.1, 0);
                    Fenetre.MettreAJour();
                    break;
                case (Keys.Right):
                    FonctionsNatives.deplacerCamera(-0.1, 0);
                    Fenetre.MettreAJour();
                    break;
                case (Keys.Up): 
                    FonctionsNatives.deplacerCamera(0, -0.1);
                    Fenetre.MettreAJour();
                    break;
                case (Keys.Down):
                    FonctionsNatives.deplacerCamera(0, 0.1);
                    Fenetre.MettreAJour();
                    break;
                case Keys.Oemplus:
                case Keys.Add:
                    FonctionsNatives.zoomIn();
                    Fenetre.MettreAJour();
                    break;
                case Keys.OemMinus:
                case Keys.Subtract:
                    FonctionsNatives.zoomOut();
                    Fenetre.MettreAJour();
                    break;
            }
            
        }

        /// <summary>
        /// public virtual void toucheRelachee(KeyEventArgs e)
        /// 
        /// Cette fonction gere l'evenement lorsque une touche est relachee
        /// </summary>
        /// <param name="e"> 
        /// L'évènement relié à l'enfoncement de la touche du clavier
        /// </param>
        public virtual void ToucheRelachee(KeyEventArgs e)
        {


        }
        
        /// <summary>
        /// public virtual void sourisGaucheEnfoncee(MouseEventArgs e)
        /// </summary>
        /// <param name="e"> 
        /// L'évènement relié à l'enfoncement du bouton gauche de la souris
        /// </param>
        public virtual void SourisGaucheEnfoncee(MouseEventArgs e)
        {
        }

        /// <summary>
        /// public virtual void sourisDroiteEnfoncee(MouseEventArgs e)
        /// </summary>
        /// <param name="e"> 
        /// L'évènement relié à l'enfoncement du bouton droit de la souris
        /// </param>
        public virtual void SourisDroiteEnfoncee(MouseEventArgs e)
        {
            PositionYDernier = e.Y;
            PositionXDernier = e.X;
        }

        /// <summary>
        /// public virtual void sourisDroiteRelachee(MouseEventArgs e)
        /// </summary>
        /// <param name="e"> 
        /// L'évènement relié au relâchement du bouton droit de la souris
        /// </param>
        public virtual  void SourisDroiteRelachee(MouseEventArgs e)
        {
        }

        /// <summary>
        /// public virtual void sourisGaucheRelachee(MouseEventArgs e)
        /// </summary>
        /// <param name="e"> 
        /// L'évènement relié au relâchement du bouton gauche de la souris
        /// </param>
        public virtual void SourisGaucheRelachee(MouseEventArgs e)
        {
        }

        /// <summary>
        /// public virtual void sourisDeplacee(object o, MouseEventArgs e)
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e">
        /// L'évènement relié au déplacement de la souris
        /// </param>
        public virtual void SourisDeplacee(object o, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var dx = PositionXDernier - e.X;
                var dy = -(PositionYDernier - e.Y);
                PositionYDernier = e.Y;
                PositionXDernier = e.X;
                FonctionsNatives.deplacerCameraInt(dx, dy);
                Fenetre.MettreAJour();
            }

        }

        /// <summary>
        /// public virtual void scroll(MouseEventArgs e)
        /// </summary>
        /// <param name="e">
        /// L'évènement relié à la roulette de la souris
        /// </param>
        public virtual void Scroll(MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                FonctionsNatives.zoomOut();
            }
            else
            {
                FonctionsNatives.zoomIn();
            }
            Fenetre.MettreAJour();

        }
    }
   

    static partial class FonctionsNatives
    {
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void deplacerCamera(double dx, double dy);
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void deplacerCameraInt(int dx, int dy);
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void initRectangle(int dx, int dy);
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void miseAJourRectangle(int posOrigX, int posOrigY, int posPrecX, int posPrecY, int posX, int posY);
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void terminerRectangle(int posOrigX, int posOrigY, int posX, int posY);
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void zoomIn();
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void zoomOut();
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void zoomInRect(int posX1, int posY1, int posX2, int posY2);
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool creerObjet(StringBuilder objet, int longueur, int x, int y);
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void murFantome(int x, int y);
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void zoomOutRect(int posX1, int posY1, int posX2, int posY2);
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void deplacerSelection(int x1, int x2, int y1, int y2, bool force);
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void redimensionnerSelection(int y1, int y2);
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void rotaterSelection(int y1, int y2);
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void assignerCentreSelection();
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int selectionner(int x, int y, int longueur, int hauteur, bool ajout);
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool initialiserDuplication(int x, int y);
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool finirDuplication();
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double obtenirRotationSelection();
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double obtenirEchelleSelection();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void annulerCreation();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void finirCreationMur();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getPg1();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getPg2();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getPd1();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getPd2();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getRes();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void appuyerBouton(int keycode);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void relacherBouton(int keycode);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void reinitialiser();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void demarrerPartie();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void basculerDebug();

    }
}
