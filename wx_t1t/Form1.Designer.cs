namespace wx_t1t
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.SessionId = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ScoreNum = new System.Windows.Forms.NumericUpDown();
            this.TimeNum = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ScoreNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeNum)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "session_id：";
            // 
            // SessionId
            // 
            this.SessionId.Location = new System.Drawing.Point(99, 24);
            this.SessionId.Multiline = true;
            this.SessionId.Name = "SessionId";
            this.SessionId.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.SessionId.Size = new System.Drawing.Size(215, 97);
            this.SessionId.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(239, 160);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "提交";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(52, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "得分：";
            // 
            // ScoreNum
            // 
            this.ScoreNum.Location = new System.Drawing.Point(99, 130);
            this.ScoreNum.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.ScoreNum.Name = "ScoreNum";
            this.ScoreNum.Size = new System.Drawing.Size(66, 21);
            this.ScoreNum.TabIndex = 11;
            this.ScoreNum.Value = new decimal(new int[] {
            3999,
            0,
            0,
            0});
            // 
            // TimeNum
            // 
            this.TimeNum.Location = new System.Drawing.Point(252, 130);
            this.TimeNum.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.TimeNum.Name = "TimeNum";
            this.TimeNum.Size = new System.Drawing.Size(62, 21);
            this.TimeNum.TabIndex = 13;
            this.TimeNum.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(181, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "时间系数：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 195);
            this.Controls.Add(this.TimeNum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ScoreNum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.SessionId);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "跳一跳改分";
            ((System.ComponentModel.ISupportInitialize)(this.ScoreNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SessionId;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown ScoreNum;
        private System.Windows.Forms.NumericUpDown TimeNum;
        private System.Windows.Forms.Label label2;
    }
}

