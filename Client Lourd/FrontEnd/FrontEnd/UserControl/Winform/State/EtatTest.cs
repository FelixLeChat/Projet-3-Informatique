using System;
using System.Windows.Forms;

namespace FrontEnd.UserControl.Winform.State
{
    class EtatTest : EtatBase
    {
        private readonly int _touchePg1;
        private readonly int _touchePg2;
        private readonly int _touchePd1;
        private readonly int _touchePd2;
        private readonly int _toucheRessort;
        private bool _pg1Enfonce, _pg2Enfonce, _pd1Enfonce, _pd2Enfonce, _ressortEnfonce;
        readonly bool _peutReset;
        public EtatTest(IntegratedOpenGl editeur, bool reset)
        {
            Fenetre = editeur;
            _touchePd1 = FonctionsNatives.getPd1();
            _touchePd2 = FonctionsNatives.getPd2();
            _touchePg1 = FonctionsNatives.getPg1();
            _touchePg2 = FonctionsNatives.getPg2();
            _toucheRessort = FonctionsNatives.getRes();
            _peutReset = reset;

        }

        /// <summary>
        /// public virtual void toucheEnfoncee(KeyEventArgs e)
        /// 
        /// Cette fonction vérifie quelle touche du clavier est enfoncée
        /// </summary>
        /// <param name="e"> 
        /// L'évènement relié à l'enfoncement de la touche du clavier
        /// </param>
        override public void ToucheEnfoncee(KeyEventArgs e)
        {
            base.ToucheEnfoncee(e);
            var keyCode = (int)e.KeyCode;
            if (!IntegratedOpenGl.EstEnPause())
            {
                if (keyCode == _touchePd1 && !_pd1Enfonce)
                {
                    _pd1Enfonce = true;
                    FonctionsNatives.appuyerBouton(keyCode);
                }
                else if (keyCode == _touchePd2 && !_pd2Enfonce)
                {
                    _pd2Enfonce = true;
                    FonctionsNatives.appuyerBouton(keyCode);
                }
                else if (keyCode == _touchePg1 && !_pg1Enfonce)
                {
                    _pg1Enfonce = true;
                    FonctionsNatives.appuyerBouton(keyCode);
                }
                else if (keyCode == _touchePg2 && !_pg2Enfonce)
                {
                    _pg2Enfonce = true;
                    FonctionsNatives.appuyerBouton(keyCode);
                }
                else if (keyCode == _toucheRessort && !_ressortEnfonce)
                {
                    _ressortEnfonce = true;
                    FonctionsNatives.appuyerBouton(keyCode);
                }
                else if (_peutReset && keyCode == (int)Keys.Back)
                {
                    FonctionsNatives.reinitialiser();
                    FonctionsNatives.demarrerPartie();
                }
                else if (keyCode == (int)Keys.B)
                {
                    FonctionsNatives.basculerDebug();
                }
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
        override public void ToucheRelachee(KeyEventArgs e)
        {
            var keyCode = (int)e.KeyCode;
            if (!IntegratedOpenGl.EstEnPause())
            {
                if (keyCode == _touchePd1)
                {
                    _pd1Enfonce = false;
                    FonctionsNatives.relacherBouton(keyCode);
                }
                else if (keyCode == _touchePd2)
                {
                    _pd2Enfonce = false;
                    FonctionsNatives.relacherBouton(keyCode);
                }
                else if (keyCode == _touchePg1)
                {
                    _pg1Enfonce = false;
                    FonctionsNatives.relacherBouton(keyCode);
                }
                else if (keyCode == _touchePg2)
                {
                    _pg2Enfonce = false;
                    FonctionsNatives.relacherBouton(keyCode);
                }
                else if (keyCode == _toucheRessort)
                {
                    _ressortEnfonce = false;
                    FonctionsNatives.relacherBouton(keyCode);
                }
            }
        }


    }


}
