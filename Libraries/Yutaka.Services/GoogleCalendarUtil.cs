using System;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

namespace Yutaka.Services
{
	public class GoogleCalendarUtil
	{
		// A known public activity.
		private static String ACTIVITY_ID = "z12gtjhq3qn2xxl2o224exwiqruvtda0i";

		public static void Main(string[] args)
		{
			Console.WriteLine("Plus API - Service Account");
			Console.WriteLine("==========================");

			String serviceAccountEmail = "SERVICE_ACCOUNT_EMAIL_HERE";

			var certificate = new X509Certificate2(@"key.p12", "notasecret", X509KeyStorageFlags.Exportable);

			ServiceAccountCredential credential = new ServiceAccountCredential(
			   new ServiceAccountCredential.Initializer(serviceAccountEmail)
			   {
				   Scopes = new[] { CalendarService.Scope.Calendar }
			   }.FromCertificate(certificate));

			// Create the service.
			var service = new CalendarService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = "Plus API Sample",
			});

			//Activity activity = service.Activities.Get(ACTIVITY_ID).Execute();
			//Console.WriteLine("  Activity: " + activity.Object.Content);
			//Console.WriteLine("  Video: " + activity.Object.Attachments[0].Url);

			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}
	}
}