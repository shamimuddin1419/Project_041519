USE [DOSDB]
GO
/****** Object:  StoredProcedure [dbo].[DailyTaskAutoProcess]    Script Date: 3/5/2022 2:02:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* ----------------------------------------------------------------------------
PROC:	DailyTaskAutoProcess
DESC:	Process Daily Task Commission Automatically for users

PARAMS:
HIST:	
	2021-10-26 Newaz Created
NOTES:

------------------------------------------------------------------------------- */
ALTER PROCEDURE [dbo].[DailyTaskAutoProcess]
	@CreatedBy	varchar(37),
	@ErrCode			varchar(2) = NULL OUTPUT,
	@UserMsg			varchar(200) = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT, XACT_ABORT ON;
	declare @StartDate date
	declare @EndDate date = CONVERT(DATE,DATEADD(day,-1,getdate()))
	declare @IvProcessId int;


	SELECT @StartDate =CONVERT(DATE,DATEADD(day,1,max(ProcessDate))) FROM tblAutoTaskRewardProcess
	IF @StartDate is NULL SELECT @StartDate = CONVERT(DATE,min(ApprovedDate)) from tblUser
		
	

	--- ASSERT (User is not Active or not exists)
	IF NOT EXISTS (SELECT 1 FROM tblUser WHERE UserId = @CreatedBy) GOTO ERR_INVALID_USER
	
	BEGIN TRANSACTION;  

	BEGIN TRY
		
	WHILE (@StartDate <= @EndDate)
	BEGIN
	---Insert into tblAutoTaskRewardProcess
		INSERT INTO [dbo].[tblAutoTaskRewardProcess]
           ([ProcessDate]
           ,[CreatedDate]
           ,[CreatedBy])
     VALUES
           (@StartDate
           ,GETDATE()
           ,@CreatedBy)

		SELECT @IvProcessId = SCOPE_IDENTITY();

		---Insert into transactionMaster
		INSERT INTO [dbo].[tblTransationMaster]
           ([Date]
           ,[UserId]
           ,[TransactionType]
           ,[TransactionAmt]
           ,[AutoTaskRewardProcessId]
		   ,[PackageIdAsRef]
           ,[Remarks]
           ,[CreatedBy]
           ,[CreatedDate])
		SELECT 
           @StartDate
           ,UserId
           ,'Cr.'
           ,p.DailyValue
           ,@IvProcessId
		   ,p.PackageId
           ,'Added By Daily Automatic Task Commission'
           ,@CreatedBy
           ,GETDATE()
		FROM tbluser u 
		INNER JOIN tblPackage p on u.PackageId = p.PackageId
		WHERE u.PackageId is not null and CONVERT(DATE,PackageValidityDate) >= @StartDate and CONVERT(DATE,u.ApprovedDate) <=@StartDate

		----Update Customer Balance

		MERGE tblCustomerBalance as target
		USING
		(
			SELECT 
				u.UserId
				,p.DailyValue as Credit
			FROM tbluser u 
			INNER JOIN tblPackage p on u.PackageId = p.PackageId
			WHERE u.PackageId is not null and CONVERT(DATE,PackageValidityDate) >= @StartDate and CONVERT(DATE,u.ApprovedDate) <=@StartDate
		) as source
		on(source.UserId = target.UserId)
		WHEN NOT MATCHED BY target THEN
		INSERT 
		  ([UserId]
           ,[TotalDebit]
           ,[TotalCredit]
		   ,[TotalTaskEarn]
           ,[TotalBalance]
           ,[CreatedBy]
           ,[CreatedDate]
           )
		VALUES(source.UserId,0,Credit,Credit,Credit,@CreatedBy,GETDATE())
		WHEN MATCHED THEN 
		UPDATE SET
		target.TotalCredit = isnull(target.TotalCredit,0) + source.Credit,
		target.TotalBalance = isnull(target.TotalBalance,0) + source.Credit,
		target.TotalTaskEarn = isnull(target.TotalTaskEarn,0) + source.Credit,
		target.EditedBy = @CreatedBy,
		target.EditedDate = getdate();
		---------------------------START Commission Distribution------------------------------------
		
		If(OBJECT_ID('tempdb..#tempUser') Is Not Null)
		begin
			Drop Table #tempUser
		end
		SELECT UserId
		INTO #tempUser
		FROM tbluser u 
		INNER JOIN tblPackage p on u.PackageId = p.PackageId
		WHERE u.PackageId is not null and CONVERT(DATE,PackageValidityDate) >= @StartDate and CONVERT(DATE,u.ApprovedDate) <=@StartDate


		If(OBJECT_ID('tempdb..#tempData') Is Not Null)
		begin
			Drop Table #tempData
		end
		
		
			----prepare data
			
			SELECT 
			pulm.UserId,
			u2.UserName as ChildUserName,
			pulm.ParentUserId,
			pulm.ParentUserLevel,
			u.UserName ParentUserName,
			ulc.TaskCount,
			pc.MaxLevel,
			p.PackageValue,
			l.LimitQtyTask,
			p.PackageId,
			culm.UserId NewChildUserId,
			CASE WHEN (culm.UserId IS NOT NULL  OR ulc.TaskCount < l.LimitQtyTask) AND (pulm.ParentUserLevel <= pc.MaxLevel AND CONVERT(DATE, u.PackageValidityDate) >=  @StartDate)  then (p.DailyValue*l.TaskCommission) else 0 end as Commission,
			CASE WHEN (culm.UserId IS NULL AND pulm.ParentUserLevel <= pc.MaxLevel AND ulc.TaskCount < l.LimitQtyTask AND CONVERT(DATE, u.PackageValidityDate) >=  @StartDate) then 1 else 0 end as NeedIncrement
			INTO #tempData
			FROM tblParentUserLevelMapping pulm
			INNER JOIN #tempUser tu ON pulm.UserId = tu.UserId
			INNER JOIN tblUser u ON pulm.ParentUserId = u.UserId
			INNER JOIN tblUser u2 ON pulm.UserId = u2.UserId
			INNER JOIN tblUserLevelCount ulc ON pulm.ParentUserId = ulc.UserId and pulm.ParentUserLevel = ulc.Level
			INNER JOIN tblLevelInfo l ON pulm.ParentUserLevel = l.Level
			INNER JOIN tblPackage p ON u2.PackageId = p.PackageId
			INNER JOIN tblPackageCategory pc ON p.PackageCategoryId = pc.PackageCategoryId
			LEFT JOIN tblParentUserMappingForTask culm ON pulm.ParentUserId = culm.ParentUserId AND pulm.UserId = culm.UserId AND pulm.ParentUserLevel = culm.Level
			--where pulm.UserId = @CursorUserId
			

			---insert into transaction master
			INSERT INTO [dbo].[tblTransationMaster]
			([Date]
			,[UserId]
			,[TransactionType]
			,[TransactionAmt]
			,[AutoTaskRewardProcessId]
			,[PackageIdAsRef]
			,[UserIdAsRef]
			,[Remarks]
			,[CreatedBy]
			,[CreatedDate])
		SELECT 
			@StartDate
			,ParentUserId
			,'Cr.'
			,Commission
			,@IvProcessId
			,PackageId
			,UserId
			,'Task Commission From Level : '+CAST(ParentUserLevel AS VARCHAR)+' UserName : '+ChildUserName
			,@CreatedBy
			,GETDATE()
		FROM #tempData t
		WHERE t.Commission > 0


		----update customer balance
		UPDATE cb
		SET 
		cb.TotalBalance = isnull(TotalBalance,0) + isnull(t.Commission,0),
		TotalCredit = isnull(TotalCredit,0) + isnull(t.Commission,0),
		TotalTaskCommission = isnull(TotalTaskCommission,0) + isnull(t.Commission,0),
		EditedBy = @CreatedBy,
		EditedDate = GETDATE()
		FROM tblCustomerBalance cb
		INNER JOIN #tempData t ON cb.UserId = t.ParentUserId
		WHERE t.Commission > 0


		--update taskcount
		UPDATE  ulc
		SET  TaskCount = ulc.TaskCount +1
		FROM tblUserLevelCount ulc
		INNER JOIN #tempData t ON ulc.UserId = t.ParentUserId AND ulc.Level = t.ParentUserLevel
		WHERE t.NeedIncrement = 1


		---ADD data tbParentUserMappingForTask 
		INSERT INTO tblParentUserMappingForTask
		SELECT UserId,ParentUserId,ParentUserLevel,@CreatedBy,GETDATE() FROM #tempData t
		WHERE  t.NeedIncrement = 1 --AND  t.ParentUserLevel <= t.MaxLevel

		---------------------------END Commission Distribution----------------------------------------
		
		set @StartDate = DATEADD(day, 1, @StartDate);
	END
		
	    COMMIT TRANSACTION;
		SELECT	@ErrCode	= '00'
			,@UserMsg	= 'Daily Automatic Task Commission Disbursed Successfully' 
		RETURN 0;
	END TRY
	BEGIN CATCH
		 SELECT	@ErrCode	= '11'
			,@UserMsg	= ERROR_MESSAGE ( )   --'An internal DB Error has occured' 
		 ROLLBACK TRANSACTION;
		 GOTO EXIT_ERROR

	END CATCH

	ERR_INVALID_USER:
			SELECT	@ErrCode	= '12'
			,@UserMsg	= 'The Input User is not Valid' 
		GOTO EXIT_ERROR

	EXIT_ERROR:
	RETURN 0
END

