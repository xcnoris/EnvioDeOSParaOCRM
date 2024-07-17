namespace EnvioDeOSParaOCRM
{
    partial class FrmTelaPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTelaPrincipal));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.TBC_Dados = new System.Windows.Forms.TabControl();
            this.Btn_Salvar = new System.Windows.Forms.Button();
            this.Btn_Fechar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // TBC_Dados
            // 
            this.TBC_Dados.Location = new System.Drawing.Point(3, 12);
            this.TBC_Dados.Name = "TBC_Dados";
            this.TBC_Dados.SelectedIndex = 0;
            this.TBC_Dados.Size = new System.Drawing.Size(829, 421);
            this.TBC_Dados.TabIndex = 1;
            // 
            // Btn_Salvar
            // 
            this.Btn_Salvar.Location = new System.Drawing.Point(245, 439);
            this.Btn_Salvar.Name = "Btn_Salvar";
            this.Btn_Salvar.Size = new System.Drawing.Size(117, 36);
            this.Btn_Salvar.TabIndex = 2;
            this.Btn_Salvar.Text = "Salvar";
            this.Btn_Salvar.UseVisualStyleBackColor = true;
            // 
            // Btn_Fechar
            // 
            this.Btn_Fechar.Location = new System.Drawing.Point(430, 439);
            this.Btn_Fechar.Name = "Btn_Fechar";
            this.Btn_Fechar.Size = new System.Drawing.Size(117, 36);
            this.Btn_Fechar.TabIndex = 3;
            this.Btn_Fechar.Text = "Fechar";
            this.Btn_Fechar.UseVisualStyleBackColor = true;
            // 
            // FrmTelaPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 487);
            this.Controls.Add(this.Btn_Fechar);
            this.Controls.Add(this.Btn_Salvar);
            this.Controls.Add(this.TBC_Dados);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(850, 526);
            this.MinimumSize = new System.Drawing.Size(850, 526);
            this.Name = "FrmTelaPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmTelaPrincipal";
            this.SizeChanged += new System.EventHandler(this.FrmTelaPrincipal_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.TabControl TBC_Dados;
        private System.Windows.Forms.Button Btn_Salvar;
        private System.Windows.Forms.Button Btn_Fechar;
    }
}