namespace Jesture
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
         this._paper = new System.Windows.Forms.Panel();
         this.SuspendLayout();
         // 
         // _paper
         // 
         this._paper.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._paper.Location = new System.Drawing.Point(12, 12);
         this._paper.Name = "_paper";
         this._paper.Size = new System.Drawing.Size(639, 323);
         this._paper.TabIndex = 0;
         this._paper.Paint += new System.Windows.Forms.PaintEventHandler(this.paper_Paint);
         this._paper.MouseDown += new System.Windows.Forms.MouseEventHandler(this.paper_MouseDown);
         this._paper.MouseMove += new System.Windows.Forms.MouseEventHandler(this.paper_MouseMove);
         this._paper.MouseUp += new System.Windows.Forms.MouseEventHandler(this.paper_MouseUp);
         this._paper.Resize += new System.EventHandler(this._paper_Resize);
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(663, 347);
         this.Controls.Add(this._paper);
         this.Name = "Form1";
         this.Text = "Jesture";
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Panel _paper;
   }
}

