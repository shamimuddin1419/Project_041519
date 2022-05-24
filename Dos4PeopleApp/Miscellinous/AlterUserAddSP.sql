USE [DOSDB]
GO
/****** Object:  StoredProcedure [dbo].[UserInfo_Add]    Script Date: 4/24/2022 2:38:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* ----------------------------------------------------------------------------
PROC:	UserInfo_Add
DESC:	Creates a password record for an User.

PARAMS:
HIST:	
	2021-10-09 Newaz Created
NOTES:
--exec UserInfo_Add @UserName=N'Sha4mim Uddin',@Email=N'sh4amim.cse19@gmail',@FullName=N'Sh4amim',@Password=N'123',@ReferrelUserName=N'Sh4am44im'

------------------------------------------------------------------------------- */
ALTER PROCEDURE [dbo].[UserInfo_Add]
	@UserName			udt_userName,
	@FullName      nvarchar(200),
	@Email				udt_email,
	@Mobile           nvarchar(15),
	@Password		nvarchar(20),
	@ReferrelUserName udt_userName = NULL,
	@CountryId varchar(30) = NULL,
	@IsSendEmail bit = 0,
	@UserId		   varchar(37) = NULL OUTPUT,
	@ErrCode			varchar(2) = NULL OUTPUT,
	@UserMsg			varchar(200) = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT, XACT_ABORT ON;
	
	DECLARE @ReferrelUserId  varchar(37)
	DECLARE @LoopCounter int;
	DECLARE @LoopUserId varchar(37)
					
	----Setting Referrel User
	SELECT @ReferrelUserId = UserId FROM tblUser WHERE UserName = ISNULL(@ReferrelUserName,'mdosadmin') 

    ----ASSERT If FullName is NULL
	IF COALESCE(@FullName,'')  =  '' GOTO ERR_REQUIRED_FULLNAME

	--- ASSERT (UserName is not already in use.)
	IF EXISTS (SELECT 1 FROM tblUser WHERE UserName = @UserName) GOTO ERR_DUPLICATE_USER


	--- ASSERT (Email is not already in use.)
	IF EXISTS (SELECT 1 FROM tblUser WHERE Email = @Email) GOTO ERR_DUPLICATE_EMAIL

	--- ASSERT (Mobile is not already in use.)
	IF EXISTS (SELECT 1 FROM tblUser WHERE   Mobile= @Mobile) GOTO ERR_DUPLICATE_MOBILE

	----Setting UserId
	SELECT @UserId = NEWID()


	BEGIN TRANSACTION;  

	BEGIN TRY
		
		INSERT INTO tblUser( UserId,UserName,FullName,Email,Mobile, Password, ReferrerId,[Status], CountryId,IsSendEmail, CreatedDate)
		VALUES ( @UserId,@UserName,@FullName,@Email,@Mobile,  @Password,@ReferrelUserId, 'A', @CountryId,@IsSendEmail, GETDATE());
		
		SET @LoopCounter = 1;
		SET @LoopUserId = @UserId
		WHILE(@LoopCounter <= 10)
		BEGIN
			DECLARE @ParentUserId varchar(37) = NULL
			SELECT @ParentUserId = ReferrerId FROM tblUser WHERE UserId = @LoopUserId
			IF @ParentUserId IS NOT NULL
			BEGIN
				INSERT INTO tblParentUserLevelMapping VALUES(@UserId,@ParentUserId,@LoopCounter)
			    SET @LoopUserId = @ParentUserId;
				SET @LoopCounter  = @LoopCounter + 1 
			END
			ELSE
			BEGIN
				BREAK
			END
		END
		;WITH cte as(
		SELECT @UserId UserId ,1 Level
		UNION ALL
		SELECT @UserId UserId,2 Level
		UNION ALL
		SELECT @UserId UserId,3 Level
		UNION ALL
		SELECT @UserId UserId,4 Level
		UNION ALL
		SELECT @UserId UserId,5 Level
		UNION ALL
		SELECT @UserId UserId,6 Level
		UNION ALL
		SELECT @UserId UserId,7 Level
		UNION ALL
		SELECT @UserId UserId,8 Level
		UNION ALL
		SELECT @UserId UserId,9 Level
		UNION ALL
		SELECT @UserId UserId,10 Level
		)
		INSERT INTO tblUserLevelCount
		 (
			[UserId]
           ,[Level]
		 )
		SELECT UserId,Level FROM CTE

	    COMMIT TRANSACTION;
		SELECT	@ErrCode	= '00'
			,@UserMsg	= 'User Saved Successfuly' 
		RETURN 0;
	END TRY
	BEGIN CATCH
		 SELECT	@ErrCode	= '11'
			,@UserMsg	= 'An internal DB Error has occured' 
		 ROLLBACK TRANSACTION;
		 GOTO EXIT_ERROR

	END CATCH
			
	ERR_REQUIRED_FULLNAME:
		SELECT	@ErrCode	= '12'
			,@UserMsg	= 'Please Provide Full Name' 
		GOTO EXIT_ERROR

	ERR_DUPLICATE_EMAIL:
		SELECT	@ErrCode	= '12'
			,@UserMsg	= 'This email ('+@Email+') already is in use' 
		GOTO EXIT_ERROR

		ERR_DUPLICATE_MOBILE:
		SELECT	@ErrCode	= '12'
			,@UserMsg	= 'This mobile ('+@Mobile+') already is in use' 
		GOTO EXIT_ERROR

	ERR_DUPLICATE_USER:
		SELECT	@ErrCode	= '12'
			,@UserMsg	= 'This User Name ('+@UserName+') already is in use' 
		GOTO EXIT_ERROR
	EXIT_ERROR:
	RETURN 0
END

