namespace Gtcs
{
    partial class w_Getvip
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
            this.tbVipName = new System.Windows.Forms.TextBox();
            this.tbVipCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbVipName
            // 
            this.tbVipName.BackColor = System.Drawing.SystemColors.Highlight;
            this.tbVipName.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbVipName.ForeColor = System.Drawing.SystemColors.Info;
            this.tbVipName.Location = new System.Drawing.Point(170, 100);
            this.tbVipName.Name = "tbVipName";
            this.tbVipName.ReadOnly = true;
            this.tbVipName.Size = new System.Drawing.Size(245, 33);
            this.tbVipName.TabIndex = 18;
            this.tbVipName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbVipCode
            // 
            this.tbVipCode.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tbVipCode.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbVipCode.Location = new System.Drawing.Point(170, 42);
            this.tbVipCode.Name = "tbVipCode";
            this.tbVipCode.Size = new System.Drawing.Size(245, 33);
            this.tbVipCode.TabIndex = 15;
            this.tbVipCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(62, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 28);
            this.label2.TabIndex = 17;
            this.label2.Text = "会员姓名:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(62, 43);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 28);
            this.label10.TabIndex = 16;
            this.label10.Text = "会员卡号:";
            // 
            // w_Getvip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(476, 177);
            this.ControlBox = false;
            this.Controls.Add(this.tbVipName);
            this.Controls.Add(this.tbVipCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label10);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "w_Getvip";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "w_Getvip";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbVipName;
        private System.Windows.Forms.TextBox tbVipCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label10;
    }
}