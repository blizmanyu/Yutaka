using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.QuickBooks.Data
{
	public class Customer_ContactsRet
	{
		public string ListID;
		public DateTime TimeCreated;
		public DateTime TimeModified;
		public string EditSequence;
		public string Contact;
		public string Salutation;
		public string FirstName;
		public string MiddleName;
		public string LastName;
		public string JobTitle;
		public AdditionalContactRef AdditionalContactRef;

		public Customer_ContactsRet()
		{
			AdditionalContactRef = new AdditionalContactRef();
		}
	}
}
