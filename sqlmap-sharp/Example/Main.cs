using System;
using sqlmapsharp;
using System.Collections.Generic;

namespace Example
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			using (SqlmapSession session = new SqlmapSession("127.0.0.1", 8775))
			{
				using (SqlmapManager manager = new SqlmapManager(session))
				{
					string taskid = manager.NewTask();

					Console.WriteLine(taskid);

					Dictionary<string, object> options = manager.GetOptions(taskid);

					manager.SetOption(taskid, "msfPath", "/path/to/msf");

					Dictionary<string, object> newoptions = manager.GetOptions(taskid);

					Console.WriteLine("Old msfpath: " + options["msfPath"].ToString());
					Console.WriteLine("New msfpath: " + newoptions["msfPath"].ToString());

					options["url"] = "http://192.168.1.254/xslt?PAGE=C_0_0";

					manager.StartTask(taskid, options);

					SqlmapStatus status = manager.GetScanStatus(taskid);

					while (status.Status != "terminated")
					{
						System.Threading.Thread.Sleep(new TimeSpan(0,0,10));
						status = manager.GetScanStatus(taskid);
					}

					List<SqlmapLogItem> logItems = manager.GetLog(taskid);

					foreach (SqlmapLogItem item in logItems)
						Console.WriteLine(item.Message);

					manager.DeleteTask(taskid);
				}
			}
		}
	}
}
