
using iFakeLocation.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iFakeLocation
{
	internal class HttpHelper
	{
		private static HttpClient httpClient;

		static HttpHelper()
		{
			
			   HttpClientHandler httpClientHandler = new HttpClientHandler();
			X509Certificate2 value = new X509Certificate2(Resources.cert);
			httpClientHandler.ClientCertificates.Add(value);
			httpClient = new HttpClient(httpClientHandler);
		}

		internal static HttpStatusCode uploadException(Exception e)
		{
			string text = new Random().Next(1000, 10000).ToString();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("message", e.Message);
			HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
			{
				{
					"a",
					DESHelper.Encrypt(JsonConvert.SerializeObject(dictionary), Resources.secertKey, text)
				},
				{
					"b",
					Resources.appId
				},
				{
					"c",
					Resources.version
				},
				{
					"d",
					text
				}
			});
			Task<HttpResponseMessage> task = httpClient.PostAsync(Resources.baseUrl + "c", content);
			try
			{
				return task.Result.StatusCode;
			}
			catch (Exception)
			{
				return (HttpStatusCode)0;
			}
		}

		public static DeviceStatus GetStatus(string udid)
		{
			//string random = new Random().Next(1000, 10000).ToString();
			//Dictionary<string, string> dictionary = new Dictionary<string, string>();
			//dictionary.Add("deviceId", udid);
			//string encryptUrl = DESHelper.GetEncryptUrl(Resources.baseUrl + "a", JsonConvert.SerializeObject(dictionary), Resources.appId, Resources.version, Resources.secertKey, random);
			//Task<HttpResponseMessage> async = httpClient.GetAsync(encryptUrl);
			//if (async.Result.StatusCode == HttpStatusCode.OK)
			//{
			//	return JsonConvert.DeserializeObject<DeviceStatus>(DESHelper.Decrypt(async.Result.Content.ReadAsByteArrayAsync().Result, Resources.secertKey, random));
			//}
			//return new DeviceStatus(false, "网络异常");
			return new DeviceStatus(true,"OK") {Code = "Code", Available = true, EndTime = new DateTime(9999, 12, 31), Msg = "OK", Success = true, Trial = false };
		}

		public static bool verifyDevice(string udid)
		{
			DeviceStatus status;
			try
			{
				status = GetStatus(udid);
			}
			catch (Exception e)
			{
				uploadException(e);
				return false;
			}
			return status.Available;
		}

		public static DeviceStatus active(string udid, string deviceName, string cardNum)
		{
			string random = new Random().Next(1000, 10000).ToString();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("deviceId", udid);
			dictionary.Add("deviceName", deviceName);
			dictionary.Add("cardNum", cardNum);
			string encryptUrl = DESHelper.GetEncryptUrl(Resources.baseUrl + "b", JsonConvert.SerializeObject(dictionary), Resources.appId, Resources.version, Resources.secertKey, random);
			Task<HttpResponseMessage> async = httpClient.GetAsync(encryptUrl);
			if (async.Result.StatusCode == HttpStatusCode.OK)
			{
				return JsonConvert.DeserializeObject<DeviceStatus>(DESHelper.Decrypt(async.Result.Content.ReadAsByteArrayAsync().Result, Resources.secertKey, random));
			}
			return new DeviceStatus(false, "网络异常");
		}

		public static string downLoadImage(string[] p, TextBox prog)
		{
			try
			{
				for (int i = 0; i < p.Length; i++)
				{
					string text = p[i];
					int num = text.IndexOf("DeveloperImages\\") + "DeveloperImages\\".Length;
					int length = text.Length;
					string requestUriString = Resources.driverUrl + text.Substring(num, length - num);
					string path = text.Substring(0, text.LastIndexOf("\\"));
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
					Stream responseStream = ((HttpWebResponse)((HttpWebRequest)WebRequest.Create(requestUriString)).GetResponse()).GetResponseStream();
					Stream stream = new FileStream(text, FileMode.Create);
					long num2 = 0L;
					byte[] array = new byte[1024];
					int num3 = responseStream.Read(array, 0, array.Length);
					string str = prog.Text + DateTime.Now.ToString("HH:mm:ss") + "\t正在下载驱动程序：";
					while (num3 > 0)
					{
						num2 = num3 + num2;
						Application.DoEvents();
						stream.Write(array, 0, num3);
						if (prog != null && i == 0)
						{
							prog.Text = str + Math.Round((double)num2 / 1024.0) + "KB \r\n";
						}
						num3 = responseStream.Read(array, 0, array.Length);
					}
					stream.Close();
					responseStream.Close();
				}
				return "";
			}
			catch (Exception ex)
			{
				if (ex is WebException)
				{
					WebException ex2 = (WebException)ex;
					if (ex2.Status == WebExceptionStatus.ProtocolError && ex2.Response != null && ((HttpWebResponse)ex2.Response).StatusCode == HttpStatusCode.NotFound)
					{
						return "官方未更新驱动程序，请稍后再试";
					}
				}
				if (ex is IOException && ex.Message.Contains("used by another process"))
				{
					return "下载驱动程序失败，文件被另一个进程占用，请关闭其他iFakeLocation程序或重启电脑再试";
				}
				uploadException(ex);
				return "下载驱动程序失败，请关闭软件重试";
			}
		}
	}
}
