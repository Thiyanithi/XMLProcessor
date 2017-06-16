SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author      :  M Thiyanithi
-- Create date :  09.07.2017
-- Description :  Create Time stamp
-- =============================================

CREATE PROCEDURE CalculateTimeStamp
	@Hours int,
	@Minits int,
	@Seconds int

AS
BEGIN

	DECLARE @TimeStampValue int

	SET @TimeStampValue = 0

	IF(@TimeStampValue = 0)
	BEGIN

		SET @TimeStampValue = @Hours + @Minits + @Seconds
	END
	ELSE

		SET @TimeStampValue = 0

	RETURN @TimeStampValue

END
GO
