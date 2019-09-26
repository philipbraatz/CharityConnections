BEGIN
	INSERT INTO Charity(Charity_ID, Charity_Cause, Charity_EIN, Charity_Contact_ID, Charity_Deductibility, 
	Charity_Email, Charity_URL, Charity_Category_ID, Location_ID, Charity_Requirements)
	VALUES
	(1, 'cause 1', '12-3456789', 1, 2, 'unitedway@gmail.com', 'www.unitedway.org', 1, 1, 'Must have drivers license'),
	(2, 'cause 2', '98-7654321', 2, 32, 'stjude@gmail.com', 'www.stjude.org', 2, 2, 'Must be 18 or over'),
	(3, 'cause 3', '11-2323455', 3, 22, 'salvationarmy@gmail.com', 'www.salvationarmy.org', 3, 3, 'Background check')
END