BEGIN
	INSERT INTO CharityEvents(CharityEvent_ID, CharityEvent_Email, CharityEventCharityID, 
	CharityEventStartDate, CharityEventEndDate, CharityEventName, CharityEventTime)
	VALUES
	(1, 'unitedwayevent@gmail.com', 1, '2019-03-03', '2019-03-05', 'Pet Therapy Volunteer', '2019-03-03 12:00'),
	(2, 'stjudeevent@gmail.com', 2, '2019-04-04', '2019-04-06', 'Greeter', '2019-04-04 9:00'),
	(3, 'salvationarmyevent@gmail.com', 3, '2019-05-05', '2019-05-07', 'Bell Ringing', '2019-05-05 10:00')
END