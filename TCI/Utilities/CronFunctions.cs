using NCrontab;
using System;
using System.Data.SqlTypes;



public partial class CronFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction(IsDeterministic = true, IsPrecise = true)]
    public static SqlDateTime GetNextOccurrence(SqlString cronExpression, SqlDateTime now)
    {
        var nowDateTime = now.IsNull ? DateTime.Now : (DateTime)now;
        var cronParser = CrontabSchedule.Parse(cronExpression.ToString());

        return new SqlDateTime(cronParser.GetNextOccurrence(nowDateTime));
    }
}
