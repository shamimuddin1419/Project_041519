
/* ----------------------------------------------------------------------------
PROC:	Withdraw_Get
DESC:	Get the Withdraw Information.


HIST:	
	2022-01-02	Newaz		Created
NOTES:
------------------------------------------------------------------------------- */
ALTER PROCEDURE [dbo].[Withdraw_Get]
    @UserId	varchar(37),
	@WithdrawStatus   char(1),
	@ErrCode			varchar(2) = NULL OUTPUT,
	@UserMsg			varchar(200) = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
		SET	@ErrCode	= '00'
		SET	@UserMsg	= ''


		SELECT
		   WithdrawId
		  ,WithdrawBalanceType
		  ,CASE WHEN WithdrawBalanceType = 'E' Then 'Earning Balance' Else 'Commission Balance' END as WithdrawBalanceTypeName
		  ,w.PaymentMethodTypeId
		  ,pmt.PaymentMethodTypeName
		  ,PaymentDetails
		  ,WithdrawRequestBalance
		  ,WithdrawCharge
		  ,WithdrawStatus
		  ,w.Remarks
		  ,CreatedBy
		  ,CreatedDate
		  ,EditedBy
		  ,EditedDate
  FROM tblWithdraw w
  INNER JOIN tblPaymentMethodType pmt on w.PaymentMethodTypeId = pmt.PaymentMethodTypeId
  WHERE (@UserId IS NULL OR w.UserId = @UserId) AND (@WithdrawStatus IS NULL OR w.WithdrawStatus = @WithdrawStatus)

		RETURN 0;
	
	EXIT_ERROR:
		RETURN  0
	
END

