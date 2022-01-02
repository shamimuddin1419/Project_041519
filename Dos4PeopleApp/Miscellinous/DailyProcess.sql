USE [DOSDB]
GO
/****** Object:  StoredProcedure [dbo].[DailyTaskAutoProcess]    Script Date: 1/1/2022 2:45:51 PM ******/
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
		target.TotalCredit = target.TotalCredit + source.Credit,
		target.TotalBalance = target.TotalBalance + source.Credit,
		target.TotalTaskEarn = target.TotalTaskEarn + source.Credit,
		target.EditedBy = @CreatedBy,
		target.EditedDate = getdate();

		print @StartDate;
		set @StartDate = DATEADD(day, 1, @StartDate);
	END
		
	    COMMIT TRANSACTION;
		SELECT	@ErrCode	= '00'
			,@UserMsg	= 'Daily Automatic Task Commission Disbursed Successfully' 
		RETURN 0;
	END TRY
	BEGIN CATCH
		 SELECT	@ErrCode	= '11'
			,@UserMsg	= 'An internal DB Error has occured' 
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

