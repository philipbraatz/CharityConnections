﻿BEGIN
	INSERT INTO Charities(Charity_ID, Charity_Name, Charity_Cause, Charity_EIN, Charity_Deductibility, 
	Charity_Email, Charity_URL, Charity_Category_ID, Location_ID, Charity_Requirements)
	VALUES
	(1,'United Way', 'Suspendisse elementum cursus fringilla. Nulla tincidunt nec sapien sed mollis. Etiam venenatis nec leo at auctor. Praesent nunc turpis, luctus eu gravida in, porttitor sed urna. Nunc nec purus et nisl gravida auctor auctor vitae quam. Aliquam ex enim, lobortis eu cursus eget, consectetur id lectus. Nam quis ipsum erat. Etiam efficitur eu arcu quis dapibus. Cras placerat est quis nisi ullamcorper, ac rutrum nisl euismod. Nullam mattis, magna sit amet aliquam convallis, nibh erat fringilla nunc, at scelerisque magna tortor non arcu. Phasellus vel nunc elit. Praesent ut tristique mauris, sit amet accumsan erat. Quisque nec nisi eu ligula ultricies pellentesque. Etiam pharetra, nisi quis condimentum semper, justo magna accumsan lectus, et pharetra nulla risus et risus.',
		'12-3456789', 2, 'unitedway@gmail.com', 'www.unitedway.org', 1, 1, 'Must have drivers license'),
	(2,'St.Jude', 'Suspendisse elementum cursus fringilla. Nulla tincidunt nec sapien sed mollis. Etiam venenatis nec leo at auctor. Praesent nunc turpis, luctus eu gravida in, porttitor sed urna. Nunc nec purus et nisl gravida auctor auctor vitae quam. Aliquam ex enim, lobortis eu cursus eget, consectetur id lectus. Nam quis ipsum erat. Etiam efficitur eu arcu quis dapibus. Cras placerat est quis nisi ullamcorper, ac rutrum nisl euismod. Nullam mattis, magna sit amet aliquam convallis, nibh erat fringilla nunc, at scelerisque magna tortor non arcu. Phasellus vel nunc elit. Praesent ut tristique mauris, sit amet accumsan erat. Quisque nec nisi eu ligula ultricies pellentesque. Etiam pharetra, nisi quis condimentum semper, justo magna accumsan lectus, et pharetra nulla risus et risus.',
	'98-7654321', 32, 'stjude@gmail.com', 'www.stjude.org', 2, 2, 'Must be 18 or over'),
	(3,'Salvation Army', 'Suspendisse elementum cursus fringilla. Nulla tincidunt nec sapien sed mollis. Etiam venenatis nec leo at auctor. Praesent nunc turpis, luctus eu gravida in, porttitor sed urna. Nunc nec purus et nisl gravida auctor auctor vitae quam. Aliquam ex enim, lobortis eu cursus eget, consectetur id lectus. Nam quis ipsum erat. Etiam efficitur eu arcu quis dapibus. Cras placerat est quis nisi ullamcorper, ac rutrum nisl euismod. Nullam mattis, magna sit amet aliquam convallis, nibh erat fringilla nunc, at scelerisque magna tortor non arcu. Phasellus vel nunc elit. Praesent ut tristique mauris, sit amet accumsan erat. Quisque nec nisi eu ligula ultricies pellentesque. Etiam pharetra, nisi quis condimentum semper, justo magna accumsan lectus, et pharetra nulla risus et risus.',
	'11-2323455', 22, 'salvationarmy@gmail.com', 'www.salvationarmy.org', 3, 3, 'Background check'),
	(4,'AutoCharity', 'For a good Cause', '98-7654321', 77, 'auto@login.net', 'www.autoparts.org', 2, 2, 'Must be a robot')
END