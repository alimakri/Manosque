using NCrontab;

namespace ComLineCommon
{
    public static class CronTools
    {
        public static readonly List<Absence> JoursExclus = [];
        public static List<DateTime> GetDates(string cronExpression, DateTime dateDebut, DateTime dateFin, Guid personneId)
        {
            List<DateTime> dates = [];
            CrontabSchedule schedule = CrontabSchedule.Parse(cronExpression);
            DateTime date = schedule.GetNextOccurrence(dateDebut);

            while (date <= dateFin)
            {
                if (!JoursExclus.Any(x => x.Date == DateOnly.FromDateTime(date) && (x.Personne == null || x.Personne == personneId)))
                    dates.Add(date);
                date = schedule.GetNextOccurrence(date);
            }
            return dates;
        }
        public static bool IsValid(string? cronExpression)
        {
            if (cronExpression == null) return false;
            return CrontabSchedule.TryParse(cronExpression) != null;
        }
    }
    public class Absence
    {
        public Guid? Personne { get; set; }
        public DateOnly Date { get; set; }
    }
}