
/* ----------------------------------------------------------------------------
PROC:	UserInfo_Get
DESC:	Get the User Information By UserId.


HIST:	
	2021-10-09	Newaz		Created
NOTES:
------------------------------------------------------------------------------- */
ALTER PROCEDURE [dbo].[UserInfo_Get]
	@UserId				varchar(37),
	@ErrCode			varchar(2) = NULL OUTPUT,
	@UserMsg			varchar(200) = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	
		IF NOT EXISTS (SELECT 1 FROM tblUser WHERE UserId = @UserId)  
			GOTO ERR_INVALID_USER;
		
		SET	@ErrCode	= '00'
		SET	@UserMsg	= ''
		SELECT u.UserId,u.UserName,u.FullName,u.Email,u.Mobile,
		u.ImagePath,u1.UserName as ReferrelUserName,u.PackageId,
		ISNULL(p.PackageName,'N/A') as Package, 
		isnull(p.PackageDurationDays,'N/A') as Duration,
		case when u.PackageValidityDate is null then 'N/A' else FORMAT(u.PackageValidityDate,'dd MMM yyyy') end as Expire,u.CountryId,
		FORMAT(u.createdDate,'dd MMM yyyy') as JoinDate,u.IsSendEmail
		FROM tblUser u
		LEFT JOIN tblUser u1 on u.ReferrerId = u1.UserId
		LEFT JOIN tblPackage p on u.PackageId = p.PackageId
		where u.UserId = @UserId

		RETURN 0;


	ERR_INVALID_USER:
		SELECT	@ErrCode	= '12'
			,	@UserMsg	= 'Could not find User'
		GOTO EXIT_ERROR
	
	EXIT_ERROR:
		RETURN  0
	
END



