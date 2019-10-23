BEGIN
	INSERT INTO dbo.Charity_Event(CharityEvent_ID, CharityEventCharity_ID, 
	CharityEventStartDate, CharityEventEndDate, CharityEventName, CharityEventStatus, CharityEventRequirements, CharityEventContactInfo_ID)
	VALUES
	(1, 1, '2019-03-03 12:00', '2019-03-05 15:00', 'Pet Therapy Volunteer', 'Upcoming',  'Must bring dog',	 1),
	(2, 2, '2019-04-04 9:00',  '2019-04-06 12:00', 'Greeter',			    'Completed', 'Must be 18',		 2),
	(3, 3, '2019-05-05 10:00', '2019-05-07 12:30', 'Bell Ringing',			'Ongoing',	 'Background check', 3)
END
