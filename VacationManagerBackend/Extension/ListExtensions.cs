using System;
using System.Collections.Generic;
using System.Data;

namespace VacationManagerBackend.Extension
{
    public static class ListExtensions
    {
        public static DataTable ToDayDataTable(this List<DateTime> dates)
        {
            var table = new DataTable();
            table.Columns.Add("Day", typeof(DateTime));
            foreach (var date in dates)
            {
                var row = table.NewRow();
                row["Day"] = date.Date;
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
