namespace 暗夜辅助服务端
{
    partial class MainForm
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
            components = new System.ComponentModel.Container();
            label1 = new Label();
            comboBoxfwqzt = new ComboBox();
            comboBoxkaika = new ComboBox();
            label2 = new Label();
            dataGridViewkami = new DataGridView();
            Columnkami = new DataGridViewTextBoxColumn();
            Columnshijian = new DataGridViewTextBoxColumn();
            contextMenuStrip = new ContextMenuStrip(components);
            toolStripMenuItemfuzhi = new ToolStripMenuItem();
            button1 = new Button();
            textBox1 = new TextBox();
            label3 = new Label();
            button2 = new Button();
            textBox2 = new TextBox();
            label4 = new Label();
            textBox3 = new TextBox();
            button3 = new Button();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            textBox4 = new TextBox();
            label9 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewkami).BeginInit();
            contextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(71, 17);
            label1.TabIndex = 0;
            label1.Text = "服务器状态:";
            // 
            // comboBoxfwqzt
            // 
            comboBoxfwqzt.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxfwqzt.FormattingEnabled = true;
            comboBoxfwqzt.Items.AddRange(new object[] { "正常", "禁止使用", "强制更新", "维护" });
            comboBoxfwqzt.Location = new Point(89, 6);
            comboBoxfwqzt.Name = "comboBoxfwqzt";
            comboBoxfwqzt.Size = new Size(121, 25);
            comboBoxfwqzt.TabIndex = 1;
            // 
            // comboBoxkaika
            // 
            comboBoxkaika.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxkaika.FormattingEnabled = true;
            comboBoxkaika.Items.AddRange(new object[] { "试用", "天卡", "周卡", "月卡", "季卡", "年卡", "永久卡" });
            comboBoxkaika.Location = new Point(365, 3);
            comboBoxkaika.Name = "comboBoxkaika";
            comboBoxkaika.Size = new Size(121, 25);
            comboBoxkaika.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(324, 6);
            label2.Name = "label2";
            label2.Size = new Size(35, 17);
            label2.TabIndex = 3;
            label2.Text = "开卡:";
            // 
            // dataGridViewkami
            // 
            dataGridViewkami.AllowUserToAddRows = false;
            dataGridViewkami.BackgroundColor = Color.White;
            dataGridViewkami.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewkami.Columns.AddRange(new DataGridViewColumn[] { Columnkami, Columnshijian });
            dataGridViewkami.ContextMenuStrip = contextMenuStrip;
            dataGridViewkami.Location = new Point(492, 3);
            dataGridViewkami.Name = "dataGridViewkami";
            dataGridViewkami.ReadOnly = true;
            dataGridViewkami.RowHeadersVisible = false;
            dataGridViewkami.RowTemplate.Height = 25;
            dataGridViewkami.Size = new Size(402, 535);
            dataGridViewkami.TabIndex = 4;
            dataGridViewkami.CellMouseDown += dataGridViewkami_CellMouseDown;
            dataGridViewkami.CellValueChanged += dataGridViewkami_CellValueChanged;
            // 
            // Columnkami
            // 
            Columnkami.HeaderText = "卡密";
            Columnkami.Name = "Columnkami";
            Columnkami.ReadOnly = true;
            Columnkami.SortMode = DataGridViewColumnSortMode.NotSortable;
            Columnkami.Width = 330;
            // 
            // Columnshijian
            // 
            Columnshijian.HeaderText = "类型";
            Columnshijian.Name = "Columnshijian";
            Columnshijian.ReadOnly = true;
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { toolStripMenuItemfuzhi });
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new Size(101, 26);
            // 
            // toolStripMenuItemfuzhi
            // 
            toolStripMenuItemfuzhi.Name = "toolStripMenuItemfuzhi";
            toolStripMenuItemfuzhi.Size = new Size(100, 22);
            toolStripMenuItemfuzhi.Text = "复制";
            toolStripMenuItemfuzhi.Click += toolStripMenuItemfuzhi_Click;
            // 
            // button1
            // 
            button1.Location = new Point(324, 34);
            button1.Name = "button1";
            button1.Size = new Size(162, 128);
            button1.TabIndex = 5;
            button1.Text = "开卡";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 54);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(306, 108);
            textBox1.TabIndex = 6;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 34);
            label3.Name = "label3";
            label3.Size = new Size(35, 17);
            label3.TabIndex = 7;
            label3.Text = "公告:";
            // 
            // button2
            // 
            button2.Location = new Point(12, 402);
            button2.Name = "button2";
            button2.Size = new Size(162, 128);
            button2.TabIndex = 8;
            button2.Text = "全部加时";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click_1;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(13, 373);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(71, 23);
            textBox2.TabIndex = 9;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(90, 376);
            label4.Name = "label4";
            label4.Size = new Size(32, 17);
            label4.TabIndex = 10;
            label4.Text = "小时";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(325, 371);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(71, 23);
            textBox3.TabIndex = 11;
            // 
            // button3
            // 
            button3.Location = new Point(324, 400);
            button3.Name = "button3";
            button3.Size = new Size(162, 128);
            button3.TabIndex = 12;
            button3.Text = "批量开卡";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(401, 375);
            label5.Name = "label5";
            label5.Size = new Size(20, 17);
            label5.TabIndex = 13;
            label5.Text = "张";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(13, 182);
            label6.Name = "label6";
            label6.Size = new Size(68, 17);
            label6.TabIndex = 14;
            label6.Text = "在线人数：";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(74, 182);
            label7.Name = "label7";
            label7.Size = new Size(15, 17);
            label7.TabIndex = 15;
            label7.Text = "0";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(15, 259);
            label8.Name = "label8";
            label8.Size = new Size(32, 17);
            label8.TabIndex = 16;
            label8.Text = "换绑";
            // 
            // textBox4
            // 
            textBox4.Location = new Point(53, 257);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(121, 23);
            textBox4.TabIndex = 17;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(206, 259);
            label9.Name = "label9";
            label9.Size = new Size(56, 17);
            label9.TabIndex = 18;
            label9.Text = "换绑信息";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(896, 542);
            Controls.Add(label9);
            Controls.Add(textBox4);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(button3);
            Controls.Add(textBox3);
            Controls.Add(label4);
            Controls.Add(textBox2);
            Controls.Add(button2);
            Controls.Add(label3);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Controls.Add(dataGridViewkami);
            Controls.Add(label2);
            Controls.Add(comboBoxkaika);
            Controls.Add(comboBoxfwqzt);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "服务端";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewkami).EndInit();
            contextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox comboBoxfwqzt;
        private ComboBox comboBoxkaika;
        private Label label2;
        private DataGridView dataGridViewkami;
        private Button button1;
        private DataGridViewTextBoxColumn Columnkami;
        private DataGridViewTextBoxColumn Columnshijian;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem toolStripMenuItemfuzhi;
        private TextBox textBox1;
        private Label label3;
        private Button button2;
        private TextBox textBox2;
        private Label label4;
        private TextBox textBox3;
        private Button button3;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private TextBox textBox4;
        private Label label9;
    }
}