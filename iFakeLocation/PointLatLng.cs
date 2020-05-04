using System.Runtime.CompilerServices;

namespace iFakeLocation
{
	internal struct PointLatLng
	{
		public static readonly PointLatLng Empty = new PointLatLng(0.0, 0.0);

		public double Lng
		{
			get;
			set;
		}

		public double Lat
		{
			get;
			set;
		}

		public PointLatLng(double lng, double lat)
		{
			Lng = lng;
			Lat = lat;
		}
	}
}
