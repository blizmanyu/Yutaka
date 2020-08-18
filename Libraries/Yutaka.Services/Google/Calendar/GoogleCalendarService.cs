using System;
using System.Globalization;
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
		public static readonly CultureInfo CurrentCulture = CultureInfo.CurrentCulture;
		#region Commented out Jun 4, 2020
		//public static readonly TimeSpan LocalTimeZoneOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
		//public static readonly string DateTimeFormat = @"yyyy-MM-ddTHH:mm:ss";
		//public static readonly string RFC3339 = String.Format("yyyy-MM-ddTHH:mm:ss-{0}", LocalTimeZoneOffset.ToString(@"hh\:mm"));
		#endregion Commented out Jun 4, 2020
		public static readonly string DefaultApplicationName = "Yutaka's Google Calendar Service";
		public static readonly string TimeZone_LosAngeles = "America/Los_Angeles";
		public static readonly string TimeZone_NewYork = "America/New_York";
		public string ApplicationName;
		public string CertificateFileName;
		public string CertificatePassword = "notasecret";
		public string ServiceAccountEmail;
		public string UserEmail;
		#endregion Fields

		#region Constructor
		/// <summary>
		/// Creates a new <see cref="GoogleCalendarService"/>.
		/// </summary>
		/// <param name="applicationName">Your application's name.</param>
		/// <param name="certificateFileName">The full file path of a certificate file.</param>
		/// <param name="serviceAccountEmail">The service account email.</param>
		/// <param name="userEmail">The email address of the user the application is trying to impersonate in the service account flow.</param>
		public GoogleCalendarService(string applicationName = null, string certificateFileName = null, string serviceAccountEmail = null, string userEmail = null)
		{
			if (String.IsNullOrWhiteSpace(applicationName))
				ApplicationName = DefaultApplicationName;
			else
				ApplicationName = applicationName;

			if (!String.IsNullOrWhiteSpace(certificateFileName))
				CertificateFileName = certificateFileName;
			if (!String.IsNullOrWhiteSpace(serviceAccountEmail))
				ServiceAccountEmail = serviceAccountEmail;
			if (!String.IsNullOrWhiteSpace(userEmail))
				UserEmail = userEmail;
		}
		#endregion Constructor

		#region Utilities
		/// <summary>
		/// Creates a new Google <see cref="CalendarService"/>.
		/// </summary>
		protected void CreateService()
		{
			var certificate = new X509Certificate2(CertificateFileName, CertificatePassword, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);
			var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(ServiceAccountEmail) {
				User = UserEmail,
				Scopes = new[] { CalendarService.Scope.Calendar, CalendarService.Scope.CalendarEvents, }
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
				response = String.Format("{0}'CertificateFileName' is required.{1}", response, Environment.NewLine);
			else if (!File.Exists(CertificateFileName))
				response = String.Format("{0}Certificate file '{2}' doesn't exist.{1}", response, Environment.NewLine, CertificateFileName);

			if (String.IsNullOrWhiteSpace(CertificatePassword))
				response = String.Format("{0}'CertificatePassword' is required.{1}", response, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(ServiceAccountEmail))
				response = String.Format("{0}'ServiceAccountEmail' is required.{1}", response, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(UserEmail))
				response = String.Format("{0}'UserEmail' is required.{1}", response, Environment.NewLine);

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

		/// <summary>
		/// Deletes an event.
		/// </summary>
		/// <param name="eventId">The body of the request.</param>
		/// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you want to
		/// access the primary calendar of the currently logged in user, use the "primary" keyword, or, the full email address if
		/// you're using a service account.</param>
		/// <param name="response">When this method returns, contains any error messages. It will be blank on success.</param>
		/// <returns></returns>
		public bool TryDeleteEvent(string eventId, string calendarId, out string response)
		{
			response = "";

			#region Validation
			if (String.IsNullOrWhiteSpace(eventId))
				response = String.Format("{0}<eventId> is required.{1}", response, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(calendarId))
				response = String.Format("{0}<calendarId> is required.{1}", response, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(response)) {
				response = String.Format("{0}Exception thrown in GoogleCalendarService.TryDeleteEvent(string eventId='{2}', string calendarId='{3}', out string response).{1}", response, Environment.NewLine, eventId ?? "NULL", calendarId ?? "NULL");
				return false;
			}
			#endregion Validation

			try {
				if (_service == null || !UserEmail.Equals(calendarId, StringComparison.OrdinalIgnoreCase)) {
					UserEmail = calendarId;

					if (!TryCreateService(out response))
						return false;
				}

				response = _service.Events.Delete(calendarId, eventId).Execute();

				if (String.IsNullOrWhiteSpace(response))
					return true;
				else
					return false;
			}

			catch (Exception ex) {
				if (ex.Message.Contains("410") && CurrentCulture.CompareInfo.IndexOf(ex.Message, "deleted", CompareOptions.OrdinalIgnoreCase) > -1) {
					response = "Already deleted.";
					return true;
				}

				#region Log
				if (ex.InnerException == null)
					response = String.Format("{0}{2}Exception thrown in GoogleCalendarService.TryDeleteEvent(string eventId='{3}', string calendarId='{4}', out string response).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, eventId, calendarId);
				else
					response = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of GoogleCalendarService.TryDeleteEvent(string eventId='{3}', string calendarId='{4}', out string response).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, eventId, calendarId);
				#endregion Log

				return false;
			}
		}

		/// <summary>
		/// Gets an event.
		/// </summary>
		/// <param name="eventId">The body of the request.</param>
		/// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you want to
		/// access the primary calendar of the currently logged in user, use the "primary" keyword, or, the full email address if
		/// you're using a service account.</param>
		/// <param name="response">When this method returns, contains any error messages. It will be blank on success.</param>
		/// <returns></returns>
		public Event TryGetEvent(string eventId, string calendarId, out string response)
		{
			response = "";

			#region Validation
			if (String.IsNullOrWhiteSpace(eventId))
				response = String.Format("{0}<eventId> is required.{1}", response, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(calendarId))
				response = String.Format("{0}<calendarId> is required.{1}", response, Environment.NewLine);

			if (!String.IsNullOrWhiteSpace(response)) {
				response = String.Format("{0}Exception thrown in GoogleCalendarService.TryGetEvent(string eventId, string calendarId, out string response).{1}", response, Environment.NewLine);
				return null;
			}
			#endregion Validation

			try {
				if (_service == null || !UserEmail.Equals(calendarId, StringComparison.OrdinalIgnoreCase)) {
					UserEmail = calendarId;

					if (!TryCreateService(out response))
						return null;
				}

				return _service.Events.Get(calendarId, eventId).Execute();
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					response = String.Format("{0}{2}Exception thrown in GoogleCalendarService.TryGetEvent(string eventId='{3}', string calendarId='{4}', out string response).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, eventId, calendarId);
				else
					response = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of GoogleCalendarService.TryGetEvent(string eventId='{3}', string calendarId='{4}', out string response).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, eventId, calendarId);
				#endregion Log

				return null;
			}
		}

		/// <summary>
		/// Creates an event.
		/// </summary>
		/// <param name="ev">The body of the request.</param>
		/// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you want to
		/// access the primary calendar of the currently logged in user, use the "primary" keyword, or, the full email address if
		/// you're using a service account.</param>
		/// <param name="response">When this method returns, contains any error messages. It will be blank on success.</param>
		/// <returns></returns>
		public Event TryInsertEvent(Event ev, string calendarId, out string response)
		{
			response = "";

			#region Validation
			if (String.IsNullOrWhiteSpace(calendarId))
				response = String.Format("{0}<calendarId> is required.{1}", response, Environment.NewLine);
			if (ev == null)
				response = String.Format("{0}<ev> is required.{1}", response, Environment.NewLine);
			else {
				if (ev.Start == null || (ev.Start.DateTime == null && String.IsNullOrWhiteSpace(ev.Start.Date) && String.IsNullOrWhiteSpace(ev.Start.DateTimeRaw)))
					response = String.Format("{0}ev.Start is required.{1}", response, Environment.NewLine);
				if (ev.End == null || (ev.End.DateTime == null && String.IsNullOrWhiteSpace(ev.End.Date) && String.IsNullOrWhiteSpace(ev.End.DateTimeRaw)))
					response = String.Format("{0}ev.End is required.{1}", response, Environment.NewLine);
			}

			if (!String.IsNullOrWhiteSpace(response)) {
				response = String.Format("{0}Exception thrown in GoogleCalendarService.TryInsertEvent(Event ev, string calendarId, out string response).{1}", response, Environment.NewLine);
				return null;
			}
			#endregion Validation

			try {
				if (_service == null || !UserEmail.Equals(calendarId, StringComparison.OrdinalIgnoreCase)) {
					UserEmail = calendarId;

					if (!TryCreateService(out response))
						return null;
				}

				return _service.Events.Insert(ev, calendarId).Execute();
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					response = String.Format("{0}{2}Exception thrown in GoogleCalendarService.TryInsertEvent(Event ev, string calendarId='{3}', out string response).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, calendarId);
				else
					response = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of GoogleCalendarService.TryInsertEvent(Event ev, string calendarId='{3}', out string response).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, calendarId);
				#endregion Log

				return null;
			}
		}

		/// <summary>
		/// Updates an event. This method supports patch semantics. The field values you specify replace the existing values. Fields
		/// that you don’t specify in the request remain unchanged. Array fields, if specified, overwrite the existing arrays; this
		/// discards any previous array elements.
		/// </summary>
		/// <param name="ev">The body of the request.</param>
		/// <param name="eventId">Event identifier.</param>
		/// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you want to
		/// access the primary calendar of the currently logged in user, use the "primary" keyword, or, the full email address if
		/// you're using a service account.</param>
		/// <param name="response">When this method returns, contains any error messages. It will be blank on success.</param>
		/// <returns></returns>
		public Event TryPatchEvent(Event ev, string eventId, string calendarId, out string response)
		{
			response = "";

			#region Validation
			if (String.IsNullOrWhiteSpace(eventId))
				response = String.Format("{0}<eventId> is required.{1}", response, Environment.NewLine);
			if (String.IsNullOrWhiteSpace(calendarId))
				response = String.Format("{0}<calendarId> is required.{1}", response, Environment.NewLine);
			if (ev == null)
				response = String.Format("{0}<ev> is required.{1}", response, Environment.NewLine);
			else {
				if (ev.Start == null || (ev.Start.DateTime == null && String.IsNullOrWhiteSpace(ev.Start.Date) && String.IsNullOrWhiteSpace(ev.Start.DateTimeRaw)))
					response = String.Format("{0}ev.Start is required.{1}", response, Environment.NewLine);
				if (ev.End == null || (ev.End.DateTime == null && String.IsNullOrWhiteSpace(ev.End.Date) && String.IsNullOrWhiteSpace(ev.End.DateTimeRaw)))
					response = String.Format("{0}ev.End is required.{1}", response, Environment.NewLine);
			}

			if (!String.IsNullOrWhiteSpace(response)) {
				response = String.Format("{0}Exception thrown in GoogleCalendarService.TryPatchEvent(Event ev, string eventId, string calendarId, out string response).{1}", response, Environment.NewLine);
				return null;
			}
			#endregion Validation

			try {
				if (_service == null || !UserEmail.Equals(calendarId, StringComparison.OrdinalIgnoreCase)) {
					UserEmail = calendarId;

					if (!TryCreateService(out response))
						return null;
				}

				return _service.Events.Patch(ev, calendarId, eventId).Execute();
			}

			catch (Exception ex) {
				#region Log
				if (ex.InnerException == null)
					response = String.Format("{0}{2}Exception thrown in GoogleCalendarService.TryPatchEvent(Event ev, string eventId='{3}', string calendarId='{4}', out string response).{2}{1}{2}{2}", ex.Message, ex.ToString(), Environment.NewLine, eventId, calendarId);
				else
					response = String.Format("{0}{2}Exception thrown in INNER EXCEPTION of GoogleCalendarService.TryPatchEvent(Event ev, string eventId='{3}', string calendarId='{4}', out string response).{2}{1}{2}{2}", ex.InnerException.Message, ex.InnerException.ToString(), Environment.NewLine, eventId, calendarId);
				#endregion Log

				return null;
			}
		}
		#endregion Public Methods
	}
}