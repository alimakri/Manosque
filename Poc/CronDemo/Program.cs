using System;
using System.Collections.Generic;
using NCrontab;

class Program
{
    static void Main()
    {
        string cronExpression = "0 9 * * 1-5"; // Exemple d'expression cron
        DateTime start = new DateTime(2025, 2, 1);
        DateTime end = new DateTime(2025, 3, 1);

        List<DateTime> dates = GetCronDates(cronExpression, start, end);
        foreach (var date in dates)
        {
            Console.WriteLine(date.ToString("dd/MM/yyyy - dddd"));
        }
        Console.ReadLine();
    }

    static List<DateTime> GetCronDates(string cronExpression, DateTime start, DateTime end)
    {
        List<DateTime> dates = new List<DateTime>();
        CrontabSchedule schedule = CrontabSchedule.Parse(cronExpression);
        DateTime nextOccurrence = schedule.GetNextOccurrence(start);

        while (nextOccurrence <= end)
        {
            dates.Add(nextOccurrence);
            nextOccurrence = schedule.GetNextOccurrence(nextOccurrence);
        }

        return dates;
    }
}
