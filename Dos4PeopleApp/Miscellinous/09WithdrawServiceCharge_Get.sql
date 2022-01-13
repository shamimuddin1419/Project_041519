
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Newaz Sharif
-- Create date: 13/01/2022
-- Description: Get the service charge for withdraw
-- =============================================
CREATE FUNCTION WithdrawServiceCharge_Get
(
	@Amount decimal(18,4),
	@WithdrawBalanceType char(1)
)
RETURNS decimal(18,4)
AS
BEGIN
	
	DECLARE @ChargeAmount decimal(18,4) = 0

	
		SELECT  top(1) @ChargeAmount = (@Amount * ChargeMulti) + FixedCharge FROM tblWithdrawalConfig 
		WHERE WithdrawBalanceType = @WithdrawBalanceType

	
	RETURN @ChargeAmount

END
GO

