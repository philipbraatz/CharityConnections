BEGIN
	INSERT INTO Charities(Charity_Email, [Name], EIN, Deductibility, [URL], Cause, Category_ID, Location_ID)
	VALUES
	('unitedway@gmail.com','United Way', '12-3456789', 0,'https://www.unitedway.org/',
	'United Way advances the common good in communities across the world. Our focus is on education, income and health—the building blocks for a good quality of life.',
	 '366b1493-adf6-4229-a791-5f91478833c8','fbe8a4ce-61c7-4fc9-b5ff-8b63ac19d9a0'),
	('stjude@gmail.com','St.Jude', '98-7654321' ,1 , 'https://www.stjude.org/',
	'Finding cures. Saving children. ®',
	 '366b1493-adf6-4229-a791-5f91478833c8','d4cd8954-0b83-4c74-8cc2-2699de262507'),
	('salvationarmy@gmail.com','Salvation Army', '11-2323455', 1, 'https://www.salvationarmyusa.org/',
	'The Salvation Army, an international movement, is an evangelical part of the universal Christian Church. Its message is based on the Bible. Its ministry is motivated by the love of God. Its mission is to preach the gospel of Jesus Christ and to meet human needs in His name without discrimination.',
	'8c3e154c-024f-45bc-be4b-793687a67b52','8c745578-2d05-45a0-a73c-fe32772dc3dc'),
	('auto@login.net','AutoCharity', '98-7654321',0,'https://www.autozone.com/', 'For a good Cause',
	'd91d8aeb-8324-45e8-84b3-7b9cef01ee9e','3111090d-b1a8-481c-828e-2fc2460aa3ee')
END