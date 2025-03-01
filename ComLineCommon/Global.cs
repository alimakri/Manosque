namespace ComLineCommon
{
    public static class Global
    {
        // System
        public readonly static string ApplicationName = "Manosque";
        public readonly static string VersionSystem = "Version 2.0";
        public readonly static string Prompt = @"Execute-File -Name scenario9.ps1";

        // Data
        public readonly static string DatabaseName = "ManosqueBD";
        public readonly static string VersionDatabase = "Version Database 1.0";
        public readonly static List<string> SpecialParameters = ["Liste", "Select", "Return", "Mode", "Compute", "Filter", "Continue", "Service"];

        // WorkingDirectory
        public static string WorkingDirectory_ServiceApi = "";
        public static string WorkingDirectory_ServiceSystem = "";
        public static string WorkingDirectory_ServiceData = "";

        // ServiceData
        //public readonly static string Url = "https://makrisoft.net/";
        public readonly static string Url = "https://localhost:7250/";

        // QueryFactory
        public readonly static List<string> Services = ["System", "Data", "Api"];
        public readonly static Dictionary<string, string> ForeignKeys = new()
        {
            {"Absence.Personne", "Personne" },
            {"CompetencePersonne.Competence", "Competence" },
            {"CompetencePersonne.Personne", "Personne" },
            {"Emplacement.Emplacement", "Emplacement" },
            {"Execution.Execution", "Execution" },
            {"Execution.Tache", "Tache" },
            {"Execution.Personne", "Personne" },
            {"Execution.Emplacement", "Emplacement" },
            {"Message.Expediteur", "Personne" },
            {"Message.Destinataire", "Personne" },
            {"ProduitTache.Produit", "Produit" },
            {"ProduitTache.Tache", "Tache" },
            {"Action.Tache", "Tache" },
            {"Stock.Execution", "Execution" },
            {"Stock.Emplacement", "Emplacement" },
            {"Stock.Produit", "Produit" },
            {"Tache.Tache", "Tache" },
        };
    }
}