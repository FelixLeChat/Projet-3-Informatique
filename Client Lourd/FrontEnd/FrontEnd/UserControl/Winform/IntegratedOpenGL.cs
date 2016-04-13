using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Game;
using FrontEnd.Game.Wrap;
using FrontEnd.Player;
using FrontEnd.ProfileHelper;
using FrontEnd.UserControl.Winform.State;
using FrontEndAccess.APIAccess;

namespace FrontEnd.UserControl.Winform
{
    public partial class IntegratedOpenGl : Form
    {
        private static FinPartie _finPartie;
        private static Enregistrer _enregistrer;

        const string FichierDefaut = "zones\\zoneJeuDefaut.xml";

        private EtatBase _etat;
        private string _nomFichier = "";
        private int _lastRotate;
        private int _lastScale;
        private bool _attributsModifiables = true;
        private static bool _enPause;
        private bool _estTerminee;
        private const int PosXPanelDef = 111;
        private const int PosYPanelDef = 27;
        public enum Mode { ModeEditeur, ModeTest, ModePartieRapide, ModeCampagne };
        public Mode CurrentMode;
        public bool EstEnregistre = true;


        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public IntegratedOpenGl()
        {
            InitializeComponent();

            InitOpenGl();
            supprimerToolStripMenuItem.Enabled = false;
            panel1.KeyDown += ToucheEnfonce;
            panel1.KeyUp += ToucheRelachee;
            panel1.MouseMove += SourisDeplacee;
            panel1.MouseDown += SourisEnfoncee;
            panel1.MouseUp += SourisRelachee;

            rotationObjet.Scroll += rotationObjet_Scroll;
            rotationUpDown.ValueChanged += rotationUpDown_ValueChanged;
            scaleUpDown.ValueChanged += scaleUpDown_ValueChanged;
            facteurEchelleObjet.Scroll += facteurEchelleObjet_Scroll;

            posXUpDown.ValueChanged += posXUpDown_ValueChanged;
            posYUpDown.ValueChanged += posYUpDown_ValueChanged;

            MouseWheel += scroll;
            panel1.MouseHover += SourisHover;
            Resize += Editeur_redimensionner;
            orthographiqueToolStripMenuItem.Click += orthographiqueToolStripMenuItem1_Click;
            orbiteToolStripMenuItem.Click += orbiteToolStripMenuItem1_Click;
            panel1.Paint += panel1_Paint;
            _etat = new EtatSelection(this);
            panel1.Focus();
            _lastRotate = rotationObjet.Value;
            _lastScale = facteurEchelleObjet.Value;
            menuStrip2.Hide();

            HandleUserLevelVisibility();

            pointPanel.Visible = false;
            ballPanel.Visible = false;



            //Visibility of labels
            Player1PointLabel.Visible = false;
            Player2PointLabel.Visible = false;
            Player3PointLabel.Visible = false;
            Player4PointLabel.Visible = false;

            Player1BallLabel.Visible = false;
            Player2BallLabel.Visible = false;
            Player3BallLabel.Visible = false;
            Player4BallLabel.Visible = false;

            CampagneInfo.Visible = false;



            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

        }

        private void InitOpenGl()
        {
            var path = Directory.GetCurrentDirectory();
            NativeFunction.initialiserOpenGL(panel1.Handle, path.ToCharArray(), path.Length);
        }

        private void HandleUserLevelVisibility()
        {
            var user = Profile.Instance?.CurrentProfile;
            if (user != null)
            {
                PlateauArgent.Enabled = false;
                ChampDeForce.Enabled = false;
                plateauDargentToolStripMenuItem.Enabled = false;
                champDeForceToolStripMenuItem.Enabled = false;

                var level = user.Level;

                if (level > 0)
                {
                    ChampDeForce.Enabled = true;
                    champDeForceToolStripMenuItem.Enabled = true;
                }
                else
                    ChampDeForce.BackgroundImage = Properties.Resources.Level1;

                if (level > 1)
                {
                    PlateauArgent.Enabled = true;
                    plateauDargentToolStripMenuItem.Enabled = true;
                }
                else
                    PlateauArgent.BackgroundImage = Properties.Resources.Level2;



            }
        }

        /// <summary>
        /// public void MettreAJour(double tempsInterAffichage)
        /// 
        /// Cette fonction met à jour la fenêtre
        /// </summary>
        public void MettreAJour()
        {
            NativeFunction.dessinerOpenGL();
            panel1.Focus();
        }

        public void Animer(double tempsInterAffichage)
        {
            if (!_enPause && CurrentMode != Mode.ModeEditeur)
            {
                try
                {
                    Invoke((MethodInvoker)delegate
                    {
                        var temps = Math.Min(1.0 / 30.0, tempsInterAffichage);
                        NativeFunction.animer(temps);
                        NativeFunction.dessinerOpenGL();

                        if (CampagneInfo.Visible && !NativeFunction.obtenirRondeGagnee())
                        {
                            CampagneInfo.Visible = false;
                        }
                        else if (!CampagneInfo.Visible && NativeFunction.obtenirRondeGagnee())
                        {
                            var sb = new StringBuilder(255);
                            NativeFunction.obtenirNomZone(sb);
                            NomCampagne.Text = sb.ToString();
                            PointsCampagne.Text = NativeFunction.obtenirPointsAAtteindre().ToString();
                            DifficulteCampagne.Text = NativeFunction.obtenirDifficulteZone().ToString();
                            CampagneInfo.Visible = true;
                        }
                    });
                }
                catch
                {
                }

                if (NativeFunction.obtenirEstTerminee())
                {
                    if (_estTerminee != NativeFunction.obtenirEstTerminee())
                    {
                        _estTerminee = NativeFunction.obtenirEstTerminee();
                        NativeFunction.reinitialiser();
                        _enPause = true;
                        MettreAJour();

                        // Hack: no popup if online game
                        if (OnlineSession.Instance == null)
                        {
                            _finPartie = new FinPartie(this, NativeFunction.obtenirEstGagnee());
                            _finPartie.ShowDialog();
                        }
                        else
                        {
                            // Todo: vérifier transition
                            //Program.afficherMenuPrincipal();
                            var end = EndGameType.Dead;
                            if (NativeFunction.obtenirEstGagnee())
                                end = EndGameType.Won;
                            EventManager.Instance.Notice(new EndGameEvent(end, true));

                            //var endGamePopup = new FinPartieOnline(this, NativeFunction.obtenirEstGagnee());
                            //endGamePopup.ShowDialog();
                        }
                    }
                }
            }
        }

        #region changement de mode
        public void AssignerMode(Mode mode)
        {
            selection.Checked = true;
            panel1.Cursor = Cursors.Arrow;
            CurrentMode = mode;
            if (CurrentMode == Mode.ModeEditeur)
            {
                statusStrip1.Show();
                AfficherEnEditeur();
                _etat = new EtatSelection(this);
                Text = "Editeur";
            }
            else
            {
                AfficherEnPartie();
                if (CurrentMode == Mode.ModeCampagne)
                {
                    statusStrip1.Hide();
                    _etat = new EtatTest(this, false);
                }
                else
                {
                    statusStrip1.Hide();
                    _etat = new EtatTest(this, true);
                }
                Text = "Partie";
            }
            _enPause = false;
        }

/*
        private bool _isConnected;
*/
        private void AfficherEnEditeur()
        {
            panel2.Show();
            menuStrip1.Show();
            menuStrip2.Hide();
            panel3.Show();

            _enPause = false;
            panel1.Left = PosXPanelDef;
            panel1.Top = PosYPanelDef;
            Redimensionner();
        }

        private void AfficherEnPartie()
        {
            panel2.Hide();
            panel3.Hide();

            menuStrip1.Hide();
            menuStrip2.Hide();
            panelEditionObjet.Hide();
            panel1.Left = 0;
            panel1.Top = 0;
            Redimensionner();
        }

        #endregion

        #region evenement Globaux

        /// <summary>
        /// private void panel1_Paint(object sender, PaintEventArgs e)
        /// Cette fonction repaint la fenêtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            MettreAJour();
        }
        /// <summary>
        /// private void ToucheEnfonce(Object o, KeyEventArgs e)
        /// 
        /// Cette fonction applique la suppression et l'appui sur la touche Alt
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e">
        /// L'évènement relié à l'enfoncement sur une touche du clavier
        /// </param>
        private void ToucheEnfonce(Object o, KeyEventArgs e)
        {
            if (CurrentMode == Mode.ModeEditeur)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    NativeFunction.supprimerSelection();
                    toolStripStatusLabel2.Text = "Attention! La zone de jeu doit contenir au moins un ressort, un trou et un générateur de bille.";
                    supprimerToolStripMenuItem.Enabled = false;
                    AfficherPanelEditionObjet(false);
                    MettreAJour();
                    EstEnregistre = false;
                }
                else if (e.KeyCode == Keys.D && deplacement.Enabled)
                    deplacement.Checked = true;
                else if (e.KeyCode == Keys.S)
                    selection.Checked = true;
                else if (e.KeyCode == Keys.R && rotation.Enabled)
                    rotation.Checked = true;
                else if (e.KeyCode == Keys.E && miseEchelle.Enabled)
                    miseEchelle.Checked = true;
                else if (e.KeyCode == Keys.C && Duplication.Enabled)
                    Duplication.Checked = true;
                else if (e.KeyCode == Keys.Z)
                    zoom.Checked = true;
                else if (e.KeyCode == Keys.T)
                {
                    AssignerMode(Mode.ModeTest);
                    statusStrip1.Hide();
                    NativeFunction.reinitialiser();
                    NativeFunction.demarrerPartie();
                }

            }
            else
            {   //MODE CAMPAGNE
                if (CurrentMode == Mode.ModeCampagne)
                {
                    if (e.KeyCode == Keys.Space)
                    {
                        NativeFunction.appuyerBouton((int)e.KeyCode);
                    }
                }

                // MODE CAMPAGNE / PARTIE RAPIDE / TEST
                if (e.KeyCode == Keys.Escape)
                {
                    _enPause = !_enPause;
                    if (_enPause)
                    {
                        if (CurrentMode != Mode.ModeEditeur)
                        {
                            menuStrip2.Show();
                            menuStrip2.BringToFront();
                            if (CurrentMode != Mode.ModeTest)
                            {
                                modeÉditionToolStripMenuItem.Visible = false;
                            }
                            else
                            {
                                modeÉditionToolStripMenuItem.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        menuStrip2.Hide();
                    }
                }
                else if (CurrentMode == Mode.ModeTest && e.KeyCode == Keys.T)
                {
                    AssignerMode(Mode.ModeEditeur);
                    NativeFunction.reinitialiser();
                    MettreAJour();
                }

            }

            var keyCode = (int)e.KeyCode;
            if (keyCode == 'J')
            {
                NativeFunction.appuyerBouton(keyCode);
                MettreAJour();
            }
            else if (keyCode == 'K')
            {
                NativeFunction.appuyerBouton(keyCode);
                MettreAJour();
            }
            else if (keyCode == 'L')
            {
                NativeFunction.appuyerBouton(keyCode);
                MettreAJour();
            }
            else if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
            {
                NativeFunction.cameraOrtho();
                Redimensionner();
            }
            else if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
            {
                NativeFunction.cameraOrbite();
                Redimensionner();
            }
            _etat.ToucheEnfoncee(e);
        }

        private void ToucheRelachee(Object o, KeyEventArgs e)
        {
            _etat.ToucheRelachee(e);
        }
        /// <summary>
        /// private void sourisEnfoncee(Object o, MouseEventArgs e)
        /// 
        /// Cette fonction interprète les boutons enfoncés de la souris
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e">
        /// L'évènement relié à l'enfoncement d'un bouton de la souris
        /// </param>
        private void SourisEnfoncee(Object o, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _etat.SourisDroiteEnfoncee(e);
            }
            else if (e.Button == MouseButtons.Left)
            {
                _etat.SourisGaucheEnfoncee(e);
            }
        }

        /// <summary>
        /// private void sourisRelachee(Object o, MouseEventArgs e)
        /// 
        /// Cette fonction interprète les boutons enfoncés de la souris
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e">
        /// L'évènement relié au relâchement d'un bouton de la souris
        /// </param>
        private void SourisRelachee(Object o, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                _etat.SourisDroiteRelachee(e);
            }
            else if (e.Button == MouseButtons.Left)
            {
                _etat.SourisGaucheRelachee(e);
            }
        }

        /// <summary>
        /// private void scroll(object o, MouseEventArgs e)
        /// 
        /// Cette fonction interprète le roullement de la mollette de la souris
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e">
        /// L'évènement reliée au roulement de la mollette de la souris
        /// </param>
        private void scroll(object o, MouseEventArgs e)
        {
            _etat.Scroll(e);
        }

        /// <summary>
        /// private void sourisDeplacee(object o, MouseEventArgs e)
        /// 
        /// Cette fonction interprète les déplacements de la souris
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e">
        /// L'évènement relié au déplacement de la souris
        /// </param>
        private void SourisDeplacee(object o, MouseEventArgs e)
        {

            _etat.SourisDeplacee(o, e);
        }

        /// <summary>
        /// public void checkSouris(double x, double y)
        /// 
        /// Cette fonction change le curseur de la souris selon sa position sur le panel
        /// </summary>
        /// <param name="x">
        /// La position en x de la souris
        /// </param>
        /// <param name="y">
        /// La position en y de la souris
        /// </param>
        public void CheckSouris(double x, double y)
        {
            if (panel1.Cursor == Cursors.No)
            {
                if (NativeFunction.positionDansBornes(x, y))
                    panel1.Cursor = Cursors.Arrow;

            }
            else if (panel1.Cursor == Cursors.Arrow)
            {
                if (!NativeFunction.positionDansBornes(x, y))
                    panel1.Cursor = Cursors.No;
            }
        }

        public void DesactiverBoutons(bool enable)
        {
            rotation.Enabled = enable;
            rotation.IsAccessible = true;
            deplacement.Enabled = enable;
            Duplication.Enabled = enable;
            miseEchelle.Enabled = enable;
            if (enable)
                toolStripStatusLabel2.Text = "Au moins un objet sélectionné";
            else
                toolStripStatusLabel2.Text = "Aucun objet sélectionné : seulement l'outil d'édition 'Sélection' est activé!";
        }

        private void SourisHover(object sender, EventArgs e)
        {
            var toolTipBouton = new ToolTip();

            toolTipBouton.SetToolTip(selection, "Sélection \n Clic : sélectionne un objet à la fois \n Ctrl + Clic : sélectionne plusieurs objets");
            toolTipBouton.SetToolTip(zoom, "Zoom avec rectangle élastique \n Clic et glisse : zoom avant \n Alt + Clic et glisse : zoom arrière");
            toolTipBouton.SetToolTip(rotation, "Rotation \n Clic et glisser vers le haut : rotation horaire \n Clic et glisser vers le bas : rotation anti-horaire");
            toolTipBouton.SetToolTip(miseEchelle, "Mise à échelle \n Clic et glisser vers le haut : agrandissement \n Clic et glisser vers le bas : rétrécissement");
            toolTipBouton.SetToolTip(deplacement, "Déplacement \n Clic et glisser : déplacement de l'objet selon la position de la souris");
            toolTipBouton.SetToolTip(Duplication, "Duplication \n Clic : dépose une copie de la sélection");
            toolTipBouton.SetToolTip(generateurBille, "Générateur de bille");
            toolTipBouton.SetToolTip(ressort, "Ressort");
            toolTipBouton.SetToolTip(mur, "Mur");
            toolTipBouton.SetToolTip(trou, "Trou");
            toolTipBouton.SetToolTip(butoirCercle, "Butoir circulaire");
            toolTipBouton.SetToolTip(butoirTriangulaireDroit, "Butoir triangulaire droit");
            toolTipBouton.SetToolTip(butoirTriangulaireGauche, "Butoir triangulaire gauche");
            toolTipBouton.SetToolTip(cible, "Cible");
            toolTipBouton.SetToolTip(portail, "Portail");
            toolTipBouton.SetToolTip(PlateauArgent, "Plateau d'argent");
            toolTipBouton.SetToolTip(ChampDeForce, "Champ de force");
            toolTipBouton.SetToolTip(paletteDroiteJ1, "Palette droite du joueur 1");
            toolTipBouton.SetToolTip(paletteDroiteJ2, "Palette droite du joueur 2");
            toolTipBouton.SetToolTip(paletteGaucheJ1, "Palette gauche du joueur 1");
            toolTipBouton.SetToolTip(paletteGaucheJ2, "Palette gauche du joueur 2");

        }


        /// <summary>
        /// private void Exemple_FormClosing(object sender, FormClosingEventArgs e)
        /// 
        /// Cette fonction ferme l'éditeur.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exemple_FormClosing(object sender, FormClosingEventArgs e)
        {
            lock (Program.Lock)
            {
                NativeFunction.libererOpenGL();
                NativeFunction.arreterSons();

                if (CurrentMode == Mode.ModeEditeur || CurrentMode == Mode.ModeTest)
                {
                    if (!EstEnregistre)
                    {
                        if (CurrentMode == Mode.ModeTest)
                        {
                            _enPause = true;
                            NativeFunction.reinitialiser();
                        }
                        AssignerMode(Mode.ModeEditeur);
                        AfficherPourSauvegarde();
                        _enregistrer = new Enregistrer();
                        _enregistrer.ShowDialog();
                        AfficherEnEditeur();
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// public void assignerEstTerminee(bool b)
        /// 
        /// Cette fonction permet d'assigner si la partie est terminé.
        /// </summary>
        /// <param name="b"></param>
        public void AssignerEstTerminee(bool b)
        {
            _estTerminee = b;
            _enPause = false;
        }

        /// <summary>
        /// public bool estEnPause()
        /// 
        /// Cette fonction retourne si la partie est en pause.
        /// </summary>
        public static bool EstEnPause()
        {
            return _enPause;
        }
        #region Evenements Éditeur (boutons)

        // Fichier/Menu principal

        /// <summary>
        ///  private void menuPrincipalToolStripMenuItem_Click(object sender, EventArgs e)
        ///  
        /// Cette fonction affiche le menu principal lorsque l'option est sélectionnée dans l'onglet "Fichier"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option "Menu principal"
        /// </param>
        private void menuPrincipalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuPrincipalToolStripMenuItem1_Click(sender, e);
        }

        // Informations/Aide

        /// <summary>
        ///  private void menuPrincipalToolStripMenuItem_Click(object sender, EventArgs e)
        ///  
        /// Cette fonction affiche le menu d'aide lorsque l'option est sélectionnée dans l'onglet "Informations"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option "Aide"
        /// </param>
        private void aideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentMode == Mode.ModeEditeur)
            {
                // Program.
            }
        }
        //Fichier/Nouveau
        private void nouveauToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentMode == Mode.ModeEditeur)
            {
                _nomFichier = "";
                var s = new StringBuilder("zones\\zoneJeuDefaut.xml");
                NativeFunction.ouvrirPartieTest(s, s.Length);
                MettreAJour();
            }
        }

        // Fichier/Enregistrer

        /// <summary>
        ///  private void enregistrerToolStripMenuItem_Click(object sender, EventArgs e)
        ///  
        /// Cette fonction effectue l'option d'enregistrement lorsque l'option est sélectionnée dans l'onglet "Fichier"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option "Enregistrer"
        /// </param>
        private void enregistrerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentMode == Mode.ModeEditeur)
            {
                if (_nomFichier == "")
                {
                    enregistrerSousToolStripMenuItem_Click(sender, e);
                }
                else if (!_nomFichier.Contains(FichierDefaut))
                {
                    selection.Checked = true;
                    AfficherPourSauvegarde();
                    ZoneHelper.Save(_nomFichier);
                    AfficherEnEditeur();
                    return;
                }
            }
            EstEnregistre = true;
        }

        private void AfficherPourSauvegarde()
        {
            NativeFunction.cameraOrtho();
            AfficherEnPartie();
            FonctionsNatives.selectionner(10000, 10000, 3, 3, false);
            Redimensionner();
            panelEditionObjet.Visible = false;
        }

        //Fichier/EnregistrerSous

        /// <summary>
        ///  private void enregistrerSousToolStripMenuItem_Click(object sender, EventArgs e)
        ///  
        /// Cette fonction effectue l'option d'enregistrement lorsque l'option est sélectionnée dans l'onglet "Fichier"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option "Enregistrer Sous"
        /// </param>
        private void enregistrerSousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentMode == Mode.ModeEditeur)
            {
                //ouvrir une boite de dialogue pour créer un fichier
                var enregistrerSous = new SaveFileDialog();
                enregistrerSous.Title = "Enregistrer sous";
                enregistrerSous.DefaultExt = "xml";
                enregistrerSous.Filter = "Fichiers xml (*.xml)|*.xml|Tous les fichiers (*.*)|*.*";
                enregistrerSous.CheckPathExists = true;
                enregistrerSous.InitialDirectory = ZoneHelper.GetZonePath();


                while (true)
                {
                    if (enregistrerSous.ShowDialog() == DialogResult.OK)
                    {
                        _nomFichier = enregistrerSous.FileName;
                        if (!_nomFichier.Contains(FichierDefaut))
                        {
                            selection.Checked = true;
                            AfficherPourSauvegarde();
                            ZoneHelper.Save(_nomFichier);
                            AfficherEnEditeur();

                            return;
                        }
                        else
                        {
                            MessageBox.Show("Vous ne pouvez pas modifier le fichier XML par défaut");
                            return;
                        }
                    }
                    else
                        return; //Bouton annuler
                }
            }
            EstEnregistre = true;
        }

        //Fichier/Ouvrir

        /// <summary>
        ///  private void ouvrirToolStripMenuItem_Click(object sender, EventArgs e)
        ///  
        /// Cette fonction effectue l'option d'ouverture de fichier lorsque l'option est sélectionnée dans l'onglet "Fichier"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option "Ouvrir"
        /// </param>
        private void ouvrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentMode == Mode.ModeEditeur)
            {
                var ouvrir = new OpenFileDialog();
                ouvrir.Title = "Ouvrir";
                ouvrir.DefaultExt = "xml";
                ouvrir.Filter = "Fichiers xml (*.xml)|*.xml|Tous les fichiers (*.*)|*.*";
                ouvrir.CheckFileExists = true;
                ouvrir.CheckPathExists = true;
                ouvrir.InitialDirectory = ZoneHelper.GetZonePath();


                if (ouvrir.ShowDialog() == DialogResult.OK)
                {
                    _nomFichier = ouvrir.FileName.Replace(Directory.GetCurrentDirectory() + "\\", "");
                    /*NativeFunction.chargerFichierXML(nomFichier.ToCharArray(), nomFichier.Length);*/
                    var s = new StringBuilder(_nomFichier);
                    NativeFunction.ouvrirPartieTest(s, s.Length);
                    MettreAJour();
                }
                else
                    return;
            }
        }

        /// <summary>
        ///  private void Editeur_redimensionner(object sender, EventArgs e)
        ///  
        /// Cette fonction redimensionne la fenêtre d'édition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié à l'élargissement de la fenêtre
        /// </param>
        private void Editeur_redimensionner(object sender, EventArgs e)
        {
            Redimensionner();
        }


        /// <summary>
        ///  private void Editeur_redimensionner(object sender, EventArgs e)
        ///  
        /// Cette fonction redimensionne la fenêtre d'édition
        /// </summary>
        /// 
        /// </param>
        private void Redimensionner()
        {
            //panel1.Size = new Size(ClientSize.Width - panel1.Left, ClientSize.Height - panel1.Top);

            NativeFunction.redimensionnerFenetre(panel1.Size.Width, panel1.Size.Height);
            MettreAJour();
        }

        /// <summary>
        /// private void propriétésToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction affiche la fenêtre des propriétés
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'optiuon "Propriétés"
        /// </param>
        private void propriétésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentMode == Mode.ModeEditeur)
            {
                var proprietes = new Proprietes();
                proprietes.ShowDialog();
            }
        }

        /// <summary>
        /// private void selection_CheckedChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction détermine si l'option est sélectionné ou pas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selection_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Cursor = Cursors.Arrow;
            if (selection.Checked)
                _etat = new EtatSelection(this);
        }

        /// <summary>
        /// private void deplacement_CheckedChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction détermine si l'option est sélectionné ou pas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deplacement_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Cursor = Cursors.Arrow;
            if (deplacement.Checked)
            {
                _etat = new EtatDeplacement(this);
                EstEnregistre = false;
            }
        }

        /// <summary>
        /// private void rotation_CheckedChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction détermine si l'option est sélectionné ou pas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rotation_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Cursor = Cursors.Arrow;
            if (rotation.Checked)
            {
                _etat = new EtatRotation(this);
                EstEnregistre = false;
            }
        }

        /// <summary>
        /// private void miseEchelle_CheckedChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction détermine si l'option est sélectionné ou pas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miseEchelle_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Cursor = Cursors.Arrow;
            if (miseEchelle.Checked)
            {
                _etat = new EtatEchelle(this);
                EstEnregistre = false;
            }
        }

        /// <summary>
        /// private void Duplication_CheckedChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction détermine si l'option est sélectionné ou pas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Duplication_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Cursor = Cursors.Arrow;
            if (Duplication.Checked)
                _etat = new EtatDuplication(this);
            else
            {
                NativeFunction.finirDuplication();
                MettreAJour();
            }
            EstEnregistre = false;
        }

        /// <summary>
        /// private void butoirCercle_CheckedChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction détermine si l'option est sélectionné ou pas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butoirCercle_CheckedChanged(object sender, EventArgs e)
        {
            if (butoirCercle.Checked)
            {
                _etat = new EtatCreation(this, new StringBuilder("butoirCercle"));
                EstEnregistre = false;
            }
        }

        /// <summary>
        /// private void butoirTriangulaireDroit_CheckedChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction détermine si l'option est sélectionné ou pas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butoirTriangulaireDroit_CheckedChanged(object sender, EventArgs e)
        {
            if (butoirTriangulaireDroit.Checked)
            {
                _etat = new EtatCreation(this, new StringBuilder("butoirTriangleDroit"));
                EstEnregistre = false;
            }
        }

        /// <summary>
        /// private void butoirTriangulaireGauche_CheckedChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction détermine si l'option est sélectionné ou pas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butoirTriangulaireGauche_CheckedChanged(object sender, EventArgs e)
        {
            if (butoirTriangulaireGauche.Checked)
            {
                _etat = new EtatCreation(this, new StringBuilder("butoirTriangleGauche"));
                EstEnregistre = false;
            }
        }

        /// <summary>
        /// private void cible_CheckedChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction détermine si l'option est sélectionné ou pas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cible_CheckedChanged(object sender, EventArgs e)
        {
            if (cible.Checked)
            {
                _etat = new EtatCreation(this, new StringBuilder("cible"));
                EstEnregistre = false;
            }
        }

        /// <summary>
        /// private void ressort_CheckedChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction détermine si l'option est sélectionné ou pas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ressort_CheckedChanged(object sender, EventArgs e)
        {
            if (ressort.Checked)
            {
                _etat = new EtatCreation(this, new StringBuilder("ressort"));
                EstEnregistre = false;
            }
        }

        /// <summary>
        /// private void generateurBille_CheckedChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction détermine si l'option est sélectionné ou pas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void generateurBille_CheckedChanged(object sender, EventArgs e)
        {
            if (generateurBille.Checked)
            {
                _etat = new EtatCreation(this, new StringBuilder("generateurbille"));
                EstEnregistre = false;
            }
        }

        /// <summary>
        /// private void trou_CheckedChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction détermine si l'option est sélectionné ou pas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trou_CheckedChanged(object sender, EventArgs e)
        {
            if (trou.Checked)
            {
                _etat = new EtatCreation(this, new StringBuilder("trou"));
                EstEnregistre = false;
            }
        }

        /// <summary>
        /// private void paletteGaucheJ1_CheckedChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction détermine si l'option est sélectionné ou pas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void paletteGaucheJ1_CheckedChanged(object sender, EventArgs e)
        {
            if (paletteGaucheJ1.Checked)
            {
                _etat = new EtatCreation(this, new StringBuilder("paletteGaucheJ1"));
                EstEnregistre = false;
            }
        }

        /// <summary>
        /// private void paletteDroiteJ1_CheckedChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction détermine si l'option est sélectionné ou pas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void paletteDroiteJ1_CheckedChanged(object sender, EventArgs e)
        {
            if (paletteDroiteJ1.Checked)
            {
                _etat = new EtatCreation(this, new StringBuilder("paletteDroitJ1"));
                EstEnregistre = false;
            }
        }

        /// <summary>
        /// private void paletteDroiteJ2_CheckedChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction détermine si l'option est sélectionné ou pas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void paletteDroiteJ2_CheckedChanged(object sender, EventArgs e)
        {
            if (paletteDroiteJ2.Checked)
            {
                _etat = new EtatCreation(this, new StringBuilder("paletteDroitJ2"));
                EstEnregistre = false;
            }
        }

        /// <summary>
        /// private void paletteGaucheJ2_CheckedChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction détermine si l'option est sélectionné ou pas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void paletteGaucheJ2_CheckedChanged(object sender, EventArgs e)
        {
            if (paletteGaucheJ2.Checked)
            {
                _etat = new EtatCreation(this, new StringBuilder("paletteGaucheJ2"));
                EstEnregistre = false;
            }
        }

        /// <summary>
        /// private void portail_CheckedChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction détermine si l'option est sélectionné ou pas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void portail_CheckedChanged(object sender, EventArgs e)
        {
            if (portail.Checked)
            {
                toolStripStatusLabel2.Text = "Attention! Il faut ajouter les portails en paires!";
                _etat = new EtatCreation(this, new StringBuilder("portail"));
            }
            else if (((EtatCreation)_etat).EstEnCreation())
            {
                NativeFunction.annulerCreation();
                MettreAJour();
            }
            EstEnregistre = false;

        }

        /// <summary>
        /// private void mur_CheckedChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction détermine si l'option est sélectionné ou pas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mur_CheckedChanged(object sender, EventArgs e)
        {
            if (mur.Checked)
                _etat = new EtatCreation(this, new StringBuilder("mur"));
            else if (((EtatCreation)_etat).EstEnCreation())
            {
                NativeFunction.annulerCreation();
                MettreAJour();
            }
            EstEnregistre = false;
        }

        /// <summary>
        /// private void zoom_CheckedChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction détermine si l'option est sélectionné ou pas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zoom_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Cursor = Cursors.Arrow;
            if (zoom.Checked)
                _etat = new EtatZoom(this);
        }

        /// <summary>
        /// private void sélectionToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction change l'état d'édition selon l'option sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option désirée
        /// </param>
        private void sélectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selection.Checked = true;
            _etat = new EtatSelection(this);
        }

        /// <summary>
        /// private void déplacementToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction change l'état d'édition selon l'option sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option désirée
        /// </param>
        private void déplacementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deplacement.Checked = true;
            _etat = new EtatDeplacement(this);
        }

        /// <summary>
        /// private void rotationToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction change l'état d'édition selon l'option sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option désirée
        /// </param>
        private void rotationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rotation.Checked = true;
            _etat = new EtatRotation(this);
        }

        /// <summary>
        /// private void miseÀÉchelleToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction change l'état d'édition selon l'option sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option désirée
        /// </param>
        private void miseÀÉchelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            miseEchelle.Checked = true;
            _etat = new EtatEchelle(this);
        }

        /// <summary>
        /// private void duplicationToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction change l'état d'édition selon l'option sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option désirée
        /// </param>
        private void duplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Duplication.Checked = true;
            _etat = new EtatDuplication(this);

        }

        /// <summary>
        /// private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction change l'état d'édition selon l'option sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option désirée
        /// </param>
        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoom.Checked = true;
            _etat = new EtatZoom(this);
        }

        /// <summary>
        ///  private void butoirCirculaireToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction change l'état d'édition selon l'option sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option désirée
        /// </param>
        private void butoirCirculaireToolStripMenuItem_Click(object sender, EventArgs e)
        {
            butoirCercle.Checked = true;
            _etat = new EtatCreation(this, new StringBuilder("butoirCirculaire"));
        }

        /// <summary>
        /// private void cibleToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction change l'état d'édition selon l'option sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option désirée
        /// </param>
        private void cibleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cible.Checked = true;
            _etat = new EtatCreation(this, new StringBuilder("cible"));

        }

        /// <summary>
        /// private void ressortToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction change l'état d'édition selon l'option sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option désirée
        /// </param>
        private void ressortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ressort.Checked = true;
            _etat = new EtatCreation(this, new StringBuilder("ressort"));
        }

        /// <summary>
        /// private void générateurBilleToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction change l'état d'édition selon l'option sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option désirée
        /// </param>
        private void générateurBilleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            generateurBille.Checked = true;
            _etat = new EtatCreation(this, new StringBuilder("generateurbille"));
        }

        /// <summary>
        /// private void trouToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction change l'état d'édition selon l'option sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option désirée
        /// </param>
        private void trouToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trou.Checked = true;
            _etat = new EtatCreation(this, new StringBuilder("trou"));
        }

        /// <summary>
        /// private void portailToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction change l'état d'édition selon l'option sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option désirée
        /// </param>
        private void portailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            portail.Checked = true;
            _etat = new EtatCreation(this, new StringBuilder("portail"));
        }

        /// <summary>
        /// private void murToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction change l'état d'édition selon l'option sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option désirée
        /// </param>
        private void murToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mur.Checked = true;
            _etat = new EtatCreation(this, new StringBuilder("mur"));
        }


        /// <summary>
        /// public void afficherPanelEditionObjet(bool visible)
        /// 
        /// Cette fonction affiche le panel d'édition d'objets
        /// </summary>
        /// <param name="visible">
        /// Si le panel est visible ou non
        /// </param>
        public void AfficherPanelEditionObjet(bool visible)
        {
            panelEditionObjet.Visible = visible;
        }

        /// <summary>
        /// private void paletteDroiteJ1ToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction change l'état d'édition selon l'option sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option désirée
        /// </param>
        private void paletteDroiteJ1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            paletteDroiteJ1.Checked = true;
            _etat = new EtatCreation(this, new StringBuilder("paletteDroitJ1"));
        }

        /// <summary>
        /// private void paletteGaucheJ1ToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction change l'état d'édition selon l'option sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option désirée
        /// </param>
        private void paletteGaucheJ1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            paletteGaucheJ1.Checked = true;
            _etat = new EtatCreation(this, new StringBuilder("paletteGaucheJ1"));
        }

        /// <summary>
        /// private void paletteDroiteJ2ToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction change l'état d'édition selon l'option sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option désirée
        /// </param>
        private void paletteDroiteJ2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            paletteDroiteJ2.Checked = true;
            _etat = new EtatCreation(this, new StringBuilder("paletteDroitJ2"));
        }

        /// <summary>
        /// private void paletteGaucheJ2ToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction change l'état d'édition selon l'option sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option désirée
        /// </param>
        private void paletteGaucheJ2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            paletteGaucheJ2.Checked = true;
            _etat = new EtatCreation(this, new StringBuilder("paletteGaucheJ2"));
        }

        /// <summary>
        /// private void butoirTriangulaireDroitToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction change l'état d'édition selon l'option sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option désirée
        /// </param>
        private void butoirTriangulaireDroitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            butoirTriangulaireDroit.Checked = true;
            _etat = new EtatCreation(this, new StringBuilder("butoirTriangleDroit"));
        }

        /// <summary>
        /// private void butoirTriangulaireGaucheToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction change l'état d'édition selon l'option sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option désirée
        /// </param>
        private void butoirTriangulaireGaucheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            butoirTriangulaireGauche.Checked = true;
            _etat = new EtatCreation(this, new StringBuilder("butoirTriangleGauche"));
        }

        /// <summary>
        /// public void assignerBoutonSupprimer(bool b)
        /// 
        /// Cette fonction active ou désactive le bouton Supprimer
        /// </summary>
        /// <param name="b">
        /// Si le bouton est activé ou pas
        /// </param>
        public void AssignerBoutonSupprimer(bool b)
        {
            supprimerToolStripMenuItem.Enabled = b;
        }

        #endregion


        #region Evenements panel objet unique
        /// <summary>
        /// private void supprimerToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction change l'état d'édition selon l'option sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au clic sur l'option désirée
        /// </param>
        private void supprimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NativeFunction.supprimerSelection();
            toolStripStatusLabel2.Text = "Attention! La zone de jeu doit contenir au moins un ressort, un trou et un générateur de bille.";
            AfficherPanelEditionObjet(false);
            supprimerToolStripMenuItem.Enabled = false;
            MettreAJour();
        }

        /// <summary>
        /// private void rotationObjet_Scroll(object sender, EventArgs e)
        /// 
        /// Cette fonction applique la rotation avec un glisseur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au déplacement du curseur du glisseur
        /// </param>
        private void rotationObjet_Scroll(object sender, EventArgs e)
        {
            if (_attributsModifiables)
            {
                NativeFunction.assignerCentreSelection();
                NativeFunction.rotaterSelection(rotationObjet.Value, _lastRotate);
                _lastRotate = rotationObjet.Value;
                rotationUpDown.Value = _lastRotate;
                MettreAJour();
                MettreAJourRotation();
            }
        }

        /// <summary>
        /// private void facteurEchelleObjet_Scroll(object sender, EventArgs e)
        /// 
        /// Cette fonction applique le redimensionnement avec un glisseur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// L'évènement relié au déplacement du curseur du glisseur
        /// </param>
        private void facteurEchelleObjet_Scroll(object sender, EventArgs e)
        {
            if (_attributsModifiables)
            {
                NativeFunction.redimensionnerSelection(facteurEchelleObjet.Value * 10, _lastScale * 10);
                _lastScale = facteurEchelleObjet.Value;
                scaleUpDown.Value = (decimal)(_lastScale / 10.0);
                MettreAJour();
                MettreAJourScale();
            }
        }

        /// <summary>
        /// public void mettreAJourRotation()
        /// 
        /// Cette fonction met à jour le niveau de rotation sur le glisseur
        /// </summary>
        public void MettreAJourRotation()
        {
            _attributsModifiables = false;
            var angle = NativeFunction.obtenirRotationSelection();

            angle = -(angle % 360);
            if (angle < 0)
                angle = 360 + angle;
            _lastRotate = ((int)angle) % 360;
            rotationObjet.Value = _lastRotate;
            rotationUpDown.Value = (decimal)(_lastRotate);
            _attributsModifiables = true;
        }

        /// <summary>
        /// public void mettreAJourScale()
        /// 
        /// Cette fonction met à jour le niveau d'agrandissement sur le glisseur
        /// </summary>
        public void MettreAJourScale()
        {
            _attributsModifiables = false;
            var echelle = NativeFunction.obtenirEchelleSelection();

            _lastScale = (int)(echelle * 10);
            facteurEchelleObjet.Value = (int)(echelle * 10);
            scaleUpDown.Value = (decimal)echelle;
            _attributsModifiables = true;
        }

        /// <summary>
        /// public void mettreAJourPosition()
        /// 
        /// Cette fonction met à jour la position de la sélection sur le panel de modifications
        /// </summary>
        public void MettreAJourPosition()
        {
            _attributsModifiables = false;
            var posX = NativeFunction.obtenirPosXSel();
            var posY = NativeFunction.obtenirPosYSel();

            posXUpDown.Value = (decimal)posX;
            posYUpDown.Value = (decimal)posY;
            _attributsModifiables = true;
        }

        /// <summary>
        /// public void mettreAJourParametres()
        /// 
        /// Mise à jour des différentes modifications appliquées sur la sélection
        /// </summary>
        public void MettreAJourParametres()
        {
            MettreAJourPosition();
            MettreAJourRotation();
            MettreAJourScale();
        }

        /// <summary>
        /// private void scaleUpDown_ValueChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction applique le redimensionnement sur la sélection avec le glisseur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scaleUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (_attributsModifiables)
            {
                NativeFunction.redimensionnerSelection((int)(scaleUpDown.Value * 100), _lastScale * 10);
                _lastScale = (int)(scaleUpDown.Value * 10);
                facteurEchelleObjet.Value = _lastScale;
                MettreAJour();
                MettreAJourScale();
            }
        }


        /// <summary>
        ///  private void rotationUpDown_ValueChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction applique la rotation sur la sélection avec le glisseur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rotationUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (_attributsModifiables)
            {
                NativeFunction.assignerCentreSelection();
                NativeFunction.rotaterSelection((int)rotationUpDown.Value, _lastRotate);
                _lastRotate = (int)rotationUpDown.Value;
                rotationObjet.Value = _lastRotate;
                MettreAJour();
                MettreAJourRotation();
            }
        }


        /// <summary>
        /// private void posXUpDown_ValueChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction applique le déplacement en x sur la sélection 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void posXUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (_attributsModifiables)
            {
                NativeFunction.assignerSelPosition((double)posXUpDown.Value, (double)posYUpDown.Value);
                MettreAJour();
                MettreAJourPosition();
            }
        }

        /// <summary>
        /// private void posYUpDown_ValueChanged(object sender, EventArgs e)
        /// 
        /// Cette fonction applique le déplacement en y sur la sélection 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void posYUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (_attributsModifiables)
            {
                NativeFunction.assignerSelPosition((double)posXUpDown.Value, (double)posYUpDown.Value);
                MettreAJour();
                MettreAJourPosition();
            }
        }


        #endregion


        #region evenements mode test

        /// <summary>
        /// private void modeTestToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction change à l'état mode test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AssignerMode(Mode.ModeTest);
            NativeFunction.reinitialiser();
            NativeFunction.demarrerPartie();

        }


        #endregion

        /// <summary>
        /// private void menuPrincipalToolStripMenuItem1_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction ouvre le menu principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuPrincipalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _enPause = true;

            if (CurrentMode == Mode.ModeEditeur || CurrentMode == Mode.ModeTest)
            {
                if (!EstEnregistre)
                {
                    if (CurrentMode == Mode.ModeTest)
                    {
                        _enPause = true;
                        NativeFunction.reinitialiser();
                    }
                    _enregistrer = new Enregistrer();
                    _enregistrer.ShowDialog();
                }
            }
            else
            {
                if (WantToQuit())
                {
                    NativeFunction.arreterSons();
                    NativeFunction.quitterPartie();
                    EventManager.Instance.Notice(new EndGameEvent(EndGameType.Forfeit));
                    // TODO: Trouver pourquoi arreterSon prend une éternité en mode editeur
                }
                else
                {
                    _enPause = false;
                    return;
                }
            }
            Hide();
            //Todo check comment effectuer transition
            //Program.afficherMenuPrincipal();
            EventManager.Instance.Notice(new ChangeStateEvent() { NextState = Enums.States.MainMenu });
        }

        /// <summary>
        /// private void modeÉditionToolStripMenuItem_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction passe en mode éditeur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modeÉditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AssignerMode(Mode.ModeEditeur);
            NativeFunction.reinitialiser();
        }

        /// <summary>
        /// private void orthographiqueToolStripMenuItem1_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction active la vue orthographique
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void orthographiqueToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            NativeFunction.cameraOrtho();
            Redimensionner();
        }

        /// <summary>
        /// private void orbiteToolStripMenuItem1_Click(object sender, EventArgs e)
        /// 
        /// Cette fonction active la vue orbite
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void orbiteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            NativeFunction.cameraOrbite();
            Redimensionner();
        }

        private void IntegratedOpenGL_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CurrentMode == Mode.ModePartieRapide || CurrentMode == Mode.ModeCampagne)
            {
                if (WantToQuit())
                {
                    NativeFunction.quitterPartie();
                    EventManager.Instance.Notice(new EndGameEvent(EndGameType.Forfeit));
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else if (CurrentMode == Mode.ModeEditeur || CurrentMode == Mode.ModeTest)
            {
                EventManager.Instance.Notice(new ChangeStateEvent() { NextState = Enums.States.MainMenu });
            }
        }

        private bool WantToQuit()
        {
            var window = MessageBox.Show("Êtes vous vraiment le type de princesse qui abandonne?", "Êtes vous certaine?", MessageBoxButtons.YesNo);
            return window == DialogResult.Yes;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void champForce_CheckedChanged(object sender, EventArgs e)
        {
            if (ChampDeForce.Checked)
            {
                _etat = new EtatCreation(this, new StringBuilder("champForce"));
                EstEnregistre = false;
            }
        }

        private void plateauDArgent_CheckedChanged(object sender, EventArgs e)
        {
            if (PlateauArgent.Checked)
            {
                _etat = new EtatCreation(this, new StringBuilder("plateauDArgent"));
                EstEnregistre = false;
            }
        }

        private void plateauDargentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlateauArgent.Checked = true;
            _etat = new EtatCreation(this, new StringBuilder("plateauDArgent"));
        }

        private void champDeForceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChampDeForce.Checked = true;
            _etat = new EtatCreation(this, new StringBuilder("champForce"));
        }

        private void refaireTutorielToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowTutorial = true;

            if (User.Instance.IsConnected)
            {
                ProfileAccess.Instance.SetEditorTutorialVisibility(true);
                Profile.Instance.CurrentProfile.ShowEditorTutorial = true;
            }
        }
    }

}