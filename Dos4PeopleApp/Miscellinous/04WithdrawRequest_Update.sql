/* ----------------------------------------------------------------------------
PROC:	WithdrawRequest_Approve
DESC:	Approve a withdrawal request for an user

PARAMS:
HIST:	
	2022-01-01 Newaz Created
NOTES:

------------------------------------------------------------------------------- */
CREATE PROCEDURE [dbo].[WithdrawRequest_Update]
	@WithdrawId 	int,	
	@WithdrawStatus char(1),
	@CreatedBy	varchar(37),	
	@ErrCode			varchar(2) = NULL OUTPUT,
	@UserMsg			varchar(200) = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT, XACT_ABORT ON;
	
	DECLARE @IvUserId varchar(37),
					@WithdrawAmount numeric(18,4),
					@ChargeAmount numeric(18,4),
					@WithdrawBalanceType numeric(18,4)

					--@IvUserName varchar(100),
					--@IvPackageId int,
					--@IvPackageDuration int

	   --- ASSERT (Userid is Null.)
	IF @CreatedBy IS NULL
		GOTO ERR_REQUIRED_USERID

		--- ASSERT (UserPackageRequestId is Null.)
	IF @WithdrawId IS NULL
		GOTO ERR_REQUIRED_WITHDRAWID

	IF @WithdrawStatus IS NULL
		GOTO ERR_REQUIRED_WITHDRAWSTATUS

		--- ASSERT (If UserId Exists)
	IF NOT EXISTS (SELECT 1 FROM tblUser WHERE UserId = @CreatedBy)  
			GOTO ERR_INVALID_USER;

	--- ASSERT (If WithdrawId Exists)
	IF NOT EXISTS (SELECT 1 FROM tblWithdraw WHERE WithdrawId = @WithdrawId)  
			GOTO ERR_INVALID_WITHDRAWID;
	
	SELECT @IvUserId = UserId,@WithdrawAmount = ISNULL(WithdrawRequestBalance,0),
	@ChargeAmount = ISNULL(WithdrawCharge,0),@WithdrawBalanceType = WithdrawBalanceType
	FROM  tblWithdraw WHERE WithdrawId = @WithdrawId

	BEGIN TRANSACTION;  

	BEGIN TRY
		
		update [dbo].[tblWithdraw]
           set WithdrawStatus =@WithdrawStatus,
		   EditedBy = @CreatedBy,
		   EditedDate = getdate()
		WHERE WithdrawId = @WithdrawId
		
	  -------------Deduction Balance  Starts--------------

		IF @WithdrawStatus = 'A'
		BEGIN
				INSERT INTO [dbo].[tblTransationMaster]
				([Date]
				,[UserId]
				,[TransactionType]
				,[TransactionAmt]
				,[WithDrawId]
				,[Remarks]
				,[CreatedBy]
				,[CreatedDate])
				VALUES
				( 
					GETDATE()
					,@IvUserId
					,'Dr.'
					,@WithdrawAmount
					,@WithDrawId
					,CASE WHEN @WithdrawBalanceType = 'E' THEN 'Withdraw From Earned Balance' ELSE  'Withdraw From Commission' END
					,@CreatedBy
					,GETDATE()
				)
				IF @ChargeAmount > 0
				BEGIN

						INSERT INTO [dbo].[tblTransationMaster]
						([Date]
						,[UserId]
						,[TransactionType]
						,[TransactionAmt]
						,[WithDrawId]
						,[Remarks]
						,[CreatedBy]
						,[CreatedDate])
						VALUES
						( 
						GETDATE()
						,@IvUserId
						,'Dr.'
						,@ChargeAmount
						,@WithDrawId
						,'Withdraw Charge'
						,@CreatedBy
						,GETDATE()
						)
				END
			UPDATE cb
			SET 
			cb.TotalBalance = ISNULL(TotalBalance,0) - (@WithdrawAmount + @ChargeAmount),
			TotalDebit = ISNULL(TotalDebit,0) +  (@WithdrawAmount + @ChargeAmount),
			TotalEarnWithdraw = ISNULL(TotalEarnWithdraw,0) + (CASE WHEN @WithdrawBalanceType = 'E' THEN @WithdrawAmount + @ChargeAmount ELSE 0 END),
			TotalCommissionWithdraw = ISNULL(TotalCommissionWithdraw,0) + (CASE WHEN @WithdrawBalanceType = 'C' THEN @WithdrawAmount + @ChargeAmount ELSE 0 END),
			EditedBy = @CreatedBy,
			EditedDate = GETDATE()
			FROM tblCustomerBalance cb
			END
	
	COMMIT TRANSACTION;
		SELECT	@ErrCode	= '00'
			,@UserMsg	= 'Success' 
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

	ERR_REQUIRED_WITHDRAWID:
		SELECT	@ErrCode	= '12'
		,@UserMsg	= 'WithdrawId cannot be null' 
	GOTO EXIT_ERROR

	ERR_REQUIRED_WITHDRAWSTATUS: 
	SELECT	@ErrCode	= '12'
		,@UserMsg	= 'WithdrawStatus cannot be null' 
	GOTO EXIT_ERROR


	ERR_INVALID_WITHDRAWID:
		SELECT	@ErrCode	= '12'
		,@UserMsg	= 'The WithdrawId you provided does not exists' 
	GOTO EXIT_ERROR

	ERR_INVALID_USER:
		SELECT	@ErrCode	= '12'
		,@UserMsg	= 'Input User is not valid' 
	GOTO EXIT_ERROR

	EXIT_ERROR:
	RETURN 0
END


