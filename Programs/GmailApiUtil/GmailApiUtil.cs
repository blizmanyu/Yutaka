using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GmailApiUtil
{
    public class GmailApiUtil
    {
		public string[] Scopes { get; set; }
		public string ApplicationName { get; set; }

		public GmailApiUtil() { }

		public GmailService GetGmailService(string userEmail)
		{

		}
	}
}