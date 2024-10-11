CREATE FUNCTION dbo.GetNextOccurrence (
	@CRONExpression nvarchar(256)
	, @Now datetime
)
RETURNS [datetime]
AS EXTERNAL NAME Utilities.CronFunctions.GetNextOccurrence;