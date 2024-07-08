namespace MultiFaceRec
{
    partial class odunc
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.arama = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.kitapara = new System.Windows.Forms.TextBox();
            this.oduncal = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(0, 1);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(839, 387);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // arama
            // 
            this.arama.Location = new System.Drawing.Point(578, 465);
            this.arama.Name = "arama";
            this.arama.Size = new System.Drawing.Size(217, 45);
            this.arama.TabIndex = 5;
            this.arama.Text = "ARA";
            this.arama.UseVisualStyleBackColor = true;
            this.arama.Click += new System.EventHandler(this.arama_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(507, 431);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "KİTAP ADI :";
            // 
            // kitapara
            // 
            this.kitapara.Location = new System.Drawing.Point(578, 428);
            this.kitapara.Name = "kitapara";
            this.kitapara.Size = new System.Drawing.Size(217, 20);
            this.kitapara.TabIndex = 3;
            // 
            // oduncal
            // 
            this.oduncal.Location = new System.Drawing.Point(578, 516);
            this.oduncal.Name = "oduncal";
            this.oduncal.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.oduncal.Size = new System.Drawing.Size(217, 45);
            this.oduncal.TabIndex = 6;
            this.oduncal.Text = "ODUNC AL";
            this.oduncal.UseVisualStyleBackColor = true;
            this.oduncal.Click += new System.EventHandler(this.oduncal_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(655, 403);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "label1";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(164, 403);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(285, 157);
            this.richTextBox1.TabIndex = 8;
            this.richTextBox1.Text = "";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(19, 403);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(127, 157);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // odunc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 573);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.oduncal);
            this.Controls.Add(this.arama);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.kitapara);
            this.Controls.Add(this.dataGridView1);
            this.Name = "odunc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "odunc";
            this.Load += new System.EventHandler(this.odunc_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button arama;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox kitapara;
        private System.Windows.Forms.Button oduncal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}