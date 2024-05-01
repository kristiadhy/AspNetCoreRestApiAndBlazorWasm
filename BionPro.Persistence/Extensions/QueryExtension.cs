using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Reflection;

namespace Persistence.Extensions;

public static class QueryExtension
{
    public static string DynamicConditionOfStringByQP<T>(T QP)
    {
        List<string> conditions = new List<string>();
        string whereByQP = string.Empty;

        PropertyInfo[] properties = typeof(T).GetProperties();
        foreach (PropertyInfo property in properties)
        {
            var value = property.GetValue(QP);

            if (value != null)
                conditions.Add($"@{property.Name}={value}");
        }

        if (conditions.Any())
        {
            whereByQP = $"" + string.Join(" ", conditions.ToArray());
        }

        return whereByQP;
    }

    public static string GenerateSPQuery<T>(T QPCondition, string prmSPName, string prmSPOrderBy)
    {
        string spSelect = $"EXEC {prmSPName}";
        string spWhere = DynamicConditionOfStringByQP(QPCondition);
        string spOrderBy = $"ORDER BY {prmSPOrderBy}";

        string spQuery = $"{spSelect} {spWhere} {spOrderBy}";

        return spQuery;
    }

    public static string DynamicConditionOfSqlParameterByQP<T>(T QP)
    {
        List<string> conditions = new List<string>();
        string whereByQP = string.Empty;

        PropertyInfo[] properties = typeof(T).GetProperties();
        foreach (PropertyInfo property in properties)
        {
            var value = property.GetValue(QP);

            if (value != null)
                conditions.Add($"@{property.Name}={value}");
        }

        if (conditions.Any())
        {
            whereByQP = $"" + string.Join(" ", conditions.ToArray());
        }

        return whereByQP;
    }

    public static SqlParameter[] CreateSQLParameterFromQP<T>(T QPCondition)
    {
        List<SqlParameter> lstSqlParam = new List<SqlParameter>();
        PropertyInfo[] properties = typeof(T).GetProperties();
        foreach (PropertyInfo property in properties)
        {
            var value = property.GetValue(QPCondition);

            if (value != null)
                lstSqlParam.Add(new SqlParameter($"@{property.Name}", value));
        }
        return lstSqlParam.ToArray();
    }

    public static async Task<List<T>> QueryProcedure<T>(AppDBContext dbContext, string procedureName, CancellationToken cancellationToken = default, params SqlParameter[] sqlParameters)
    where T : class
    {
        var query = $"EXEC {procedureName} {string.Join(", ", sqlParameters.Select(s => $"{s.ParameterName}"))}";

        return await dbContext.Set<T>()
            .FromSqlRaw(query, sqlParameters as object[])
            .ToListAsync(cancellationToken);
    }

}