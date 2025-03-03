using ComLineCommon;
using ComLineData;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Text;


namespace ComlineApp.Services
{
    public static class QueryFactory
    {
        private static string SelectClause = "*";
        private static string OutputClause = "";
        private static string JoinClause = "";
        private static string AvgClause = "";
        private static string WhereClause = "";

        private static string ParameterListe = "";
        private static ComlineData Command = new();
        private static StringBuilder Result = new();

        #region Queries

        #region SELECT
        public static string Select(ComlineData command)
        {
            // Variables
            Command = command;
            Command.TableName = Command.Noun;
            Command.Filter = "";
            Result = new();
            SelectClause = "*";

            // Factory
            PrepareQuery();

            if (!string.IsNullOrEmpty(command.Filter))
                DirectSelect();
            else
                FactorySelect();

            // ModeDebug
            if (command.ModeDebug)
            {
                command.Results.AddInfo(Result, "Info");
                command.Results.AddInfo(command.Prompts[0], "Info");
            }
            return Result.ToString();
        }
        private static void PrepareQuery()
        {
            ParameterListe = ""; Command.ModeDebug = false;
            List<string> cles = new(Command.Parameters.Keys); string val = "";
            foreach (var cle in cles)
            {
                val = Command.Parameters[cle].Item2.Replace('\"', '\'');
                Command.Parameters[cle] = new Tuple<string, string>(Command.Parameters[cle].Item1, val);

                // -Return OnlyCount 
                if (cle == "Return" && val.Trim('"') == "'OnlyCount'") SelectClause = "Count(*) as count";
                // -Mode Debug 
                else if (cle == "Mode") Command.ModeDebug = val.Trim('"') == "Debug";

                // Convert DateOnly
                else if (DateOnly.TryParse(val.Trim('\''), out _)) { Command.Parameters[cle] = new Tuple<string, string>($"CAST({cle} as Date)", $"CONVERT(Date, '{val.Trim('\'')}', 103)"); }
                // Convert DateTime
                else if (DateTime.TryParse(val.Trim('\''), out _)) { Command.Parameters[cle] = new Tuple<string, string>($"CAST({cle} as DateTime)", $"CONVERT(DateTime, '{val.Trim('\'')}', 103)"); }
                // Enum avec @
                else if (val.StartsWith('@')) Result.Append($@"Declare {val} int; select {val} = e.valeur from Enum e where e.reference='{val.Replace("@", "")}';");

                // -Foreign Key avec ^
                else if (val == "^") { Result.Append($@"Declare @id_{cle} nvarchar(50); select @id_{cle}=id  from TableId where Reference='{cle}';"); Command.Parameters[cle] = new Tuple<string, string>(cle, $"@id_{cle}"); }
                // -Foreign Key simple
                else if (Global.ForeignKeys.ContainsKey($"{Command.Noun}.{cle}"))
                {
                    Result.Append($@"
                                Declare @id_{cle} nvarchar(50);");
                    if (val.ToUpper() == "NULL")
                        Result.Append($@"select @id_{cle}=NULL;");
                    else if (Guid.TryParse(val.Trim('\''), out _))
                        Result.Append($@"select @id_{cle}={val};");
                    else
                        Result.Append($@"
                                select @id_{cle}=id  from {Global.ForeignKeys[$"{Command.Noun}.{cle}"]} where Reference={val};");
                    Command.Parameters[cle] = new Tuple<string, string>(cle, $"@id_{cle}");
                }
                // -Liste 
                else if (cle == "Liste") ParameterListe = val;
                // -Select
                else if (cle == "Select") { SelectClause = val.Trim('"').Trim('\''); JoinClause = ""; Join(Command); }
                // -Filter
                else if (cle == "Filter") Command.Filter = val.Trim('"').Trim('\'');
                // -Compute
                else if (cle == "Compute")
                    AvgClause = $@"
                           DECLARE @avg decimal(18,2);
                           DECLARE @somme int
                           DECLARE @compte int
                           DECLARE @statut int

                           select @avg = AVG(Avancement), @somme=SUM(Statut), @compte=COUNT(*)  from Execution where Execution = @id;

                           IF @somme < 0
	                           Select @statut = Valeur from Enum where Reference='Empechement'
                           ELSE IF @somme=@compte
	                           Select @statut = Valeur from Enum where Reference='AFaire'

                           update Execution set Avancement = @avg, Statut=@statut where id=@id;";
            }
            // Clean Special Parameters
            var keysToRemove = Command.Parameters.Keys.Where(key => Global.SpecialParameters.Contains(key)).ToList();
            keysToRemove.ForEach(key => Command.Parameters.Remove(key));
        }
        private static void DirectSelect()
        {
            switch (Command.Filter)
            {
                case "ListeSites":
                    if (!Command.ContainsAllQueryParameters("Personne", "DateDebut"))
                    {
                        Command.ErrorCode = ErrorCodeEnum.WrongParameter;
                        Command.TableName = "Error";
                        Result.Append($@"
                                      select 'Personne et DateDebut sont nécessaires !'");
                    }
                    else if (Command.ContainsOneOfExtraParameters("Select"))
                    {
                        Command.ErrorCode = ErrorCodeEnum.WrongParameter;
                        Command.TableName = "Error";
                        Result.Append($@"select 'Select n''est pas autorisé !'");
                    }
                    else
                        Result.Append($@"
                                select x.Reference, x.Id, x.Emplacement, e.Reference, x.Statut, t.Reference tache from Execution x 
                                    inner join Emplacement e on x.Emplacement=e.Id
                                    left join Tache t on x.tache=t.Id
                                    where ((@id_Execution is null and x.Execution is NULL) or (x.Execution = @id_Execution)) and CAST(DateDebut as Date) = CONVERT(Date, {Command.Parameters["DateDebut"].Item2}, 103) and Personne=@id_Personne");
                    break;
                case "TopTache":
                    if (!Command.ContainsAllQueryParameters("Personne", "Emplacement"))
                    {
                        Command.ErrorCode = ErrorCodeEnum.WrongParameter;
                        Command.TableName = "Error";
                        Result.Append($@"
                                      select 'Personne et Emplacement sont nécessaires !'");
                    }
                    else
                        Result.Append($@"
                                select {SelectClause} from Execution inner join Tache on Execution.Tache=Tache.Id where Emplacement={Command.Parameters["Emplacement"].Item2.Replace('"', '\'')} and Personne={Command.Parameters["Personne"].Item2} and Tache.Tache IS NULL;");
                    break;
                case "SousTaches":
                    if (!Command.ContainsAllQueryParameters("Personne", "Emplacement", "Tache"))
                    {
                        Command.ErrorCode = ErrorCodeEnum.WrongParameter;
                        Command.TableName = "Error";
                        Result.Append($@"
                                      select 'Personne, Tache et Emplacement sont nécessaires !'");
                    }
                    else
                        Result.Append($@"
                                DECLARE @id uniqueidentifier;
                                select @id = Execution.id from Execution inner join Tache on Execution.Tache=Tache.Id where Emplacement={Command.Parameters["Emplacement"]} and Personne={Command.Parameters["Personne"]} and Execution.Tache={Command.Parameters["Tache"]};
                                select {SelectClause} from Execution inner join Tache on Execution.Tache=Tache.Id where Emplacement={Command.Parameters["Emplacement"]} and Personne={Command.Parameters["Personne"]} and Execution.Tache={Command.Parameters["Tache"]};
                                Update TableId set id=@id where Reference= '{Command.TableName}';
                                if @@ROWCOUNT = 0 
                                    insert TableId (Reference, id) values('{Command.TableName}', @id);");
                    break;
            }
        }
        private static void FactorySelect()
        {
            // whereClause
            WhereClause = string.Join(" and ", Command.Parameters.Keys.Select(key => $"{Command.Parameters[key].Item1}={Command.Parameters[key].Item2}")).Replace("=NULL", " IS NULL ");
            WhereClause = string.IsNullOrEmpty(WhereClause) ? "" : $" where {WhereClause}";
            // Query
            Result.Append($@"
                            DECLARE @id uniqueidentifier;
                            select @id=id from {Command.TableName} {WhereClause};
                            if @@ROWCOUNT=1
                                BEGIN
                                Update TableId set id=@id where Reference= '{Command.TableName}';
                                if @@ROWCOUNT = 0 
                                    insert TableId (Reference, id) values('{Command.TableName}', @id);
                                END
                            else
                                delete TableId where Reference='{Command.TableName}';
                            {AvgClause}");

            if (!string.IsNullOrEmpty(ParameterListe))
            {
                // Query with tableList
                JoinClause = $@"inner join {ParameterListe} on {ParameterListe}.id = x.{ParameterListe}";
                Result.Append($@"
                            select {SelectClause} from {Command.TableName} x {JoinClause} {WhereClause}");
                Command.TableName = ParameterListe;
            }
            else
            {
                // Query normal
                Result.Append($@"
                            select {SelectClause} from {Command.TableName} x {WhereClause}");
            }
        }
        #endregion

        #region Insert Update Delete...
        public static string Insert(ComlineData command)
        {
            // Variables
            Command = command;
            var dico = command.Parameters;
            command.TableName = command.Noun;
            Result = new();

            // OutputClause
            OutputClause = dico.ContainsKey("Reference") ?
                "output inserted.Id, inserted.reference into @IDs(ID, Reference)" :
                "output inserted.Id into @IDs(ID)";

            // Factory
            PrepareQuery();

            Result.Append($@"
                DECLARE @IDs TABLE(ID uniqueidentifier, Reference nvarchar(50));" +
                $@"
                insert {command.TableName} ({string.Join(", ", dico.Keys)}) {OutputClause} values ({string.Join(", ", dico.Values.Select(x => x.Item2))});
                select * from {command.TableName} p inner join @IDs t on p.id=t.Id");

            // ModeDebug
            if (command.ModeDebug)
            {
                command.Results.AddInfo(Result, "Info");
                command.Results.AddInfo(command.Prompts[0], "Info");
            }

            // Result
            return Result.ToString();
        }
        public static string Delete(ComlineData command)
        {
            // Variables
            command.TableName = command.Noun;

            if (command.Noun == "All")
                Result.Append(@"
                        delete CompetencePersonne;
                        Delete ProduitTache;
                        Delete Message;
                        Delete Enum;
                        Delete Stock;
                        Delete Reponse;
                        Delete Action;
                        Delete Execution;
                        Delete Tache;
                        Delete Produit;
                        Delete Emplacement;
                        Delete Absence;
                        Delete Personne;
                        Delete Competence;
                        Delete TableId;
                        select 'Plus de données'");
            else
                Result.Append($"" +
                    $"delete {command.Noun}; " +
                    $"select count(*) n from {command.Noun}");

            command.TableName = "Info";
            // ModeDebug
            if (command.ModeDebug)
            {
                command.Results.AddInfo(Result, "Info");
                command.Results.AddInfo(command.Prompts[0], "Info");
            }
            return Result.ToString();
        }
        public static string Remove(ComlineData command)
        {
            // Variables
            var dico = command.Parameters;
            var tableName = command.Noun;
            command.TableName = command.Noun;

            // Remove factory Part 1
            string result = "DECLARE @IDs TABLE(ID uniqueidentifier, Reference nvarchar(50));";
            var tableName2 = dico.FirstOrDefault().Value.Item2;
            if (tableName2 == null) return "select 'Table non trouvée !'";
            tableName = $"{tableName}{tableName2}";
            result += $"Declare @id_{tableName2} nvarchar(50); select @id_{tableName2}=id  from {tableName2} where Reference='{dico[tableName2].Item2.Trim('\"')}';";
            var name = dico[tableName2].Item2;
            dico[tableName2] = new Tuple<string, string>(tableName2, $"@id_{tableName2}");

            // ModeDebug
            if (command.ModeDebug)
            {
                command.Results.AddInfo(Result, "Info");
                command.Results.AddInfo(command.Prompts[0], "Info");
            }

            // Remove factory Part 2
            return result +
                $@"delete {tableName} where {tableName2}={dico[tableName2]};
                   select '{tableName} for {name} deleted'";
        }
        public static string Add(ComlineData command)
        {
            // Variables
            var dico = command.Parameters;
            var tableName1 = command.Noun;
            var tableName2 = command.Parameters.Where(p => p.Value.Item2 == "^").FirstOrDefault().Key;
            command.TableName = command.Noun;

            // Prepare
            PrepareQuery();

            // Add factory
            if (tableName2 == null)
            {
                Result.Append($"select 'Pas de table de référence. Voir ^ !'");
                command.ErrorCode = ErrorCodeEnum.UnExistedTable;
                command.TableName = "Error";
            }
            else if (!command.Parameters.ContainsKey("Reference"))
            {
                Result.Append($"select 'Pas de paramètre référence !'");
                command.ErrorCode = ErrorCodeEnum.UnExistedTable;
                command.TableName = "Error";
            }
            else
                Result.Append($@"
                            DECLARE @id uniqueidentifier;
                            select @id=id from {tableName1} where Reference={dico["Reference"]}
                            insert {tableName1}{tableName2} ({tableName1}, {tableName2}) 
                                values (
                                 @id,
                                 (SELECT id FROM TableID WHERE Reference = '{tableName2}'));
                                select * from {tableName1}{tableName2};");
            // ModeDebug
            if (command.ModeDebug)
            {
                command.Results.AddInfo(Result, "Info");
                command.Results.AddInfo(command.Prompts[0], "Info");
            }

            return Result.ToString();
        }
        public static string Update(ComlineData command)
        {
            // Variables
            var dico = command.Parameters;
            var tableName = command.Noun;
            command.TableName = command.Noun;

            // Prepare
            PrepareQuery();

            // Update factory Part 1
            OutputClause = dico.ContainsKey("Reference") ?
                "output inserted.Id, inserted.reference into @IDs(ID, Reference)" :
                "output inserted.Id into @IDs(ID)";

            // ModeDebug
            if (command.ModeDebug)
            {
                command.Results.AddInfo(Result, "Info");
                command.Results.AddInfo(command.Prompts[0], "Info");
            }

            return Result.Append($@"
                DECLARE @IDs TABLE(ID uniqueidentifier, Reference nvarchar(50));
                DECLARE @id uniqueidentifier;
                select @id=id from TableId where Reference='{tableName}';
                update {tableName} set {string.Join(", ", dico.Select(item => $"{item.Key}={item.Value}"))} {OutputClause} where id=@id
                select * from {tableName} p inner join @IDs t on p.id=t.Id").ToString();
        }
        #endregion

        #region Autres
        public static string GenerateExecution(ComlineData command)
        {
            // Variables
            var dico = command.Parameters;
            var cron = dico["Frequence"].Item2.Trim('\'');
            DateTime date1, date2;

            // Les dates
            if (dico.ContainsKey("DateDebut")) DateTime.TryParseExact(dico["DateDebut"].Item2.Trim('\''), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date1); else date1 = DateTime.Now;
            if (dico.ContainsKey("DateFin")) DateTime.TryParseExact(dico["DateFin"].Item2.Trim('\''), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date2); else date2 = date1.AddDays(7);
            _ = Guid.TryParse(dico["Personne"].Item2.Trim('"'), out Guid idPersonne);
            var dates = CronTools.GetDates(cron, date1, date2, idPersonne);

            // Prepare
            PrepareQuery();

            // Generate factory
            Result.Append($@"
                        DECLARE @IDs TABLE(ID uniqueidentifier, Reference nvarchar(50));
                        INSERT INTO Execution (Tache, Emplacement, DateDebut, DateFin, Personne, Statut, Type, Avancement) values ");

            // Construire les valeurs
            List<string> valuesList = [];
            foreach (var date in dates)
                valuesList.Add($@"
                                    (
                                        (SELECT id FROM Tache WHERE Id = @id_Tache),
                                        (SELECT id FROM Emplacement WHERE Id = @id_Emplacement),
                                        '{date:dd-MM-yyyy HH:mm:ss}',
                                        '{date.AddHours(2):dd-MM-yyyy HH:mm:ss}', -- Fin après 2h
                                        (SELECT id FROM Personne WHERE Id = @id_Personne),
                                        1, 
                                        (SELECT valeur FROM Enum WHERE Reference = '{dico["Type"].Item2.Replace("@", "")}'),
                                        0 
                                    )");

            command.TableName = "Execution";

            return Result.Append($@"
                    {string.Join(",\n", valuesList)};
                    SELECT * FROM Execution WHERE Tache = {dico["Tache"].Item2.Trim('"')};").ToString();
        }
        #endregion

        #endregion

        #region Méthodes
        private static void Join(ComlineData command)
        {
            var columns = SelectClause
                .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Contains('.') ? x : $"x.{x}");

            var columnsSelect = columns
                .Where(x => !x.StartsWith("x."))
                .Select(x => x.Split('.')[0])
                .Distinct();

            JoinClause = string.Join(" ", columnsSelect.Select(col => $"left join {col} on {col}.Id = x.{col}"));
            SelectClause = string.Join(',', columns);
        }
        #endregion
    }
}
