DECLARE @Roots  TABLE(Id int PRIMARY KEY)
DECLARE @Result TABLE(Id int PRIMARY KEY)

INSERT INTO @Roots(Id)
SELECT Id FROM Structure WHERE RowGuid IN 
(
	 'A1BE19EC-E360-491D-81BF-8F3C418DBEF9' --ОАО РЖД
	,'BB78805C-9C46-4A70-B8EC-B93E629FF515' --Сервисные компании
)
ORDER BY Id

DECLARE @currentId int
SET @currentId = 0

WHILE (1=1) BEGIN

	SELECT 
		@currentId = MIN(Id) 
	FROM 
		@Roots R 
	WHERE 
		@CurrentId < R.Id
	
	PRINT(@currentId)
	
	IF(@currentId IS NULL) BREAK
	
	INSERT INTO @Result
	SELECT 
		Id 
	FROM 
		dbo.Tree(@currentId) T 
		INNER JOIN Structure S ON S.Id = T.ItemId
	WHERE 
		ISNULL(S.fDel,0) = 0
	
END

SELECT 
	S.Id, 
	ParentId = S.NODId, 
	S.RowGuid,
	S.NodGuid,
	S.Name
FROM 
	@Result R
	INNER JOIN Structure S ON S.Id = R.Id
ORDER BY S.Name