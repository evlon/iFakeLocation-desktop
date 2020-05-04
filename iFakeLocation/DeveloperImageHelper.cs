using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace iFakeLocation
{
	internal static class DeveloperImageHelper
	{
		private class WebClientEx : WebClient
		{
			protected override WebRequest GetWebRequest(Uri address)
			{
				HttpWebRequest httpWebRequest = base.GetWebRequest(address) as HttpWebRequest;
				if (httpWebRequest != null)
				{
					httpWebRequest.AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate);
				}
				return httpWebRequest;
			}
		}

		private static readonly WebClient WebClient = new WebClientEx();

		private static readonly Dictionary<string, string> VersionToImageUrl = new Dictionary<string, string>();

		private const string ImagePath = "DeveloperImages";

		private static readonly Dictionary<string, string> VersionMapping = new Dictionary<string, string>();

		public static bool HasImageForDevice(DeviceInformation device)
		{
			string[] paths;
			return HasImageForDevice(device, out paths);
		}

		public static string GetSoftwareVersion(DeviceInformation device)
		{
			string[] array = ((string)device.Properties["ProductVersion"]).Split('.');
			string text = array[0] + "." + array[1];
			if (!VersionMapping.ContainsKey(text))
			{
				return text;
			}
			return VersionMapping[text];
		}

		public static bool HasImageForDevice(DeviceInformation device, out string[] paths)
		{
			string softwareVersion = GetSoftwareVersion(device);
			string text = Application.StartupPath + Path.Combine("DeveloperImages", softwareVersion, "DeveloperDiskImage.dmg");
			string text2 = text + ".signature";
			paths = new string[2]
			{
				text,
				text2
			};
			if (File.Exists(text))
			{
				return File.Exists(text2);
			}
			return false;
		}

		public static Tuple<string, string>[] GetLinksForDevice(DeviceInformation device)
		{
			string softwareVersion = GetSoftwareVersion(device);
			if (VersionToImageUrl.Count == 0)
			{
				string str = "795fc91f28cb3884edc45b876482911c797de85c";
				try
				{
					WebClient.Headers.Set("X-Requested-With","XMLHttpRequest");
					string text = WebClient.DownloadString("https://github.com/xushuduo/Xcode-iOS-Developer-Disk-Image/find/master?_pjax=%23js-repo-pjax-container");
					string text2 = "/tree-list/";
					int num = text.IndexOf(text2, StringComparison.InvariantCultureIgnoreCase);
					if (num != -1)
					{
						str = text.Substring(num + text2.Length, text.IndexOf('"', num) - (num + text2.Length));
					}
				}
				catch
				{
				}
				WebClient.Headers.Add("Accept", "application/json");
				WebClient.Headers.Set("X-Requested-With", "XMLHttpRequest");
				string[] array = (from s in WebClient.DownloadString("https://github.com/xushuduo/Xcode-iOS-Developer-Disk-Image/tree-list/" + str).Split('"')
					where s.EndsWith(".dmg", StringComparison.InvariantCultureIgnoreCase)
					select s).ToArray();
				foreach (string text3 in array)
				{
					VersionToImageUrl[text3.Split('/')[1].Split(' ')[0]] = "https://github.com/xushuduo/Xcode-iOS-Developer-Disk-Image/raw/master/" + text3;
				}
			}
			string value;
			if (!VersionToImageUrl.TryGetValue(softwareVersion, out value))
			{
				return null;
			}
			return new Tuple<string, string>[2]
			{
				new Tuple<string, string>(value, Path.Combine("DeveloperImages", softwareVersion, "DeveloperDiskImage.dmg")),
				new Tuple<string, string>(value + ".signature", Path.Combine("DeveloperImages", softwareVersion, "DeveloperDiskImage.dmg.signature"))
			};
		}
	}
}
