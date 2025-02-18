﻿using ComlineApp.Manager;
using ComlineApp.Services;
using ComLineCommon;
using System.Data;
using System.Diagnostics;

namespace Manosque.ServiceData
{
    public class ServiceData : ServiceDataBase, IServiceData
    {
        public ServiceData()
        {
            DataTable t = new();
            Query.CommandText = "select * from Absence";
            try
            {
                Adapter.Fill(t);
            }
            catch (Exception)
            {
                return;
            }
            foreach (DataRow row in t.Rows)
            {
                CronTools.JoursExclus.Add(new Absence { Date = DateOnly.FromDateTime((DateTime)row["Jour"]), Personne = row["Personne"] as Guid? });
            }
        }
        public override void Execute(ComlineData command)
        {
            // Init
            Command = command;

            // Pre Process : Build Query
            Before();

            // Execute builded query
            if (Command.TableName != "")
                try
                {
                    Adapter.Fill(Command.Results, Command.TableName);
                }
                catch (Exception ex)
                {
                    Command.Results.AddError($"ServiceData.cs.{new StackTrace(true).GetFrame(0)?.GetFileLineNumber()} : \n{Query.CommandText}\n {ex.Message}", ErrorCodeEnum.QueryError);
                    Command.ErrorCode = ErrorCodeEnum.QueryError;
                    return;
                }
            // Post Process
            //After();
        }

        //private void After()
        //{
        //}

        private void Before()
        {
            switch (Command.Verb)
            {
                case "Get":
                    switch (Command.Noun)
                    {
                        case "Version": Query.CommandText = $"select '{Global.VersionDatabase}' Version"; break;
                        case "Execution":
                            //if (Command.ContainsAllParameters("Emplacement", "Tache", "DateDebut"))
                            //{
                            //    Query.CommandText = QueryFactory.SelectExecution(Command);
                            //}
                            //else if (Command.ContainsAllParameters("Id", "Compute"))
                            //{
                            //    Query.CommandText = QueryFactory.SelectAndCompute(Command);
                            //}
                            if (Command.ContainsParameter("Id"))
                            {
                                Query.CommandText = QueryFactory.Select(Command);
                            }
                            else
                            {
                                Query.CommandText = QueryFactory.Select(Command);

                                //command.ErrorCode = ErrorCodeEnum.WrongParameter;
                                //Query.CommandText = "select 'Paramètre manquant !'";
                            }
                            break;
                        default: Query.CommandText = QueryFactory.Select(Command); break;
                    }
                    break;
                case "Add":
                    Query.CommandText = QueryFactory.Add(Command);
                    break;
                case "Update":
                    Query.CommandText = QueryFactory.Update(Command);
                    break;
                case "Remove":
                    Query.CommandText = QueryFactory.Remove(Command); break;
                case "New":
                    Query.CommandText = QueryFactory.Insert(Command);
                    break;
                case "Delete":
                    Query.CommandText = QueryFactory.Delete(Command); break;
                //case "Generate":
                //    if (Command.ContainsAllParameters("Tache", "DateDebut", "DateFin"))
                //    {
                //        Query.CommandText = $"select Id, Reference, Frequence from Tache where Id={Command.Parameters["Tache"].Replace('"', '\'')}";
                //        Command.TableName = "Tache";
                //    }
                //    else
                //    {
                //        Query.CommandText = "select'Missing parameters'";
                //        Command.ErrorCode = ErrorCodeEnum.WrongParameter;
                //    }
                //    break;
                case "Generate":
                    if (Command.Noun == "Execution")
                        Query.CommandText = QueryFactory.GenerateExecution(Command);
                    break;
                default:
                    Query.CommandText = $"select -1001 [Index], 'Service Data.cs.{new StackTrace(true).GetFrame(0)?.GetFileLineNumber()}: la requête {Command.QueryName} n''existe pas !' Libelle";
                    Command.TableName = "Error";
                    Command.ErrorCode = ErrorCodeEnum.ServiceData_UnexistedCommand;
                    break;
            }
        }
    }
}
