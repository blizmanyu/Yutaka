using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;

namespace Yutaka.Services
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

		/// <summary>
		/// Creates a new <see cref="GoogleCalendarService"/>. If parameters &lt;certificateFileName&gt;, &lt;certificatePassword&gt;, and
		/// &lt;serviceAccountEmail&gt; are all specified and valid, it will also call <see cref="GoogleCalendarService.CreateService()"/> to initialize a new Google <see cref="CalendarService"/>.
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

			if (String.IsNullOrWhiteSpace(ApplicationName) || String.IsNullOrWhiteSpace(CertificateFileName) || String.IsNullOrWhiteSpace(CertificatePassword) || String.IsNullOrWhiteSpace(ServiceAccountEmail))
				return;
			else
				CreateService();
		}

		/// <summary>
		/// Creates a new Google <see cref="CalendarService"/>. This is automatically called at the end of the constructor, but is
		/// here in case you change any of the fields and want to re-initialize with the API.
		/// </summary>
		public void CreateService()
		{
			if (String.IsNullOrWhiteSpace(ApplicationName) || String.IsNullOrWhiteSpace(CertificateFileName) ||
				String.IsNullOrWhiteSpace(CertificatePassword) || String.IsNullOrWhiteSpace(ServiceAccountEmail))
				return;

			var certificate = new X509Certificate2(CertificateFileName, CertificatePassword, CertificateKeyStorageFlags);
			var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(ServiceAccountEmail) {
				Scopes = new[] { CalendarService.Scope.Calendar }
			}.FromCertificate(certificate));

			_service = new CalendarService(new BaseClientService.Initializer() {
				HttpClientInitializer = credential,
				ApplicationName = ApplicationName,
			});
		}
	}
}