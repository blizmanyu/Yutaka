using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.Core.Net
{
	public class FtpClient : WebRequest
	{
		#region Fields

		#endregion Fields

		#region Constructors
		/// <summary>
		/// Creates a new instance of an FTP Client.
		/// </summary>
		public FtpClient()
		{
			m_listParser = new FtpListParser(this);
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host.
		/// </summary>
		public FtpClient(string host)
		{
			Host = host ?? throw new ArgumentNullException("Host must be provided");
			m_listParser = new FtpListParser(this);
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host and credentials.
		/// </summary>
		public FtpClient(string host, NetworkCredential credentials)
		{
			Host = host ?? throw new ArgumentNullException("Host must be provided");
			Credentials = credentials ?? throw new ArgumentNullException("Credentials must be provided");
			m_listParser = new FtpListParser(this);
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host, port and credentials.
		/// </summary>
		public FtpClient(string host, int port, NetworkCredential credentials)
		{
			Host = host ?? throw new ArgumentNullException("Host must be provided");
			Port = port;
			Credentials = credentials ?? throw new ArgumentNullException("Credentials must be provided");
			m_listParser = new FtpListParser(this);
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host, username and password.
		/// </summary>
		public FtpClient(string host, string user, string pass)
		{
			Host = host;
			Credentials = new NetworkCredential(user, pass);
			m_listParser = new FtpListParser(this);
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host, username, password and account
		/// </summary>
		public FtpClient(string host, string user, string pass, string account)
		{
			Host = host;
			Credentials = new NetworkCredential(user, pass, account);
			m_listParser = new FtpListParser(this);
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host, port, username and password.
		/// </summary>
		public FtpClient(string host, int port, string user, string pass)
		{
			Host = host;
			Port = port;
			Credentials = new NetworkCredential(user, pass);
			m_listParser = new FtpListParser(this);
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host, port, username, password and account
		/// </summary>
		public FtpClient(string host, int port, string user, string pass, string account)
		{
			Host = host;
			Port = port;
			Credentials = new NetworkCredential(user, pass, account);
			m_listParser = new FtpListParser(this);
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host.
		/// </summary>
		public FtpClient(Uri host)
		{
			Host = ValidateHost(host);
			Port = host.Port;
			m_listParser = new FtpListParser(this);
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host and credentials.
		/// </summary>
		public FtpClient(Uri host, NetworkCredential credentials)
		{
			Host = ValidateHost(host);
			Port = host.Port;
			Credentials = credentials;
			m_listParser = new FtpListParser(this);
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host and credentials.
		/// </summary>
		public FtpClient(Uri host, string user, string pass)
		{
			Host = ValidateHost(host);
			Port = host.Port;
			Credentials = new NetworkCredential(user, pass);
			m_listParser = new FtpListParser(this);
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host and credentials.
		/// </summary>
		public FtpClient(Uri host, string user, string pass, string account)
		{
			Host = ValidateHost(host);
			Port = host.Port;
			Credentials = new NetworkCredential(user, pass, account);
			m_listParser = new FtpListParser(this);
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host, port and credentials.
		/// </summary>
		public FtpClient(Uri host, int port, string user, string pass)
		{
			Host = ValidateHost(host);
			Port = port;
			Credentials = new NetworkCredential(user, pass);
			m_listParser = new FtpListParser(this);
		}

		/// <summary>
		/// Creates a new instance of an FTP Client, with the given host, port and credentials.
		/// </summary>
		public FtpClient(Uri host, int port, string user, string pass, string account)
		{
			Host = ValidateHost(host);
			Port = port;
			Credentials = new NetworkCredential(user, pass, account);
			m_listParser = new FtpListParser(this);
		}
		#endregion Constructors
	}
}