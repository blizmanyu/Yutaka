using System;
using System.IO;
using System.Net;
using System.Text;

namespace Yutaka.Services
{
	public class FtpClient
	{
		#region Fields
		public int Port;
		public string Host;
		public NetworkCredential Credentials;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="FtpClient"/> class.
		/// </summary>
		public FtpClient() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="FtpClient"/> class using the specified FTP server.
		/// </summary>
		/// <param name="host">A <see cref="string"/> that contains the name or IP address of the host computer used for FTP transactions.</param>
		public FtpClient(string host)
		{
			#region Check Input
			var log = "";

			if (String.IsNullOrWhiteSpace(host))
				log = String.Format("{0}host is required.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}{1}Exception thrown in Constructor FtpClient(string host).", log, Environment.NewLine));
			#endregion

			Host = host;
			Port = 21;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FtpClient"/> class using the specified FTP server and port.
		/// </summary>
		/// <param name="host">A <see cref="string"/> that contains the name or IP address of the host computer used for FTP transactions.</param>
		/// <param name="port">An <see cref="int"/> greater than zero that contains the port to be used on &lt;host&gt;</param>
		public FtpClient(string host, int port)
		{
			#region Check Input
			var log = "";

			if (String.IsNullOrWhiteSpace(host))
				log = String.Format("{0}host is required.{1}", log, Environment.NewLine);
			if (port < 1)
				log = String.Format("{0}port must be greater than 0.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}{1}Exception thrown in Constructor FtpClient(string host, int port).", log, Environment.NewLine));
			#endregion

			Host = host;
			Port = port;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FtpClient"/> class using the specified FTP server and <see cref="NetworkCredential"/>s.
		/// </summary>
		/// <param name="host">A <see cref="string"/> that contains the name or IP address of the host computer used for FTP transactions.</param>
		/// <param name="credentials">The <see cref="NetworkCredential"/>s for logging into the server.</param>
		public FtpClient(string host, NetworkCredential credentials)
		{
			#region Check Input
			var log = "";

			if (String.IsNullOrWhiteSpace(host))
				log = String.Format("{0}'host' is required.{1}", log, Environment.NewLine);
			if (credentials == null)
				log = String.Format("{0}'credentials' is required.{1}", log, Environment.NewLine);
			else {
				if (String.IsNullOrWhiteSpace(credentials.UserName))
					log = String.Format("{0}'username' is required.{1}", log, Environment.NewLine);
				if (String.IsNullOrWhiteSpace(credentials.Password))
					log = String.Format("{0}'password' is required.{1}", log, Environment.NewLine);
			}

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}{1}Exception thrown in Constructor FtpClient(string host, NetworkCredential credentials).", log, Environment.NewLine));
			#endregion

			Host = host;
			Port = 21;
			Credentials = credentials;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FtpClient"/> class using the specified FTP server, port, and <see cref="NetworkCredential"/>s.
		/// </summary>
		/// <param name="host">A <see cref="string"/> that contains the name or IP address of the host computer used for FTP transactions.</param>
		/// <param name="port">An <see cref="int"/> greater than zero that contains the port to be used on &lt;host&gt;</param>
		/// <param name="credentials">The <see cref="NetworkCredential"/>s for logging into the server.</param>
		/// <exception cref="Exception"></exception>
		public FtpClient(string host, int port, NetworkCredential credentials)
		{
			#region Check Input
			var log = "";

			if (String.IsNullOrWhiteSpace(host))
				log = String.Format("{0}'host' is required.{1}", log, Environment.NewLine);
			if (port < 1)
				log = String.Format("{0}port must be greater than 0.{1}", log, Environment.NewLine);
			if (credentials == null)
				log = String.Format("{0}'credentials' is required.{1}", log, Environment.NewLine);
			else {
				if (String.IsNullOrWhiteSpace(credentials.UserName))
					log = String.Format("{0}'username' is required.{1}", log, Environment.NewLine);
				if (String.IsNullOrWhiteSpace(credentials.Password))
					log = String.Format("{0}'password' is required.{1}", log, Environment.NewLine);
			}

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}{1}Exception thrown in Constructor FtpClient(string host, int port, NetworkCredential credentials).", log, Environment.NewLine));
			#endregion

			Host = host;
			Port = port;
		}
		#endregion Constructors

		#region Public Methods
		/// <summary>
		/// WIP: Do NOT use yet!
		/// </summary>
		/// <param name="source"></param>
		/// <param name="dest"></param>
		public void UploadFile(string source, string dest)
		{
			// Get the object used to communicate with the server.
			var request = (FtpWebRequest) WebRequest.Create(Path.Combine(Host, dest));
			request.Method = WebRequestMethods.Ftp.UploadFile;

			if (Credentials == null || String.IsNullOrWhiteSpace(Credentials.UserName))
				request.Credentials = new NetworkCredential("anonymous", "anonymous");
			else
				request.Credentials = Credentials;

			// Copy the contents of the file to the request stream.
			byte[] fileContents;
			using (var sourceStream = new StreamReader("testfile.txt")) {
				fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
			}

			request.ContentLength = fileContents.Length;

			using (var requestStream = request.GetRequestStream()) {
				requestStream.Write(fileContents, 0, fileContents.Length);
			}

			using (var response = (FtpWebResponse) request.GetResponse()) {
				Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
			}
		}
		#endregion Public Methods
	}
}