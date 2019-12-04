BEGIN
	INSERT INTO Charities(Charity_ID, Charity_Name, Charity_Cause, Charity_EIN, Charity_Deductibility, 
	Charity_Email, Charity_URL, Charity_Category_ID, Location_ID, Charity_Requirements)
	VALUES
	(0,'null', 'null', 'null', 0, 'null', 'null', 0, 0, 'null'),
	(1,'United Way', 'cause 1', '12-3456789', 2, 'unitedway@gmail.com', 'www.unitedway.org', 1, 1, 'Must have drivers license'),
	(2,'St.Jude', 'cause 2', '98-7654321', 32, 'stjude@gmail.com', 'www.stjude.org', 2, 2, 'Must be 18 or over'),
	(3,'Salvation Army', 'cause 3', '11-2323455', 22, 'salvationarmy@gmail.com', 'www.salvationarmy.org', 3, 3, 'Background check')
END