

internal class Processor
{
    List<Report> reports = new List<Report>();

    internal void ProcessTask1(string @filename)
    {
        int sum = 0;
        reports.Clear();
        foreach (string line in File.ReadLines(@filename))
        {
            //levels...
            string[] levels = line.Split(' ');
            //5 of them...
            Report report = new Report();
            foreach (string level in levels)
            {
                report.Levels.Add(int.Parse(level));
            }

            reports.Add(report);
        }

        foreach (Report report in reports)
        {
            if (report.IsSafe()) sum++;
        }

        Console.WriteLine($"RESULT: {sum}");
    }

    internal void ProcessTask2(string @filename)
    {
        int sum = 0;
        reports.Clear();
        foreach (string line in File.ReadLines(@filename))
        {
            //levels...
            string[] levels = line.Split(' ');
            //5 of them...
            Report report = new Report();
            foreach (string level in levels)
            {
                report.Levels.Add(int.Parse(level));
            }
            reports.Add(report);
        }

        foreach (Report report in reports)
        {
            if (report.IsSafe()) sum++;
            else if (report.CheckSubReports()) sum++;
        }

        Console.WriteLine($"RESULT: {sum}");
    }

}

internal class Report
{
    public List<int> Levels = new List<int>();

    internal bool IsSafe()
    {
        bool increasing = CheckIsIncresing();
        bool decreasing = CheckIsDecreasing();

        int maxDifference = GetMaxDifference();

        return (increasing || decreasing)
            &&
            (1 <= maxDifference  && maxDifference<=3);
    }

    private int GetMaxDifference()
    {
        //get max difference between levels
        int diff = 0;
        for (int i = 0; i < Levels.Count - 1; i++)
        {
            int current = Levels[i];
            int next = Levels[i + 1];
            int currentDiff = Math.Abs(current - next);
            if (currentDiff > diff)
            {
                diff = currentDiff;
            }
        }
        return diff;
    }

    private bool CheckIsDecreasing()
    {
        //check if levels are decreasing
        for (int i = 0; i < Levels.Count - 1; i++)
        {
            int current = Levels[i];
            int next = Levels[i + 1];
            if (current >= next)
            {
                return false;
            }
        }
        return true;
    }

    private bool CheckIsIncresing()
    {
        for (int i = 0; i < Levels.Count - 1; i++)
        {
            int current = Levels[i];
            int next = Levels[i + 1];
            if (current <= next)
            {
                return false;
            }
        }
        return true;
    }

    internal bool CheckSubReports()
    {
        for (int i = 0; i < Levels.Count; i++)
        {
            Report subReport = new Report();
            for (int j = 0; j < Levels.Count; j++)
            {
                if (j != i)
                {
                    subReport.Levels.Add(Levels[j]);
                }
            }
            //check if subreport is safe
            if (subReport.IsSafe())
            {
                return true;
            }
        }
        return false;
    }
}