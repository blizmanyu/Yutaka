using System;
using System.IO;
using System.Net;
using System.Text;

namespace Yutaka.Net
{
	public static class FtpUtil
	{
		public static void UploadExcel(string source, string dest, string username, string password)
		{
			#region Input Validation
			if (String.IsNullOrEmpty(source))
				throw new ArgumentNullException("source", "source is required");
			if (String.IsNullOrEmpty(dest))
				throw new ArgumentNullException("dest", "dest is required");
			if (String.IsNullOrEmpty(username))
				throw new ArgumentNullException("username", "username is required");
			if (String.IsNullOrEmpty(password))
				throw new ArgumentNullException("password", "password is required");
			#endregion

			FtpWebResponse response = null;

			try {
				#region Get the contents of the file to the request stream
				byte[] fileContents = File.ReadAllBytes(source);
				#endregion

				#region Get the object used to communicate with the server
				var request = (FtpWebRequest) WebRequest.Create(dest);
				request.ContentLength = fileContents.Length;
				request.Credentials = new NetworkCredential(username, password);
				request.Method = WebRequestMethods.Ftp.UploadFile;
				#endregion

				#region Copy the contents of the file to the request stream
				using (var requestStream = request.GetRequestStream()) {
					requestStream.Write(fileContents, 0, fileContents.Length);
					requestStream.Close();
				}
				#endregion

				#region Get response
				using (response = (FtpWebResponse) request.GetResponse()) {
					response.Close();
				}
				#endregion
			}

			catch (Exception) {
				throw;
			}
		}

		public static void UploadFile(string source, string dest, string username, string password)
		{
			#region Input Validation
			if (String.IsNullOrEmpty(source))
				throw new ArgumentNullException("source", "source is required");
			if (String.IsNullOrEmpty(dest))
				throw new ArgumentNullException("dest", "dest is required");
			if (String.IsNullOrEmpty(username))
				throw new ArgumentNullException("username", "username is required");
			if (String.IsNullOrEmpty(password))
				throw new ArgumentNullException("password", "password is required");
			#endregion

			FtpWebResponse response = null;
			byte[] fileContents;

			try {
				#region Get the contents of the file to the request stream
				using (var sourceStream = new StreamReader(source)) {
					fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
					sourceStream.Close();
				}
				#endregion

				#region Get the object used to communicate with the server
				var request = (FtpWebRequest) WebRequest.Create(dest);
				request.ContentLength = fileContents.Length;
				request.Credentials = new NetworkCredential(username, password);
				request.Method = WebRequestMethods.Ftp.UploadFile;
				#endregion

				#region Copy the contents of the file to the request stream
				using (var requestStream = request.GetRequestStream()) {
					requestStream.Write(fileContents, 0, fileContents.Length);
					requestStream.Close();
				}
				#endregion

				#region Get response
				using (response = (FtpWebResponse) request.GetResponse()) {
					response.Close();
				}
				#endregion
			}

			catch (Exception) {
				throw;
			}
		}

		public static void UploadImage(string source, string dest, string username, string password, bool enableSsl = true, bool keepAlive = true, IWebProxy proxy = null, bool useBinary = true, bool usePassive = false)
		{
			#region Input Validation
			if (String.IsNullOrEmpty(source))
				throw new ArgumentNullException("source", "source is required");
			if (String.IsNullOrEmpty(dest))
				throw new ArgumentNullException("dest", "dest is required");
			if (String.IsNullOrEmpty(username))
				throw new ArgumentNullException("username", "username is required");
			if (String.IsNullOrEmpty(password))
				throw new ArgumentNullException("password", "password is required");
			#endregion

			FtpWebResponse response = null;
			byte[] fileContents;

			try {
				#region Get the contents of the file to the request stream
				using (var sourceStream = File.OpenRead(source)) {
					fileContents = new byte[sourceStream.Length];
					sourceStream.Read(fileContents, 0, fileContents.Length);
					sourceStream.Close();
				}
				#endregion

				#region Get the object used to communicate with the server
				var request = (FtpWebRequest) WebRequest.Create(dest);
				request.ContentLength = fileContents.Length;
				request.Credentials = new NetworkCredential(username, password);
				request.EnableSsl = enableSsl;
				request.KeepAlive = keepAlive;
				request.Method = WebRequestMethods.Ftp.UploadFile;
				request.Proxy = proxy;
				request.UseBinary = useBinary;
				request.UsePassive = usePassive;
				#endregion

				#region Copy the contents of the file to the request stream
				using (var requestStream = request.GetRequestStream()) {
					requestStream.Write(fileContents, 0, fileContents.Length);
					requestStream.Close();
				}
				#endregion

				#region Get response
				using (response = (FtpWebResponse) request.GetResponse()) {
					response.Close();
				}
				#endregion
			}

			catch (Exception ex) {
				throw new Exception(String.Format("Exception thrown in FtpUtil.UploadImage(source: {3}, dest: {4}, enableSsl: {5}, keepAlive: {6}, useBinary: {7}, usePassive: {8}){2}{0}{2}{2}{1}", ex.Message, ex.ToString(), Environment.NewLine, source, dest, enableSsl, keepAlive, useBinary, usePassive));
			}
		}
	}
}