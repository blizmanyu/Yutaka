using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
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
		public static readonly TimeSpan LocalTimeZoneOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
		public static readonly string DefaultApplicationName = "Yutaka's Google Calendar Service";
		public static readonly string RFC3339 = String.Format("yyyy-MM-ddTHH:mm:ss-{0}", LocalTimeZoneOffset.ToString(@"hh\:mm"));
		public string ApplicationName;
		public string CertificateFileName;
		public string CertificatePassword;
		public string ServiceAccountEmail;
		public string UserEmail;
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
		/// <param name="userEmail">The email address of the user the application is trying to impersonate in the service account flow.</param>
		/// <param name="certificateKeyStorageFlags">A bitwise combination of the enumeration values that control where and how to import the certificate. The default is <see cref="X509KeyStorageFlags.Exportable"/>.</param>
		public GoogleCalendarService(string applicationName = null, string certificateFileName = null, string certificatePassword = null, string serviceAccountEmail = null, string userEmail = null, X509KeyStorageFlags certificateKeyStorageFlags = X509KeyStorageFlags.Exportable)
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
			if (!String.IsNullOrWhiteSpace(userEmail))
				UserEmail = userEmail;

			CertificateKeyStorageFlags = certificateKeyStorageFlags;
		}
		#endregion Constructor

		#region Utilities
		/// <summary>
		/// Creates a new Google <see cref="CalendarService"/>.
		/// </summary>
		protected void CreateService(string userEmail)
		{
			var certificate = new X509Certificate2(CertificateFileName, CertificatePassword, CertificateKeyStorageFlags);
			var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(ServiceAccountEmail) {
				User = userEmail,
				Scopes = new[] { CalendarService.Scope.Calendar, CalendarService.Scope.CalendarEvents, }
			}.FromCertificate(certificate));

			_service = new CalendarService(new BaseClientService.Initializer() {
				HttpClientInitializer = credential,
				ApplicationName = ApplicationName,
			});
		}

		protected Event InsertEvent(Event e, string calendarId)
		{
			var request = _service.Events.Insert(e, calendarId);
			return request.Execute();
		}
		#endregion Utilities

		#region Public Methods
		/// <summary>
		/// Creates a new Google <see cref="CalendarService"/>. A return value indicates whether the creation succeeded. Any error messages are
		/// contained in output variable response.
		/// </summary>
		/// <param name="response">When this method returns, contains any error messages. It will be blank on success.</param>
		/// <returns>true if created successfully; otherwise, false</returns>
		public bool TryCreateService(string userEmail, out string response)
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
				CreateService(userEmail);
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

		public Event TryInsertEvent(Event e, string calendarId, out string response)
		{
			response = "";

			#region Validation
			if (String.IsNullOrWhiteSpace(calendarId))
				response = String.Format("{0}<calendarId> is required.{1}", response, Environment.NewLine);
			if (e == null)
				response = String.Format("{0}<event> is required.{1}", response, Environment.NewLine);
			else {
				if (e.Start == null || (e.Start.DateTime == null && String.IsNullOrWhiteSpace(e.Start.Date) && String.IsNullOrWhiteSpace(e.Start.DateTimeRaw)))
					response = String.Format("{0}Event.Start is required.{1}", response, Environment.NewLine);
				if (e.End == null || (e.End.DateTime == null && String.IsNullOrWhiteSpace(e.End.Date) && String.IsNullOrWhiteSpace(e.End.DateTimeRaw)))
					response = String.Format("{0}Event.End is required.{1}", response, Environment.NewLine);
			}

			if (!String.IsNullOrWhiteSpace(response)) {
				response = String.Format("{0}Exception thrown in GoogleCalendarService.TryInsertEvent(Event e, string calendarId, out string response).{1}", response, Environment.NewLine);
				return null;
			}
			#endregion Validation

			#region Formatting
			if (!String.IsNullOrWhiteSpace(e.Start.DateTimeRaw)) {
				Console.Write("\n{0}", e.Start.DateTimeRaw);
				if (DateTime.TryParse(e.Start.DateTimeRaw, out var result)) {
					e.Start.DateTimeRaw = result.ToString(RFC3339);
					Console.Write("\n{0}", e.Start.DateTimeRaw);
				}
			}

			if (!String.IsNullOrWhiteSpace(e.End.DateTimeRaw)) {
				Console.Write("\n{0}", e.End.DateTimeRaw);
				if (DateTime.TryParse(e.End.DateTimeRaw, out var result)) {
					e.End.DateTimeRaw = result.ToString(RFC3339);
					Console.Write("\n{0}", e.End.DateTimeRaw);
				}
			}

			if (!String.IsNullOrWhiteSpace(e.Summary))
				e.Summary = e.Summary.Trim();

			if (String.IsNullOrWhiteSpace(e.Description))
				e.Description = "**Created from the Intranet.";
			else
				e.Description = String.Format("{0}{1}{1}**Created from the Intranet.", e.Description, Environment.NewLine);
			#endregion Formatting

			try {
				return InsertEvent(e, calendarId);
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					response = String.Format("{0}{2}Exception thrown in GoogleCalendarService.TryInsertEvent(Event e, string calendarId='{3}', out string response).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, calendarId);
				else
					response = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of GoogleCalendarService.TryInsertEvent(Event e, string calendarId='{3}', out string response).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, calendarId);
				#endregion Log

				return null;
			}
		}
		#endregion Public Methods
	}
}