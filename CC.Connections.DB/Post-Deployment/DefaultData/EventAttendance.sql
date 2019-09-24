BEGIN
	INSERT INTO Event_Attendance(EventAttendance_ID, Event_ID, Member_ID, Status)
	VALUES
	(1, 1, 1, 'Completed'),
	(2, 2, 2, 'Upcoming'),
	(3, 3, 3, 'Ongoing')
END