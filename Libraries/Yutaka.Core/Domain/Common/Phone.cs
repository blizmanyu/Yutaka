using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.Core.Domain.Common
{
	public class Phone
	{
		public DateTime CreatedOnUtc;
		public int CreatedById;
		public int Id;
		public int ContactId;

		public bool IsPrimary;
		public string Label;
		public string Number;
		public string Ext;

		public int UpdatedById;
		public DateTime UpdatedOnUtc;

		//public Phone(string number = null, string ext = null, string label = null, bool isPrimary = null)
		//{

		//}
	}
}