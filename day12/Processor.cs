internal class Processor
{
    public Qube qube = new Qube();
    List<string> lines = new List<string>();

    internal void LoadFile(string @filename)
    {
        lines.Clear();
        foreach (string line in File.ReadLines(@filename))
        {
            lines.Add(line);
        }
    }

    internal void ResetQube()
    {
        qube = new Qube();
        qube.LoadValues(lines);
    }

    internal void ProcessTask1(int algorithm)
    {
        ResetQube();
        if (algorithm == 1)
        {
            qube.CleanUpTheMemory();
            long checksum = qube.CalculateChecksum();
            Console.WriteLine($"RESULT: {checksum}");
        }
    }

}

public class Qube
{



    internal void CleanUpTheMemory()
    {
    }

    internal void LoadValues(List<string> lines)
    {
        Memory.Clear();

    }

    internal long CalculateChecksum()
    {
        return sum;
    }

}

