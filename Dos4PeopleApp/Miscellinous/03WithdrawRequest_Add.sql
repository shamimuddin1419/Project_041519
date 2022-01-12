/* ----------------------------------------------------------------------------
PROC:	WithdrawRequest_Add
DESC:	Creates a Withdraw Request for an user.

PARAMS:
HIST:	
	2022-01-01 Newaz Created
NOTES:

------------------------------------------------------------------------------- */
ALTER PROCEDURE [dbo].[WithdrawRequest_Add]
	@UserId	varchar(37),	
	@WithdrawBalanceType	char(1),	---WithdrawBalanceType E for Earn Balance C for Commission Balance; C= TotalTaskCommission+TotalReferrelCommission
	@PaymentMethodTypeId 	int,	
	@PaymentDetails	 nvarchar(300),	
	@WithdrawRequestBalance	numeric(18,4),	
	@Remarks nvarchar(300),
	--@WithdrawStatus     char(1),
	@ErrCode			varchar(2) = NULL OUTPUT,
	@UserMsg			varchar(200) = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT, XACT_ABORT ON;
	
	DECLARE @WithdrawDate int = 0;
	DECLARE @WithdrawalChargeMulti numeric(18,4) = 0;
	DECLARE @WithdrawalChargeFixed numeric(18,4) = 0;
	DECLARE @AvailableBalance numeric(18,4) = 0;
	DECLARE @MinimumAmount numeric(18,4)
	DECLARE @MaximumAmount numeric(18,4)

	--ASSERT(WithdrawBalanceType is null)
	IF @WithdrawBalanceType IS  NULL
	BEGIN
		GOTO ERR_REQUIRED_BALANCE_TYPE
	END
	--ASSERT (PaymentMethodId is null)
	IF @PaymentMethodTypeId IS  NULL
	BEGIN
		GOTO ERR_REQUIRED_PAYMENT_METHOD
	END

	--ASSERT(WithdrawalRequestBalance is null)
	IF @WithdrawRequestBalance is null
	BEGIN
		GOTO ERR_REQUIRED_WITHDRAW_BALANCE
	END
	   --- ASSERT (Userid is Null.)
	IF @UserId IS NULL
	BEGIN
		GOTO ERR_REQUIRED_USERID
	END

		--- ASSERT (If pending withdrawal request Exists)
	IF EXISTS (SELECT 1 FROM tblWithdraw  WHERE WithdrawBalanceType = @WithdrawBalanceType AND WithdrawStatus = 'P' and UserId = @UserId)  --P = Pending , A = Approved, R = Rejected
			GOTO ERR_EXISTS_PENDING_WITHDRAW;

	--- ASSERT (If UserId Exists)
	IF NOT EXISTS (SELECT 1 FROM tblUser WHERE UserId = @UserId)  
			GOTO ERR_INVALID_USER;

	SELECT @WithdrawDate = [day],@WithdrawalChargeMulti = ISNULL([ChargeMulti],0) ,@WithdrawalChargeFixed = ISNULL([FixedCharge],0)
	from tblWithdrawalConfig where WithdrawBalanceType = @WithdrawBalanceType
	
	--ASSERT(IF try to withdrarw more than available balance)
	SELECT @AvailableBalance = CASE WHEN @WithdrawBalanceType = 'E' then
												ISNULL(TotalTaskEarn,0) - ISNULL(TotalEarnWithdraw,0)
												ELSE ISNULL(TotalReferrelCommission,0)	+ ISNULL(TotalTaskCommission,0) -  ISNULL(TotalCommissionWithdraw,0) END  
												FROM tblCustomerBalance where UserId = @UserId

	
	IF @WithdrawRequestBalance + (@WithdrawRequestBalance*@WithdrawalChargeMulti) + @WithdrawalChargeFixed > @AvailableBalance
		GOTO ERR_EXISTS_BALANCE_SHORTAGE;

	IF @MinimumAmount is not null AND  @WithdrawRequestBalance < @MinimumAmount
		GOTO ERR_MINIMUM_BALANCE

	IF @MaximumAmount is not null AND  @WithdrawRequestBalance > @MaximumAmount
		GOTO ERR_MAXIMUM_BALANCE

	IF @WithdrawDate <> 0  and  @WithdrawDate <>  DATEPART(day, getdate()) 
		GOTO ERR_INVALID_WITHDRAW_DATE;
		

	BEGIN TRANSACTION;  


	BEGIN TRY
		
		INSERT INTO [dbo].[tblWithdraw]
           ([WithdrawBalanceType]
           ,[UserId]
           ,[PaymentMethodTypeId]
           ,[PaymentDetails]
           ,[WithdrawRequestBalance]
		   ,[WithdrawCharge]
           ,[WithdrawStatus]
		   ,[Remarks]
           ,[CreatedBy]
           ,[CreatedDate]
		)
     VALUES
           (@WithdrawBalanceType
           ,@UserId
           ,@PaymentMethodTypeId
           ,@PaymentDetails
           ,@WithdrawRequestBalance
		   ,(@WithdrawRequestBalance*ISNULL(@WithdrawalChargeMulti,0)) + ISNULL(@WithdrawalChargeFixed,0)
           ,'P'
		   ,@Remarks
           ,@UserId
           ,GETDATE()
         )
		
	    COMMIT TRANSACTION;
		SELECT	@ErrCode	= '00'
			,@UserMsg	= 'Withdrawal Request is created. After Disbursing you will be notified' 
		RETURN 0;
	END TRY
	BEGIN CATCH
		 SELECT	@ErrCode	= '11'
			,@UserMsg	= 'An internal DB Error has occured' 
		 ROLLBACK TRANSACTION;
		 GOTO EXIT_ERROR

	END CATCH
	
	ERR_REQUIRED_BALANCE_TYPE:
		SELECT	@ErrCode	= '12'
			,@UserMsg	= 'Please Provide Balance Type From which you want to withdraw' 
		GOTO EXIT_ERROR	

	ERR_REQUIRED_PAYMENT_METHOD:
			SELECT	@ErrCode	= '12'
			,@UserMsg	= 'Please provide the payment method information' 
		GOTO EXIT_ERROR

		ERR_REQUIRED_WITHDRAW_BALANCE:
		SELECT	@ErrCode	= '12'
		,@UserMsg	= 'Please Provide the balance amount you want to withdraw' 
	GOTO EXIT_ERROR

	ERR_REQUIRED_USERID:
			SELECT	@ErrCode	= '12'
			,@UserMsg	= 'UserId cannot be null' 
		GOTO EXIT_ERROR

	ERR_EXISTS_PENDING_WITHDRAW:
		SELECT	@ErrCode	= '12'
		,@UserMsg	= 'You aleady have a pending withdraw request not processed. Cannot process further' 
	GOTO EXIT_ERROR

	ERR_EXISTS_BALANCE_SHORTAGE:
		SELECT	@ErrCode	= '12'
		,@UserMsg	= 'Your Requested Balance Exceeds your available balance' 
	GOTO EXIT_ERROR


	ERR_INVALID_WITHDRAW_DATE:
		SELECT	@ErrCode	= '12'
		,@UserMsg	= 'Cannot make withdrawal request today' 
	GOTO EXIT_ERROR

	ERR_MINIMUM_BALANCE: 
		SELECT	@ErrCode	= '12'
		,@UserMsg	= 'Minimum withdrawal balance is : ' + @MinimumAmount + ' for withdrawal'
	GOTO EXIT_ERROR
	
	ERR_MAXIMUM_BALANCE: 
	SELECT	@ErrCode	= '12'
		,@UserMsg	= 'You cannot withdraw more than : '+ @MaximumAmount
	GOTO EXIT_ERROR


	ERR_INVALID_USER:
		SELECT	@ErrCode	= '12'
		,@UserMsg	= 'Input User is not valid' 
	GOTO EXIT_ERROR

	EXIT_ERROR:
	RETURN 0
END
