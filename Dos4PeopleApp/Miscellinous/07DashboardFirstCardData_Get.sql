
/* ----------------------------------------------------------------------------
PROC:	DashboardFirstCardData_Get
DESC:	Get the DashBoard Card Data of first Part


HIST:	
	2021-10-31	Newaz		Created
NOTES:
------------------------------------------------------------------------------- */
ALTER PROCEDURE [dbo].[DashboardFirstCardData_Get]
	@UserId			varchar(37),
	@ErrCode			varchar(2) = NULL OUTPUT,
	@UserMsg			varchar(200) = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
		SET	@ErrCode	= '00'
		SET	@UserMsg	= ''
		DECLARE @IvTotalBalance DECIMAL(18,4), 
						@IvCurrentPackageName VARCHAR(250), 
						@IvCurrentPackageValue DECIMAL(18,4), 
						@IvTodaysEarn DECIMAL(18,4),
						@IvTotalReferrelCommission DECIMAL(18,4),
						@IvTotalTaskEarn DECIMAL(18,4),
						@IvTotalWorkCommission DECIMAL(18,4);

		IF NOT EXISTS (SELECT 1 FROM tblUser WHERE UserId = @UserId)  
			GOTO ERR_INVALID_USER;
		
		SELECT
			@IvTotalBalance = ISNULL(cb.TotalBalance,0),
			@IvCurrentPackageName = ISNULL(p.PackageName,''),
			@IvCurrentPackageValue = ISNULL(p.PackageValue ,0),
			@IvTotalReferrelCommission = ISNULL(TotalReferrelCommission,0),
			@IvTotalTaskEarn = ISNULL(TotalTaskEarn,0),
			@IvTotalWorkCommission = ISNULL(TotalTaskCommission,0)

		FROM tblUser u
		LEFT JOIN tblPackage p ON u.PackageId = p.PackageId
		LEFT JOIN tblCustomerBalance cb on u.UserId = cb.UserId
		WHERE u.UserId = @userId

		SELECT @IvTodaysEarn = ISNULL(SUM(TransactionAmt),0) FROM tblTransationMaster
		WHERE UserId = @UserId and TransactionType = 'Cr.'  and CONVERT(DATE,Date) = CONVERT(DATE,DATEADD(DAY,0,GETDATE()))

		SELECT @IvTotalBalance as CurrentBalance,
					@IvCurrentPackageName as CurrentPackageName,
					@IvCurrentPackageValue as CurrentPackageValue,
					@IvTodaysEarn as TodaysEarn,
					@IvTotalReferrelCommission as TotalReferrelCommission,
					@IvTotalTaskEarn as TotalTaskEarn,
					@IvTotalWorkCommission as TotalWorkCommission

		RETURN 0;

	ERR_INVALID_USER:
		SELECT	@ErrCode	= '12'
		,@UserMsg	= 'The Input User is not Valid' 
	GOTO EXIT_ERROR
	
	EXIT_ERROR:
		RETURN  0
	
END


