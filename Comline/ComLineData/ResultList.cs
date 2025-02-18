﻿using System.Data;

namespace ComlineApp.Manager
{
    public class ResultList : DataSet
    {
        #region Add Errors
        public void AddError(string libelle, ErrorCodeEnum code)
        {
            // CreateTableError
            if (!Tables.Contains("Error"))
            {

                DataTable table = new DataTable("Error");
                // Index
                DataColumn indexColumn = new DataColumn("Index", typeof(int));
                indexColumn.AutoIncrement = true;
                indexColumn.AutoIncrementSeed = 1;
                indexColumn.AutoIncrementStep = 1;
                table.Columns.Add(indexColumn);
                // Code
                table.Columns.Add("Code", typeof(int));
                // Libelle
                table.Columns.Add("Libelle", typeof(string));

                Tables.Add(table);
            }
            // Add row
            var tableError = Tables["Error"];
            if (tableError != null)
            {
                DataRow newRow = tableError.NewRow();
                newRow["Libelle"] = libelle;
                newRow["Code"] = code;
                tableError.Rows.Add(newRow);
            }
        }
        #endregion

        #region Add Infos

        public void AddInfo(string libelle, string tableName)
        {
            // CreateTableInfo
            if (!Tables.Contains(tableName))
            {
                DataTable table = new DataTable(tableName);
                // Index
                DataColumn indexColumn = new DataColumn("Index", typeof(int));
                indexColumn.AutoIncrement = true;
                indexColumn.AutoIncrementSeed = 1;
                indexColumn.AutoIncrementStep = 1;
                table.Columns.Add(indexColumn);
                // Libellé
                table.Columns.Add("Libelle", typeof(string));

                Tables.Add(table);
            }

            // Add row
            var tableInfo = Tables[tableName];
            if (tableInfo != null)
            {
                DataRow newRow = tableInfo.NewRow();
                newRow["Libelle"] = libelle;
                tableInfo.Rows.Add(newRow);
            }
        }
        public void AddInfos(string[] lignes, string tableName)
        {
            foreach(string ligne in lignes) AddInfo(ligne, tableName);
        }
        #endregion
    }
}
