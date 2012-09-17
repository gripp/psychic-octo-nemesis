namespace PicturePuzzle
{
    partial class PuzzleForm
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
            this._Pan = new System.Windows.Forms.Panel();
            this._Ref = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this._Ref)).BeginInit();
            this.SuspendLayout();
            // 
            // _Pan
            // 
            this._Pan.BackColor = System.Drawing.Color.Black;
            this._Pan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._Pan.Location = new System.Drawing.Point(5, 5);
            this._Pan.Name = "_Pan";
            this._Pan.Size = new System.Drawing.Size(280, 280);
            this._Pan.TabIndex = 1;
            // 
            // _Ref
            // 
            this._Ref.BackColor = System.Drawing.Color.Black;
            this._Ref.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this._Ref.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._Ref.Location = new System.Drawing.Point(291, 5);
            this._Ref.Name = "_Ref";
            this._Ref.Size = new System.Drawing.Size(228, 241);
            this._Ref.TabIndex = 3;
            this._Ref.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(517, 287);
            this.Controls.Add(this._Ref);
            this.Controls.Add(this._Pan);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "Picture Puzzle";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this._Ref)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _Pan;
        private System.Windows.Forms.PictureBox _Ref;
    }
}

