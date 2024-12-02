

internal class Processor
{
    List<string> lines = new List<string>();

    internal void ProcessTask1(string @filename)
    {
        int sum = 0;
        lines.Clear();
        foreach (string line in File.ReadLines(@filename))
        {
            lines.Add(line);

        }

        Console.WriteLine($"RESULT: {sum}");
    }

    internal void ProcessTask2(string @filename)
    {
        int sum = 0;
        lines.Clear();
        foreach (string line in File.ReadLines(@filename))
        {
            lines.Add(line);
        }


        Console.WriteLine($"RESULT: {sum}");
    }

}
