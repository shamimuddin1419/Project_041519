USE [DOSDB]
GO
/****** Object:  StoredProcedure [dbo].[Withdraw_Get]    Script Date: 1/27/2022 11:18:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* ----------------------------------------------------------------------------
PROC:	Withdraw_Get
DESC:	Get the Withdraw Information.


HIST:	
	2022-01-02	Newaz		Created
NOTES:
------------------------------------------------------------------------------- */
ALTER PROCEDURE [dbo].[Withdraw_Get]
    @UserId	varchar(37)=NULL,
	@WithdrawStatus   char(1)=NULL,
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
		  ,CASE WHEN WithdrawBalanceType = 'E' Then 'Main Balance' Else 'Commission' END as WithdrawBalanceType
		  ,w.PaymentMethodTypeId
		  ,pmt.PaymentMethodTypeName as PaymentMethod
		  ,PaymentDetails as WalletAddress
		  ,WithdrawRequestBalance as WithdrawAmount
		  ,WithdrawCharge
		  ,Case when WithdrawStatus='R' Then 'Rejected' when WithdrawStatus='A' then 'Approved' else 'Pending' end as WithdrawStatus 
		  ,w.Remarks
		  ,w.CreatedBy
		  ,w.CreatedDate
		  ,convert(varchar, w.CreatedDate, 103) as StringCreateDate
		  ,EditedBy
		  ,EditedDate
		  ,tblUser.FullName as UserFullName
		  ,w.UserId
  FROM tblWithdraw w
  INNER JOIN tblPaymentMethodType pmt on w.PaymentMethodTypeId = pmt.PaymentMethodTypeId
  INNER JOIN tblUser on w.UserId=tblUser.UserId
  WHERE (@UserId IS NULL OR w.UserId = @UserId) AND (@WithdrawStatus IS NULL OR w.WithdrawStatus = @WithdrawStatus)
  order by CreatedDate desc
		RETURN 0;
	
	EXIT_ERROR:
		RETURN  0
	
END

