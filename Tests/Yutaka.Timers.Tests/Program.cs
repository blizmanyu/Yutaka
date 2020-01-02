using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Yutaka.Timers.Tests
{
	class Program
	{
		private const int TIMER_INTERVAL = 60000;
		private int eventId = 1;

		static void Main(string[] args)
		{
			var timer = new Timer { Interval = TIMER_INTERVAL };
			timer.Elapsed += new ElapsedEventHandler(OnTimer);
			timer.Start();
		}

		static public void OnTimer(object sender, ElapsedEventArgs args)
		{
			try {
				if (eventId == 1 && ProcessUtil.GetUpTime() < ONE_HOUR) {
					_rcwSmtpClient.Send("info@rcw1.com", "yblizman@rcw1.com", String.Format("{0} Restarted", Environment.MachineName), "");
					eventLog1.WriteEntry("Reboot Email Sent", EventLogEntryType.Information, eventId++);
				}
			}

			catch (Exception ex) {
				string log;

				if (ex.InnerException == null)
					log = String.Format("{0}{2}Exception thrown in SendRebootEmail().{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
				else
					log = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of SendRebootEmail().{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine);

				eventLog1.WriteEntry(log, EventLogEntryType.Error, eventId++);
			}
		}
	}
}