using System;
using System.Windows.Forms;

namespace iFakeLocation
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			try
			{
				Application.SetHighDpiMode(HighDpiMode.SystemAware);
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				WelcomeForm welcomeForm = new WelcomeForm();
				if (welcomeForm.ShowDialog() == DialogResult.OK)
				{
					welcomeForm.timer1.Stop();
					Application.Run(new MainForm());
				}
			}
			catch (Exception e)
			{
				HttpHelper.uploadException(e);
				throw;
			}
		}
	}
}
