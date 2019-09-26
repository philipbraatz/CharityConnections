BEGIN
	INSERT INTO dbo.Charity_Event(CharityEvent_ID, CharityEventCharity_ID, 
	CharityEventStartDate, CharityEventEndDate, CharityEventName, CharityEventTime, CharityEventStatus, CharityEventRequirements, CharityEventContactInfo_ID)
	VALUES
	(1, 1, '2019-03-03', '2019-03-05', 'Pet Therapy Volunteer', '2019-03-03 12:00','Upcoming', 'Must bring dog', 1),
	(2, 2, '2019-04-04', '2019-04-06', 'Greeter', '2019-04-04 9:00','Completed', 'Must be 18', 2),
	(3, 3, '2019-05-05', '2019-05-07', 'Bell Ringing', '2019-05-05 10:00','Ongoing', 'Background check', 3)
END
