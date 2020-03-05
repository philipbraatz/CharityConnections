BEGIN
	INSERT INTO [dbo].[EventAttendances](ID, EventID, VolunteerEmail, VolunteerStatus)
	VALUES
	('8bc023ba-ed46-49d7-87ac-584f4d6f15ee', 'b72f0be0-e835-43ab-b6f6-f4b0611ef19d', 'jimbob@gmail.com', 0),
	('2700a9c4-a640-400f-a23e-7556ff14f7b7', '7cab01e2-2cc5-4a28-9d6c-3af78e65a1de', 'briandoe@gmail.com', 1),
	('64bab537-3afa-4f6e-9500-615486e12cdd', 'b031febe-f9ba-4797-94b0-ebd9ab7ebb65', 'jaredpitts@gmail.com', 2)
END