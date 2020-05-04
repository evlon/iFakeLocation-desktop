using System;

namespace iFakeLocation
{
	public class DeviceStatus
	{
		public bool Success
		{
			get;
			set;
		}

		public string Code
		{
			get;
			set;
		}

		public string Msg
		{
			get;
			set;
		}

		public bool Trial
		{
			get;
			set;
		}

		public bool Available
		{
			get;
			set;
		}

		public DateTime EndTime
		{
			get;
			set;
		}

		public DeviceStatus(bool Success, string Msg)
		{
			this.Success = Success;
			this.Msg = Msg;
		}
	}
}
