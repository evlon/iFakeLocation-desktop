using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace iFakeLocation
{
	internal class DESHelper
	{
		public static string GetEncryptUrl(string url, string param, string appId, string version, string secertKey, string random)
		{
			string text = random + secertKey;
			byte[] bytes = Encoding.UTF8.GetBytes(text.Substring(0, 8));
			byte[] data = EncryptTextToMemory(param, bytes, bytes);
			return url + "?a=" + BytesToHexString(data) + "&b=" + appId + "&c=" + version + "&d=" + random;
		}

		public static string Encrypt(string param, string secertKey, string random)
		{
			string text = random + secertKey;
			byte[] bytes = Encoding.UTF8.GetBytes(text.Substring(0, 8));
			return BytesToHexString(EncryptTextToMemory(param, bytes, bytes));
		}

		public static string Decrypt(byte[] byteData, string secertKey, string random)
		{
			string s = (random + secertKey).Substring(0, 8);
			return DecryptTextFromMemory(byteData, Encoding.UTF8.GetBytes(s), Encoding.UTF8.GetBytes(s));
		}

		public static byte[] EncryptTextToMemory(string Data, byte[] Key, byte[] IV)
		{
			try
			{
				MemoryStream memoryStream = new MemoryStream();
				DES dES = DES.Create();
				CryptoStream cryptoStream = new CryptoStream(memoryStream, dES.CreateEncryptor(Key, IV), CryptoStreamMode.Write);
				byte[] bytes = Encoding.UTF8.GetBytes(Data);
				cryptoStream.Write(bytes, 0, bytes.Length);
				cryptoStream.FlushFinalBlock();
				byte[] result = memoryStream.ToArray();
				cryptoStream.Close();
				memoryStream.Close();
				return result;
			}
			catch (CryptographicException ex)
			{
				Console.WriteLine("A Cryptographic error occurred: {0}", ex.Message);
				return null;
			}
		}

		public static string DecryptTextFromMemory(byte[] Data, byte[] Key, byte[] IV)
		{
			try
			{
				MemoryStream stream = new MemoryStream(Data);
				DES dES = DES.Create();
				CryptoStream cryptoStream = new CryptoStream(stream, dES.CreateDecryptor(Key, IV), CryptoStreamMode.Read);
				byte[] array = new byte[Data.Length];
				cryptoStream.Read(array, 0, array.Length);
				return Encoding.UTF8.GetString(array).Replace("\0", "");
			}
			catch (CryptographicException ex)
			{
				Console.WriteLine("A Cryptographic error occurred: {0}", ex.Message);
				return null;
			}
		}

		public static byte[] HexStringToBytes(string Data)
		{
			byte[] array = new byte[Data.Length / 2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Convert.ToByte(Data.Substring(i * 2, 2), 16);
			}
			return array;
		}

		public static string BytesToHexString(byte[] data)
		{
			return BitConverter.ToString(data, 0).Replace("-", string.Empty);
		}
	}
}
