using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;

namespace Yutaka.Google.Calendar
{
	/// <summary>
	/// The certificate needs to be downloaded from the Google API Console <see cref="https://console.developers.google.com/">
	/// "Create another client ID..." -> "Service Account" -> Download the certificate, rename it as "key.p12" and add it to the
	/// project. Don't forget to change the Build action to "Content" and the Copy to Output Directory to "Copy if newer".
	/// 
	/// Available scopes are here: https://developers.google.com/calendar/auth
	/// Also add them to the Admin Console here: https://admin.google.com/rcw1.com/AdminHome?chromeless=1#OGX:ManageOauthClients
	/// </summary>
	public class GoogleCalendarService
	{
		#region Fields
		private CalendarService _service;
		public static readonly string DefaultApplicationName = "Yutaka's Google Calendar Service";
		public string ApplicationName;
		public string CertificateFileName;
		public string CertificatePassword;
		public string ServiceAccountEmail;
		public X509KeyStorageFlags CertificateKeyStorageFlags;
		#endregion Fields

		#region Constructor
		/// <summary>
		/// Creates a new <see cref="GoogleCalendarService"/>.
		/// </summary>
		/// <param name="applicationName">Your application's name.</param>
		/// <param name="certificateFileName">The full file path of a certificate file.</param>
		/// <param name="certificatePassword">The password required to access the X.509 certificate data.</param>
		/// <param name="serviceAccountEmail">The service account email.</param>
		/// <param name="certificateKeyStorageFlags">A bitwise combination of the enumeration values that control where and how to import the certificate. The default is <see cref="X509KeyStorageFlags.Exportable"/>.</param>
		public GoogleCalendarService(string applicationName = null, string certificateFileName = null, string certificatePassword = null, string serviceAccountEmail = null, X509KeyStorageFlags certificateKeyStorageFlags = X509KeyStorageFlags.Exportable)
		{
			if (String.IsNullOrWhiteSpace(applicationName))
				ApplicationName = DefaultApplicationName;
			else
				ApplicationName = applicationName;

			if (!String.IsNullOrWhiteSpace(certificateFileName) && File.Exists(certificateFileName))
				CertificateFileName = certificateFileName;
			if (!String.IsNullOrWhiteSpace(certificatePassword))
				CertificatePassword = certificatePassword;
			if (!String.IsNullOrWhiteSpace(serviceAccountEmail))
				ServiceAccountEmail = serviceAccountEmail;

			CertificateKeyStorageFlags = certificateKeyStorageFlags;
		}
		#endregion Constructor

		#region Utilities
		/// <summary>
		/// Creates a new Google <see cref="CalendarService"/>.
		/// </summary>
		protected void CreateService()
		{
			var certificate = new X509Certificate2(CertificateFileName, CertificatePassword, CertificateKeyStorageFlags);
			var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(ServiceAccountEmail) {
				Scopes = new[] { CalendarService.Scope.Calendar }
			}.FromCertificate(certificate));

			_service = new CalendarService(new BaseClientService.Initializer() {
				HttpClientInitializer = credential,
				ApplicationName = ApplicationName,
			});
		}
		#endregion Utilities

		#region Public Methods
		/// <summary>
		/// Creates a new Google <see cref="CalendarService"/>. A return value indicates whether the creation succeeded. Any error messages are
		/// contained in output variable response.
		/// </summary>
		/// <param name="response">When this method returns, contains any error messages. It will be blank on success.</param>
		/// <returns>true if created successfully; otherwise, false</returns>
		public bool TryCreateService(out string response)
		{
			response = "";

			#region Validation
			if (String.IsNullOrWhiteSpace(ApplicationName))
				ApplicationName = DefaultApplicationName;

			if (String.IsNullOrWhiteSpace(CertificateFileName))
				response = String.Format("{0}<CertificateFileName> is required.{1}", response, Environment.NewLine);
			else if (!File.Exists(CertificateFileName))
				response = String.Format("{0}Certificate file '{2}' doesn't exist.{1}", response, Environment.NewLine, CertificateFileName);

			if (String.IsNullOrWhiteSpace(CertificatePassword))
				response = String.Format("{0}<CertificatePassword> is required.{1}", response, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(ServiceAccountEmail))
				response = String.Format("{0}<ServiceAccountEmail> is required.{1}", response, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(response)) {
				response = String.Format("{0}Exception thrown in GoogleCalendarService.TryCreateService(out string response).{1}", response, Environment.NewLine);
				return false;
			}
			#endregion Validation

			try {
				CreateService();
				return true;
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					response = String.Format("{0}{2}Exception thrown in GoogleCalendarService.TryCreateService(out string response).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine);
				else
					response = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of GoogleCalendarService.TryCreateService(out string response).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine);
				#endregion Log

				return false;
			}
		}
		#endregion Public Methods
	}
}