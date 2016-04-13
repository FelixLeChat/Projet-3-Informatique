using System.ComponentModel;
using System.Windows.Forms;

namespace FrontEnd.UserControl.Winform
{
    partial class FinPartie
    {
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FinPartie));
            this.recommencer = new System.Windows.Forms.Button();
            this.menuPrincipal = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // recommencer
            // 
            this.recommencer.Location = new System.Drawing.Point(106, 132);
            this.recommencer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.recommencer.Name = "recommencer";
            this.recommencer.Size = new System.Drawing.Size(390, 85);
            this.recommencer.TabIndex = 0;
            this.recommencer.Text = "Recommencer";
            this.recommencer.UseVisualStyleBackColor = true;
            this.recommencer.Click += new System.EventHandler(this.recommencer_Click);
            // 
            // menuPrincipal
            // 
            this.menuPrincipal.Location = new System.Drawing.Point(106, 240);
            this.menuPrincipal.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.menuPrincipal.Name = "menuPrincipal";
            this.menuPrincipal.Size = new System.Drawing.Size(390, 85);
            this.menuPrincipal.TabIndex = 1;
            this.menuPrincipal.Text = "Retourner au menu";
            this.menuPrincipal.UseVisualStyleBackColor = true;
            this.menuPrincipal.Click += new System.EventHandler(this.menuPrincipal_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Magenta;
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(576, 74);
            this.label1.TabIndex = 2;
            this.label1.Text = "Magnifique! \r\nGagner vous a donné de la victoire!!!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FinPartie
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 339);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuPrincipal);
            this.Controls.Add(this.recommencer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FinPartie";
            this.Text = "Tatatararata TA TA! Le bal est maintenant terminé!";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button recommencer;
        private Button menuPrincipal;
        private Label label1;
    }
}