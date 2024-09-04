using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chart_report_api.Helper
{
    public static class Queries
    {
        public const string ViewAndColumnNames = @"
                SELECT
                    v.table_name AS view_name,
                    ARRAY_AGG(c.column_name::text) AS columns
                FROM
                    information_schema.views v
                INNER JOIN
                    information_schema.columns c
                ON
                    v.table_name = c.table_name
                WHERE
                    v.table_schema NOT IN ('pg_catalog', 'information_schema')
                    AND c.table_schema = v.table_schema
                GROUP BY
                    v.table_name;";
        public const string FuncAndColumnNames = @"SELECT
            r.routine_name AS function_name,
            ARRAY_AGG(p.parameter_name::text) AS columns
        FROM
            information_schema.routines r
        LEFT JOIN
            information_schema.parameters p
        ON
            r.specific_name = p.specific_name
        WHERE
            r.routine_schema NOT IN ('pg_catalog', 'information_schema')
            AND r.routine_type = 'FUNCTION'
        GROUP BY
            r.routine_name;
        ";

    }
}