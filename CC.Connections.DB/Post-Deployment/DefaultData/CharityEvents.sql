BEGIN
	INSERT INTO dbo.CharityEvents(ID, CharityEmail , [Name], StartDate, EndDate, LocationID, Requirements, [Description])
	VALUES
	('b72f0be0-e835-43ab-b6f6-f4b0611ef19d', 'unitedway@gmail.com','Pet Therapy Volunteer', 
	'2019-03-03 12:00', '2019-03-05 15:00', 
	'fbe8a4ce-61c7-4fc9-b5ff-8b63ac19d9a0' ,  'Must bring dog',
	'Consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.'),
	
	('7cab01e2-2cc5-4a28-9d6c-3af78e65a1de', 'unitedway@gmail.com','Clean Cats and Dogs',
	'2019-04-04 12:00', '2019-03-05 15:00',
	'd4cd8954-0b83-4c74-8cc2-2699de262507',  'Must be 18',
	'Consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.'),
	
	('b031febe-f9ba-4797-94b0-ebd9ab7ebb65', 'unitedway@gmail.com','Greeter', 
	'2019-05-05 9:00',  '2019-04-06 12:00',
	'8c745578-2d05-45a0-a73c-fe32772dc3dc',  'Must be 18',
	'Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?'),
	
	('e66b69db-2036-4551-a664-81a116b06bfe', 'unitedway@gmail.com', 'Bell Ringing',
	'2019-06-06 10:00', '2019-05-07 12:30',
	'3111090d-b1a8-481c-828e-2fc2460aa3ee',  'Background check',
	'At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae. Itaque earum rerum hic tenetur a sapiente delectus, ut aut reiciendis voluptatibus maiores alias consequatur aut perferendis doloribus asperiores repellat.')
END