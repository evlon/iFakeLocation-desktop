using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace iFakeLocation
{
	internal class SqliteHelper
	{
		private static SQLiteConnection Conn;

		public static void InitSqliteDB()
		{
			string text = Application.StartupPath + "\\sqlite.db";
			if (!File.Exists(text))
			{
				SQLiteConnection.CreateFile(text);
				Conn = new SQLiteConnection("Data Source=" + text + ";Version=3;");
				Conn.Open();
				new SQLiteCommand("create table settings (name varchar(500), lng double,  lat double, orderd int)", Conn).ExecuteNonQuery();
			}
			else
			{
				Conn = new SQLiteConnection("Data Source=" + text + ";Version=3;");
				Conn.Open();
			}
		}

		public static void SaveSettings(string name, double lng, double lat, int order)
		{
			DelSettings(name);
			new SQLiteCommand("insert into settings (name, lng, lat, orderd) values ('" + name + "'," + lng + ", " + lat + "," + order + ")", Conn).ExecuteNonQuery();
		}

		public static Dictionary<string, PointLatLng> GetSettings()
		{
			Dictionary<string, PointLatLng> dictionary = new Dictionary<string, PointLatLng>();
			SQLiteDataReader sQLiteDataReader = new SQLiteCommand("select * from settings order by orderd", Conn).ExecuteReader();
			while (sQLiteDataReader.Read())
			{
				dictionary.Add((string)sQLiteDataReader["name"], new PointLatLng((double)sQLiteDataReader["lng"], (double)sQLiteDataReader["lat"]));
			}
			return dictionary;
		}

		public static void DelSettings(string name)
		{
			new SQLiteCommand("delete from settings where name='" + name + "'", Conn).ExecuteNonQuery();
		}
	}
}
