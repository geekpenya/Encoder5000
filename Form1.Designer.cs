namespace Encoder5000
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.viewFolder = new System.Windows.Forms.TreeView();
            this.viewFile = new System.Windows.Forms.ListView();
            this.encod = new System.Windows.Forms.Button();
            this.decode = new System.Windows.Forms.Button();
            this.userlabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.пользовательToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.входToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.новыйПользовательToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // viewFolder
            // 
            this.viewFolder.Location = new System.Drawing.Point(12, 27);
            this.viewFolder.Name = "viewFolder";
            this.viewFolder.Size = new System.Drawing.Size(216, 345);
            this.viewFolder.TabIndex = 0;
            this.viewFolder.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeExp_AfterSelect);
            // 
            // viewFile
            // 
            this.viewFile.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.viewFile.HideSelection = false;
            this.viewFile.LabelEdit = true;
            this.viewFile.Location = new System.Drawing.Point(234, 27);
            this.viewFile.Name = "viewFile";
            this.viewFile.Size = new System.Drawing.Size(488, 345);
            this.viewFile.TabIndex = 5;
            this.viewFile.UseCompatibleStateImageBehavior = false;
            this.viewFile.View = System.Windows.Forms.View.Details;
            this.viewFile.DoubleClick += new System.EventHandler(this.viewFile_DoubleClick);
            // 
            // encod
            // 
            this.encod.Location = new System.Drawing.Point(235, 374);
            this.encod.Name = "encod";
            this.encod.Size = new System.Drawing.Size(106, 39);
            this.encod.TabIndex = 6;
            this.encod.Text = "Зашифровать";
            this.encod.UseVisualStyleBackColor = true;
            this.encod.Click += new System.EventHandler(this.encod_Click);
            // 
            // decode
            // 
            this.decode.Location = new System.Drawing.Point(340, 374);
            this.decode.Name = "decode";
            this.decode.Size = new System.Drawing.Size(106, 39);
            this.decode.TabIndex = 7;
            this.decode.Text = "Расшифровать";
            this.decode.UseVisualStyleBackColor = true;
            this.decode.Click += new System.EventHandler(this.decode_Click);
            // 
            // userlabel
            // 
            this.userlabel.AutoSize = true;
            this.userlabel.Location = new System.Drawing.Point(452, 385);
            this.userlabel.Name = "userlabel";
            this.userlabel.Size = new System.Drawing.Size(117, 17);
            this.userlabel.TabIndex = 9;
            this.userlabel.Text = "Вход не выполнен";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(235, 420);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(211, 23);
            this.progressBar1.TabIndex = 10;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.пользовательToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(735, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // пользовательToolStripMenuItem
            // 
            this.пользовательToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.входToolStripMenuItem,
            this.новыйПользовательToolStripMenuItem,
            this.выходToolStripMenuItem});
            this.пользовательToolStripMenuItem.Name = "пользовательToolStripMenuItem";
            this.пользовательToolStripMenuItem.Size = new System.Drawing.Size(96, 20);
            this.пользовательToolStripMenuItem.Text = "Пользователь";
            // 
            // входToolStripMenuItem
            // 
            this.входToolStripMenuItem.Name = "входToolStripMenuItem";
            this.входToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.входToolStripMenuItem.Text = "Вход";
            this.входToolStripMenuItem.Click += new System.EventHandler(this.входToolStripMenuItem_Click_1);
            // 
            // новыйПользовательToolStripMenuItem
            // 
            this.новыйПользовательToolStripMenuItem.Name = "новыйПользовательToolStripMenuItem";
            this.новыйПользовательToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.новыйПользовательToolStripMenuItem.Text = "Новый пользователь";
            this.новыйПользовательToolStripMenuItem.Click += new System.EventHandler(this.новыйПользовательToolStripMenuItem_Click);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click_1);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(735, 453);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.userlabel);
            this.Controls.Add(this.decode);
            this.Controls.Add(this.encod);
            this.Controls.Add(this.viewFile);
            this.Controls.Add(this.viewFolder);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Encoder5000";
         
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butBk;
        private System.Windows.Forms.Button butFw;
        private System.Windows.Forms.Button butOp;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.TreeView treeExp;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TreeView viewFolder;
        private System.Windows.Forms.ListView viewFile;
        private System.Windows.Forms.Button encod;
        private System.Windows.Forms.Button decode;
        private System.Windows.Forms.Label userlabel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem пользовательToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem входToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem новыйПользовательToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
    }
}