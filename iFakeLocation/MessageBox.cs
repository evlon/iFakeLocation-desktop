using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace iFakeLocation
{
	public class MessageBox : Form
	{
		private IContainer components;

		private TableLayoutPanel tableLayoutPanel1;

		private Label label1;

		private Button button1;

		public MessageBox(string msg)
		{
			InitializeComponent();
			label1.Text = msg;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Close();
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
			button1 = new System.Windows.Forms.Button();
			tableLayoutPanel1.SuspendLayout();
			SuspendLayout();
			tableLayoutPanel1.ColumnCount = 3;
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15f));
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70f));
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15f));
			tableLayoutPanel1.Controls.Add(label1, 1, 1);
			tableLayoutPanel1.Controls.Add(button1, 1, 2);
			tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 3;
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15f));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35f));
			tableLayoutPanel1.Size = new System.Drawing.Size(332, 155);
			tableLayoutPanel1.TabIndex = 0;
			label1.Anchor = (System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(52, 23);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(226, 77);
			label1.TabIndex = 0;
			label1.Text = "";
			button1.Dock = System.Windows.Forms.DockStyle.Fill;
			button1.Location = new System.Drawing.Point(79, 110);
			button1.Margin = new System.Windows.Forms.Padding(30, 11, 30, 11);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(172, 35);
			button1.TabIndex = 1;
			button1.Text = "确定";
			button1.UseVisualStyleBackColor = true;
			button1.Click += new System.EventHandler(button1_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 15f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(200, 130);
			base.ControlBox = false;
			base.Controls.Add(tableLayoutPanel1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			base.Name = "MessageBox";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "";
			
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel1.PerformLayout();
			ResumeLayout(false);
		}
	}
}
