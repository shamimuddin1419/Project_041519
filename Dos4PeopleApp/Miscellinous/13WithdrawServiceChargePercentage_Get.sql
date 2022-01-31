-- =============================================
-- Author:		Newaz Sharif
-- Create date: 31/01/2022
-- Description: Get the service charge for withdraw
-- =============================================

--exec WithdrawServiceChargePercentage_Get
alter Procedure WithdrawServiceChargePercentage_Get

AS
BEGIN
	
	DECLARE @MainBalancePer decimal(18,4) = 0
	DECLARE @CommissionBalancePer decimal(18,4) = 0

	
		SELECT @MainBalancePer = (100 * ChargeMulti)  FROM tblWithdrawalConfig 
		WHERE WithdrawBalanceType = 'E'

			SELECT  top(1) @CommissionBalancePer = (100 * ChargeMulti)  FROM tblWithdrawalConfig 
		WHERE WithdrawBalanceType = 'C'


		select isnull(@MainBalancePer,0) as MainBalanceServiceChargePer,
		isnull(@CommissionBalancePer,0) as CommissionBalanceServiceChargePer

	
	

END
GO