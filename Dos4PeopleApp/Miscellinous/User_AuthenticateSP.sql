/* ----------------------------------------------------------------------------
PROC:	User_Autehticate
DESC:	Authenticates the User.

	o Specifed user exists and is active.
	o Authenticates the User's password.

HIST:	
	2021-10-09	Newaz		Created
NOTES:
------------------------------------------------------------------------------- */
ALTER PROCEDURE [dbo].[User_Autehticate]
	@UserName				udt_UserName,
	@Password		nvarchar(20),
	@ErrCode			varchar(2) = NULL OUTPUT,
	@UserMsg			varchar(200) = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @Status char(1),
					@IvUserId varchar(37) = NULL;

		SELECT @IvUserId = UserId ,@Status = Status FROM tblUser where (UserName = @UserName or Email = @UserName or Mobile=@UserName) and Password = @Password;
		IF @IvUserId is NULL
			GOTO ERR_INVALID_USER;
		IF @Status <> 'A'
			GOTO ERR_USER_STATUS;
		
		SET	@ErrCode	= '00'
		SET	@UserMsg	= ''
		SELECT u1.UserId,u1.UserName,u1.FullName,u1.Email,u1.ImagePath,isnull(u2.UserName,'mdosadmin') as ReferrelUserName 
		,isnull(p.PackageName,'N/A') Package
		FROM tblUser u1
		left join tbluser u2 on u1.ReferrerId = u2.UserId
		left join tblPackage p on u1.PackageId = p.PackageId
		where u1.UserId = @IvUserId

		RETURN 0;


	ERR_INVALID_USER:
		SELECT	@ErrCode	= '12'
			,	@UserMsg	= 'UserName or Password is not valid'
		GOTO EXIT_ERROR

	ERR_USER_STATUS:
		SELECT	@ErrCode	= '12'
			,	@UserMsg	= 'The user is not in an active state.'
		GOTO EXIT_ERROR
	
	EXIT_ERROR:
		RETURN  0
	
END


