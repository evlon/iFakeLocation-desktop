using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace iFakeLocation
{
	public class WelcomeForm : Form
	{
		private static readonly object syncLock = new object();

		private IContainer components;

		private FontDialog fontDialog1;

		private TableLayoutPanel tableLayoutPanel1;

		private Label label1;

		private PictureBox pictureBox1;

		private TableLayoutPanel tableLayoutPanel2;

		private Label label2;

		private Label label3;

		private Label label4;

		private Label label5;

		public Timer timer1;

		public WelcomeForm()
		{
			InitializeComponent();
		}

		public void checkDevice()
		{
			if (DeviceInformation.GetDevices().FirstOrDefault() != null)
			{
				base.DialogResult = DialogResult.OK;
			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			checkDevice();
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
			components = new System.ComponentModel.Container();
			fontDialog1 = new System.Windows.Forms.FontDialog();
			tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			label1 = new System.Windows.Forms.Label();
			pictureBox1 = new System.Windows.Forms.PictureBox();
			tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			label2 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			label4 = new System.Windows.Forms.Label();
			label5 = new System.Windows.Forms.Label();
			tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			tableLayoutPanel2.SuspendLayout();
			SuspendLayout();
			timer1 = new System.Windows.Forms.Timer(components);
			tableLayoutPanel1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			tableLayoutPanel1.ColumnCount = 3;
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60f));
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
			tableLayoutPanel1.Controls.Add(label1, 1, 0);
			tableLayoutPanel1.Controls.Add(pictureBox1, 1, 1);
			tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 2);
			tableLayoutPanel1.Location = new System.Drawing.Point(6, 2);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 3;
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20f));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30f));
			tableLayoutPanel1.Size = new System.Drawing.Size(881, 586);
			tableLayoutPanel1.TabIndex = 0;
			label1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			label1.AutoSize = true;
			label1.Font = new System.Drawing.Font("微软雅黑", 25.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			label1.Location = new System.Drawing.Point(179, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(522, 117);
			label1.TabIndex = 0;
			label1.Text = "请连接您的设备";
			label1.Margin = new System.Windows.Forms.Padding(0, 60, 0, 0);
			label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			pictureBox1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			pictureBox1.Image = iFakeLocation.Properties.Resources.bgimage;
			pictureBox1.Location = new System.Drawing.Point(179, 120);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new System.Drawing.Size(522, 287);
			pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			pictureBox1.TabIndex = 1;
			pictureBox1.TabStop = false;
			tableLayoutPanel2.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			tableLayoutPanel2.ColumnCount = 3;
			tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
			tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60f));
			tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
			tableLayoutPanel2.Controls.Add(label2, 1, 0);
			tableLayoutPanel2.Controls.Add(label3, 1, 1);
			tableLayoutPanel2.Controls.Add(label4, 1, 2);
			tableLayoutPanel2.Controls.Add(label5, 1, 3);
			tableLayoutPanel2.Location = new System.Drawing.Point(179, 413);
			tableLayoutPanel2.Name = "tableLayoutPanel2";
			tableLayoutPanel2.RowCount = 5;
			tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20f));
			tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20f));
			tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20f));
			tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20f));
			tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20f));
			tableLayoutPanel2.Size = new System.Drawing.Size(522, 170);
			tableLayoutPanel2.TabIndex = 2;
			label2.AutoSize = true;
			label2.Dock = System.Windows.Forms.DockStyle.Top;
			label2.Location = new System.Drawing.Point(107, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(307, 34);
			label2.TabIndex = 4;
			label2.Text = "如出现多次尝试仍无法连接情况请按以下内容操作";
			label3.AutoSize = true;
			label3.Dock = System.Windows.Forms.DockStyle.Top;
			label3.Location = new System.Drawing.Point(107, 34);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(307, 20);
			label3.TabIndex = 5;
			label3.Text = "1、请解锁设备屏幕，重新插拔数据线";
			label4.AutoSize = true;
			label4.Dock = System.Windows.Forms.DockStyle.Top;
			label4.Location = new System.Drawing.Point(107, 68);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(307, 34);
			label4.TabIndex = 6;
			label4.Text = "2、尝试更换其他USB接口或更换原装数据线进行重连";
			label5.AutoSize = true;
			label5.Dock = System.Windows.Forms.DockStyle.Top;
			label5.Location = new System.Drawing.Point(107, 102);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(307, 20);
			label5.TabIndex = 7;
			label5.Text = "3、检查电脑是否已经安装iTunes";
			label5.Visible = true;
			timer1.Enabled = true;
			timer1.Interval = 1000;
			timer1.Tick += new System.EventHandler(timer1_Tick);
			base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 20f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			BackColor = System.Drawing.Color.White;
			int width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
			int height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
			base.ClientSize = new System.Drawing.Size(width / 10 * 5, height / 10 * 6);
			base.Controls.Add(tableLayoutPanel1);
			Font = new System.Drawing.Font("微软雅黑", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			base.HelpButton = true;
			//base.Icon = iFakeLocation.Properties.Resources.logo;
			base.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "WelcomeForm";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = iFakeLocation.Properties.Resources.name + " v" + iFakeLocation.Properties.Resources.version;
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			tableLayoutPanel2.ResumeLayout(false);
			tableLayoutPanel2.PerformLayout();
			ResumeLayout(false);
			BackgroundImage = iFakeLocation.Properties.Resources.bgimage;
			base.ShowInTaskbar = false;
		}
	}
}
