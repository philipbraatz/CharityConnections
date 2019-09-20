BEGIN
	INSERT INTO Contact_Info(Contact_Info_ID, Contact_Info_FName, Contact_Info_LName, Contact_Info_Email, 
	Contact_Info_Address, Contact_Info_City, Contact_Info_State,Contact_Info_Zip, Contact_Info_Phone)
	VALUES
	(1, 'Jim', 'Bob', 'jimbob@gmail.com', '123 Main St.', 'Appleton', 'WI', '54911', '920-555-5555'),
	(2, 'Brian', 'Doe', 'briandoe@gmail.com', '321 Main St.', 'Green Bay', 'WI', '54229', '920-123-2233'),
	(3, 'Jared', 'Pitts', 'jaredpitts@gmail.com', '563 Main St.', 'Appleton', 'WI', '54911', '715-421-8877')
END