namespace Lab4
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            BtnAkce = new Button();
            BtnCancel = new Button();
            LblText = new Label();
            ProgressBarMain = new ProgressBar();
            statusStrip1 = new StatusStrip();
            StatusText = new ToolStripStatusLabel();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // BtnAkce
            // 
            BtnAkce.Location = new Point(408, 48);
            BtnAkce.Name = "BtnAkce";
            BtnAkce.Size = new Size(153, 50);
            BtnAkce.TabIndex = 0;
            BtnAkce.Text = "Akce";
            BtnAkce.UseVisualStyleBackColor = true;
            BtnAkce.Click += BtnAkce_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Enabled = false;
            BtnCancel.Location = new Point(586, 48);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(124, 50);
            BtnCancel.TabIndex = 1;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // LblText
            // 
            LblText.AutoSize = true;
            LblText.Location = new Point(84, 57);
            LblText.Name = "LblText";
            LblText.Size = new Size(50, 15);
            LblText.TabIndex = 2;
            LblText.Text = "Priprava";
            // 
            // ProgressBarMain
            // 
            ProgressBarMain.Location = new Point(168, 149);
            ProgressBarMain.Name = "ProgressBarMain";
            ProgressBarMain.Size = new Size(496, 34);
            ProgressBarMain.TabIndex = 3;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { StatusText });
            statusStrip1.Location = new Point(0, 428);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(800, 22);
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "statusStrip1";
            // 
            // StatusText
            // 
            StatusText.Name = "StatusText";
            StatusText.Size = new Size(0, 17);
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(statusStrip1);
            Controls.Add(ProgressBarMain);
            Controls.Add(LblText);
            Controls.Add(BtnCancel);
            Controls.Add(BtnAkce);
            Name = "Form1";
            Text = "Form1";
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnAkce;
        private Button BtnCancel;
        private Label LblText;
        private ProgressBar ProgressBarMain;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel StatusText;
    }
}
