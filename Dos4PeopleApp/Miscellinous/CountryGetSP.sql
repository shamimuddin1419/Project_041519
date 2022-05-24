/* ----------------------------------------------------------------------------
PROC:	Country_Get
DESC:	Get the Countries


HIST:	
	2022-04-24	Newaz		Created
NOTES:
------------------------------------------------------------------------------- */
CREATE PROCEDURE [dbo].[Country_Get]
	@ErrCode			varchar(2) = NULL OUTPUT,
	@UserMsg			varchar(200) = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
		SET	@ErrCode	= '00'
		SET	@UserMsg	= ''

		SELECT 
			 id,text
		FROM tblCountry

		RETURN 0;
	
	EXIT_ERROR:
		RETURN  0
	
END

