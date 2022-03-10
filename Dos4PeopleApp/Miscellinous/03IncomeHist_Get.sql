USE [DOSDB]
GO
/****** Object:  StoredProcedure [dbo].[IncomeHist_Get]    Script Date: 3/5/2022 12:38:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* ----------------------------------------------------------------------------
PROC:	IncomeHist_Get
DESC:	Get the Income History of a specific user


HIST:	
	2022-03-05	Newaz		Created
NOTES:
------------------------------------------------------------------------------- */
CREATE PROCEDURE [dbo].[IncomeHist_Get]
    @UserId	uniqueidentifier,
	@FromDate   DateTime = NULL,
	@ToDate DateTime = NULL,
	@ErrCode			varchar(2) = NULL OUTPUT,
	@UserMsg			varchar(200) = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
		SET	@ErrCode	= '00'
		SET	@UserMsg	= ''

		
		SELECT @FromDate = ISNULL(@FromDate, CAST(CAST(GETDATE() AS DATE) AS DATETIME))
		SELECT @ToDate = ISNULL(@ToDate, CAST(CAST(GETDATE()+1 AS DATE) AS DATETIME))

		SELECT tm.TransactionMasterId,FORMAT(tm.Date,'dd-MMM-yyyy') AS TransactionDate,tm.TransactionAmt,
		tm.Remarks,
		FORMAT(u.ApprovedDate,'dd-MMM-yyyy') JoiningDate,
		DATEDIFF(DAY,GETDATE(),u.PackageValidityDate) AS CurrentDuration,
		FORMAT(u.PackageValidityDate,'dd-MMM-yyyy') as ExpiryDate
		FROM tblTransationMaster tm
		INNER JOIN tblUser u on ISNULL(tm.UserIdAsRef,tm.UserId) = u.UserId
		WHERE tm.UserId = @UserId
		AND tm.Date >= @FromDate AND tm.Date <= @ToDate
		AND TransactionType = 'Cr.'
		ORDER By Date Desc

		RETURN 0;
	
	EXIT_ERROR:
		RETURN  0
	
END

