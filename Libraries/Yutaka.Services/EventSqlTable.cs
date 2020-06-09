using System;
using System.Collections.Generic;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Requests;

namespace Helper
{
	public class EventSqlTable : IDirectResponseSchema
	{
		public virtual string Kind { get; set; }
		public virtual string ETag { get; set; }
		public virtual string Id { get; set; }
		public virtual string Status { get; set; }
		public virtual string HtmlLink { get; set; }
		public virtual DateTime? Created { get; set; }
		public virtual string CreatedRaw { get; set; }
		public virtual DateTime? Updated { get; set; }
		public virtual string UpdatedRaw { get; set; }
		public virtual string Summary { get; set; }
		public virtual string Description { get; set; }
		public virtual string Location { get; set; }
		public virtual string ColorId { get; set; }
		public virtual string Creator_Id { get; set; }
		public virtual string Creator_Email { get; set; }
		public virtual string Creator_DisplayName { get; set; }
		public virtual bool? Creator_Self { get; set; }
		public virtual string Organizer_Id { get; set; }
		public virtual string Organizer_Email { get; set; }
		public virtual string Organizer_DisplayName { get; set; }
		public virtual bool? Organizer_Self { get; set; }
		public virtual string Start_ETag { get; set; }
		public virtual string Start_Date { get; set; }
		public virtual DateTime? Start_DateTime { get; set; }
		public virtual string Start_DateTimeRaw { get; set; }
		public virtual string Start_TimeZone { get; set; }
		public virtual string End_ETag { get; set; }
		public virtual string End_Date { get; set; }
		public virtual DateTime? End_DateTime { get; set; }
		public virtual string End_DateTimeRaw { get; set; }
		public virtual string End_TimeZone { get; set; }
		public virtual bool? EndTimeUnspecified { get; set; }
		public virtual IList<string> Recurrence { get; set; }
		public virtual string RecurringEventId { get; set; }
		public virtual string OriginalStartTime_ETag { get; set; }
		public virtual string OriginalStartTime_Date { get; set; }
		public virtual DateTime? OriginalStartTime_DateTime { get; set; }
		public virtual string OriginalStartTime_DateTimeRaw { get; set; }
		public virtual string OriginalStartTime_TimeZone { get; set; }
		public virtual string Transparency { get; set; }
		public virtual string Visibility { get; set; }
		public virtual string ICalUID { get; set; }
		public virtual int? Sequence { get; set; }
		public virtual IList<EventAttendee> Attendees { get; set; }
		public virtual bool? AttendeesOmitted { get; set; }
		public virtual IDictionary<string, string> ExtendedProperties_Private { get; set; }
		public virtual IDictionary<string, string> ExtendedProperties_Shared { get; set; }
		public virtual string HangoutLink { get; set; }
		public virtual string ConferenceData_ETag { get; set; }
		public virtual string ConferenceData_ConferenceId { get; set; }
		public virtual string ConferenceData_Signature { get; set; }
		public virtual string ConferenceData_Notes { get; set; }
		public virtual string Gadget_Type { get; set; }
		public virtual string Gadget_Title { get; set; }
		public virtual string Gadget_Link { get; set; }
		public virtual string Gadget_IconLink { get; set; }
		public virtual int? Gadget_Width { get; set; }
		public virtual int? Gadget_Height { get; set; }
		public virtual string Gadget_Display { get; set; }
		public virtual IDictionary<string, string> Gadget_Preferences { get; set; }
		public virtual bool? AnyoneCanAddSelf { get; set; }
		public virtual bool? GuestsCanInviteOthers { get; set; }
		public virtual bool? GuestsCanModify { get; set; }
		public virtual bool? GuestsCanSeeOtherGuests { get; set; }
		public virtual bool? PrivateCopy { get; set; }
		public virtual bool? Locked { get; set; }
		public virtual bool? Reminders_UseDefault { get; set; }
		public virtual IList<EventReminder> Reminders_Overrides { get; set; }
		public virtual string Source_Url { get; set; }
		public virtual string Source_Title { get; set; }
		public virtual IList<EventAttachment> Attachments { get; set; }
	}
}