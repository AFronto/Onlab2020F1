DECLARE @cnt INT = 6;
DECLARE @query NVARCHAR(MAX);

WHILE @cnt < 48 --vagy mennyi filed van
BEGIN

	SET @query =
'
DECLARE @XmlFile XML;
SELECT @XmlFile = BulkColumn
FROM  OPENROWSET(BULK  ''F:\StackOverflowdata\dataFragments\Posts'+CAST(@cnt AS NVARCHAR(10))+'.xml'' , SINGLE_BLOB) x;

INSERT INTO dbo.Links(AnswerId, QuestionId)
SELECT
    AnswerId = post.value(''@Id'', ''int''),
    QuestionId = post.value(''@ParentId'', ''int'')
FROM @XmlFile.nodes(''/posts/row'') AS XTbl(post)
WHERE post.value(''@ParentId'', ''int'') IS NOT NULL
'

	EXEC sp_executesql @query;

	SET @cnt = @cnt + 1;
END;
