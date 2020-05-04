using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace iFakeLocation
{
	public class ActiveForm : Form
	{
		private string deviceName;

		private string productType;

		private string productVersion;

		private string deviceUDID;

		private string statusdesc;

		private MainForm mainForm;

		private IContainer components;

		private TableLayoutPanel tableLayoutPanel1;

		private Label label1;

		private Label label2;

		private Label label3;

		private Label label4;

		private Label label5;

		private Label label6;

		private Label label7;

		private Label label9;

		private Label label10;

		private Label label11;

		private TextBox textBox1;

		private TextBox textBox2;

		private Button button1;

		public ActiveForm()
		{
			InitializeComponent();
		}

		public ActiveForm(string deviceName, string productType, string productVersion, string deviceUDID, string statusdesc, MainForm mainForm)
		{
			this.deviceName = deviceName;
			this.productType = productType;
			this.productVersion = productVersion;
			this.deviceUDID = deviceUDID;
			this.statusdesc = statusdesc;
			this.mainForm = mainForm;
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string text = textBox1.Text;
			if (!string.IsNullOrWhiteSpace(text))
			{
				DeviceStatus deviceStatus = HttpHelper.active(deviceUDID, deviceName, text);
				if (deviceStatus.Success)
				{
					new MessageBox(deviceStatus.Msg).ShowDialog();
					mainForm.initToolStripStatus();
					Close();
				}
				else
				{
					new MessageBox(deviceStatus.Msg).ShowDialog();
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			label4 = new System.Windows.Forms.Label();
			label5 = new System.Windows.Forms.Label();
			label6 = new System.Windows.Forms.Label();
			label7 = new System.Windows.Forms.Label();
			label9 = new System.Windows.Forms.Label();
			label10 = new System.Windows.Forms.Label();
			label11 = new System.Windows.Forms.Label();
			textBox1 = new System.Windows.Forms.TextBox();
			textBox2 = new System.Windows.Forms.TextBox();
			button1 = new System.Windows.Forms.Button();
			tableLayoutPanel1.SuspendLayout();
			SuspendLayout();
			tableLayoutPanel1.ColumnCount = 2;
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30f));
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70f));
			tableLayoutPanel1.Controls.Add(label1, 0, 0);
			tableLayoutPanel1.Controls.Add(label2, 1, 0);
			tableLayoutPanel1.Controls.Add(label3, 0, 1);
			tableLayoutPanel1.Controls.Add(label4, 1, 1);
			tableLayoutPanel1.Controls.Add(label5, 0, 2);
			tableLayoutPanel1.Controls.Add(label6, 1, 2);
			tableLayoutPanel1.Controls.Add(label7, 0, 3);
			tableLayoutPanel1.Controls.Add(textBox2, 1, 3);
			tableLayoutPanel1.Controls.Add(label9, 0, 4);
			tableLayoutPanel1.Controls.Add(label10, 1, 4);
			tableLayoutPanel1.Controls.Add(label11, 0, 5);
			tableLayoutPanel1.Controls.Add(textBox1, 1, 5);
			tableLayoutPanel1.Controls.Add(button1, 1, 6);
			tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 8;
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.5f));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.5f));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.5f));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.5f));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.5f));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.5f));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.5f));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.5f));
			tableLayoutPanel1.TabIndex = 0;
			label1.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(67, 35);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(82, 15);
			label1.Text = "设备名称：";
			label2.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(155, 35);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(55, 15);
			label2.Text = deviceName;
			label3.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(67, 85);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(82, 15);
			label3.Text = "设备型号：";
			label4.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(155, 85);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(55, 15);
			label4.Text = productType;
			label5.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(67, 135);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(82, 15);
			label5.Text = "系统版本：";
			label6.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(155, 135);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(55, 15);
			label6.Text = productVersion;
			label7.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			label7.AutoSize = true;
			label7.Location = new System.Drawing.Point(67, 185);
			label7.Name = "label7";
			label7.Size = new System.Drawing.Size(82, 15);
			label7.Text = "设备标识：";
			textBox2.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			textBox2.ReadOnly = true;
			textBox2.Location = new System.Drawing.Point(155, 185);
			textBox2.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
			textBox2.Name = "textBox2";
			textBox2.Size = new System.Drawing.Size(325, 15);
			textBox2.Text = deviceUDID;
			label9.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			label9.AutoSize = true;
			label9.Location = new System.Drawing.Point(67, 235);
			label9.Name = "label9";
			label9.Size = new System.Drawing.Size(82, 15);
			label9.Text = "到期时间：";
			label10.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			label10.AutoSize = true;
			label10.Location = new System.Drawing.Point(155, 235);
			label10.Name = "label10";
			label10.Size = new System.Drawing.Size(63, 15);
			label10.Text = statusdesc;
			label11.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			label11.AutoSize = true;
			label11.Location = new System.Drawing.Point(67, 282);
			label11.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			label11.Name = "label11";
			label11.Size = new System.Drawing.Size(82, 15);
			label11.Text = "输入卡密：";
			textBox1.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			textBox1.Location = new System.Drawing.Point(155, 275);
			textBox1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			textBox1.Name = "textBox1";
			textBox1.Size = new System.Drawing.Size(167, 25);
			textBox1.TabIndex = 0;
			button1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			button1.Location = new System.Drawing.Point(155, 316);
			button1.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(75, 23);
			button1.TabIndex = 1;
			button1.Text = "激活卡密";
			button1.UseVisualStyleBackColor = true;
			button1.Click += new System.EventHandler(button1_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 15f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoScroll = true;
			AutoSize = true;
			base.ClientSize = new System.Drawing.Size(480, 280);
			base.Controls.Add(tableLayoutPanel1);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ActiveForm";
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "";
			base.ShowIcon = false;
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel1.PerformLayout();
			ResumeLayout(false);
			base.AcceptButton = button1;
		}
	}
}
