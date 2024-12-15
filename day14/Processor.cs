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

    internal void ResetQube(int algorithm)
    {
        qube = new Qube();
        if (algorithm ==1)
            qube.LoadValues(lines, 11, 7);
        else
            qube.LoadValues(lines, 101, 103);
    }

    internal void ProcessTask1(int algorithm)
    {
        ResetQube(algorithm);
        //steps
        int steps = algorithm switch
        {
            1 => 100,
            2 => 100,
            3 => 101 * 103 ,
            _ => 0
        };
        qube.ProcessSteps(steps, algorithm==3);
        long checksum = qube.CalculateChecksum();
        Console.WriteLine($"RESULT: {checksum}");
    }

}

public class Qube
{

    int sum = 0;
    int MemWidth = 0;
    int MemHeight = 0;
    List<Robot> Robots = new List<Robot>();
    int Steps = 0;


    internal void LoadValues(List<string> lines, int w, int h)
    {
        MemWidth = w;
        MemHeight = h;
        //Memory = new int[w, h];
        foreach (string line in lines)
        {
            //calculate robots
            string[] parts = line.Split(' ');
            string[] pos = parts[0].Substring(2).Split(',');
            string[] vel = parts[1].Substring(2).Split(',');
            //
            Robot r = new Robot()
            {
                X = int.Parse(pos[0]),
                Y = int.Parse(pos[1]),
                SpeedX = int.Parse(vel[0]),
                SpeedY = int.Parse(vel[1])
            };
            Robots.Add(r);
        }
    }

    internal long CalculateChecksum()
    {
        //check robits
        int qw = (MemWidth - 1) / 2;
        int qh = (MemHeight - 1) / 2;

        //calculate for q1
        int[] sumq = [0,0,0,0];
        foreach (Robot r in Robots)
        {
            int x = r.X;
            int y = r.Y;
            if (x < qw && y < qh) sumq[0]++;
            if (x > qw && y < qh) sumq[1]++;
            if (x < qw && y > qh) sumq[2]++;
            if (x > qw && y > qh) sumq[3]++;
        }

        return sumq[0] * sumq[1] * sumq[2] * sumq[3];
    }

    internal void ProcessSteps(int steps, bool tree)
    {
        //Print();
        for (int i = 0; i < steps; i++)
        {
            for (int r = 0; r < Robots.Count; r++)
            {

                Robots[r].CaculatePos(1, MemWidth, MemHeight);
            }
            Steps++;
            if (tree) if (ThisIsTree())
            {
                PrintMem(Steps);
                Console.WriteLine("");
            }
        }
    }

    private bool ThisIsTree()
    {
        char[,] mem = new char[MemWidth, MemHeight];
        for (int x = 0; x < MemWidth; x++)
            for (int y = 0; y < MemHeight; y++)
            {
                mem[x, y] = '.';
            }
        foreach (Robot r in Robots)
        {
            mem[r.X, r.Y] = '#';
        }
        bool tree = true;
        for (int  a = 0;  a < 24;  a++)
        {
            if (mem[a+40,48] != '#')
            {
                tree = false;
                break;
            }
            if (mem[a + 40, 80] != '#')
            {
                tree = false;
                break;
            }
        }
        return tree;

    }

    private void PrintMem(int step)
    {
        char[,] mem = new char[MemWidth, MemHeight];
        for (int x = 0; x < MemWidth; x++)
            for (int y = 0; y < MemHeight; y++)
            {
                mem[x, y] = '.';
            }
        foreach (Robot r in Robots)
        {
            mem[r.X,r.Y] = '#';
        }
        Console.WriteLine($"MEMORY step {step}");
        for (int y = 0; y < mem.GetLength(1); y++)
        {
            for (int x = 0; x < mem.GetLength(0); x++)
            {
                Console.Write(mem[x, y]);
            }
            Console.WriteLine();
        }
        Console.WriteLine($"MEMORY step {step}");

    }


    private void Print()
    {
        Console.WriteLine("Positions:");
        foreach (Robot r in Robots)
        {
            //write robot position
            Console.Write($":({r.X},{r.Y})");
        }
        Console.WriteLine();
    }
}

internal class Robot
{
    internal int X;
    internal int Y;
    internal int SpeedX;
    internal int SpeedY;

    internal void CaculatePos(int steps, int MemWidht, int MemHeight)
    {
        X += SpeedX * steps;
        Y += SpeedY * steps;
        //calcualte position that is within map
        X = X % MemWidht;
        Y = Y % MemHeight;
        if (X < 0) X += MemWidht;
        if (Y < 0) Y += MemHeight;
    }
}