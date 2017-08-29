using System;
using System.IO;
using System.Net;

namespace Yutaka.Net
{
	public static class FtpHelper
	{
		#region Fields
		const int CONNECTION_LIMIT = 5;
		const string USERNAME = @"www.rarecoinwholesalers.com|yutaka";
		const string PASSWORD = "Waw9vtb7rqd8cgf5";
		#endregion

		#region Public Methods
		public static Result UploadImage(string source, string destUrl)
		{
			var result = new Result() { success = false };

			if (String.IsNullOrEmpty(destUrl) || String.IsNullOrEmpty(source)) {
				result.message = "source & destUrl are both required.";
				return result;
			}

			try {
				byte[] fileContents;

				using (var sourceStream = File.OpenRead(source)) {
					fileContents = new byte[sourceStream.Length];
					sourceStream.Read(fileContents, 0, fileContents.Length);
				}

				var request = (FtpWebRequest) WebRequest.Create(destUrl);
				request.ContentLength = fileContents.Length;
				request.Credentials = new NetworkCredential(USERNAME, PASSWORD);
				request.EnableSsl = false;
				request.KeepAlive = true;
				request.Method = WebRequestMethods.Ftp.UploadFile;
				request.Proxy = null;
				request.UseBinary = true;
				request.UsePassive = false;

				using (var requestStream = request.GetRequestStream()) {
					requestStream.Write(fileContents, 0, fileContents.Length);
				}

				using (var response = (FtpWebResponse) request.GetResponse()) {
					result.message = String.Format("Upload File Complete: {0}", source);
				}

				result.success = true;
				return result;
			}

			catch (Exception ex) {
				result.message = ex.Message;
				result.exception = ex.ToString();
			}

			return result;
		}

		public static Result UploadFile(string source, string destUrl)
		{
			var result = new Result() { success = false };

			if (String.IsNullOrEmpty(destUrl) || String.IsNullOrEmpty(source)) {
				result.message = "source & destUrl are both required.";
				return result;
			}

			try {
				var request = (FtpWebRequest) WebRequest.Create(new Uri(destUrl));
				request.ConnectionGroupName = "DefaultGroup";
				request.Credentials = new NetworkCredential(USERNAME, PASSWORD);
				request.EnableSsl = false;
				request.KeepAlive = true;
				request.UseBinary = true;
				request.UsePassive = false;
				request.Method = WebRequestMethods.Ftp.UploadFile;
				//request.ServicePoint.ConnectionLimit = CONNECTION_LIMIT;

				using (var sourceStream = File.OpenRead(source)) {
					byte[] fileContents = new byte[sourceStream.Length];
					sourceStream.Read(fileContents, 0, fileContents.Length);
					sourceStream.Close();
					request.ContentLength = fileContents.Length;

					using (var requestStream = request.GetRequestStream()) {
						requestStream.Write(fileContents, 0, fileContents.Length);
					}

					using (var response = (FtpWebResponse) request.GetResponse()) {
						result.success = true;
						result.message = response.StatusDescription;
					}
				}
			}

			catch (Exception ex) {
				result.message = ex.Message;
				result.exception = ex.ToString();
			}

			return result;
		}
		#endregion
	}

	#region Struct
	public struct Result
	{
		public bool success;
		public string message;
		public string exception;
	}
	#endregion
}