using System;
using System.IO;
using System.Net;
using System.Text;

namespace Yutaka.Ftp
{
	public class FtpClient
	{
		#region Fields
		public NetworkCredential Credentials;
		public string Host;
		public int Port;
		#endregion Fields

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="FtpClient"/> class.
		/// </summary>
		public FtpClient() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="FtpClient"/> class using the specified FTP server.
		/// </summary>
		/// <param name="host">A <see cref="string"/> that contains the name or IP address of the host computer used for FTP transactions.</param>
		public FtpClient(string host) {
			Host = host;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FtpClient"/> class using the specified FTP server and port.
		/// </summary>
		/// <param name="host">A <see cref="string"/> that contains the name or IP address of the host computer used for FTP transactions.</param>
		/// <param name="port">An <see cref="int"/> greater than zero that contains the port to be used on &lt;host&gt;</param>
		public FtpClient(string host, int port)
		{
			if (port < 1)
				throw new ArgumentOutOfRangeException("port");

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