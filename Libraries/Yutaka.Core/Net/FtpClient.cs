using System;
using System.Net;

namespace Yutaka.Core.Net
{
	public class FtpClient //: WebRequest
	{
		#region Fields
		private string m_host = null;
		/// <summary>
		/// The server to connect to
		/// </summary>
		public string Host
		{
			get => m_host;
			set { // remove unwanted prefix/postfix
				if (value.StartsWith("ftp://"))
					value = value.Substring(value.IndexOf("ftp://") + "ftp://".Length);
				if (value.EndsWith("/"))
					value = value.Replace("/", "");

				m_host = value;
			}
		}

		private int m_port = 0;
		/// <summary>
		/// The port to connect to. If this value is set to 0 (Default) the port used will be determined
		/// by the type of SSL used or if no SSL is to be used it  will automatically connect to port 21.
		/// </summary>
		public int Port
		{
			get { // automatically determine port when m_port is 0.
				if (m_port == 0)
					return 21;

				return m_port;
			}
			set => m_port = value;
		}

		/// <summary>
		/// Credentials used for authentication
		/// </summary>
		public NetworkCredential Credentials { get; set; } = new NetworkCredential("anonymous", "anonymous");
		#endregion Fields

		#region Constructors
		/// <summary>
		/// Creates a new instance of an FTP Client.
		/// </summary>
		public FtpClient() { }

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host.
		/// </summary>
		public FtpClient(string host)
		{
			Host = host ?? throw new ArgumentNullException("Host is required.");
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host and credentials.
		/// </summary>
		public FtpClient(string host, NetworkCredential credentials)
		{
			Host = host ?? throw new ArgumentNullException("Host is required.");
			Credentials = credentials ?? throw new ArgumentNullException("Credentials are required.");
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host, port and credentials.
		/// </summary>
		public FtpClient(string host, int port, NetworkCredential credentials)
		{
			Host = host ?? throw new ArgumentNullException("Host is required.");
			Port = port;
			Credentials = credentials ?? throw new ArgumentNullException("Credentials is required.");
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host, username and password.
		/// </summary>
		public FtpClient(string host, string user, string pass)
		{
			Host = host;
			Credentials = new NetworkCredential(user, pass);
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host, username, password and domain
		/// </summary>
		public FtpClient(string host, string user, string pass, string domain)
		{
			Host = host;
			Credentials = new NetworkCredential(user, pass, domain);
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host, port, username and password.
		/// </summary>
		public FtpClient(string host, int port, string user, string pass)
		{
			Host = host;
			Port = port;
			Credentials = new NetworkCredential(user, pass);
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host, port, username, password and domain
		/// </summary>
		public FtpClient(string host, int port, string user, string pass, string domain)
		{
			Host = host;
			Port = port;
			Credentials = new NetworkCredential(user, pass, domain);
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host.
		/// </summary>
		public FtpClient(Uri host)
		{
			Host = ValidateHost(host);
			Port = host.Port;
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host and credentials.
		/// </summary>
		public FtpClient(Uri host, NetworkCredential credentials)
		{
			Host = ValidateHost(host);
			Port = host.Port;
			Credentials = credentials;
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host and credentials.
		/// </summary>
		public FtpClient(Uri host, string user, string pass)
		{
			Host = ValidateHost(host);
			Port = host.Port;
			Credentials = new NetworkCredential(user, pass);
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host and credentials.
		/// </summary>
		public FtpClient(Uri host, string user, string pass, string domain)
		{
			Host = ValidateHost(host);
			Port = host.Port;
			Credentials = new NetworkCredential(user, pass, domain);
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host, port and credentials.
		/// </summary>
		public FtpClient(Uri host, int port, string user, string pass)
		{
			Host = ValidateHost(host);
			Port = port;
			Credentials = new NetworkCredential(user, pass);
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host, port and credentials.
		/// </summary>
		public FtpClient(Uri host, int port, string user, string pass, string domain)
		{
			Host = ValidateHost(host);
			Port = port;
			Credentials = new NetworkCredential(user, pass, domain);
		}
		#endregion Constructors

		#region Utilities
		/// <summary>
		/// Check if the host parameter is valid
		/// </summary>
		/// <param name="host"></param>
		private static string ValidateHost(Uri host)
		{
			if (host == null) 
				throw new ArgumentNullException("Host is required");

			return host.Host;
		}
		#endregion Utilities
	}
}