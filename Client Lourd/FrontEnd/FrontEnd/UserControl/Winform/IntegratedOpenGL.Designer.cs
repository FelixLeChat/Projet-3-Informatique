using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FrontEnd.Properties;

namespace FrontEnd.UserControl.Winform
{
    partial class IntegratedOpenGl
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ChampDeForce = new System.Windows.Forms.RadioButton();
            this.PlateauArgent = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.paletteGaucheJ2 = new System.Windows.Forms.RadioButton();
            this.paletteDroiteJ2 = new System.Windows.Forms.RadioButton();
            this.mur = new System.Windows.Forms.RadioButton();
            this.paletteGaucheJ1 = new System.Windows.Forms.RadioButton();
            this.paletteDroiteJ1 = new System.Windows.Forms.RadioButton();
            this.portail = new System.Windows.Forms.RadioButton();
            this.butoirTriangulaireDroit = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.butoirCercle = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.butoirTriangulaireGauche = new System.Windows.Forms.RadioButton();
            this.trou = new System.Windows.Forms.RadioButton();
            this.generateurBille = new System.Windows.Forms.RadioButton();
            this.zoom = new System.Windows.Forms.RadioButton();
            this.selection = new System.Windows.Forms.RadioButton();
            this.deplacement = new System.Windows.Forms.RadioButton();
            this.rotation = new System.Windows.Forms.RadioButton();
            this.miseEchelle = new System.Windows.Forms.RadioButton();
            this.Duplication = new System.Windows.Forms.RadioButton();
            this.ressort = new System.Windows.Forms.RadioButton();
            this.cible = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CampagneInfo = new System.Windows.Forms.Panel();
            this.PointsCampagne = new System.Windows.Forms.Label();
            this.DifficulteCampagne = new System.Windows.Forms.Label();
            this.NomCampagne = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.CampagneNomLabel = new System.Windows.Forms.Label();
            this.CampagneTitre = new System.Windows.Forms.Label();
            this.ballPanel = new System.Windows.Forms.Panel();
            this.ballTitle = new System.Windows.Forms.Label();
            this.Player1BallLabel = new System.Windows.Forms.Label();
            this.Player2BallLabel = new System.Windows.Forms.Label();
            this.Player4BallLabel = new System.Windows.Forms.Label();
            this.Player3BallLabel = new System.Windows.Forms.Label();
            this.pointPanel = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.Player1PointLabel = new System.Windows.Forms.Label();
            this.Player2PointLabel = new System.Windows.Forms.Label();
            this.Player4PointLabel = new System.Windows.Forms.Label();
            this.Player3PointLabel = new System.Windows.Forms.Label();
            this.panelEditionObjet = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.posYUpDown = new System.Windows.Forms.NumericUpDown();
            this.posXUpDown = new System.Windows.Forms.NumericUpDown();
            this.scaleUpDown = new System.Windows.Forms.NumericUpDown();
            this.rotationUpDown = new System.Windows.Forms.NumericUpDown();
            this.facteurEchelleObjet = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rotationObjet = new System.Windows.Forms.TrackBar();
            this.panel3 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.retourEditeurToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPrincipalToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.modeÉditionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changerCaméraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orthographiqueToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.orbiteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fichierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nouveauToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ouvrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enregistrerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enregistrerSousToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propriétésToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modeTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPrincipalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refaireTutorielToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.éditionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.supprimerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outilsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sélectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.déplacementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miseÀÉchelleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.duplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.créationDobjetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.butoirCirculaireToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.butoirTriangulaireToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.butoirTriangulaireDroitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.butoirTriangulaireGaucheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cibleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ressortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.générateurBilleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trouToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.paletteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.paletteDroiteJ1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.paletteGaucheJ1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.paletteDroiteJ2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.paletteGaucheJ2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.portailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.murToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plateauDargentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.champDeForceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orthographiqueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orbiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.CampagneInfo.SuspendLayout();
            this.ballPanel.SuspendLayout();
            this.pointPanel.SuspendLayout();
            this.panelEditionObjet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.posYUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.posXUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotationUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.facteurEchelleObjet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotationObjet)).BeginInit();
            this.panel3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 74);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 643F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1229, 642);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(239)))), ((int)(((byte)(244)))));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1229, 642);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.ChampDeForce);
            this.panel2.Controls.Add(this.PlateauArgent);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.paletteGaucheJ2);
            this.panel2.Controls.Add(this.paletteDroiteJ2);
            this.panel2.Controls.Add(this.mur);
            this.panel2.Controls.Add(this.paletteGaucheJ1);
            this.panel2.Controls.Add(this.paletteDroiteJ1);
            this.panel2.Controls.Add(this.portail);
            this.panel2.Controls.Add(this.butoirTriangulaireDroit);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.butoirCercle);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.butoirTriangulaireGauche);
            this.panel2.Controls.Add(this.trou);
            this.panel2.Controls.Add(this.generateurBille);
            this.panel2.Controls.Add(this.zoom);
            this.panel2.Controls.Add(this.selection);
            this.panel2.Controls.Add(this.deplacement);
            this.panel2.Controls.Add(this.rotation);
            this.panel2.Controls.Add(this.miseEchelle);
            this.panel2.Controls.Add(this.Duplication);
            this.panel2.Controls.Add(this.ressort);
            this.panel2.Controls.Add(this.cible);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(4, 5);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(190, 632);
            this.panel2.TabIndex = 2;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // ChampDeForce
            // 
            this.ChampDeForce.Appearance = System.Windows.Forms.Appearance.Button;
            this.ChampDeForce.BackColor = System.Drawing.Color.Transparent;
            this.ChampDeForce.BackgroundImage = global::FrontEnd.Properties.Resources.ChampDeForce;
            this.ChampDeForce.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ChampDeForce.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChampDeForce.Location = new System.Drawing.Point(86, 580);
            this.ChampDeForce.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ChampDeForce.Name = "ChampDeForce";
            this.ChampDeForce.Size = new System.Drawing.Size(60, 61);
            this.ChampDeForce.TabIndex = 25;
            this.ChampDeForce.UseVisualStyleBackColor = false;
            this.ChampDeForce.CheckedChanged += new System.EventHandler(this.champForce_CheckedChanged);
            // 
            // PlateauArgent
            // 
            this.PlateauArgent.Appearance = System.Windows.Forms.Appearance.Button;
            this.PlateauArgent.BackColor = System.Drawing.Color.Transparent;
            this.PlateauArgent.BackgroundImage = global::FrontEnd.Properties.Resources.PlateauDArgent;
            this.PlateauArgent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PlateauArgent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlateauArgent.Location = new System.Drawing.Point(18, 580);
            this.PlateauArgent.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PlateauArgent.Name = "PlateauArgent";
            this.PlateauArgent.Size = new System.Drawing.Size(60, 61);
            this.PlateauArgent.TabIndex = 24;
            this.PlateauArgent.UseVisualStyleBackColor = false;
            this.PlateauArgent.CheckedChanged += new System.EventHandler(this.plateauDArgent_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(21, 886);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(124, 22);
            this.label8.TabIndex = 21;
            this.label8.Text = "Outils de vue";
            // 
            // paletteGaucheJ2
            // 
            this.paletteGaucheJ2.Appearance = System.Windows.Forms.Appearance.Button;
            this.paletteGaucheJ2.BackgroundImage = global::FrontEnd.Properties.Resources.PaletteGaucheJ2;
            this.paletteGaucheJ2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.paletteGaucheJ2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.paletteGaucheJ2.Location = new System.Drawing.Point(18, 799);
            this.paletteGaucheJ2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.paletteGaucheJ2.Name = "paletteGaucheJ2";
            this.paletteGaucheJ2.Size = new System.Drawing.Size(60, 62);
            this.paletteGaucheJ2.TabIndex = 0;
            this.paletteGaucheJ2.UseVisualStyleBackColor = true;
            this.paletteGaucheJ2.CheckedChanged += new System.EventHandler(this.paletteGaucheJ2_CheckedChanged);
            // 
            // paletteDroiteJ2
            // 
            this.paletteDroiteJ2.Appearance = System.Windows.Forms.Appearance.Button;
            this.paletteDroiteJ2.BackgroundImage = global::FrontEnd.Properties.Resources.PaletteDroitJ2;
            this.paletteDroiteJ2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.paletteDroiteJ2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.paletteDroiteJ2.Location = new System.Drawing.Point(86, 797);
            this.paletteDroiteJ2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.paletteDroiteJ2.Name = "paletteDroiteJ2";
            this.paletteDroiteJ2.Size = new System.Drawing.Size(60, 63);
            this.paletteDroiteJ2.TabIndex = 1;
            this.paletteDroiteJ2.UseVisualStyleBackColor = true;
            this.paletteDroiteJ2.CheckedChanged += new System.EventHandler(this.paletteDroiteJ2_CheckedChanged);
            // 
            // mur
            // 
            this.mur.Appearance = System.Windows.Forms.Appearance.Button;
            this.mur.BackgroundImage = global::FrontEnd.Properties.Resources.Mur1;
            this.mur.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.mur.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mur.Location = new System.Drawing.Point(50, 651);
            this.mur.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.mur.Name = "mur";
            this.mur.Size = new System.Drawing.Size(60, 61);
            this.mur.TabIndex = 16;
            this.mur.UseVisualStyleBackColor = true;
            this.mur.CheckedChanged += new System.EventHandler(this.mur_CheckedChanged);
            // 
            // paletteGaucheJ1
            // 
            this.paletteGaucheJ1.Appearance = System.Windows.Forms.Appearance.Button;
            this.paletteGaucheJ1.BackgroundImage = global::FrontEnd.Properties.Resources.PaletteGaucheJ1;
            this.paletteGaucheJ1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.paletteGaucheJ1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.paletteGaucheJ1.Location = new System.Drawing.Point(18, 722);
            this.paletteGaucheJ1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.paletteGaucheJ1.Name = "paletteGaucheJ1";
            this.paletteGaucheJ1.Size = new System.Drawing.Size(60, 66);
            this.paletteGaucheJ1.TabIndex = 14;
            this.paletteGaucheJ1.UseVisualStyleBackColor = true;
            this.paletteGaucheJ1.CheckedChanged += new System.EventHandler(this.paletteGaucheJ1_CheckedChanged);
            // 
            // paletteDroiteJ1
            // 
            this.paletteDroiteJ1.Appearance = System.Windows.Forms.Appearance.Button;
            this.paletteDroiteJ1.BackgroundImage = global::FrontEnd.Properties.Resources.PaletteDroitJ1;
            this.paletteDroiteJ1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.paletteDroiteJ1.Cursor = System.Windows.Forms.Cursors.Default;
            this.paletteDroiteJ1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.paletteDroiteJ1.Location = new System.Drawing.Point(87, 722);
            this.paletteDroiteJ1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.paletteDroiteJ1.Name = "paletteDroiteJ1";
            this.paletteDroiteJ1.Size = new System.Drawing.Size(60, 66);
            this.paletteDroiteJ1.TabIndex = 0;
            this.paletteDroiteJ1.UseVisualStyleBackColor = true;
            this.paletteDroiteJ1.CheckedChanged += new System.EventHandler(this.paletteDroiteJ1_CheckedChanged);
            // 
            // portail
            // 
            this.portail.Appearance = System.Windows.Forms.Appearance.Button;
            this.portail.BackColor = System.Drawing.Color.Transparent;
            this.portail.BackgroundImage = global::FrontEnd.Properties.Resources.Portail1;
            this.portail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.portail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.portail.Location = new System.Drawing.Point(87, 509);
            this.portail.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.portail.Name = "portail";
            this.portail.Size = new System.Drawing.Size(60, 61);
            this.portail.TabIndex = 15;
            this.portail.UseVisualStyleBackColor = false;
            this.portail.CheckedChanged += new System.EventHandler(this.portail_CheckedChanged);
            // 
            // butoirTriangulaireDroit
            // 
            this.butoirTriangulaireDroit.Appearance = System.Windows.Forms.Appearance.Button;
            this.butoirTriangulaireDroit.BackColor = System.Drawing.Color.Transparent;
            this.butoirTriangulaireDroit.BackgroundImage = global::FrontEnd.Properties.Resources.ButoirTriangleDroit;
            this.butoirTriangulaireDroit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.butoirTriangulaireDroit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butoirTriangulaireDroit.Location = new System.Drawing.Point(87, 439);
            this.butoirTriangulaireDroit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.butoirTriangulaireDroit.Name = "butoirTriangulaireDroit";
            this.butoirTriangulaireDroit.Size = new System.Drawing.Size(60, 61);
            this.butoirTriangulaireDroit.TabIndex = 9;
            this.butoirTriangulaireDroit.UseVisualStyleBackColor = false;
            this.butoirTriangulaireDroit.CheckedChanged += new System.EventHandler(this.butoirTriangulaireDroit_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(50, 268);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 22);
            this.label7.TabIndex = 20;
            this.label7.Text = "Objets";
            // 
            // butoirCercle
            // 
            this.butoirCercle.Appearance = System.Windows.Forms.Appearance.Button;
            this.butoirCercle.BackgroundImage = global::FrontEnd.Properties.Resources.ButoirCirculaire;
            this.butoirCercle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.butoirCercle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butoirCercle.Location = new System.Drawing.Point(87, 368);
            this.butoirCercle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.butoirCercle.Name = "butoirCercle";
            this.butoirCercle.Size = new System.Drawing.Size(60, 61);
            this.butoirCercle.TabIndex = 8;
            this.butoirCercle.UseVisualStyleBackColor = true;
            this.butoirCercle.CheckedChanged += new System.EventHandler(this.butoirCercle_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(14, 29);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(138, 22);
            this.label6.TabIndex = 19;
            this.label6.Text = "Outils d\'édition";
            // 
            // butoirTriangulaireGauche
            // 
            this.butoirTriangulaireGauche.Appearance = System.Windows.Forms.Appearance.Button;
            this.butoirTriangulaireGauche.BackgroundImage = global::FrontEnd.Properties.Resources.ButoirTriangleGauche;
            this.butoirTriangulaireGauche.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.butoirTriangulaireGauche.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butoirTriangulaireGauche.Location = new System.Drawing.Point(18, 439);
            this.butoirTriangulaireGauche.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.butoirTriangulaireGauche.Name = "butoirTriangulaireGauche";
            this.butoirTriangulaireGauche.Size = new System.Drawing.Size(60, 61);
            this.butoirTriangulaireGauche.TabIndex = 18;
            this.butoirTriangulaireGauche.UseVisualStyleBackColor = true;
            this.butoirTriangulaireGauche.CheckedChanged += new System.EventHandler(this.butoirTriangulaireGauche_CheckedChanged);
            // 
            // trou
            // 
            this.trou.Appearance = System.Windows.Forms.Appearance.Button;
            this.trou.BackgroundImage = global::FrontEnd.Properties.Resources.Trou1;
            this.trou.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.trou.Location = new System.Drawing.Point(18, 368);
            this.trou.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trou.Name = "trou";
            this.trou.Size = new System.Drawing.Size(60, 61);
            this.trou.TabIndex = 13;
            this.trou.UseVisualStyleBackColor = true;
            this.trou.CheckedChanged += new System.EventHandler(this.trou_CheckedChanged);
            // 
            // generateurBille
            // 
            this.generateurBille.Appearance = System.Windows.Forms.Appearance.Button;
            this.generateurBille.BackgroundImage = global::FrontEnd.Properties.Resources.Generateur;
            this.generateurBille.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.generateurBille.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.generateurBille.Location = new System.Drawing.Point(86, 297);
            this.generateurBille.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.generateurBille.Name = "generateurBille";
            this.generateurBille.Size = new System.Drawing.Size(60, 61);
            this.generateurBille.TabIndex = 12;
            this.generateurBille.UseVisualStyleBackColor = true;
            this.generateurBille.CheckedChanged += new System.EventHandler(this.generateurBille_CheckedChanged);
            // 
            // zoom
            // 
            this.zoom.Appearance = System.Windows.Forms.Appearance.Button;
            this.zoom.BackgroundImage = global::FrontEnd.Properties.Resources.zoom;
            this.zoom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.zoom.Location = new System.Drawing.Point(50, 923);
            this.zoom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.zoom.Name = "zoom";
            this.zoom.Size = new System.Drawing.Size(60, 61);
            this.zoom.TabIndex = 17;
            this.zoom.UseVisualStyleBackColor = true;
            this.zoom.CheckedChanged += new System.EventHandler(this.zoom_CheckedChanged);
            // 
            // selection
            // 
            this.selection.Appearance = System.Windows.Forms.Appearance.Button;
            this.selection.BackgroundImage = global::FrontEnd.Properties.Resources.curseur;
            this.selection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.selection.Checked = true;
            this.selection.Cursor = System.Windows.Forms.Cursors.Default;
            this.selection.Location = new System.Drawing.Point(18, 59);
            this.selection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selection.Name = "selection";
            this.selection.Size = new System.Drawing.Size(60, 61);
            this.selection.TabIndex = 3;
            this.selection.TabStop = true;
            this.selection.UseVisualStyleBackColor = true;
            this.selection.CheckedChanged += new System.EventHandler(this.selection_CheckedChanged);
            // 
            // deplacement
            // 
            this.deplacement.Appearance = System.Windows.Forms.Appearance.Button;
            this.deplacement.BackgroundImage = global::FrontEnd.Properties.Resources.deplacement;
            this.deplacement.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.deplacement.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.deplacement.Enabled = false;
            this.deplacement.Location = new System.Drawing.Point(86, 59);
            this.deplacement.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.deplacement.Name = "deplacement";
            this.deplacement.Size = new System.Drawing.Size(60, 61);
            this.deplacement.TabIndex = 4;
            this.deplacement.UseVisualStyleBackColor = true;
            this.deplacement.CheckedChanged += new System.EventHandler(this.deplacement_CheckedChanged);
            // 
            // rotation
            // 
            this.rotation.Appearance = System.Windows.Forms.Appearance.Button;
            this.rotation.BackgroundImage = global::FrontEnd.Properties.Resources.rot;
            this.rotation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rotation.Enabled = false;
            this.rotation.Location = new System.Drawing.Point(18, 123);
            this.rotation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rotation.Name = "rotation";
            this.rotation.Size = new System.Drawing.Size(60, 61);
            this.rotation.TabIndex = 5;
            this.rotation.UseVisualStyleBackColor = true;
            this.rotation.CheckedChanged += new System.EventHandler(this.rotation_CheckedChanged);
            // 
            // miseEchelle
            // 
            this.miseEchelle.Appearance = System.Windows.Forms.Appearance.Button;
            this.miseEchelle.BackgroundImage = global::FrontEnd.Properties.Resources.scale;
            this.miseEchelle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.miseEchelle.Enabled = false;
            this.miseEchelle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.miseEchelle.Location = new System.Drawing.Point(86, 123);
            this.miseEchelle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.miseEchelle.Name = "miseEchelle";
            this.miseEchelle.Size = new System.Drawing.Size(60, 61);
            this.miseEchelle.TabIndex = 6;
            this.miseEchelle.UseVisualStyleBackColor = true;
            this.miseEchelle.CheckedChanged += new System.EventHandler(this.miseEchelle_CheckedChanged);
            // 
            // Duplication
            // 
            this.Duplication.Appearance = System.Windows.Forms.Appearance.Button;
            this.Duplication.BackColor = System.Drawing.Color.White;
            this.Duplication.BackgroundImage = global::FrontEnd.Properties.Resources.duplication;
            this.Duplication.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Duplication.Enabled = false;
            this.Duplication.Location = new System.Drawing.Point(54, 194);
            this.Duplication.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Duplication.Name = "Duplication";
            this.Duplication.Size = new System.Drawing.Size(60, 61);
            this.Duplication.TabIndex = 7;
            this.Duplication.UseVisualStyleBackColor = false;
            this.Duplication.CheckedChanged += new System.EventHandler(this.Duplication_CheckedChanged);
            // 
            // ressort
            // 
            this.ressort.Appearance = System.Windows.Forms.Appearance.Button;
            this.ressort.BackColor = System.Drawing.Color.Transparent;
            this.ressort.BackgroundImage = global::FrontEnd.Properties.Resources.Ressort2;
            this.ressort.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ressort.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ressort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ressort.Location = new System.Drawing.Point(18, 297);
            this.ressort.Margin = new System.Windows.Forms.Padding(0);
            this.ressort.Name = "ressort";
            this.ressort.Size = new System.Drawing.Size(60, 61);
            this.ressort.TabIndex = 11;
            this.ressort.UseVisualStyleBackColor = false;
            this.ressort.CheckedChanged += new System.EventHandler(this.ressort_CheckedChanged);
            // 
            // cible
            // 
            this.cible.Appearance = System.Windows.Forms.Appearance.Button;
            this.cible.BackColor = System.Drawing.Color.Transparent;
            this.cible.BackgroundImage = global::FrontEnd.Properties.Resources.PtitCroteJone;
            this.cible.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cible.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cible.Location = new System.Drawing.Point(18, 509);
            this.cible.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cible.Name = "cible";
            this.cible.Size = new System.Drawing.Size(60, 61);
            this.cible.TabIndex = 10;
            this.cible.UseVisualStyleBackColor = false;
            this.cible.CheckedChanged += new System.EventHandler(this.cible_CheckedChanged);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(198, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1031, 642);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.CampagneInfo);
            this.panel1.Controls.Add(this.ballPanel);
            this.panel1.Controls.Add(this.pointPanel);
            this.panel1.Controls.Add(this.panelEditionObjet);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.MinimumSize = new System.Drawing.Size(150, 154);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1031, 602);
            this.panel1.TabIndex = 1;
            // 
            // CampagneInfo
            // 
            this.CampagneInfo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CampagneInfo.BackColor = System.Drawing.Color.Pink;
            this.CampagneInfo.CausesValidation = false;
            this.CampagneInfo.Controls.Add(this.PointsCampagne);
            this.CampagneInfo.Controls.Add(this.DifficulteCampagne);
            this.CampagneInfo.Controls.Add(this.NomCampagne);
            this.CampagneInfo.Controls.Add(this.label11);
            this.CampagneInfo.Controls.Add(this.label10);
            this.CampagneInfo.Controls.Add(this.CampagneNomLabel);
            this.CampagneInfo.Controls.Add(this.CampagneTitre);
            this.CampagneInfo.Location = new System.Drawing.Point(288, 173);
            this.CampagneInfo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CampagneInfo.MaximumSize = new System.Drawing.Size(456, 206);
            this.CampagneInfo.Name = "CampagneInfo";
            this.CampagneInfo.Size = new System.Drawing.Size(456, 206);
            this.CampagneInfo.TabIndex = 5;
            // 
            // PointsCampagne
            // 
            this.PointsCampagne.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PointsCampagne.Font = new System.Drawing.Font("Monotype Corsiva", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PointsCampagne.ForeColor = System.Drawing.Color.DeepPink;
            this.PointsCampagne.Location = new System.Drawing.Point(223, 160);
            this.PointsCampagne.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PointsCampagne.Name = "PointsCampagne";
            this.PointsCampagne.Size = new System.Drawing.Size(199, 35);
            this.PointsCampagne.TabIndex = 7;
            this.PointsCampagne.Text = "Nom";
            this.PointsCampagne.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DifficulteCampagne
            // 
            this.DifficulteCampagne.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DifficulteCampagne.Font = new System.Drawing.Font("Monotype Corsiva", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DifficulteCampagne.ForeColor = System.Drawing.Color.DeepPink;
            this.DifficulteCampagne.Location = new System.Drawing.Point(142, 114);
            this.DifficulteCampagne.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.DifficulteCampagne.Name = "DifficulteCampagne";
            this.DifficulteCampagne.Size = new System.Drawing.Size(280, 35);
            this.DifficulteCampagne.TabIndex = 6;
            this.DifficulteCampagne.Text = "Nom";
            this.DifficulteCampagne.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NomCampagne
            // 
            this.NomCampagne.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NomCampagne.Font = new System.Drawing.Font("Monotype Corsiva", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NomCampagne.ForeColor = System.Drawing.Color.DeepPink;
            this.NomCampagne.Location = new System.Drawing.Point(91, 68);
            this.NomCampagne.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.NomCampagne.Name = "NomCampagne";
            this.NomCampagne.Size = new System.Drawing.Size(332, 35);
            this.NomCampagne.TabIndex = 5;
            this.NomCampagne.Text = "Nom";
            this.NomCampagne.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Monotype Corsiva", 14F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.DeepPink;
            this.label11.Location = new System.Drawing.Point(24, 160);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(211, 34);
            this.label11.TabIndex = 3;
            this.label11.Text = "Points a atteindre";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Monotype Corsiva", 14F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.DeepPink;
            this.label10.Location = new System.Drawing.Point(24, 114);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(126, 34);
            this.label10.TabIndex = 2;
            this.label10.Text = "Difficulté";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CampagneNomLabel
            // 
            this.CampagneNomLabel.AutoSize = true;
            this.CampagneNomLabel.Font = new System.Drawing.Font("Monotype Corsiva", 14F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CampagneNomLabel.ForeColor = System.Drawing.Color.DeepPink;
            this.CampagneNomLabel.Location = new System.Drawing.Point(24, 68);
            this.CampagneNomLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CampagneNomLabel.Name = "CampagneNomLabel";
            this.CampagneNomLabel.Size = new System.Drawing.Size(66, 34);
            this.CampagneNomLabel.TabIndex = 1;
            this.CampagneNomLabel.Text = "Nom";
            this.CampagneNomLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CampagneTitre
            // 
            this.CampagneTitre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CampagneTitre.AutoSize = true;
            this.CampagneTitre.Font = new System.Drawing.Font("Monotype Corsiva", 20F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CampagneTitre.ForeColor = System.Drawing.Color.DeepPink;
            this.CampagneTitre.Location = new System.Drawing.Point(51, 12);
            this.CampagneTitre.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CampagneTitre.Name = "CampagneTitre";
            this.CampagneTitre.Size = new System.Drawing.Size(383, 49);
            this.CampagneTitre.TabIndex = 0;
            this.CampagneTitre.Text = "Informations de la zone";
            this.CampagneTitre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ballPanel
            // 
            this.ballPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ballPanel.AutoSize = true;
            this.ballPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(239)))), ((int)(((byte)(244)))));
            this.ballPanel.Controls.Add(this.ballTitle);
            this.ballPanel.Controls.Add(this.Player1BallLabel);
            this.ballPanel.Controls.Add(this.Player2BallLabel);
            this.ballPanel.Controls.Add(this.Player4BallLabel);
            this.ballPanel.Controls.Add(this.Player3BallLabel);
            this.ballPanel.Location = new System.Drawing.Point(800, 14);
            this.ballPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ballPanel.Name = "ballPanel";
            this.ballPanel.Size = new System.Drawing.Size(217, 281);
            this.ballPanel.TabIndex = 4;
            // 
            // ballTitle
            // 
            this.ballTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ballTitle.BackColor = System.Drawing.Color.Transparent;
            this.ballTitle.Font = new System.Drawing.Font("Monotype Corsiva", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ballTitle.Location = new System.Drawing.Point(15, 6);
            this.ballTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ballTitle.Name = "ballTitle";
            this.ballTitle.Size = new System.Drawing.Size(188, 50);
            this.ballTitle.TabIndex = 0;
            this.ballTitle.Text = "Balles";
            this.ballTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Player1BallLabel
            // 
            this.Player1BallLabel.BackColor = System.Drawing.Color.Transparent;
            this.Player1BallLabel.Font = new System.Drawing.Font("Monotype Corsiva", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player1BallLabel.ForeColor = System.Drawing.Color.Black;
            this.Player1BallLabel.Location = new System.Drawing.Point(8, 61);
            this.Player1BallLabel.Name = "Player1BallLabel";
            this.Player1BallLabel.Size = new System.Drawing.Size(201, 41);
            this.Player1BallLabel.TabIndex = 2;
            this.Player1BallLabel.Text = "ball";
            this.Player1BallLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Player2BallLabel
            // 
            this.Player2BallLabel.BackColor = System.Drawing.Color.Aqua;
            this.Player2BallLabel.Font = new System.Drawing.Font("Monotype Corsiva", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player2BallLabel.ForeColor = System.Drawing.Color.Black;
            this.Player2BallLabel.Location = new System.Drawing.Point(8, 115);
            this.Player2BallLabel.Name = "Player2BallLabel";
            this.Player2BallLabel.Size = new System.Drawing.Size(201, 41);
            this.Player2BallLabel.TabIndex = 2;
            this.Player2BallLabel.Text = "ball";
            this.Player2BallLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Player4BallLabel
            // 
            this.Player4BallLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(51)))));
            this.Player4BallLabel.Font = new System.Drawing.Font("Monotype Corsiva", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player4BallLabel.ForeColor = System.Drawing.Color.Black;
            this.Player4BallLabel.Location = new System.Drawing.Point(8, 223);
            this.Player4BallLabel.Name = "Player4BallLabel";
            this.Player4BallLabel.Size = new System.Drawing.Size(201, 41);
            this.Player4BallLabel.TabIndex = 2;
            this.Player4BallLabel.Text = "ball";
            this.Player4BallLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Player3BallLabel
            // 
            this.Player3BallLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(255)))));
            this.Player3BallLabel.Font = new System.Drawing.Font("Monotype Corsiva", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player3BallLabel.ForeColor = System.Drawing.Color.White;
            this.Player3BallLabel.Location = new System.Drawing.Point(8, 169);
            this.Player3BallLabel.Name = "Player3BallLabel";
            this.Player3BallLabel.Size = new System.Drawing.Size(201, 41);
            this.Player3BallLabel.TabIndex = 2;
            this.Player3BallLabel.Text = "ball";
            this.Player3BallLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pointPanel
            // 
            this.pointPanel.AutoSize = true;
            this.pointPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(239)))), ((int)(((byte)(244)))));
            this.pointPanel.Controls.Add(this.label9);
            this.pointPanel.Controls.Add(this.Player1PointLabel);
            this.pointPanel.Controls.Add(this.Player2PointLabel);
            this.pointPanel.Controls.Add(this.Player4PointLabel);
            this.pointPanel.Controls.Add(this.Player3PointLabel);
            this.pointPanel.Location = new System.Drawing.Point(13, 14);
            this.pointPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pointPanel.Name = "pointPanel";
            this.pointPanel.Size = new System.Drawing.Size(223, 281);
            this.pointPanel.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Monotype Corsiva", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(8, 6);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(204, 50);
            this.label9.TabIndex = 0;
            this.label9.Text = "Pointages";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Player1PointLabel
            // 
            this.Player1PointLabel.BackColor = System.Drawing.Color.Transparent;
            this.Player1PointLabel.Font = new System.Drawing.Font("Monotype Corsiva", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player1PointLabel.ForeColor = System.Drawing.Color.Black;
            this.Player1PointLabel.Location = new System.Drawing.Point(15, 67);
            this.Player1PointLabel.Name = "Player1PointLabel";
            this.Player1PointLabel.Size = new System.Drawing.Size(191, 41);
            this.Player1PointLabel.TabIndex = 2;
            this.Player1PointLabel.Text = "Points : ";
            this.Player1PointLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Player2PointLabel
            // 
            this.Player2PointLabel.BackColor = System.Drawing.Color.Aqua;
            this.Player2PointLabel.Font = new System.Drawing.Font("Monotype Corsiva", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player2PointLabel.ForeColor = System.Drawing.Color.Black;
            this.Player2PointLabel.Location = new System.Drawing.Point(15, 119);
            this.Player2PointLabel.Name = "Player2PointLabel";
            this.Player2PointLabel.Size = new System.Drawing.Size(191, 41);
            this.Player2PointLabel.TabIndex = 2;
            this.Player2PointLabel.Text = "Point : ";
            this.Player2PointLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Player4PointLabel
            // 
            this.Player4PointLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(51)))));
            this.Player4PointLabel.Font = new System.Drawing.Font("Monotype Corsiva", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player4PointLabel.ForeColor = System.Drawing.Color.Black;
            this.Player4PointLabel.Location = new System.Drawing.Point(15, 222);
            this.Player4PointLabel.Name = "Player4PointLabel";
            this.Player4PointLabel.Size = new System.Drawing.Size(191, 41);
            this.Player4PointLabel.TabIndex = 2;
            this.Player4PointLabel.Text = "Point : ";
            this.Player4PointLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Player3PointLabel
            // 
            this.Player3PointLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(255)))));
            this.Player3PointLabel.Font = new System.Drawing.Font("Monotype Corsiva", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player3PointLabel.ForeColor = System.Drawing.Color.White;
            this.Player3PointLabel.Location = new System.Drawing.Point(15, 170);
            this.Player3PointLabel.Name = "Player3PointLabel";
            this.Player3PointLabel.Size = new System.Drawing.Size(191, 41);
            this.Player3PointLabel.TabIndex = 2;
            this.Player3PointLabel.Text = "Point : ";
            this.Player3PointLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelEditionObjet
            // 
            this.panelEditionObjet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEditionObjet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(239)))), ((int)(((byte)(244)))));
            this.panelEditionObjet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelEditionObjet.Controls.Add(this.label5);
            this.panelEditionObjet.Controls.Add(this.label4);
            this.panelEditionObjet.Controls.Add(this.posYUpDown);
            this.panelEditionObjet.Controls.Add(this.posXUpDown);
            this.panelEditionObjet.Controls.Add(this.scaleUpDown);
            this.panelEditionObjet.Controls.Add(this.rotationUpDown);
            this.panelEditionObjet.Controls.Add(this.facteurEchelleObjet);
            this.panelEditionObjet.Controls.Add(this.label3);
            this.panelEditionObjet.Controls.Add(this.label2);
            this.panelEditionObjet.Controls.Add(this.label1);
            this.panelEditionObjet.Controls.Add(this.rotationObjet);
            this.panelEditionObjet.Location = new System.Drawing.Point(771, 26);
            this.panelEditionObjet.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.panelEditionObjet.Name = "panelEditionObjet";
            this.panelEditionObjet.Size = new System.Drawing.Size(233, 337);
            this.panelEditionObjet.TabIndex = 0;
            this.panelEditionObjet.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(54, 95);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 20);
            this.label5.TabIndex = 14;
            this.label5.Text = "Y:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(54, 48);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 20);
            this.label4.TabIndex = 13;
            this.label4.Text = "X:";
            // 
            // posYUpDown
            // 
            this.posYUpDown.DecimalPlaces = 1;
            this.posYUpDown.Location = new System.Drawing.Point(97, 89);
            this.posYUpDown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.posYUpDown.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.posYUpDown.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            -2147483648});
            this.posYUpDown.Name = "posYUpDown";
            this.posYUpDown.Size = new System.Drawing.Size(78, 26);
            this.posYUpDown.TabIndex = 12;
            // 
            // posXUpDown
            // 
            this.posXUpDown.DecimalPlaces = 1;
            this.posXUpDown.Location = new System.Drawing.Point(97, 41);
            this.posXUpDown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.posXUpDown.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.posXUpDown.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            -2147483648});
            this.posXUpDown.Name = "posXUpDown";
            this.posXUpDown.Size = new System.Drawing.Size(78, 26);
            this.posXUpDown.TabIndex = 11;
            // 
            // scaleUpDown
            // 
            this.scaleUpDown.DecimalPlaces = 1;
            this.scaleUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.scaleUpDown.Location = new System.Drawing.Point(166, 265);
            this.scaleUpDown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.scaleUpDown.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.scaleUpDown.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.scaleUpDown.Name = "scaleUpDown";
            this.scaleUpDown.Size = new System.Drawing.Size(65, 26);
            this.scaleUpDown.TabIndex = 10;
            this.scaleUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // rotationUpDown
            // 
            this.rotationUpDown.Location = new System.Drawing.Point(166, 163);
            this.rotationUpDown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rotationUpDown.Maximum = new decimal(new int[] {
            359,
            0,
            0,
            0});
            this.rotationUpDown.Name = "rotationUpDown";
            this.rotationUpDown.Size = new System.Drawing.Size(65, 26);
            this.rotationUpDown.TabIndex = 9;
            // 
            // facteurEchelleObjet
            // 
            this.facteurEchelleObjet.Location = new System.Drawing.Point(6, 265);
            this.facteurEchelleObjet.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.facteurEchelleObjet.Maximum = 20;
            this.facteurEchelleObjet.Minimum = 5;
            this.facteurEchelleObjet.Name = "facteurEchelleObjet";
            this.facteurEchelleObjet.Size = new System.Drawing.Size(156, 69);
            this.facteurEchelleObjet.TabIndex = 4;
            this.facteurEchelleObjet.Value = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 240);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "Facteur d\'échelle :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 12);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Position :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 139);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Rotation :";
            // 
            // rotationObjet
            // 
            this.rotationObjet.AllowDrop = true;
            this.rotationObjet.LargeChange = 30;
            this.rotationObjet.Location = new System.Drawing.Point(6, 163);
            this.rotationObjet.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rotationObjet.Maximum = 359;
            this.rotationObjet.Name = "rotationObjet";
            this.rotationObjet.Size = new System.Drawing.Size(156, 69);
            this.rotationObjet.SmallChange = 15;
            this.rotationObjet.TabIndex = 0;
            this.rotationObjet.TickFrequency = 15;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.statusStrip1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 605);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1025, 34);
            this.panel3.TabIndex = 2;
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(3, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1025, 34);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Comic Sans MS", 6F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(443, 29);
            this.toolStripStatusLabel2.Text = "Aucun objet sélectionné : seulement l\'outil d\'édition \'Sélection\' est activé!";
            // 
            // menuStrip2
            // 
            this.menuStrip2.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip2.BackgroundImage = global::FrontEnd.Properties.Resources.Background;
            this.menuStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.retourEditeurToolStripMenuItem,
            this.changerCaméraToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 37);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Padding = new System.Windows.Forms.Padding(14, 4, 0, 4);
            this.menuStrip2.Size = new System.Drawing.Size(1229, 37);
            this.menuStrip2.TabIndex = 3;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // retourEditeurToolStripMenuItem
            // 
            this.retourEditeurToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuPrincipalToolStripMenuItem1,
            this.modeÉditionToolStripMenuItem});
            this.retourEditeurToolStripMenuItem.Name = "retourEditeurToolStripMenuItem";
            this.retourEditeurToolStripMenuItem.Size = new System.Drawing.Size(74, 29);
            this.retourEditeurToolStripMenuItem.Text = "Fichier";
            // 
            // menuPrincipalToolStripMenuItem1
            // 
            this.menuPrincipalToolStripMenuItem1.Name = "menuPrincipalToolStripMenuItem1";
            this.menuPrincipalToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.menuPrincipalToolStripMenuItem1.Size = new System.Drawing.Size(278, 30);
            this.menuPrincipalToolStripMenuItem1.Text = "Menu Principal";
            this.menuPrincipalToolStripMenuItem1.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.menuPrincipalToolStripMenuItem1.Click += new System.EventHandler(this.menuPrincipalToolStripMenuItem1_Click);
            // 
            // modeÉditionToolStripMenuItem
            // 
            this.modeÉditionToolStripMenuItem.Name = "modeÉditionToolStripMenuItem";
            this.modeÉditionToolStripMenuItem.ShortcutKeyDisplayString = "T";
            this.modeÉditionToolStripMenuItem.Size = new System.Drawing.Size(278, 30);
            this.modeÉditionToolStripMenuItem.Text = "Mode édition";
            this.modeÉditionToolStripMenuItem.Click += new System.EventHandler(this.modeÉditionToolStripMenuItem_Click);
            // 
            // changerCaméraToolStripMenuItem
            // 
            this.changerCaméraToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.orthographiqueToolStripMenuItem1,
            this.orbiteToolStripMenuItem1});
            this.changerCaméraToolStripMenuItem.Name = "changerCaméraToolStripMenuItem";
            this.changerCaméraToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
            this.changerCaméraToolStripMenuItem.Text = "Vue";
            // 
            // orthographiqueToolStripMenuItem1
            // 
            this.orthographiqueToolStripMenuItem1.Name = "orthographiqueToolStripMenuItem1";
            this.orthographiqueToolStripMenuItem1.ShortcutKeyDisplayString = "1";
            this.orthographiqueToolStripMenuItem1.Size = new System.Drawing.Size(247, 30);
            this.orthographiqueToolStripMenuItem1.Text = "Orthographique";
            this.orthographiqueToolStripMenuItem1.Click += new System.EventHandler(this.orthographiqueToolStripMenuItem1_Click);
            // 
            // orbiteToolStripMenuItem1
            // 
            this.orbiteToolStripMenuItem1.Name = "orbiteToolStripMenuItem1";
            this.orbiteToolStripMenuItem1.ShortcutKeyDisplayString = "2";
            this.orbiteToolStripMenuItem1.Size = new System.Drawing.Size(247, 30);
            this.orbiteToolStripMenuItem1.Text = "Orbite";
            this.orbiteToolStripMenuItem1.Click += new System.EventHandler(this.orbiteToolStripMenuItem1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.BackgroundImage = global::FrontEnd.Properties.Resources.Background;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fichierToolStripMenuItem,
            this.éditionToolStripMenuItem,
            this.outilsToolStripMenuItem,
            this.vuesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(14, 4, 0, 4);
            this.menuStrip1.Size = new System.Drawing.Size(1229, 37);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fichierToolStripMenuItem
            // 
            this.fichierToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nouveauToolStripMenuItem,
            this.ouvrirToolStripMenuItem,
            this.enregistrerToolStripMenuItem,
            this.enregistrerSousToolStripMenuItem,
            this.propriétésToolStripMenuItem,
            this.modeTestToolStripMenuItem,
            this.menuPrincipalToolStripMenuItem,
            this.refaireTutorielToolStripMenuItem});
            this.fichierToolStripMenuItem.Name = "fichierToolStripMenuItem";
            this.fichierToolStripMenuItem.Size = new System.Drawing.Size(74, 29);
            this.fichierToolStripMenuItem.Text = "Fichier";
            // 
            // nouveauToolStripMenuItem
            // 
            this.nouveauToolStripMenuItem.Name = "nouveauToolStripMenuItem";
            this.nouveauToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.nouveauToolStripMenuItem.Size = new System.Drawing.Size(274, 30);
            this.nouveauToolStripMenuItem.Text = "Nouveau";
            this.nouveauToolStripMenuItem.Click += new System.EventHandler(this.nouveauToolStripMenuItem_Click);
            // 
            // ouvrirToolStripMenuItem
            // 
            this.ouvrirToolStripMenuItem.Name = "ouvrirToolStripMenuItem";
            this.ouvrirToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.ouvrirToolStripMenuItem.Size = new System.Drawing.Size(274, 30);
            this.ouvrirToolStripMenuItem.Text = "Ouvrir";
            this.ouvrirToolStripMenuItem.Click += new System.EventHandler(this.ouvrirToolStripMenuItem_Click);
            // 
            // enregistrerToolStripMenuItem
            // 
            this.enregistrerToolStripMenuItem.Name = "enregistrerToolStripMenuItem";
            this.enregistrerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.enregistrerToolStripMenuItem.Size = new System.Drawing.Size(274, 30);
            this.enregistrerToolStripMenuItem.Text = "Enregistrer";
            this.enregistrerToolStripMenuItem.Click += new System.EventHandler(this.enregistrerToolStripMenuItem_Click);
            // 
            // enregistrerSousToolStripMenuItem
            // 
            this.enregistrerSousToolStripMenuItem.Name = "enregistrerSousToolStripMenuItem";
            this.enregistrerSousToolStripMenuItem.Size = new System.Drawing.Size(274, 30);
            this.enregistrerSousToolStripMenuItem.Text = "Enregistrer sous";
            this.enregistrerSousToolStripMenuItem.Click += new System.EventHandler(this.enregistrerSousToolStripMenuItem_Click);
            // 
            // propriétésToolStripMenuItem
            // 
            this.propriétésToolStripMenuItem.Name = "propriétésToolStripMenuItem";
            this.propriétésToolStripMenuItem.Size = new System.Drawing.Size(274, 30);
            this.propriétésToolStripMenuItem.Text = "Propriétés";
            this.propriétésToolStripMenuItem.Click += new System.EventHandler(this.propriétésToolStripMenuItem_Click);
            // 
            // modeTestToolStripMenuItem
            // 
            this.modeTestToolStripMenuItem.Name = "modeTestToolStripMenuItem";
            this.modeTestToolStripMenuItem.ShortcutKeyDisplayString = "T";
            this.modeTestToolStripMenuItem.Size = new System.Drawing.Size(274, 30);
            this.modeTestToolStripMenuItem.Text = "Mode test";
            this.modeTestToolStripMenuItem.Click += new System.EventHandler(this.modeTestToolStripMenuItem_Click);
            // 
            // menuPrincipalToolStripMenuItem
            // 
            this.menuPrincipalToolStripMenuItem.Name = "menuPrincipalToolStripMenuItem";
            this.menuPrincipalToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl-Q";
            this.menuPrincipalToolStripMenuItem.Size = new System.Drawing.Size(274, 30);
            this.menuPrincipalToolStripMenuItem.Text = "Menu principal";
            this.menuPrincipalToolStripMenuItem.Click += new System.EventHandler(this.menuPrincipalToolStripMenuItem_Click);
            // 
            // refaireTutorielToolStripMenuItem
            // 
            this.refaireTutorielToolStripMenuItem.Name = "refaireTutorielToolStripMenuItem";
            this.refaireTutorielToolStripMenuItem.Size = new System.Drawing.Size(274, 30);
            this.refaireTutorielToolStripMenuItem.Text = "Refaire Tutoriel";
            this.refaireTutorielToolStripMenuItem.Click += new System.EventHandler(this.refaireTutorielToolStripMenuItem_Click);
            // 
            // éditionToolStripMenuItem
            // 
            this.éditionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.supprimerToolStripMenuItem});
            this.éditionToolStripMenuItem.Name = "éditionToolStripMenuItem";
            this.éditionToolStripMenuItem.Size = new System.Drawing.Size(79, 29);
            this.éditionToolStripMenuItem.Text = "Édition";
            // 
            // supprimerToolStripMenuItem
            // 
            this.supprimerToolStripMenuItem.Name = "supprimerToolStripMenuItem";
            this.supprimerToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.supprimerToolStripMenuItem.Size = new System.Drawing.Size(218, 30);
            this.supprimerToolStripMenuItem.Text = "Supprimer";
            this.supprimerToolStripMenuItem.Click += new System.EventHandler(this.supprimerToolStripMenuItem_Click);
            // 
            // outilsToolStripMenuItem
            // 
            this.outilsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sélectionToolStripMenuItem,
            this.déplacementToolStripMenuItem,
            this.rotationToolStripMenuItem,
            this.miseÀÉchelleToolStripMenuItem,
            this.duplicationToolStripMenuItem,
            this.zoomToolStripMenuItem,
            this.créationDobjetsToolStripMenuItem});
            this.outilsToolStripMenuItem.Name = "outilsToolStripMenuItem";
            this.outilsToolStripMenuItem.Size = new System.Drawing.Size(70, 29);
            this.outilsToolStripMenuItem.Text = "Outils";
            // 
            // sélectionToolStripMenuItem
            // 
            this.sélectionToolStripMenuItem.Name = "sélectionToolStripMenuItem";
            this.sélectionToolStripMenuItem.ShortcutKeyDisplayString = "S";
            this.sélectionToolStripMenuItem.Size = new System.Drawing.Size(232, 30);
            this.sélectionToolStripMenuItem.Text = "Sélection ";
            this.sélectionToolStripMenuItem.Click += new System.EventHandler(this.sélectionToolStripMenuItem_Click);
            // 
            // déplacementToolStripMenuItem
            // 
            this.déplacementToolStripMenuItem.Name = "déplacementToolStripMenuItem";
            this.déplacementToolStripMenuItem.ShortcutKeyDisplayString = "D";
            this.déplacementToolStripMenuItem.Size = new System.Drawing.Size(232, 30);
            this.déplacementToolStripMenuItem.Text = "Déplacement";
            this.déplacementToolStripMenuItem.Click += new System.EventHandler(this.déplacementToolStripMenuItem_Click);
            // 
            // rotationToolStripMenuItem
            // 
            this.rotationToolStripMenuItem.Name = "rotationToolStripMenuItem";
            this.rotationToolStripMenuItem.ShortcutKeyDisplayString = "R";
            this.rotationToolStripMenuItem.Size = new System.Drawing.Size(232, 30);
            this.rotationToolStripMenuItem.Text = "Rotation";
            this.rotationToolStripMenuItem.Click += new System.EventHandler(this.rotationToolStripMenuItem_Click);
            // 
            // miseÀÉchelleToolStripMenuItem
            // 
            this.miseÀÉchelleToolStripMenuItem.Name = "miseÀÉchelleToolStripMenuItem";
            this.miseÀÉchelleToolStripMenuItem.ShortcutKeyDisplayString = "E";
            this.miseÀÉchelleToolStripMenuItem.Size = new System.Drawing.Size(232, 30);
            this.miseÀÉchelleToolStripMenuItem.Text = "Mise à échelle";
            this.miseÀÉchelleToolStripMenuItem.Click += new System.EventHandler(this.miseÀÉchelleToolStripMenuItem_Click);
            // 
            // duplicationToolStripMenuItem
            // 
            this.duplicationToolStripMenuItem.Name = "duplicationToolStripMenuItem";
            this.duplicationToolStripMenuItem.ShortcutKeyDisplayString = "C";
            this.duplicationToolStripMenuItem.Size = new System.Drawing.Size(232, 30);
            this.duplicationToolStripMenuItem.Text = "Duplication";
            this.duplicationToolStripMenuItem.Click += new System.EventHandler(this.duplicationToolStripMenuItem_Click);
            // 
            // zoomToolStripMenuItem
            // 
            this.zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
            this.zoomToolStripMenuItem.ShortcutKeyDisplayString = "Z";
            this.zoomToolStripMenuItem.Size = new System.Drawing.Size(232, 30);
            this.zoomToolStripMenuItem.Text = "Zoom";
            this.zoomToolStripMenuItem.Click += new System.EventHandler(this.zoomToolStripMenuItem_Click);
            // 
            // créationDobjetsToolStripMenuItem
            // 
            this.créationDobjetsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.butoirCirculaireToolStripMenuItem,
            this.butoirTriangulaireToolStripMenuItem,
            this.cibleToolStripMenuItem,
            this.ressortToolStripMenuItem,
            this.générateurBilleToolStripMenuItem,
            this.trouToolStripMenuItem,
            this.paletteToolStripMenuItem,
            this.portailToolStripMenuItem,
            this.murToolStripMenuItem,
            this.plateauDargentToolStripMenuItem,
            this.champDeForceToolStripMenuItem});
            this.créationDobjetsToolStripMenuItem.Name = "créationDobjetsToolStripMenuItem";
            this.créationDobjetsToolStripMenuItem.Size = new System.Drawing.Size(232, 30);
            this.créationDobjetsToolStripMenuItem.Text = "Création d\'objets";
            // 
            // butoirCirculaireToolStripMenuItem
            // 
            this.butoirCirculaireToolStripMenuItem.Name = "butoirCirculaireToolStripMenuItem";
            this.butoirCirculaireToolStripMenuItem.Size = new System.Drawing.Size(237, 30);
            this.butoirCirculaireToolStripMenuItem.Text = "Butoir circulaire";
            this.butoirCirculaireToolStripMenuItem.Click += new System.EventHandler(this.butoirCirculaireToolStripMenuItem_Click);
            // 
            // butoirTriangulaireToolStripMenuItem
            // 
            this.butoirTriangulaireToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.butoirTriangulaireDroitToolStripMenuItem,
            this.butoirTriangulaireGaucheToolStripMenuItem});
            this.butoirTriangulaireToolStripMenuItem.Name = "butoirTriangulaireToolStripMenuItem";
            this.butoirTriangulaireToolStripMenuItem.Size = new System.Drawing.Size(237, 30);
            this.butoirTriangulaireToolStripMenuItem.Text = "Butoir triangulaire";
            // 
            // butoirTriangulaireDroitToolStripMenuItem
            // 
            this.butoirTriangulaireDroitToolStripMenuItem.Name = "butoirTriangulaireDroitToolStripMenuItem";
            this.butoirTriangulaireDroitToolStripMenuItem.Size = new System.Drawing.Size(299, 30);
            this.butoirTriangulaireDroitToolStripMenuItem.Text = "Butoir triangulaire droit";
            this.butoirTriangulaireDroitToolStripMenuItem.Click += new System.EventHandler(this.butoirTriangulaireDroitToolStripMenuItem_Click);
            // 
            // butoirTriangulaireGaucheToolStripMenuItem
            // 
            this.butoirTriangulaireGaucheToolStripMenuItem.Name = "butoirTriangulaireGaucheToolStripMenuItem";
            this.butoirTriangulaireGaucheToolStripMenuItem.Size = new System.Drawing.Size(299, 30);
            this.butoirTriangulaireGaucheToolStripMenuItem.Text = "Butoir triangulaire gauche";
            this.butoirTriangulaireGaucheToolStripMenuItem.Click += new System.EventHandler(this.butoirTriangulaireGaucheToolStripMenuItem_Click);
            // 
            // cibleToolStripMenuItem
            // 
            this.cibleToolStripMenuItem.Name = "cibleToolStripMenuItem";
            this.cibleToolStripMenuItem.Size = new System.Drawing.Size(237, 30);
            this.cibleToolStripMenuItem.Text = "Cible";
            this.cibleToolStripMenuItem.Click += new System.EventHandler(this.cibleToolStripMenuItem_Click);
            // 
            // ressortToolStripMenuItem
            // 
            this.ressortToolStripMenuItem.Name = "ressortToolStripMenuItem";
            this.ressortToolStripMenuItem.Size = new System.Drawing.Size(237, 30);
            this.ressortToolStripMenuItem.Text = "Ressort";
            this.ressortToolStripMenuItem.Click += new System.EventHandler(this.ressortToolStripMenuItem_Click);
            // 
            // générateurBilleToolStripMenuItem
            // 
            this.générateurBilleToolStripMenuItem.Name = "générateurBilleToolStripMenuItem";
            this.générateurBilleToolStripMenuItem.Size = new System.Drawing.Size(237, 30);
            this.générateurBilleToolStripMenuItem.Text = "Générateur bille";
            this.générateurBilleToolStripMenuItem.Click += new System.EventHandler(this.générateurBilleToolStripMenuItem_Click);
            // 
            // trouToolStripMenuItem
            // 
            this.trouToolStripMenuItem.Name = "trouToolStripMenuItem";
            this.trouToolStripMenuItem.Size = new System.Drawing.Size(237, 30);
            this.trouToolStripMenuItem.Text = "Trou";
            this.trouToolStripMenuItem.Click += new System.EventHandler(this.trouToolStripMenuItem_Click);
            // 
            // paletteToolStripMenuItem
            // 
            this.paletteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.paletteDroiteJ1ToolStripMenuItem,
            this.paletteGaucheJ1ToolStripMenuItem,
            this.paletteDroiteJ2ToolStripMenuItem,
            this.paletteGaucheJ2ToolStripMenuItem});
            this.paletteToolStripMenuItem.Name = "paletteToolStripMenuItem";
            this.paletteToolStripMenuItem.Size = new System.Drawing.Size(237, 30);
            this.paletteToolStripMenuItem.Text = "Palette";
            // 
            // paletteDroiteJ1ToolStripMenuItem
            // 
            this.paletteDroiteJ1ToolStripMenuItem.Name = "paletteDroiteJ1ToolStripMenuItem";
            this.paletteDroiteJ1ToolStripMenuItem.Size = new System.Drawing.Size(232, 30);
            this.paletteDroiteJ1ToolStripMenuItem.Text = "Palette droite J1";
            this.paletteDroiteJ1ToolStripMenuItem.Click += new System.EventHandler(this.paletteDroiteJ1ToolStripMenuItem_Click);
            // 
            // paletteGaucheJ1ToolStripMenuItem
            // 
            this.paletteGaucheJ1ToolStripMenuItem.Name = "paletteGaucheJ1ToolStripMenuItem";
            this.paletteGaucheJ1ToolStripMenuItem.Size = new System.Drawing.Size(232, 30);
            this.paletteGaucheJ1ToolStripMenuItem.Text = "Palette gauche J1";
            this.paletteGaucheJ1ToolStripMenuItem.Click += new System.EventHandler(this.paletteGaucheJ1ToolStripMenuItem_Click);
            // 
            // paletteDroiteJ2ToolStripMenuItem
            // 
            this.paletteDroiteJ2ToolStripMenuItem.Name = "paletteDroiteJ2ToolStripMenuItem";
            this.paletteDroiteJ2ToolStripMenuItem.Size = new System.Drawing.Size(232, 30);
            this.paletteDroiteJ2ToolStripMenuItem.Text = "Palette droite J2";
            this.paletteDroiteJ2ToolStripMenuItem.Click += new System.EventHandler(this.paletteDroiteJ2ToolStripMenuItem_Click);
            // 
            // paletteGaucheJ2ToolStripMenuItem
            // 
            this.paletteGaucheJ2ToolStripMenuItem.Name = "paletteGaucheJ2ToolStripMenuItem";
            this.paletteGaucheJ2ToolStripMenuItem.Size = new System.Drawing.Size(232, 30);
            this.paletteGaucheJ2ToolStripMenuItem.Text = "Palette gauche J2";
            this.paletteGaucheJ2ToolStripMenuItem.Click += new System.EventHandler(this.paletteGaucheJ2ToolStripMenuItem_Click);
            // 
            // portailToolStripMenuItem
            // 
            this.portailToolStripMenuItem.Name = "portailToolStripMenuItem";
            this.portailToolStripMenuItem.Size = new System.Drawing.Size(237, 30);
            this.portailToolStripMenuItem.Text = "Portail";
            this.portailToolStripMenuItem.Click += new System.EventHandler(this.portailToolStripMenuItem_Click);
            // 
            // murToolStripMenuItem
            // 
            this.murToolStripMenuItem.Name = "murToolStripMenuItem";
            this.murToolStripMenuItem.Size = new System.Drawing.Size(237, 30);
            this.murToolStripMenuItem.Text = "Mur";
            this.murToolStripMenuItem.Click += new System.EventHandler(this.murToolStripMenuItem_Click);
            // 
            // plateauDargentToolStripMenuItem
            // 
            this.plateauDargentToolStripMenuItem.Name = "plateauDargentToolStripMenuItem";
            this.plateauDargentToolStripMenuItem.Size = new System.Drawing.Size(237, 30);
            this.plateauDargentToolStripMenuItem.Text = "Plateau d\'argent";
            this.plateauDargentToolStripMenuItem.Click += new System.EventHandler(this.plateauDargentToolStripMenuItem_Click);
            // 
            // champDeForceToolStripMenuItem
            // 
            this.champDeForceToolStripMenuItem.Name = "champDeForceToolStripMenuItem";
            this.champDeForceToolStripMenuItem.Size = new System.Drawing.Size(237, 30);
            this.champDeForceToolStripMenuItem.Text = "Champ de Force";
            this.champDeForceToolStripMenuItem.Click += new System.EventHandler(this.champDeForceToolStripMenuItem_Click);
            // 
            // vuesToolStripMenuItem
            // 
            this.vuesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.orthographiqueToolStripMenuItem,
            this.orbiteToolStripMenuItem});
            this.vuesToolStripMenuItem.Name = "vuesToolStripMenuItem";
            this.vuesToolStripMenuItem.Size = new System.Drawing.Size(62, 29);
            this.vuesToolStripMenuItem.Text = "Vues";
            // 
            // orthographiqueToolStripMenuItem
            // 
            this.orthographiqueToolStripMenuItem.Name = "orthographiqueToolStripMenuItem";
            this.orthographiqueToolStripMenuItem.ShortcutKeyDisplayString = "1";
            this.orthographiqueToolStripMenuItem.Size = new System.Drawing.Size(247, 30);
            this.orthographiqueToolStripMenuItem.Text = "Orthographique";
            // 
            // orbiteToolStripMenuItem
            // 
            this.orbiteToolStripMenuItem.Name = "orbiteToolStripMenuItem";
            this.orbiteToolStripMenuItem.ShortcutKeyDisplayString = "2";
            this.orbiteToolStripMenuItem.Size = new System.Drawing.Size(247, 30);
            this.orbiteToolStripMenuItem.Text = "Orbite";
            // 
            // IntegratedOpenGl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1229, 716);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip2);
            this.Controls.Add(this.menuStrip1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(751, 402);
            this.Name = "IntegratedOpenGl";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IntegratedOpenGL_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.CampagneInfo.ResumeLayout(false);
            this.CampagneInfo.PerformLayout();
            this.ballPanel.ResumeLayout(false);
            this.pointPanel.ResumeLayout(false);
            this.panelEditionObjet.ResumeLayout(false);
            this.panelEditionObjet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.posYUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.posXUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotationUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.facteurEchelleObjet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotationObjet)).EndInit();
            this.panel3.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ToolStripMenuItem fichierToolStripMenuItem;
        private ToolStripMenuItem nouveauToolStripMenuItem;
        private ToolStripMenuItem ouvrirToolStripMenuItem;
        private ToolStripMenuItem enregistrerToolStripMenuItem;
        private ToolStripMenuItem enregistrerSousToolStripMenuItem;
        private ToolStripMenuItem propriétésToolStripMenuItem;
        private ToolStripMenuItem modeTestToolStripMenuItem;
        private ToolStripMenuItem menuPrincipalToolStripMenuItem;
        private ToolStripMenuItem éditionToolStripMenuItem;
        private ToolStripMenuItem supprimerToolStripMenuItem;
        private ToolStripMenuItem outilsToolStripMenuItem;
        private ToolStripMenuItem sélectionToolStripMenuItem;
        private ToolStripMenuItem déplacementToolStripMenuItem;
        private ToolStripMenuItem rotationToolStripMenuItem;
        private ToolStripMenuItem miseÀÉchelleToolStripMenuItem;
        private ToolStripMenuItem duplicationToolStripMenuItem;
        private ToolStripMenuItem zoomToolStripMenuItem;
        private ToolStripMenuItem créationDobjetsToolStripMenuItem;
        private ToolStripMenuItem butoirCirculaireToolStripMenuItem;
        private ToolStripMenuItem butoirTriangulaireToolStripMenuItem;
        private ToolStripMenuItem cibleToolStripMenuItem;
        private ToolStripMenuItem ressortToolStripMenuItem;
        private ToolStripMenuItem générateurBilleToolStripMenuItem;
        private ToolStripMenuItem trouToolStripMenuItem;
        private ToolStripMenuItem paletteToolStripMenuItem;
        private ToolStripMenuItem portailToolStripMenuItem;
        private ToolStripMenuItem murToolStripMenuItem;
        private ToolStripMenuItem vuesToolStripMenuItem;
        private ToolStripMenuItem orthographiqueToolStripMenuItem;
        private ToolStripMenuItem orbiteToolStripMenuItem;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem paletteDroiteJ1ToolStripMenuItem;
        private ToolStripMenuItem paletteGaucheJ1ToolStripMenuItem;
        private ToolStripMenuItem paletteDroiteJ2ToolStripMenuItem;
        private ToolStripMenuItem paletteGaucheJ2ToolStripMenuItem;
        private ToolStripMenuItem butoirTriangulaireDroitToolStripMenuItem;
        private ToolStripMenuItem butoirTriangulaireGaucheToolStripMenuItem;
        private ToolStripMenuItem retourEditeurToolStripMenuItem;
        private ToolStripMenuItem menuPrincipalToolStripMenuItem1;
        private ToolStripMenuItem modeÉditionToolStripMenuItem;
        private ToolStripMenuItem changerCaméraToolStripMenuItem;
        private ToolStripMenuItem orthographiqueToolStripMenuItem1;
        private ToolStripMenuItem orbiteToolStripMenuItem1;
        private MenuStrip menuStrip2;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel2;
        private Label label8;
        private RadioButton paletteGaucheJ2;
        private RadioButton paletteDroiteJ2;
        private RadioButton mur;
        private RadioButton paletteGaucheJ1;
        private RadioButton paletteDroiteJ1;
        private RadioButton portail;
        private RadioButton butoirTriangulaireDroit;
        private Label label7;
        private RadioButton butoirCercle;
        private Label label6;
        private RadioButton butoirTriangulaireGauche;
        private RadioButton trou;
        private RadioButton generateurBille;
        private RadioButton zoom;
        private RadioButton selection;
        private RadioButton deplacement;
        private RadioButton rotation;
        private RadioButton miseEchelle;
        private RadioButton Duplication;
        private RadioButton ressort;
        private RadioButton cible;
        private TableLayoutPanel tableLayoutPanel3;
        private Panel panel1;
        private Panel panelEditionObjet;
        private Label label5;
        private Label label4;
        private NumericUpDown posYUpDown;
        private NumericUpDown posXUpDown;
        private NumericUpDown scaleUpDown;
        private NumericUpDown rotationUpDown;
        private TrackBar facteurEchelleObjet;
        private Label label3;
        private Label label2;
        private Label label1;
        private TrackBar rotationObjet;
        private TableLayoutPanel tableLayoutPanel1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripMenuItem refaireTutorielToolStripMenuItem;
        private RadioButton ChampDeForce;
        private RadioButton PlateauArgent;
        private ToolStripMenuItem plateauDargentToolStripMenuItem;
        private ToolStripMenuItem champDeForceToolStripMenuItem;
        private Panel panel3;
        public Label Player1PointLabel;
        public Label Player2PointLabel;
        public Label Player4PointLabel;
        public Label Player3PointLabel;
        public Panel pointPanel;
        private Label label9;
        public Panel ballPanel;
        private Label ballTitle;
        public Label Player1BallLabel;
        public Label Player2BallLabel;
        public Label Player4BallLabel;
        public Label Player3BallLabel;
        private Panel CampagneInfo;
        private Label CampagneNomLabel;
        private Label CampagneTitre;
        private Label NomCampagne;
        private Label label11;
        private Label label10;
        private Label DifficulteCampagne;
        private Label PointsCampagne;
    }
}

