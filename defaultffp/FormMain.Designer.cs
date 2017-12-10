namespace defaultffp
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.listMain = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCurDef = new System.Windows.Forms.TextBox();
            this.btnSRR = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // listMain
            // 
            resources.ApplyResources(this.listMain, "listMain");
            this.listMain.FormattingEnabled = true;
            this.listMain.Name = "listMain";
            this.listMain.DoubleClick += new System.EventHandler(this.listMain_DoubleClick);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtCurDef
            // 
            resources.ApplyResources(this.txtCurDef, "txtCurDef");
            this.txtCurDef.Name = "txtCurDef";
            this.txtCurDef.ReadOnly = true;
            // 
            // btnSRR
            // 
            resources.ApplyResources(this.btnSRR, "btnSRR");
            this.btnSRR.Name = "btnSRR";
            this.btnSRR.UseVisualStyleBackColor = true;
            this.btnSRR.Click += new System.EventHandler(this.btnSRR_Click);
            // 
            // FormMain
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.btnSRR);
            this.Controls.Add(this.txtCurDef);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listMain);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "FormMain";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListBox listMain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCurDef;
        private System.Windows.Forms.Button btnSRR;
    }
}

