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
            int checksum = 0;
            qube.FindAllTracks();
            checksum = qube.MaxTrack1;
            Console.WriteLine($"RESULT 1: {checksum}");
            checksum = qube.MaxTrack2;
            Console.WriteLine($"RESULT 2: {checksum}");
        }
    }

}

public class Qube
{
    internal Position[,] MyMap = new Position[0, 0];
    internal int Cols;
    internal int Rows;
    internal int TrackCounter;
    internal int MaxTrack1;
    internal int MaxTrack2;
    internal List<(int, int)> Tops = new List<(int, int)>();

    internal void FindAllTracks()
    {
        TrackCounter = 0;
        MaxTrack1 = 0;
        MaxTrack2 = 0;
        Tops.Clear();
        for (int y = 0; y < Rows; y++)
            for (int x = 0; x < Cols; x++)
            {
                if (MyMap[x, y].Height == 0)
                {
                    Tops.Clear();
                    TrackCounter = 0;
                    StartSearch(x, y, 0);
                    MaxTrack1 += Tops.Distinct().Count();
                    MaxTrack2 += TrackCounter;// Tops.Count();
                    //Console.WriteLine($"Position {x},{y} Tracks found: {TrackCounter}");

                }

            }
    }

    internal void LoadValues(List<string> lines)
    {
        MyMap = new Position[lines.Count, lines[0].Length];
        Rows = lines.Count;
        Cols = lines[0].Length;
        for (int y = 0; y < lines.Count; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                MyMap[x, y] = new Position();
                MyMap[x, y].Height = lines[y][x] - '0';
            }
        }
    }

    private void StartSearch(int x, int y, int height)
    {
        //check if height is correct
        if (MyMap[x, y].Height != height) return;
        //check if height is 9
        if (height == 9)
        {
            //Console.WriteLine($"Track found at {x},{y}");
            Tops.Add((x, y));
            TrackCounter++;
            return;
        }

        //check up
        if (y > 0)
        {
            StartSearch(x, y - 1, height + 1);
        }
        //check down
        if (y < Rows - 1)
        {
            StartSearch(x, y + 1, height + 1);
        }
        //check right
        if (x < Cols - 1)
        {
            StartSearch(x + 1, y, height + 1);
        }
        //check left
        if (x > 0)
        {
            StartSearch(x - 1, y, height + 1);
        }

    }


    internal class Position
    {
        internal int Height;
    }
}
