/* ----------------------------------------------------------------------------
PROC:	UserInfo_Modify
DESC:	Update an user

PARAMS:
HIST:	
	2022-04-28 Newaz Created
NOTES:

------------------------------------------------------------------------------- */
ALTER PROCEDURE [dbo].[UserInfo_Modify]
	@UserId	varchar(37),	
	@FullName	varchar(200),	
	@Email 	udt_email,	
	@Mobile	varchar(15),	
	@ImagePath varchar(100),
	@CountryId	varchar(10),	
	@IsSendEmail     bit = 0,
	@ErrCode			varchar(2) = NULL OUTPUT,
	@UserMsg			varchar(200) = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT, XACT_ABORT ON;

	DECLARE @IvAmount decimal(18,4)
	DECLARE @IvCurrentPackageId int =null

	

	
	   --- ASSERT (Userid is Null.)
	IF @UserId IS NULL
	BEGIN
		GOTO ERR_REQUIRED_USERID
	END

		--- ASSERT (Fullname is Null.)
	IF @FullName IS NULL
		GOTO ERR_REQUIRED_FULLNAME

		--- ASSERT (Email is Null.)
	IF @Email IS NULL
		GOTO ERR_REQUIRED_EMAIL

		--- ASSERT (Mobile is Null.)
	IF @Mobile IS NULL
		GOTO ERR_REQUIRED_MOBILE
			
	--- ASSERT (Country is Null.)
	IF @CountryId IS NULL
		GOTO ERR_REQUIRED_COUNTRYID

	--- ASSERT (Email Already Exists)

		--- ASSERT (If Email Already Exists)
	IF  EXISTS (SELECT * FROM tblUser WHERE Email = @Email AND UserId <> @UserId)  
			GOTO ERR_EMAIL_EXISTS;

	 -- ASSERT (If Mobile Already Exists)
	IF  EXISTS (SELECT * FROM tblUser WHERE Mobile = @Mobile AND UserId <> @UserId)  
			GOTO ERR_MOBILE_EXISTS;

	--- ASSERT (If UserId Exists)
	IF NOT EXISTS (SELECT * FROM tblUser WHERE UserId = @UserId)  
			GOTO ERR_INVALID_USER;
	
	BEGIN TRANSACTION;  

	BEGIN TRY
		
		UPDATE tblUser
		SET
			Mobile = @Mobile,
			Email = @Email,
			CountryId = @CountryId,
			FullName = @FullName,
			IsSendEmail = @IsSendEmail,
			ImagePath = ISNULL(@ImagePath,ImagePath)
		WHERE UserId = @UserId
		
	    COMMIT TRANSACTION;
		SELECT	@ErrCode	= '00'
			,@UserMsg	= 'Updated Successfully' 
		RETURN 0;
	END TRY
	BEGIN CATCH
		 SELECT	@ErrCode	= '11'
			,@UserMsg	= 'An internal DB Error has occured' 
		 ROLLBACK TRANSACTION;
		 GOTO EXIT_ERROR

	END CATCH
	

	ERR_REQUIRED_USERID:
			SELECT	@ErrCode	= '12'
			,@UserMsg	= 'UserId cannot be null' 
		GOTO EXIT_ERROR

	ERR_REQUIRED_FULLNAME:
		SELECT	@ErrCode	= '12'
		,@UserMsg	= 'Provide FullName' 
	GOTO EXIT_ERROR

	ERR_REQUIRED_EMAIL:
		SELECT	@ErrCode	= '12'
		,@UserMsg	= 'Provide Email' 
	GOTO EXIT_ERROR

		ERR_REQUIRED_MOBILE:
		SELECT	@ErrCode	= '12'
		,@UserMsg	= 'Provide Mobile No' 
	GOTO EXIT_ERROR

	ERR_REQUIRED_COUNTRYID:
	SELECT	@ErrCode	= '12'
		,@UserMsg	= 'Provide Country' 
	GOTO EXIT_ERROR


	ERR_EMAIL_EXISTS:
		SELECT	@ErrCode	= '12'
		,@UserMsg	= 'This Email Already Exists for another user' 
	GOTO EXIT_ERROR

	ERR_MOBILE_EXISTS:
		SELECT	@ErrCode	= '12'
		,@UserMsg	= 'This Contact No Exists for another user' 
	GOTO EXIT_ERROR

	ERR_INVALID_USER:
		SELECT	@ErrCode	= '12'
		,@UserMsg	= 'Input User is not valid' 
	GOTO EXIT_ERROR

	EXIT_ERROR:
	RETURN 0
END



}
