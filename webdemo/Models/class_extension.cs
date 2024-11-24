using System;
using System.Data;


namespace webdemo.Models {
    public static class class_extension {
        public static T GetValue<T>(this DataRow row, string columnName) {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value) {
                try {
                    return (T)Convert.ChangeType(row[columnName], typeof(T));
                } catch {
                }
            }
            return default(T);
        }
    }
}


