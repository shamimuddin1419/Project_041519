/* ----------------------------------------------------------------------------
PROC:	UserPackageRequest_Approve
DESC:	Approve a Package Request for an user.

PARAMS:
HIST:	
	2021-10-25 Newaz Created
NOTES:

------------------------------------------------------------------------------- */
ALTER PROCEDURE [dbo].[UserPackageRequest_Approve]
	@UserPackageRequestId 	int,	
	@CreatedBy	varchar(37),	
	@ErrCode			varchar(2) = NULL OUTPUT,
	@UserMsg			varchar(200) = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT, XACT_ABORT ON;
	
	DECLARE @IvUserId varchar(37),
					@IvUserName varchar(100),
					@IvPackageId int,
					@IvPackageDuration int

	   --- ASSERT (Userid is Null.)
	IF @CreatedBy IS NULL
		GOTO ERR_REQUIRED_USERID

		--- ASSERT (UserPackageRequestId is Null.)
	IF @UserPackageRequestId IS NULL
		GOTO ERR_REQUIRED_USERPACKAGEREQUESTEDID

		--- ASSERT (If UserId Exists)
	IF NOT EXISTS (SELECT 1 FROM tblUser WHERE UserId = @CreatedBy)  
			GOTO ERR_INVALID_USER;

	--- ASSERT (If UserPackageRequestId Exists)
	IF NOT EXISTS (SELECT 1 FROM tblUserPackageRequest WHERE UserPackageRequestId = @UserPackageRequestId)  
			GOTO ERR_INVALID_USERPACKAGEREQUESTID;
	
	BEGIN TRANSACTION;  

	BEGIN TRY
		
		SELECT @IvUserId = upr.UserId  , 
		@IvUserName= u.UserName,
		@IvPackageId = p.PackageId,
		@IvPackageDuration = p.PackageDurationDays
		FROM tblUserPackageRequest upr
		INNER JOIN tblPackage p on upr.PackageId = p.PackageId
		INNER JOIN tblUser u  on upr.UserId = u.UserId
		WHERE UserPackageRequestId = @UserPackageRequestId

		update [dbo].[tblUserPackageRequest]
           set IsApproved = 1,
		   ApprovedBy = @CreatedBy,
		   ApprovedDate = getdate()
		WHERE UserPackageRequestId = @UserPackageRequestId
		
		  update  u
		  set PackageId = @IvPackageId,
		  PackageValidityDate = DATEADD(day,@IvPackageDuration,getdate()),
		  ApprovedDate=getdate()
		  FROM tblUser u
		  WHERE u.UserId = @IvUserId

		  -------------Add Balance to the Parent Starts--------------

		SELECT * INTO #tempData
		FROM 
		(SELECT pulm.UserId,pulm.ParentUserId,pulm.ParentUserLevel,u.UserName,ulc.ReferrelCount,pc.MaxLevel,p.PackageValue,l.LimitQtyReferrel,p.PackageId,
		CASE WHEN pulm.ParentUserLevel <= pc.MaxLevel AND ulc.ReferrelCount < l.LimitQtyReferrel AND u2.PackageId IS NOT NULL AND CONVERT(DATE, u2.PackageValidityDate) >=  CONVERT(DATE,GETDATE()) then (p.PackageValue*l.ReferrelCommission) else 0 end as Commission
			FROM tblParentUserLevelMapping pulm
		INNER JOIN tblUser u on pulm.UserId = u.UserId
		INNER JOIN tblUser u2 on pulm.ParentUserId = u2.UserId
		INNER JOIN tblUserLevelCount ulc on pulm.ParentUserId = ulc.UserId and pulm.ParentUserLevel = ulc.Level
		INNER JOIN tblLevelInfo l on pulm.ParentUserLevel = l.Level
		INNER JOIN tblPackage p on u2.PackageId = p.PackageId
		INNER JOIN tblPackageCategory pc on p.PackageCategoryId = pc.PackageCategoryId

		WHERE pulm.UserId = @IvUserId
		) x

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
			GETDATE()
			,ParentUserId
			,'Cr.'
			,Commission
			,NULL
			,PackageId
			,@IvUserId
			,'Referrel Commission From User : ' +@IvUserName
			,@CreatedBy
			,GETDATE()
		FROM #tempData
		WHERE Commission > 0

		--UPDATE cb
		--SET 
		--cb.TotalBalance = TotalBalance + t.Commission,
		--TotalCredit = TotalCredit + t.Commission,
		--EditedBy = @CreatedBy,
		--EditedDate = GETDATE()
		--FROM tblCustomerBalance cb
		--INNER JOIN #tempData t ON cb.UserId = t.ParentUserId



		MERGE tblCustomerBalance as target
		USING
		(
			SELECT 
			* 
			FROM #tempData
			WHERE Commission > 0
		) as source
		on(source.ParentUserId = target.UserId)
		WHEN NOT MATCHED BY target THEN
		INSERT 
		  ([UserId]
           ,[TotalDebit]
           ,[TotalCredit]
		   ,[TotalReferrelCommission]
           ,[TotalBalance]
           ,[CreatedBy]
           ,[CreatedDate]
           )
		VALUES(source.ParentUserId,0,Commission,Commission,Commission,@CreatedBy,GETDATE())
		WHEN MATCHED THEN 
		UPDATE SET
		target.TotalCredit = target.TotalCredit + source.Commission,
		target.TotalBalance = target.TotalBalance + source.Commission,
		target.TotalReferrelCommission = target.TotalReferrelCommission + source.Commission,
		target.EditedBy = @CreatedBy,
		target.EditedDate = getdate();



		UPDATE  ulc
		SET ReferrelCount = ulc.ReferrelCount +1
		FROM tblUserLevelCount ulc
		INNER JOIN #tempData t ON ulc.UserId = t.ParentUserId AND ulc.Level = t.ParentUserLevel
		INNER JOIN tblUser u ON t.ParentUserId = u.UserId
		WHERE t.ParentUserLevel <= t.MaxLevel and ulc.ReferrelCount < t.LimitQtyReferrel AND u.PackageId IS NOT NULL AND CONVERT(DATE, u.PackageValidityDate) >=  CONVERT(DATE,GETDATE())



		
	    COMMIT TRANSACTION;
		SELECT	@ErrCode	= '00'
			,@UserMsg	= 'Package Request approve has been done' 
		RETURN 0;
	END TRY
	BEGIN CATCH
		 SELECT	@ErrCode	= '11'
			,@UserMsg	= 'An internal DB Error has occured' 
		 ROLLBACK TRANSACTION;
		 GOTO EXIT_ERROR

	END CATCH
			
	ERR_REQUIRED_USERID:
			SELECT	@ErrCode	= '12'
			,@UserMsg	= 'UserId cannot be null' 
		GOTO EXIT_ERROR

	ERR_REQUIRED_USERPACKAGEREQUESTEDID:
		SELECT	@ErrCode	= '12'
		,@UserMsg	= 'UserPackageRequestedId cannot be null' 
	GOTO EXIT_ERROR

	ERR_INVALID_USERPACKAGEREQUESTID:
		SELECT	@ErrCode	= '12'
		,@UserMsg	= 'The UserPackageRequestedId provided does not exists' 
	GOTO EXIT_ERROR

	ERR_INVALID_USER:
		SELECT	@ErrCode	= '12'
		,@UserMsg	= 'Input User is not valid' 
	GOTO EXIT_ERROR

	EXIT_ERROR:
	RETURN 0
END

