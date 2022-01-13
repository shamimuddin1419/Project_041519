
/* ----------------------------------------------------------------------------
PROC:	WithdrawBalanceInfo_Get
DESC:	Get the User Balance Inforamtion for withdraw


HIST:	
	2022-01-13	Newaz Created
NOTES:
------------------------------------------------------------------------------- */
CREATE PROCEDURE [dbo].[WithdrawBalanceInfo_Get]
	@UserId			varchar(37),
	@ErrCode			varchar(2) = NULL OUTPUT,
	@UserMsg			varchar(200) = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
		SET	@ErrCode	= '00'
		SET	@UserMsg	= ''

		DECLARE 	@IvAvailableTaskEarn DECIMAL(18,4),
						@IvMaxWithdrawableTaskEarn DECIMAL(18,4),
						@IvAvailableCommissionEarn DECIMAL(18,4),
						@IvMaxWithdrawableCommission DECIMAL(18,4),
						@IvCommissionWithdrawDay int = null,
						@IvEarnWithdrawDay int = null,

						@IvMinEarningConfigAmount DECIMAL(18,4),
						@IvMaxEarningConfigAmount DECIMAL(18,4),

						@IvMinCommissionConfigAmount DECIMAL(18,4) = 0,
						@IvMaxCommissionConfigAmount DECIMAL(18,4) = null,

						@IvChargeMulti DECIMAL(18,4) = 0,
						@IvFixedCharge DECIMAL(18,4) = 0;

		IF NOT EXISTS (SELECT 1 FROM tblUser WHERE UserId = @UserId)  
			GOTO ERR_INVALID_USER;
		
		SELECT 
		@IvAvailableTaskEarn = (ISNULL( cb.TotalTaskEarn,0) - ISNULL(cb.TotalEarnWithdraw,0)),
		@IvAvailableCommissionEarn = (ISNULL(cb.TotalReferrelCommission,0) + ISNULL(cb.TotalTaskCommission,0))  -  ISNULL(cb.TotalCommissionWithdraw,0)
		FROM tblCustomerBalance cb
		WHERE cb.UserId = @UserId



		SELECT @IvChargeMulti =  ISNULL(ChargeMulti,0),
					@IvFixedCharge = ISNULL(FixedCharge,0),
					@IvMinEarningConfigAmount = ISNULL(MinimumAmount,0),
					@IvMaxEarningConfigAmount =  MaximumAmount 
					,@IvEarnWithdrawDay = Day 
					FROM tblWithdrawalConfig
	  WHERE WithdrawBalanceType = 'E' 

	  SET @IvMaxWithdrawableTaskEarn = @IvAvailableTaskEarn - ((@IvAvailableTaskEarn * @IvChargeMulti) + @IvFixedCharge)
	  
	  print @IvAvailableTaskEarn
	  print @IvChargeMulti
	  print @IvFixedCharge
	  print @IvMaxWithdrawableTaskEarn

	  SELECT @IvChargeMulti = ISNULL(ChargeMulti,0),
					@IvFixedCharge = ISNULL(FixedCharge,0),
					@IvMinCommissionConfigAmount = ISNULL(MinimumAmount,0),
					@IvMaxEarningConfigAmount =  MaximumAmount ,
					@IvCommissionWithdrawDay = Day
					FROM tblWithdrawalConfig
	  WHERE WithdrawBalanceType = 'C' 
	

	  SET @IvMaxWithdrawableCommission = @IvAvailableCommissionEarn - ((@IvAvailableCommissionEarn * @IvChargeMulti) + @IvFixedCharge) 

	  SELECT @IvAvailableTaskEarn as AvailableTaskEarn,
				  @IvAvailableCommissionEarn  as AvailableCommissionEarn,
				  CASE WHEN  @IvMaxWithdrawableCommission < @IvMinCommissionConfigAmount THEN 0
						WHEN @IvMaxCommissionConfigAmount IS NOT NULL AND @IvMaxWithdrawableCommission > @IvMaxCommissionConfigAmount THEN ISNULL(@IvMaxCommissionConfigAmount,0)
						ELSE ISNULL(@IvMaxWithdrawableCommission,0) end as MaxWithdrawableCommission,

				 CASE WHEN  @IvMaxWithdrawableTaskEarn < @IvMinEarningConfigAmount THEN 0
						WHEN @IvMaxEarningConfigAmount IS NOT NULL AND @IvMaxWithdrawableTaskEarn > @IvMaxEarningConfigAmount THEN ISNULL(@IvMaxEarningConfigAmount,0)
						ELSE ISNULL(@IvMaxWithdrawableTaskEarn,0) end as MaxWithdrawableEarn,
			   @IvEarnWithdrawDay as EarnWithdrawDay,
			   @IvCommissionWithdrawDay as  CommissionWithdrawDay


				RETURN 0;

	ERR_INVALID_USER:
		SELECT	@ErrCode	= '12'
		,@UserMsg	= 'The Input User is not Valid' 
	GOTO EXIT_ERROR
	
	EXIT_ERROR:
		RETURN  0
	
END

--exec [dbo].[WithdrawBalanceInfo_Get]  'F3DC84C1-8239-4DBA-978C-78132D8E0EC9'

