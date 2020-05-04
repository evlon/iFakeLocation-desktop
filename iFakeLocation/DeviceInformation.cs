using iMobileDevice;
using iMobileDevice.Afc;
using iMobileDevice.iDevice;
using iMobileDevice.Lockdown;
using iMobileDevice.MobileImageMounter;
using iMobileDevice.Plist;
using iMobileDevice.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace iFakeLocation
{
	internal class DeviceInformation
	{
		private enum DiskImageUploadMode
		{
			AFC,
			UploadImage
		}

		private static readonly MobileImageMounterUploadCallBack MounterUploadCallback = MounterReadCallback;

		public string Name
		{
			get;
		}

		public string UDID
		{
			get;
		}

		public Dictionary<string, object> Properties
		{
			get;
			private set;
		}

		public DeviceInformation(string name, string udid)
		{
			Name = name;
			UDID = udid;
			Properties = new Dictionary<string, object>();
		}

		private void ReadProperties(PlistHandle node)
		{
			Properties = PlistReader.ReadPlistDictFromNode(node, new string[4]
			{
				"DeviceName",
				"ProductType",
				"ProductVersion",
				"HostAttached"
			});
		}

		private static int MounterReadCallback(IntPtr buffer, uint size, IntPtr userData)
		{
			FileStream obj = (FileStream)GCHandle.FromIntPtr(userData).Target;
			byte[] array = new byte[size];
			int result = obj.Read(array, 0, array.Length);
			Marshal.Copy(array, 0, buffer, array.Length);
			return result;
		}

		public void EnableDeveloperMode(string deviceImagePath, string deviceImageSignaturePath)
		{
			if (!File.Exists(deviceImagePath) || !File.Exists(deviceImageSignaturePath))
			{
				throw new FileNotFoundException("The specified device image files do not exist.");
			}
			iDeviceHandle device = null;
			LockdownClientHandle client = null;
			LockdownServiceDescriptorHandle service = null;
			MobileImageMounterClientHandle client2 = null;
			AfcClientHandle client3 = null;
			PlistHandle result = null;
			FileStream fileStream = null;
			DiskImageUploadMode diskImageUploadMode = (int.Parse(((string)Properties["ProductVersion"]).Split('.')[0]) >= 7) ? DiskImageUploadMode.UploadImage : DiskImageUploadMode.AFC;
			IiDeviceApi iDevice = LibiMobileDevice.Instance.iDevice;
			ILockdownApi lockdown = LibiMobileDevice.Instance.Lockdown;
			IServiceApi service2 = LibiMobileDevice.Instance.Service;
			IMobileImageMounterApi mobileImageMounter = LibiMobileDevice.Instance.MobileImageMounter;
			IAfcApi afc = LibiMobileDevice.Instance.Afc;
			try
			{
				if (iDevice.idevice_new(out device, UDID) != 0)
				{
					throw new Exception("Unable to open device, is it connected?");
				}
				if (lockdown.lockdownd_client_new_with_handshake(device, out client, "iFakeLocation") != 0)
				{
					throw new Exception("Unable to connect to lockdownd.");
				}
				if (lockdown.lockdownd_start_service(client, "com.apple.mobile.mobile_image_mounter", out service) != 0)
				{
					throw new Exception("Unable to start the mobile image mounter service.");
				}
				if (mobileImageMounter.mobile_image_mounter_new(device, service, out client2) != 0)
				{
					throw new Exception("Unable to create mobile image mounter instance.");
				}
				service.Close();
				service = null;
				if (diskImageUploadMode == DiskImageUploadMode.AFC)
				{
					if (lockdown.lockdownd_start_service(client, "com.apple.afc", out service) != 0)
					{
						throw new Exception("Unable to start AFC service.");
					}
					if (afc.afc_client_new(device, service, out client3) != 0)
					{
						throw new Exception("Unable to connect to AFC service.");
					}
					service.Close();
					service = null;
				}
				client.Close();
				client = null;
				if (mobileImageMounter.mobile_image_mounter_lookup_image(client2, "Developer", out result) != 0)
				{
					goto IL_01c7;
				}
				Dictionary<string, object> dictionary = PlistReader.ReadPlistDictFromNode(result, new string[2]
				{
					"ImagePresent",
					"ImageSignature"
				});
				if ((!dictionary.ContainsKey("ImagePresent") || !(dictionary["ImagePresent"] is bool) || !(bool)dictionary["ImagePresent"]) && !dictionary.ContainsKey("ImageSignature"))
				{
					goto IL_01c7;
				}
				goto end_IL_0094;
				IL_01c7:
				result.Close();
				result = null;
				string text = "PublicStaging/staging.dimage";
				string imagePath = "/private/var/mobile/Media/" + text;
				fileStream = new FileStream(deviceImagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
				byte[] array = File.ReadAllBytes(deviceImageSignaturePath);
				switch (diskImageUploadMode)
				{
				case DiskImageUploadMode.UploadImage:
				{
					GCHandle value = GCHandle.Alloc(fileStream);
					mobileImageMounter.mobile_image_mounter_upload_image(client2, "Developer", uint.Parse(fileStream.Length.ToString()), array, ushort.Parse(array.Length.ToString()), MounterUploadCallback, GCHandle.ToIntPtr(value));
					value.Free();
					break;
				}
				case DiskImageUploadMode.AFC:
				{
					ReadOnlyCollection<string> fileInformation;
					if (afc.afc_get_file_info(client3, "PublicStaging", out fileInformation) != 0 || afc.afc_make_directory(client3, "PublicStaging") != 0)
					{
						throw new Exception("Unable to create directory 'PublicStaging' on the device.");
					}
					ulong handle = 0uL;
					if (afc.afc_file_open(client3, text, AfcFileMode.FopenWronly, ref handle) != 0)
					{
						throw new Exception("Unable to create file '" + text + "'.");
					}
					uint num = 0u;
					byte[] array2 = new byte[8192];
					do
					{
						num = (uint)fileStream.Read(array2, 0, array2.Length);
						if (num != 0)
						{
							uint bytesWritten = 0u;
							uint num2;
							for (num2 = 0u; num2 < num; num2 += bytesWritten)
							{
								if (afc.afc_file_write(client3, handle, array2, num, ref bytesWritten) != 0)
								{
									afc.afc_file_close(client3, handle);
									throw new Exception("An AFC write error occurred.");
								}
							}
							if (num2 != num)
							{
								afc.afc_file_close(client3, handle);
								throw new Exception("The developer image was not written completely.");
							}
						}
					}
					while (num != 0);
					afc.afc_file_close(client3, handle);
					break;
				}
				}
				if (mobileImageMounter.mobile_image_mounter_mount_image(client2, imagePath, array, ushort.Parse(array.Length.ToString()), "Developer", out result) != 0)
				{
					throw new Exception("Unable to mount developer image.");
				}
				Dictionary<string, object> dictionary2 = PlistReader.ReadPlistDictFromNode(result);
				if (!dictionary2.ContainsKey("Status") || dictionary2["Status"] as string!= "Complete")
				{
					object obj = dictionary2.ContainsKey("Status") ? dictionary2["Status"] : "N/A";
					string str = (obj != null) ? obj.ToString() : null;
					object obj2 = dictionary2.ContainsKey("Error") ? dictionary2["Error"] : "N/A";
					throw new Exception("Mount failed with status: " + str + " and error: " + ((obj2 != null) ? obj2.ToString() : null));
				}
				end_IL_0094:;
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
				if (result != null)
				{
					result.Close();
				}
				if (client3 != null)
				{
					client3.Close();
				}
				if (client2 != null)
				{
					client2.Close();
				}
				if (service != null)
				{
					service.Close();
				}
				if (client != null)
				{
					client.Close();
				}
				if (device != null)
				{
					device.Close();
				}
			}
		}

		private static byte[] ToBytesBE(int i)
		{
			byte[] bytes = BitConverter.GetBytes((uint)i);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			return bytes;
		}

		public bool SetLocation(PointLatLng target)
		{
			try
			{
				SetLocation(this, target);
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public void StopLocation()
		{
			SetLocation(this, null);
		}

		private static void SetLocation(DeviceInformation deviceInfo, PointLatLng? target)
		{
			iDeviceHandle device = null;
			LockdownClientHandle client = null;
			LockdownServiceDescriptorHandle service = null;
			ServiceClientHandle client2 = null;
			IiDeviceApi iDevice = LibiMobileDevice.Instance.iDevice;
			ILockdownApi lockdown = LibiMobileDevice.Instance.Lockdown;
			IServiceApi service2 = LibiMobileDevice.Instance.Service;
			try
			{
				if (iDevice.idevice_new(out device, deviceInfo.UDID) != 0)
				{
					throw new Exception("Unable to connect to the device. Make sure it is connected.");
				}
				if (lockdown.lockdownd_client_new_with_handshake(device, out client, "iFakeLocation") != 0)
				{
					throw new Exception("Unable to connect to lockdownd.");
				}
				if (lockdown.lockdownd_start_service(client, "com.apple.dt.simulatelocation", out service) != 0 || service.IsInvalid)
				{
					throw new Exception("Unable to start simulatelocation service.");
				}
				if (service2.service_client_new(device, service, out client2) != 0)
				{
					throw new Exception("Unable to create simulatelocation service client.");
				}
				if (!target.HasValue)
				{
					byte[] array = ToBytesBE(1);
					uint sent = 0u;
					if (service2.service_send(client2, array, (uint)array.Length, ref sent) != 0)
					{
						throw new Exception("Unable to send stop message to device.");
					}
				}
				else
				{
					byte[] array2 = ToBytesBE(0);
					byte[] bytes = Encoding.ASCII.GetBytes(target.Value.Lat.ToString(CultureInfo.InvariantCulture));
					byte[] bytes2 = Encoding.ASCII.GetBytes(target.Value.Lng.ToString(CultureInfo.InvariantCulture));
					byte[] array3 = ToBytesBE(bytes.Length);
					byte[] array4 = ToBytesBE(bytes2.Length);
					uint sent2 = 0u;
					if (service2.service_send(client2, array2, (uint)array2.Length, ref sent2) != 0 || service2.service_send(client2, array3, (uint)array3.Length, ref sent2) != 0 || service2.service_send(client2, bytes, (uint)bytes.Length, ref sent2) != 0 || service2.service_send(client2, array4, (uint)array4.Length, ref sent2) != 0 || service2.service_send(client2, bytes2, (uint)bytes2.Length, ref sent2) != 0)
					{
						throw new Exception("Unable to send co-ordinates to device.");
					}
				}
			}
			finally
			{
				if (client2 != null)
				{
					client2.Close();
				}
				if (service != null)
				{
					service.Close();
				}
				if (client != null)
				{
					client.Close();
				}
				if (device != null)
				{
					device.Close();
				}
			}
		}

		public static List<DeviceInformation> GetDevices()
		{
			IiDeviceApi iDevice = LibiMobileDevice.Instance.iDevice;
			ILockdownApi lockdown = LibiMobileDevice.Instance.Lockdown;
			IPlistApi plist = LibiMobileDevice.Instance.Plist;
			int count = 0;
			ReadOnlyCollection<string> devices;
			if (iDevice.idevice_get_device_list(out devices, ref count) != 0)
			{
				return new List<DeviceInformation>();
			}
			iDeviceHandle device = null;
			LockdownClientHandle client = null;
			PlistHandle value = null;
			List<DeviceInformation> list = new List<DeviceInformation>();
			foreach (string item in devices.Distinct())
			{
				try
				{
					string deviceName;
					if (iDevice.idevice_new(out device, item) == iDeviceError.Success && lockdown.lockdownd_client_new_with_handshake(device, out client, "iFakeLocation") == LockdownError.Success && lockdown.lockdownd_get_device_name(client, out deviceName) == LockdownError.Success)
					{
						DeviceInformation deviceInformation = new DeviceInformation(deviceName, item);
						if (lockdown.lockdownd_get_value(client, null, null, out value) == LockdownError.Success && plist.plist_get_node_type(value) == PlistType.Dict)
						{
							deviceInformation.ReadProperties(value);
							if (!deviceInformation.Properties.ContainsKey("HostAttached") || (bool)deviceInformation.Properties["HostAttached"])
							{
								list.Add(deviceInformation);
							}
						}
					}
				}
				finally
				{
					if (value != null)
					{
						value.Close();
					}
					if (client != null)
					{
						client.Close();
					}
					if (device != null)
					{
						device.Close();
					}
				}
			}
			return list;
		}
	}
}
