
declare @UserId varchar(37)

declare cur_test cursor fast_forward for
select userid from tblUser

open cur_test
fetch next from cur_test into @UserId
while @@FETCH_STATUS  = 0
begin
;WITH cte as(
		SELECT @UserId UserId ,1 Level
		UNION ALL
		SELECT @UserId UserId,2 Level
		UNION ALL
		SELECT @UserId UserId,3 Level
		UNION ALL
		SELECT @UserId UserId,4 Level
		UNION ALL
		SELECT @UserId UserId,5 Level
		UNION ALL
		SELECT @UserId UserId,6 Level
		UNION ALL
		SELECT @UserId UserId,7 Level
		UNION ALL
		SELECT @UserId UserId,8 Level
		UNION ALL
		SELECT @UserId UserId,9 Level
		UNION ALL
		SELECT @UserId UserId,10 Level
		)
		INSERT INTO tblUserLevelCount
		 (
			[UserId]
           ,[Level]
		 )
		 SELECT UserId,Level FROM CTE
fetch next from cur_test into @UserId
end
close cur_test
deallocate cur_test
go