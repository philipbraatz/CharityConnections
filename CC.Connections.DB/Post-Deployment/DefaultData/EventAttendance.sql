BEGIN
	INSERT INTO Event_Attendance(EventAttendance_ID, Event_ID, Member_ID, [Status])
	VALUES
	(1, 1, 1, 0),
	(2, 2, 2, 1),
	(3, 3, 3, 2)
END