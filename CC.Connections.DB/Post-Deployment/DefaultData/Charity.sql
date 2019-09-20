BEGIN
	INSERT INTO Charity(Charity_ID, Charity_Cause, Charity_EIN, Charity_ContactID, Charity_Deductibility, Charity_Email, Charity_URL)
	VALUES
	(1, 'cause 1', '12-3456789', 1, 2, 'unitedway@gmail.com', 'www.unitedway.org'),
	(2, 'cause 2', '98-7654321', 2, 32, 'stjude@gmail.com', 'www.stjude.org'),
	(3, 'cause 3', '11-2323455', 3, 22, 'salvationarmy@gmail.com', 'www.salvationarmy.org')
END