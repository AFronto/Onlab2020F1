DECLARE @XmlFile XML

SELECT @XmlFile = BulkColumn
FROM  OPENROWSET(BULK 'F:\StackOverflowdata\Posts.xml', SINGLE_BLOB) x;

--INSERT INTO dbo.Posts(Id, PostTypeId, AcceptedAnswerId, ParentId, CreationDate, Score, ViewCount, Body, Title, Tags, AnswerCount, CommentCount, FavoriteCount)
SELECT
    Id = post.value('@Id', 'int'),
    PostTypeId = post.value('@PostTypeId', 'tinyint'),
    AcceptedAnswerId = post.value('@AcceptedAnswerId', 'int'),
    ParentId = post.value('@ParentId', 'int'),
	CreationDate = post.value('@CreationDate', 'datetime'),
	Score = post.value('@Score', 'int'),
	ViewCount = post.value('@ViewCount', 'int'),
	Body = post.value('@Body', 'nvarchar(MAX)'),
	Title = post.value('@Title', 'nvarchar(MAX)'),
	Tags = post.value('@Tags', 'nvarchar(MAX)'),
	AnswerCount = post.value('@AnswerCount', 'int'),
	CommentCount = post.value('@CommentCOunt', 'int'),
	FavoriteCount = post.value('@FavoriteCount', 'int')
FROM 
 @XMLFile.nodes('/posts/row')  AS XTbl(post)
