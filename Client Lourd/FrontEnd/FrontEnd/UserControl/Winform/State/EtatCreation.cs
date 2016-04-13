using System;
using System.Text;
using System.Windows.Forms;

namespace FrontEnd.UserControl.Winform.State
{
    class EtatCreation:EtatBase
    {
        // Le nom de l'objet
        private readonly StringBuilder _nomObjet = new StringBuilder();
        
        // Si l'objet est créé
        private bool _objetCree;
        //Si on crée un portail
        private bool _creerPortail;
        //Si on crée un mur
        private bool _creerVraiMur;

        /// <summary>
        /// Constructeur par paramètre
        /// </summary>
        /// <param name="fenGL">
        /// L'éditeur
        /// </param>
         public EtatCreation(IntegratedOpenGl editeur)
        {
            Fenetre = editeur;
             _nomObjet = new StringBuilder("");
        }

        /// <summary>
        /// Constructeur par paramètre
        /// </summary>
        /// <param name="fenGL">
        /// L'éditeur
        /// </param>
        /// <param name="s">
        /// L'objet à créer
        /// </param>
        public EtatCreation(IntegratedOpenGl editeur, StringBuilder s)
         {
             Fenetre = editeur;
             _nomObjet = s;
         }

        /// <summary>
        ///  public override void sourisGaucheEnfoncee(MouseEventArgs e)
        ///  
        /// Cette fonction assigne les positions en x et y de départ selon l'endroit
        /// où le bouton gauche de la souris a été enfoncé.
        /// </summary>
        /// <param name="e">
        /// L'évènement relié à l'enfoncement du bouton gauche de la souris
        /// </param>
         public override void SourisGaucheEnfoncee(MouseEventArgs e)
         {
             PositionXDebut = e.X;
             PositionYDebut = e.Y;
         }

        /// <summary>
        /// public override void sourisGaucheRelachee(MouseEventArgs e)
        /// 
        /// Cette fonction crée l'objet à l'endroit où le bouton gauche de la
        /// souris a été relâché.
        /// </summary>
        /// <param name="e">
        /// L'évènement relié au relâchement du bouton gauche de la souris.
        /// </param>
         public override void SourisGaucheRelachee(MouseEventArgs e)
         {
             PositionXDernier = e.X;
             PositionYDernier = e.Y;
             if (!((Math.Abs(PositionXDebut - PositionXDernier) > 3) || (Math.Abs(PositionYDernier - PositionYDebut) > 3)))
             {
                 if (_nomObjet.ToString() == "mur" && _creerVraiMur)
                 {
                     _creerVraiMur = false;
                     FonctionsNatives.finirCreationMur();
                 }
                 else if (_nomObjet.ToString() == "mur" && !_creerVraiMur)
                 {
                     _objetCree = FonctionsNatives.creerObjet(_nomObjet, _nomObjet.Length, PositionXDebut, PositionYDebut);
                     if (_nomObjet.ToString() == "mur" && _objetCree)
                     {
                         _creerVraiMur = true;
                     }
                 }
                 else if (_nomObjet.ToString() == "portail" && _creerPortail)
                 {
                     FonctionsNatives.creerObjet(_nomObjet, _nomObjet.Length, PositionXDebut, PositionYDebut);
                     _creerPortail = false;
                 }
                 else
                 {
                     _objetCree = FonctionsNatives.creerObjet(_nomObjet, _nomObjet.Length, PositionXDebut, PositionYDebut);
                     if (_nomObjet.ToString() == "portail" && _objetCree)
                     {
                         _creerPortail = true;
                     }
                 }

                 Fenetre.MettreAJour();
             }
             //System.Console.WriteLine(e.Location);
         }

        /// <summary>
        /// public override void sourisDroiteEnfoncee(MouseEventArgs e)
        /// 
        /// Cette fonction assigne les positions en x et y de départ selon l'endroit
        /// où le bouton droit de la souris a été enfoncé.
        /// </summary>
        /// <param name="e">
        /// L'évènement relié à l'enfoncement du bouton droit de la souris
        /// </param>
        public override void SourisDroiteEnfoncee(MouseEventArgs e)
         {
             PositionYDernier = e.Y;
             PositionXDernier = e.X;
         }

        /// <summary>
        /// public override void sourisDeplacee(object o, MouseEventArgs e)
        /// 
        /// Cette fonction fait suivre le deuxième bout du mur à la souris
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e">
        /// L'évènement relié au déplacement de la souris
        /// </param>
        public override void SourisDeplacee(object o, MouseEventArgs e)
        {
            Fenetre.CheckSouris(e.X, e.Y);
            if(_creerVraiMur)
            {
                FonctionsNatives.murFantome(e.X, e.Y);
                Fenetre.MettreAJour();
            }
            base.SourisDeplacee(o, e);
        }

        /// <summary>
        /// public override void toucheEnfoncee(KeyEventArgs e)
        /// 
        /// Cette fonction vérifie si la touche Échappe est enfoncée et agit en conséquence
        /// </summary>
        /// <param name="e">
        /// L'évènement relié à l'enfoncement d'une touche du clavier
        /// </param>
        public override void ToucheEnfoncee(KeyEventArgs e)
        {
            base.ToucheEnfoncee(e);
            if ((_nomObjet.ToString() == "mur" || _nomObjet.ToString() == "portail") && _objetCree && (_creerPortail || _creerVraiMur) )
            {
                switch (e.KeyCode)
                {
                    case (Keys.Escape):
                        FonctionsNatives.annulerCreation();
                        _creerVraiMur = false;
                        _objetCree = false;
                        _creerPortail = false;
                        break;
                }
            }
            Fenetre.MettreAJour();
        }

        /// <summary>
        /// public bool estEnCreation()
        /// 
        /// Cette fonction vérifie si on crée un mur ou un portail
        /// </summary>
        /// <returns>
        /// si on crée un mur ou un portail.
        /// </returns>
         public bool EstEnCreation()
         {
             return _creerVraiMur || _creerPortail;
         }


    }

}
