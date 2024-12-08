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
        qube.Resize(lines.Count, lines[0].Length);
    }

    internal void ProcessTask1(int algorithm)
    {
        ResetQube();
        qube.LoadValues(lines);
        int sum = qube.CalculateAntiAntennas(algorithm);
        Console.WriteLine($"RESULT: {sum}");
    }

}

public class Qube
{
    int MapCols;
    int MapRows;

    public List<Antenna> antennas = new List<Antenna>();

    public void Resize(int x, int y)
    {
        MapCols = x;
        MapRows = y;
    }

    internal void LoadValues(List<string> lines)
    {
        for (int y = 0; y < lines.Count; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                int letter = lines[y][x];
                if (letter == '.') continue;
                Antenna? a = antennas.FirstOrDefault(a => a.letter == letter);
                if (a is not null)
                {
                    a.positions.Add((x, y));
                }
                else
                {
                    a = new Antenna();
                    a.letter = letter;
                    a.positions.Add((x, y));
                    antennas.Add(a);
                }
            }
        }

    }

    internal int CalculateAntiAntennas(int Algorithm)
    {
        //PrintAntennas();

        List<(int x, int y)> antiantennas = new List<(int x, int y)>();
        foreach (Antenna a in antennas)
        {
            a.GetAntiAntennas(MapCols, MapRows, Algorithm);
            antiantennas.AddRange(a.antiantennas);
        }
        return antiantennas.Distinct().Count();
    }

    private void PrintAntennas()
    {
        foreach (Antenna a in antennas)
        {
            Console.Write($"Antenna: {a.letter}");
            foreach ((int x, int y) pos in a.positions)
            {
                Console.Write($": {pos.x},{pos.y}");
            }
            Console.WriteLine();
        }
    }
}

public class Antenna
{
    public int letter;
    public List<(int x, int y)> positions = new List<(int x, int y)>();
    public List<(int x, int y)> antiantennas = new List<(int x, int y)>();
    int Cols;
    int Rows;

    internal void GetAntiAntennas(int cols, int rows, int algorithm)
    {
        Cols = cols;
        Rows = rows;
        antiantennas.Clear();
        //each with each
        for (int a = 0; a < positions.Count; a++)
        {
            for (int b = a + 1; b < positions.Count; b++)
            {
                //Console.Write($"CC: {letter} {positions[a]} {positions[b]} :");
                int dx = positions[b].x - positions[a].x;
                int dy = positions[b].y - positions[a].y;
                //
                switch (algorithm)
                {
                    case 1:
                        InArea1(a, b, dx, dy);
                        break;
                    case 2:
                        InArea2(a, b, dx, dy);
                        break;
                }

                //Console.WriteLine();
            }
        }
    }

    private void InArea2(int a, int b, int dx, int dy)
    {
        int p1x; int p1y;
        bool processing = true;
        p1x = positions[a].x;
        p1y = positions[a].y;
        antiantennas.Add((p1x, p1y));
        while (processing)
        {
            p1x = p1x - dx;
            p1y = p1y - dy;
            if (InArea(p1x, p1y))
            {
                antiantennas.Add((p1x, p1y));
                //Console.Write($":< {p1x},{p1y}");
            } else
            {
                processing = false;
            }
        }

        //
        int p2x, p2y;
        processing = true;
        p2x = positions[b].x ;
        p2y = positions[b].y ;
        antiantennas.Add((p2x, p2y));
        while (processing)
        {
            p2x = p2x + dx;
            p2y = p2y + dy;
            //check if in area
            if (InArea(p2x, p2y))
            {
                antiantennas.Add((p2x, p2y));
                //Console.Write($":> {p2x},{p2y}");
            }
            else
            {
                processing = false;
            }

        }
    }



    private void InArea1(int a, int b, int dx, int dy)
    {
        int p1x = positions[a].x - dx;
        int p1y = positions[a].y - dy;
        if (InArea(p1x, p1y))
        {
            antiantennas.Add((p1x, p1y));
            //Console.Write($": {p1x},{p1y}");
        }
        //
        int p2x = positions[b].x + dx;
        int p2y = positions[b].y + dy;
        //check if in area
        if (InArea(p2x, p2y))
        {
            antiantennas.Add((p2x, p2y));
            //Console.Write($": {p2x},{p2y}");
        }
    }

    private bool InArea(int xx, int yy)
    {
        return xx >= 0 && xx < Cols && yy >= 0 && yy < Rows;
    }
}