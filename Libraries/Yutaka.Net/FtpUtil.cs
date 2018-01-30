using System;
using System.IO;
using System.Net;
using System.Text;

namespace Yutaka.Net
{
	public static class FtpUtil
	{
		public static Result UploadFile(string source, string dest, string username, string password)
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

			var result = new Result() { Success = false, Message = "", Exception = "" };
			FtpWebResponse response = null;

			try {
				byte[] fileContents;

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
					result.Message = String.Format("Upload <{0}> complete, status: {1}", source, response.StatusDescription);
					response.Close();
				}
				#endregion
			}

			catch (Exception ex) {
				result.Message = ex.Message;
				if (response != null && !String.IsNullOrEmpty(response.StatusDescription))
					result.Message = String.Format("{0}: {1}", ex.Message, response.StatusDescription);
				result.Exception = ex.ToString();
			}

			return result;
		}

		public static Result UploadImage(string source, string dest, string username, string password, bool enableSsl = true, bool keepAlive = true, IWebProxy proxy = null, bool useBinary = true, bool usePassive = false)
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

			var result = new Result() { Success = false, Message = "", Exception = "" };
			FtpWebResponse response = null;

			try {
				byte[] fileContents;

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
					result.Message = String.Format("Upload <{0}> complete, status: {1}", source, response.StatusDescription);
					response.Close();
				}
				#endregion

				result.Success = true;
			}

			catch (Exception ex) {
				result.Message = ex.Message;
				if (response != null && !String.IsNullOrEmpty(response.StatusDescription))
					result.Message = String.Format("{0}: {1}", ex.Message, response.StatusDescription);
				result.Exception = ex.ToString();
			}

			return result;
		}

		#region Struct
		public struct Result
		{
			public bool Success;
			public string Message;
			public string Exception;
		}
		#endregion
	}
}