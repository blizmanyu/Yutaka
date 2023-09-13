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
			Credentials = credentials;
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
			var request = (FtpWebRequest)WebRequest.Create(Path.Combine(Host, dest));
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

		/// <summary>
		/// Uploads an image.
		/// </summary>
		/// <param name="source">The source path to upload.</param>
		/// <param name="dest">The destination path to upload to.</param>
		public void UploadImage(string source, string dest)
		{
			#region Check Input
			var log = "";

			if (String.IsNullOrWhiteSpace(source))
				log = String.Format("{0}'source' is required.{1}", log, Environment.NewLine);
			else if (!File.Exists(source))
				log = String.Format("{0}File '{2}' doesn't exist.{1}", log, Environment.NewLine, source);

			if (String.IsNullOrWhiteSpace(dest))
				log = String.Format("{0}'dest' is required.{1}", log, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(log))
				throw new Exception(String.Format("{0}{1}Exception thrown in UploadImage(string source, string dest).{1}", log, Environment.NewLine));
			#endregion

			FtpWebResponse response;

			try {
				byte[] array;
				using (var stream = File.OpenRead(source)) {
					array = new byte[stream.Length];
					stream.Read(array, 0, array.Length);
					stream.Close();
				}

				var request = (FtpWebRequest)WebRequest.Create(dest);
				request.ContentLength = array.Length;
				request.Credentials = Credentials;
				request.EnableSsl = true;
				request.KeepAlive = true;
				request.Method = WebRequestMethods.Ftp.UploadFile;
				request.UseBinary = true;
				request.UsePassive = true;

				using (var stream = request.GetRequestStream()) {
					stream.Write(array, 0, array.Length);
					stream.Close();
				}

				using (response = (FtpWebResponse) request.GetResponse()) {
					response.Close();
				}
			}

			catch (Exception ex) {
				if (ex.InnerException == null)
					throw new Exception(String.Format("{0}{2}Exception thrown in FtpClient.UploadImage(string source='{3}', string dest='{4}').", ex.Message, ex.ToString(), Environment.NewLine, source, dest));
				else
					throw;
			}
		}
		#endregion Public Methods
	}
}