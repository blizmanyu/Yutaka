USE [GoogleCalendar]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Event](
	[AnyoneCanAddSelf] [bit] NULL,
	[AttendeesOmitted] [bit] NULL,
	[EndTimeUnspecified] [bit] NULL,
	[GuestsCanInviteOthers] [bit] NULL,
	[GuestsCanModify] [bit] NULL,
	[GuestsCanSeeOtherGuests] [bit] NULL,
	[Locked] [bit] NULL,
	[PrivateCopy] [bit] NULL,
	[ConferenceData] [ConferenceData] NULL,
	[Creator] [CreatorData] NULL,
	[Created] [datetime] NULL,
	[Updated] [datetime] NULL,
	[End] [EventDateTime] NULL,
	[OriginalStartTime] [EventDateTime] NULL,
	[Start] [EventDateTime] NULL,
	[ExtendedProperties] [ExtendedPropertiesData] NULL,
	[Gadget] [GadgetData] NULL,
	[Attachments] [IList<EventAttachment>] NULL,
	[Attendees] [IList<EventAttendee>] NULL,
	[Recurrence] [IList<string>] NULL,
	[Sequence] [int] NULL,
	[Organizer] [OrganizerData] NULL,
	[Reminders] [RemindersData] NULL,
	[Source] [SourceData] NULL,
	[ColorId] [nvarchar](4000) NULL,
	[CreatedRaw] [nvarchar](4000) NULL,
	[Description] [nvarchar](4000) NULL,
	[ETag] [nvarchar](4000) NULL,
	[HangoutLink] [nvarchar](4000) NULL,
	[HtmlLink] [nvarchar](4000) NULL,
	[ICalUID] [nvarchar](4000) NULL,
	[Id] [nvarchar](4000) NULL,
	[Kind] [nvarchar](4000) NULL,
	[Location] [nvarchar](4000) NULL,
	[RecurringEventId] [nvarchar](4000) NULL,
	[Status] [nvarchar](4000) NULL,
	[Summary] [nvarchar](4000) NULL,
	[Transparency] [nvarchar](4000) NULL,
	[UpdatedRaw] [nvarchar](4000) NULL,
	[Visibility] [nvarchar](4000) NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[Created] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
