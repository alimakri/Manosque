﻿using System.Data;
using System.Text;
using ComlineApp.Manager;
using ComlineServices;
using ComLineCommon;

namespace ComLineConsoleApp
{
    public enum DisplayModeEnum { Simple = 0, WithColumnNane = 1, WithIndex = 2 }

    public class ConsoleComline(CoreComline comline)
    {
        private readonly CoreComline Comline = comline;

        public void Launch()
        {
            Console.InputEncoding = Encoding.UTF8;
            bool fin = false;

            while (!fin)
            {
                // Saisie du prompt
                Console.ForegroundColor = ConsoleColor.Gray;
                if (ServiceSystem.Options["Service"]=="Api")
                    Console.Write($"{ServiceSystem.Options["Service"]}>{ServiceApi.RemoteService}> ");
                else
                    Console.Write($"{ServiceSystem.Options["Service"]}> ");
                string? prompt = "";
                if (comline.Command.Prompts.Count == 0)
                {
                    comline.Command.Prompts.Add(Console.ReadLine() ?? "");
                    prompt = comline.Command.Prompts[0]?.Trim();
                }
                else
                {
                    prompt = comline.Command.Prompts[0]?.Trim();
                    if (prompt != null && !prompt.StartsWith('#')) Console.WriteLine(comline.Command.Prompts[0]);
                }
                // Puis execution
                switch (prompt)
                {
                    // Commandes Console
                    case "local":
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        Console.ForegroundColor = ConsoleColor.White;
                        ServiceApi.Url = "https://localhost:7298/"; // 7298 - 7250
                        Console.WriteLine(ServiceApi.Url);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        break;
                    case "cls": Console.Clear(); break;
                    case "stop": comline.Command.Prompts.RemoveRange(1, comline.Command.Prompts.Count - 1); break;
                    case "exit": fin = true; break;
                    case "token":Console.WriteLine(ServiceApi.Token?.token); break;
                    case "help": Help(); break;
                    case "sep": Console.WriteLine("==============================================================================="); break;

                    // Commandes Comline
                    default: Execute(prompt); break;
                }

                // Commande traitée
                if (comline.Command.Prompts.Count > 0) comline.Command.Prompts.RemoveAt(0);
            }
        }

        private void Execute(string prompt)
        {
            // Commentaire
            if (string.IsNullOrEmpty(prompt) || prompt.StartsWith('#'))
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine((prompt + ' ').PadRight(100, '-'));
            }
            else
            {
                // Execute ---------------------------------
                comline.Command.Reset();
                comline.Execute();

                // Display ---------------------------------
                if (ServiceSystem.Options["DisplayMode"] != "Silent")
                {
                    foreach (DataTable table in comline.Command.Results.Tables) DisplayResults(table.TableName);
                }
                if (comline.Command.ModeDebug)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    var path = Path.Combine(Global.WorkingDirectory_ServiceData, comline.Command.TableName + ".xml");
                    Console.WriteLine($"-- --> Un fichier {path} a été généré.");
                }
            }
        }

        public static void Help()
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(@"Execute-File -Name d:\batch.cl");
            Console.WriteLine("Get-Client -OrderBy Name");
            Console.WriteLine("Get-Client -User1 ali@makrisoft.net OrderBy Index");
            Console.WriteLine("Get-Info -Type Accueil");
            Console.WriteLine("Get-Version -Service System");
        }

        void DisplayResults(string tableName)
        {
            //if (Comline.Command.ErrorCode == 0 && Comline.Command.Name == "Execute-File") return;
            //if (Comline.Command.ErrorCode == 0 && tableName == "Param") return;

            // Cosmetic
            ConsoleColor color = ConsoleColor.Yellow;
            DisplayModeEnum mode = DisplayModeEnum.WithColumnNane | DisplayModeEnum.WithIndex;

            switch (tableName)
            {
                case "Debug": color = ConsoleColor.Magenta; break;
                case "Error": color = ConsoleColor.DarkRed; break;
                case "Info": color = ConsoleColor.DarkGreen; break;
                default:
                    break;
            }

            // Title with table name
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = color;
            Console.Write(" {1} {0} ", tableName.PadRight(10, ' '), (char)16);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine();

            var dataTable = Comline.Command.Results.Tables[tableName];
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        if (column.ColumnName != "Index" && (mode & DisplayModeEnum.WithColumnNane) == DisplayModeEnum.WithColumnNane || column.ColumnName == "Index" && mode == (DisplayModeEnum.WithColumnNane | DisplayModeEnum.WithIndex))
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write($"{column.ColumnName}: ");
                        }
                        if (column.ColumnName != "Index" ||
                            column.ColumnName == "Index" && (mode & DisplayModeEnum.WithIndex) == DisplayModeEnum.WithIndex)
                        {
                            Console.ForegroundColor = color;
                            Console.WriteLine($"{row[column]}");
                        }
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
