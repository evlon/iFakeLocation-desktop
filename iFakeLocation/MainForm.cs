using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace iFakeLocation
{
	[ComVisible(true)]
	public class MainForm : Form
	{
		private double lng;

		private double lat;

		private DateTime lastTime = DateTime.Now;

		private string lastMsg = "";

		private IContainer components;

		private TableLayoutPanel tableLayoutPanel1;

		private WebBrowser webBrowser1;

		private TableLayoutPanel tableLayoutPanel2;

		private GroupBox groupBox1;

		private TableLayoutPanel tableLayoutPanel3;

		private Button button1;

		private Button button2;

		private GroupBox groupBox2;

		private TableLayoutPanel tableLayoutPanel4;

		private TableLayoutPanel tableLayoutPanel5;

		private Label label1;

		private Label label2;

		private TextBox textBox1;

		private ComboBox comboBox1;

		private TableLayoutPanel tableLayoutPanel6;

		private Button button3;

		private Button button4;

		private TextBox textBox2;

		private StatusStrip statusStrip1;

		private ToolStripStatusLabel toolStripStatusLabel1;

		private ToolStripStatusLabel toolStripStatusLabel2;

		private ToolStripStatusLabel toolStripStatusLabel3;

		private ToolStripStatusLabel toolStripStatusLabel4;

		private ToolStripStatusLabel toolStripStatusLabel5;

		private BackgroundWorker bgWorker;

		public MainForm()
		{
			InitializeComponent();
			webBrowser1.Navigate(Application.StartupPath + "map.html");
			webBrowser1.ObjectForScripting = this;
			bgWorker.RunWorkerAsync();
		}

		private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			initToolStripStatus();
			InitDeviceSettings();
		}

		public void initToolStripStatus()
		{
			try
			{
				DeviceInformation deviceInformation = DeviceInformation.GetDevices().FirstOrDefault();
				if (deviceInformation != null)
				{
					appendTextBox("连接到设备：" + deviceInformation.Name);
					toolStripStatusLabel2.Text = deviceInformation.Name;
					DeviceStatus status = HttpHelper.GetStatus(deviceInformation.UDID);
					if (status.Success)
					{
						if (status.Available)
						{
							Convert.ToDateTime("2099-1-1");
							if (DateTime.Compare(Convert.ToDateTime("2099-1-1"), status.EndTime) < 0)
							{
								toolStripStatusLabel5.Text = "无限期";
							}
							else
							{
								toolStripStatusLabel5.Text = status.EndTime.ToString();
								if (status.Trial)
								{
									toolStripStatusLabel5.Text += "【试用】";
								}
							}
						}
						else
						{
							toolStripStatusLabel5.Text = status.Msg;
						}
					}
					else
					{
						toolStripStatusLabel5.Text = status.Msg;
					}
				}
				else
				{
					toolStripStatusLabel2.Text = "未找到设备";
					toolStripStatusLabel5.Text = "卡密已过期，请重新购买";
					appendTextBox("已断开设备");
				}
			}
			catch (Exception)
			{
				appendTextBox("服务器无法连接");
			}
		}

		public void onMapClick(double lng, double lat)
		{
			this.lng = lng;
			this.lat = lat;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (lng == 0.0 || lat == 0.0)
			{
				appendTextBox("请先选择位置");
				return;
			}
			DeviceInformation deviceInformation = DeviceInformation.GetDevices().FirstOrDefault();
			if (deviceInformation == null)
			{
				appendTextBox("修改失败：未找到设备");
				return;
			}
			if (!HttpHelper.verifyDevice(deviceInformation.UDID))
			{
				System.Windows.Forms.MessageBox.Show("卡密已过期，请重新购买");
				return;
			}
			appendTextBox("正在修改定位");
			if (deviceInformation.SetLocation(new PointLatLng
			{
				Lat = lat,
				Lng = lng
			}))
			{
				appendTextBox("修改成功");
				return;
			}
			button1.Enabled = false;
			string[] paths;
			if (!DeveloperImageHelper.HasImageForDevice(deviceInformation, out paths))
			{
				string text = HttpHelper.downLoadImage(paths, textBox2);
				if (!(text == ""))
				{
					appendTextBox(text);
					button1.Enabled = true;
					return;
				}
				appendTextBox("正在安装驱动程序");
			}
			try
			{
				deviceInformation.EnableDeveloperMode(paths[0], paths[1]);
				if (deviceInformation.SetLocation(new PointLatLng
				{
					Lat = lat,
					Lng = lng
				}))
				{
					appendTextBox("修改成功");
					button1.Enabled = true;
				}
				else
				{
					appendTextBox("修改失败：请解锁手机重试");
					button1.Enabled = true;
				}
			}
			catch (Exception)
			{
				appendTextBox("安装失败：请解锁手机重试");
				button1.Enabled = true;
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			try
			{
				DeviceInformation deviceInformation = DeviceInformation.GetDevices().FirstOrDefault();
				if (deviceInformation != null)
				{
					deviceInformation.StopLocation();
					appendTextBox("还原成功");
				}
				else
				{
					appendTextBox("还原失败");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				appendTextBox("还原失败");
			}
		}

		private void InitDeviceSettings()
		{
			SqliteHelper.InitSqliteDB();
			foreach (KeyValuePair<string, PointLatLng> setting in SqliteHelper.GetSettings())
			{
				comboBox1.Items.Add(setting);
			}
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			PointLatLng value = ((KeyValuePair<string, PointLatLng>)((ComboBox)sender).SelectedItem).Value;
			object[] args = new object[2]
			{
				value.Lng,
				value.Lat
			};
			lng = value.Lng;
			lat = value.Lat;
			webBrowser1.Document.InvokeScript("changePoint", args);
		}

		private void button3_Click(object sender, EventArgs e)
		{
			if (lng == 0.0 || lat == 0.0)
			{
				appendTextBox("保存常用位置失败：请先选择位置");
				return;
			}
			string text = textBox1.Text;
			if (text == string.Empty)
			{
				appendTextBox("保存常用位置失败：请输入常用位置别名");
				return;
			}
			KeyValuePair<string, PointLatLng> keyValuePair = new KeyValuePair<string, PointLatLng>(text, new PointLatLng(lng, lat));
			int num = comboBox1.Items.Add(keyValuePair);
			comboBox1.SelectedIndex = num;
			SqliteHelper.SaveSettings(textBox1.Text, lng, lat, num);
			textBox1.Text = "";
		}

		private void button4_Click(object sender, EventArgs e)
		{
			if (comboBox1.SelectedIndex >= 0)
			{
				SqliteHelper.DelSettings(((KeyValuePair<string, PointLatLng>)comboBox1.SelectedItem).Key);
				comboBox1.Items.RemoveAt(comboBox1.SelectedIndex);
			}
		}

		private void toolStripStatusLabel3_Click(object sender, EventArgs e)
		{
			DeviceInformation deviceInformation = DeviceInformation.GetDevices().FirstOrDefault();
			if (deviceInformation != null)
			{
				DeviceStatus status;
				try
				{
					status = HttpHelper.GetStatus(deviceInformation.UDID);
				}
				catch (Exception)
				{
					appendTextBox("服务器无法连接");
					return;
				}
				if (status.Success)
				{
					string deviceName = deviceInformation.Properties["DeviceName"].ToString();
					string productType = deviceInformation.Properties["ProductType"].ToString();
					string productVersion = deviceInformation.Properties["ProductVersion"].ToString();
					string uDID = deviceInformation.UDID;
					string text = (!string.IsNullOrEmpty(status.Msg)) ? status.Msg : ((DateTime.Compare(Convert.ToDateTime("2099-1-1"), status.EndTime) >= 0) ? status.EndTime.ToString() : "无限期");
					if (status.Trial && status.Available)
					{
						text += "【试用】";
					}
					new ActiveForm(deviceName, productType, productVersion, uDID, text, this).ShowDialog();
				}
				else
				{
					appendTextBox("网络异常");
				}
			}
			else
			{
				appendTextBox("未找到设备");
			}
		}

		protected override void OnClosed(EventArgs e)
		{
			Application.Exit();
		}

		private void appendTextBox(string msg)
		{
			if (!lastMsg.Equals(msg) || (DateTime.Now - lastTime).Milliseconds > 500)
			{
				textBox2.AppendText(DateTime.Now.ToString("HH:mm:ss") + "\t" + msg + "\r\n");
			}
			lastTime = DateTime.Now;
			lastMsg = msg;
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
			webBrowser1 = new System.Windows.Forms.WebBrowser();
			tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			groupBox1 = new System.Windows.Forms.GroupBox();
			textBox2 = new System.Windows.Forms.TextBox();
			tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			button1 = new System.Windows.Forms.Button();
			button2 = new System.Windows.Forms.Button();
			groupBox2 = new System.Windows.Forms.GroupBox();
			tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			textBox1 = new System.Windows.Forms.TextBox();
			comboBox1 = new System.Windows.Forms.ComboBox();
			tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			button3 = new System.Windows.Forms.Button();
			button4 = new System.Windows.Forms.Button();
			statusStrip1 = new System.Windows.Forms.StatusStrip();
			toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
			toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
			toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
			tableLayoutPanel1.SuspendLayout();
			tableLayoutPanel2.SuspendLayout();
			groupBox1.SuspendLayout();
			tableLayoutPanel3.SuspendLayout();
			groupBox2.SuspendLayout();
			tableLayoutPanel4.SuspendLayout();
			tableLayoutPanel5.SuspendLayout();
			tableLayoutPanel6.SuspendLayout();
			statusStrip1.SuspendLayout();
			SuspendLayout();
			bgWorker = new System.ComponentModel.BackgroundWorker();
			tableLayoutPanel1.ColumnCount = 1;
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
			tableLayoutPanel1.Controls.Add(webBrowser1, 0, 0);
			tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
			tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 2;
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 77.77778f));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.22222f));
			tableLayoutPanel1.Size = new System.Drawing.Size(800, 600);
			tableLayoutPanel1.TabIndex = 0;
			webBrowser1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			webBrowser1.IsWebBrowserContextMenuEnabled = false;
			webBrowser1.Location = new System.Drawing.Point(0, 0);
			webBrowser1.Margin = new System.Windows.Forms.Padding(0);
			webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
			webBrowser1.Name = "webBrowser1";
			webBrowser1.ScrollBarsEnabled = false;
			webBrowser1.Size = new System.Drawing.Size(800, 466);
			webBrowser1.TabIndex = 0;
			tableLayoutPanel2.ColumnCount = 3;
			tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35f));
			tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15f));
			tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
			tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			tableLayoutPanel2.Controls.Add(groupBox1, 0, 0);
			tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 1, 0);
			tableLayoutPanel2.Controls.Add(groupBox2, 2, 0);
			tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			tableLayoutPanel2.Location = new System.Drawing.Point(0, 476);
			tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			tableLayoutPanel2.Name = "tableLayoutPanel2";
			tableLayoutPanel2.RowCount = 1;
			tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
			tableLayoutPanel2.Size = new System.Drawing.Size(800, 124);
			tableLayoutPanel2.TabIndex = 1;
			groupBox1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			groupBox1.Controls.Add(textBox2);
			groupBox1.ForeColor = System.Drawing.SystemColors.HotTrack;
			groupBox1.Location = new System.Drawing.Point(3, 3);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(274, 118);
			groupBox1.TabIndex = 0;
			groupBox1.TabStop = false;
			groupBox1.Text = "操作日志";
			groupBox1.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
			textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			textBox2.Location = new System.Drawing.Point(3, 21);
			textBox2.Multiline = true;
			textBox2.Name = "textBox2";
			textBox2.ReadOnly = true;
			textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			textBox2.Size = new System.Drawing.Size(268, 94);
			textBox2.TabIndex = 0;
			tableLayoutPanel3.ColumnCount = 1;
			tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
			tableLayoutPanel3.Controls.Add(button1, 0, 0);
			tableLayoutPanel3.Controls.Add(button2, 0, 1);
			tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			tableLayoutPanel3.Location = new System.Drawing.Point(283, 3);
			tableLayoutPanel3.Name = "tableLayoutPanel3";
			tableLayoutPanel3.RowCount = 2;
			tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
			tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
			tableLayoutPanel3.Size = new System.Drawing.Size(114, 118);
			tableLayoutPanel3.TabIndex = 1;
			button1.Anchor = System.Windows.Forms.AnchorStyles.None;
			button1.Location = new System.Drawing.Point(19, 18);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(75, 23);
			button1.TabIndex = 0;
			button1.Text = "修改定位";
			button1.UseVisualStyleBackColor = true;
			button1.Click += new System.EventHandler(button1_Click);
			button2.Anchor = System.Windows.Forms.AnchorStyles.None;
			button2.Location = new System.Drawing.Point(19, 77);
			button2.Name = "button2";
			button2.Size = new System.Drawing.Size(75, 23);
			button2.TabIndex = 1;
			button2.Text = "还原定位";
			button2.UseVisualStyleBackColor = true;
			button2.Click += new System.EventHandler(button2_Click);
			groupBox2.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			groupBox2.Controls.Add(tableLayoutPanel4);
			groupBox2.ForeColor = System.Drawing.SystemColors.HotTrack;
			groupBox2.Location = new System.Drawing.Point(403, 3);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new System.Drawing.Size(394, 118);
			groupBox2.TabIndex = 2;
			groupBox2.TabStop = false;
			groupBox2.Text = "常用位置";
			tableLayoutPanel4.ColumnCount = 2;
			tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70f));
			tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30f));
			tableLayoutPanel4.Controls.Add(tableLayoutPanel5, 0, 0);
			tableLayoutPanel4.Controls.Add(tableLayoutPanel6, 1, 0);
			tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			tableLayoutPanel4.Location = new System.Drawing.Point(3, 21);
			tableLayoutPanel4.Name = "tableLayoutPanel4";
			tableLayoutPanel4.RowCount = 1;
			tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
			tableLayoutPanel4.Size = new System.Drawing.Size(388, 94);
			tableLayoutPanel4.TabIndex = 0;
			tableLayoutPanel5.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			tableLayoutPanel5.ColumnCount = 2;
			tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.15094f));
			tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.84906f));
			tableLayoutPanel5.Controls.Add(label2, 0, 0);
			tableLayoutPanel5.Controls.Add(label1, 0, 1);
			tableLayoutPanel5.Controls.Add(comboBox1, 1, 1);
			tableLayoutPanel5.Controls.Add(textBox1, 1, 0);
			tableLayoutPanel5.Location = new System.Drawing.Point(3, 3);
			tableLayoutPanel5.Name = "tableLayoutPanel5";
			tableLayoutPanel5.RowCount = 2;
			tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
			tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
			tableLayoutPanel5.Size = new System.Drawing.Size(265, 88);
			tableLayoutPanel5.TabIndex = 0;
			label1.Anchor = System.Windows.Forms.AnchorStyles.None;
			label1.AutoSize = true;
			label1.ForeColor = System.Drawing.SystemColors.Desktop;
			label1.Location = new System.Drawing.Point(17, 14);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(82, 15);
			label1.TabIndex = 0;
			label1.Text = "常用位置：";
			label2.Anchor = System.Windows.Forms.AnchorStyles.None;
			label2.AutoSize = true;
			label2.ForeColor = System.Drawing.SystemColors.Desktop;
			label2.Location = new System.Drawing.Point(17, 58);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(82, 15);
			label2.TabIndex = 1;
			label2.Text = "位置别名：";
			textBox1.Anchor = (System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			textBox1.Location = new System.Drawing.Point(119, 53);
			textBox1.Name = "textBox1";
			textBox1.Size = new System.Drawing.Size(143, 25);
			textBox1.TabIndex = 2;
			comboBox1.Anchor = (System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			comboBox1.FormattingEnabled = true;
			comboBox1.Location = new System.Drawing.Point(119, 10);
			comboBox1.Name = "comboBox1";
			comboBox1.Size = new System.Drawing.Size(143, 23);
			comboBox1.TabIndex = 3;
			comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			comboBox1.DisplayMember = "Key";
			comboBox1.ValueMember = "Value";
			comboBox1.SelectedIndexChanged += new System.EventHandler(comboBox1_SelectedIndexChanged);
			tableLayoutPanel6.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			tableLayoutPanel6.ColumnCount = 1;
			tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
			tableLayoutPanel6.Controls.Add(button3, 0, 0);
			tableLayoutPanel6.Controls.Add(button4, 0, 1);
			tableLayoutPanel6.Location = new System.Drawing.Point(274, 3);
			tableLayoutPanel6.Name = "tableLayoutPanel6";
			tableLayoutPanel6.RowCount = 2;
			tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
			tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
			tableLayoutPanel6.Size = new System.Drawing.Size(111, 88);
			tableLayoutPanel6.TabIndex = 1;
			button3.Anchor = (System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			button3.ForeColor = System.Drawing.SystemColors.Desktop;
			button3.Location = new System.Drawing.Point(3, 10);
			button3.Name = "button3";
			button3.Size = new System.Drawing.Size(105, 23);
			button3.TabIndex = 0;
			button3.Text = "保存常用位置";
			button3.UseVisualStyleBackColor = true;
			button3.Click += new System.EventHandler(button3_Click);
			button4.Anchor = (System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			button4.ForeColor = System.Drawing.SystemColors.Desktop;
			button4.Location = new System.Drawing.Point(3, 54);
			button4.Name = "button4";
			button4.Size = new System.Drawing.Size(105, 23);
			button4.TabIndex = 1;
			button4.Text = "删除常用位置";
			button4.UseVisualStyleBackColor = true;
			button4.Click += new System.EventHandler(button4_Click);
			statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5]
			{
				toolStripStatusLabel1,
				toolStripStatusLabel2,
				toolStripStatusLabel4,
				toolStripStatusLabel5,
				toolStripStatusLabel3
			});
			statusStrip1.Location = new System.Drawing.Point(0, 574);
			statusStrip1.Name = "statusStrip1";
			statusStrip1.Size = new System.Drawing.Size(800, 36);
			statusStrip1.TabIndex = 1;
			statusStrip1.Text = "statusStrip1";
			statusStrip1.SizingGrip = false;
			toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			toolStripStatusLabel1.Size = new System.Drawing.Size(84, 20);
			toolStripStatusLabel1.Text = "当前设备:";
			toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			toolStripStatusLabel2.Size = new System.Drawing.Size(151, 20);
			toolStripStatusLabel2.Text = "未连接";
			toolStripStatusLabel3.IsLink = true;
			toolStripStatusLabel3.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
			toolStripStatusLabel3.Name = "toolStripStatusLabel3";
			toolStripStatusLabel3.Size = new System.Drawing.Size(69, 20);
			toolStripStatusLabel3.Text = "激活卡密";
			toolStripStatusLabel3.Click += new System.EventHandler(toolStripStatusLabel3_Click);
			toolStripStatusLabel4.Name = "toolStripStatusLabel4";
			toolStripStatusLabel4.Size = new System.Drawing.Size(54, 20);
			toolStripStatusLabel4.Text = "到期时间:";
			toolStripStatusLabel5.Name = "toolStripStatusLabel5";
			toolStripStatusLabel5.Size = new System.Drawing.Size(54, 20);
			toolStripStatusLabel5.Text = "卡密已过期";
			bgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
			base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 15f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			int width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
			int height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
			base.ClientSize = new System.Drawing.Size(width / 10 * 5, height / 10 * 5);
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			base.Icon = iFakeLocation.Properties.Resources.logo;
			base.Controls.Add(tableLayoutPanel1);
			base.Name = "MainForm";
			Text = iFakeLocation.Properties.Resources.name + " v" + iFakeLocation.Properties.Resources.version;
			base.Controls.Add(statusStrip1);
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel2.ResumeLayout(false);
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			tableLayoutPanel3.ResumeLayout(false);
			groupBox2.ResumeLayout(false);
			tableLayoutPanel4.ResumeLayout(false);
			tableLayoutPanel5.ResumeLayout(false);
			tableLayoutPanel5.PerformLayout();
			tableLayoutPanel6.ResumeLayout(false);
			statusStrip1.ResumeLayout(false);
			statusStrip1.PerformLayout();
			ResumeLayout(false);
		}
	}
}
