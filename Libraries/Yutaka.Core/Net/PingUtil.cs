using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace Yutaka.Core.Net
{
	public static class PingUtil
	{
		/// <summary>
		/// Attempts to send an Internet Control Message Protocol (ICMP) echo message to the specified host, and receive a corresponding ICMP echo reply message from that host. This method allows you to specify the max-count and a time-out value for the operation.
		/// </summary>
		/// <param name="hostNameOrAddress">A <see cref="String"/> that identifies the host that is the destination for the ICMP echo message. The value specified for this parameter can be a host name or a string representation of an IP address.</param>
		/// <param name="count">An <see cref="Int32"/> value that specifies the maximum number of tries to send an ICMP echo message.</param>
		/// <param name="timeout">An <see cref="Int32"/> value that specifies the maximum number of milliseconds (after sending the echo message) to wait for the ICMP echo reply message.</param>
		/// <returns>True if pingable. Otherwise false.</returns>
		public static bool IsPingable(string hostNameOrAddress, int count = 4, int timeout = 4000)
		{
			#region Check Input
			if (hostNameOrAddress == null)
				throw new ArgumentNullException("hostNameOrAddress");
			else if (String.IsNullOrWhiteSpace(hostNameOrAddress))
				throw new ArgumentException("'hostNameOrAddress' is requred.");
			if (count < 1)
				count = 1;
			if (timeout < 2000)
				timeout = 2000;
			#endregion Check Input

			try {
				using (var ping = new Ping()) {
					PingReply reply;
					reply = ping.Send(hostNameOrAddress, timeout);

					if (reply.Status == IPStatus.Success)
						return true;
					else {
						if (--count > 0)
							return IsPingable(hostNameOrAddress, count, timeout);
						else
							return false;
					}
				}
			}

			catch (Exception ex) {
				#region Logging
				if (ex.InnerException == null)
					Console.Write("\n{0}{2}Exception thrown in PingUtil.IsPingable(string hostNameOrAddress='{3}', int count='{4}', int timeout='{5}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, hostNameOrAddress, count, timeout);
				else
					Console.Write("\n{0}{2}Exception thrown in INNER EXCEPTION of PingUtil.IsPingable(string hostNameOrAddress='{3}', int count='{4}', int timeout='{5}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, hostNameOrAddress, count, timeout);
				#endregion Logging
				return false;
			}
		}

		/// <summary>
		/// Attempts to send an Internet Control Message Protocol (ICMP) echo message to all specified hosts in an array, and receive a corresponding ICMP echo reply message from those hosts. This method allows you to specify the max-count and a time-out value for the operation.
		/// </summary>
		/// <param name="hostNamesOrAddresses">A <see cref="String"/> array that identifies the hosts that are the destination for the ICMP echo messages. The value specified for this parameter can be a host name or a string representation of an IP address.</param>
		/// <param name="count">An <see cref="Int32"/> value that specifies the maximum number of tries to send an ICMP echo message.</param>
		/// <param name="timeout">An <see cref="Int32"/> value that specifies the maximum number of milliseconds (after sending the echo message) to wait for the ICMP echo reply message.</param>
		/// <returns>True all hosts are pingable. Otherwise false.</returns>
		public static bool ArePingable(string[] hostNamesOrAddresses, int count = 4, int timeout = 4000)
		{
			#region Check Input
			if (hostNamesOrAddresses == null || hostNamesOrAddresses.Length < 1)
				throw new ArgumentNullException("hostNamesOrAddresses");
			else if (String.IsNullOrWhiteSpace(hostNamesOrAddresses[0]))
				throw new ArgumentException("'hostNamesOrAddresses[0]' is empty.");
			if (count < 1)
				count = 1;
			if (timeout < 2000)
				timeout = 2000;
			#endregion Check Input

			try {
				return hostNamesOrAddresses.All(x => IsPingable(x, count, timeout));
			}

			catch (Exception ex) {
				#region Logging
				if (ex.InnerException == null)
					Console.Write("\n{0}{2}Exception thrown in PingUtil.ArePingable(string[] hostNamesOrAddresses='{3}', int count='{4}', int timeout='{5}').{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, hostNamesOrAddresses, count, timeout);
				else
					Console.Write("\n{0}{2}Exception thrown in INNER EXCEPTION of PingUtil.ArePingable(string[] hostNamesOrAddresses='{3}', int count='{4}', int timeout='{5}').{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, hostNamesOrAddresses, count, timeout);
				#endregion Logging
				return false;
			}
		}
	}
}