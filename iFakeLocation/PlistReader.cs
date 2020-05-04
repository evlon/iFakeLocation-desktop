using iMobileDevice;
using iMobileDevice.Plist;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace iFakeLocation
{
	internal static class PlistReader
	{
		private unsafe static object ReadValueFromNode(PlistHandle node)
		{
			if (node == null || node.IsInvalid)
			{
				return null;
			}
			IPlistApi plist = LibiMobileDevice.Instance.Plist;
			switch (plist.plist_get_node_type(node))
			{
			case PlistType.Boolean:
			{
				char val2 = '\0';
				plist.plist_get_bool_val(node, ref val2);
				return val2 != '\0';
			}
			case PlistType.Uint:
			{
				ulong val6 = 0uL;
				plist.plist_get_uint_val(node, ref val6);
				return val6;
			}
			case PlistType.Real:
			{
				double val5 = 0.0;
				plist.plist_get_real_val(node, ref val5);
				return val5;
			}
			case PlistType.String:
			{
				string val4;
				plist.plist_get_string_val(node, out val4);
				return val4;
			}
			case PlistType.Key:
			{
				string val3;
				plist.plist_get_key_val(node, out val3);
				return val3;
			}
			case PlistType.Data:
			{
				ulong length = 0uL;
				string val;
				plist.plist_get_data_val(node, out val, ref length);
				byte[] array = new byte[length];
				fixed (char* value = &(val != null ? ref val.GetPinnableReference() : ref *(char*)null))
				{
					Marshal.Copy((IntPtr)(void*)value, array, 0, array.Length);
				}
				return array;
			}
			case PlistType.Date:
			{
				int sec = 0;
				int usec = 0;
				plist.plist_get_date_val(node, ref sec, ref usec);
				return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(sec).ToLocalTime();
			}
			case PlistType.Dict:
				return ReadPlistDictFromNode(node);
			default:
				return null;
			}
		}

		public static Dictionary<string, object> ReadPlistDictFromNode(PlistHandle node, ICollection<string> keys = null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			IPlistApi plist = LibiMobileDevice.Instance.Plist;
			if (plist.plist_get_node_type(node) != PlistType.Dict)
			{
				return dictionary;
			}
			PlistHandle val = null;
			PlistDictIterHandle iter = null;
			plist.plist_dict_new_iter(node, out iter);
			string key;
			plist.plist_dict_next_item(node, iter, out key, out val);
			while (val != null && !val.IsInvalid)
			{
				if (keys == null || keys.Contains(key))
				{
					dictionary[key] = ReadValueFromNode(val);
				}
				val.Close();
				plist.plist_dict_next_item(node, iter, out key, out val);
				ReadValueFromNode(val);
			}
			return dictionary;
		}
	}
}
