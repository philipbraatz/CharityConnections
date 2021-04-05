BEGIN
	INSERT INTO dbo.[LogIns](MemberEmail, MemberType, [Password], [Key])
	 	VALUES
	('jaredpitts@gmail.com',1,	'qUqP5cyxm6YcTAhz05Hph5gvu9M=','abc'),
	('jimbob@gmail.com',1		,'qUqP5cyxm6YcTAhz05Hph5gvu9M=','bcd'),
	('briandoe@gmail.com',1	,'qUqP5cyxm6YcTAhz05Hph5gvu9M=','efg'),
	('unitedway@gmail.com',2	,'qUqP5cyxm6YcTAhz05Hph5gvu9M=','hijkl'),
	('stjude@gmail.com',2		,'qUqP5cyxm6YcTAhz05Hph5gvu9M=','123'),
	('salvationarmy@gmail.com',2,'qUqP5cyxm6YcTAhz05Hph5gvu9M=','4321'),
	('auto@login.com',1,'qUqP5cyxm6YcTAhz05Hph5gvu9M=','YES'),
	('auto@login.net',2,'qUqP5cyxm6YcTAhz05Hph5gvu9M=','TEMP')
END