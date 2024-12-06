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

    internal void ProcessTask1()
    {

        qube.Readonly = false;
        qube.LoadValues(lines);

        while (qube.GuardHasLeft == false)
        {
            //qube.Print();
            qube.MakeStep();
        }


        int sum = qube.CountFieldsWithValue(2);
        Console.WriteLine($"RESULT: {sum}");
    }

    internal void ProcessTask2()
    {
        //insert values
        qube.Readonly = true;
        qube.LoadValues(lines);
        qube.RegisterPath = true;
        int sum2 = 0;

        for (int x = 0; x < qube.MapCols; x++)
            for (int y = 0; y < qube.MapRows; y++)
                if (qube.MyMap[x, y] == 0)
                    if (!(x == qube.GuardInit.x && y == qube.GuardInit.y))
                    {
                        //put an obstacle here
                        qube.MyMap[x, y] = 1;
                        //check loop
                        qube.ResetGuard();
                        bool loop = qube.CheckIfGuardLoops();
                        if (loop)
                        {
                            sum2++;
                            //Console.WriteLine($"Stone position: ({x},{y})");
                        }
                        qube.MyMap[x, y] = 0;
                    }

        Console.WriteLine($"RESULT: {sum2}");
        //1719
    }

}

public class Qube
{
    public int[,] MyMap;
    public (int x, int y) Guard;
    public bool GuardHasLeft = false;
    int Direction = 0; // 0 - up, 1 - right, 2 - down, 3 - left
    public int MapCols = 1;
    public int MapRows = 1;

    public bool Readonly { get; internal set; }
    public bool RegisterPath { get; internal set; }
    public (int x, int y) GuardInit { get; private set; }
    public int DirectionInit { get; private set; }

    public void MakeStep()
    {
        if (GuardHasLeft) return;
        if (NextStepOutside())
        {
            GuardHasLeft = true;
            return;
        }
        if (StepForwardPossible())
            StepForward();
        else
            TurnRight();
    }

    private void StepForward()
    {
        if (Direction == 0) Guard.y--;
        if (Direction == 1) Guard.x++;
        if (Direction == 2) Guard.y++;
        if (Direction == 3) Guard.x--;
        //
        if (!Readonly) MyMap[Guard.x, Guard.y] = 2;
    }

    private void TurnRight()
    {
        Direction++;
        if (Direction > 3) Direction = 0;
    }

    private bool StepForwardPossible()
    {
        if (Direction == 0
            && Guard.y > 0 && Empty(MyMap[Guard.x, Guard.y - 1])) return true;
        if (Direction == 1
            && Guard.x < MapCols - 1 && Empty(MyMap[Guard.x + 1, Guard.y])) return true;
        if (Direction == 2
            && Guard.y < MapRows - 1 && Empty(MyMap[Guard.x, Guard.y + 1])) return true;
        if (Direction == 3
            && Guard.x > 0 && Empty(MyMap[Guard.x - 1, Guard.y])) return true;
        return false;
    }

    private bool NextStepOutside()
    {
        if (Direction == 0 && Guard.y == 0) return true;
        if (Direction == 1 && Guard.x == MapCols - 1) return true;
        if (Direction == 2 && Guard.y == MapRows - 1) return true;
        if (Direction == 3 && Guard.x == 0) return true;
        return false;
    }

    private bool Empty(int value)
    {
        if (value == 1) return false;
        return true;
    }

    public Qube()
    {
        MyMap = new int[0, 0];
    }

    public void Resize(int x, int y)
    {
        MyMap = new int[x, y];
        MapCols = x;
        MapRows = y;
    }

    internal void LoadValues(List<string> lines)
    {
        //
        // Load values to Pole
        for (int y = 0; y < lines.Count; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                MyMap[x, y] = Map(lines[y][x]);
                if (MyMap[x, y] == 2)
                {
                    //Console.WriteLine($"Start position: {x},{y}");
                    if (Readonly) MyMap[x, y] = 0;
                    Guard = (x, y);
                }
            }
        }
        //remember init
        GuardInit = Guard;
        DirectionInit = Direction;

    }

    public void ResetGuard()
    {
        Guard = GuardInit;
        Direction = DirectionInit;
        GuardHasLeft = false;
    }

    private int Map(char v)
    {
        return v switch
        {
            '.' => 0,
            '#' => 1,
            '^' => 2,
            _ => 0
        };
    }

    internal void Print()
    {
        Console.WriteLine($"Map guard ({Guard.x}:{Guard.y})  :");

        for (int y = 0; y < MapRows; y++)
        {
            for (int x = 0; x < MapCols; x++)
            {
                Console.Write(MyMap[x, y]);
            }
            Console.WriteLine();
        }
    }

    internal int CountFieldsWithValue(int v)
    {
        int sum = 0;
        //count fields with value v
        for (int x = 0; x < MapCols; x++)
        {
            for (int y = 0; y < MapRows; y++)
            {
                if (MyMap[x, y] == v) sum++;
            }
        }
        return sum;
    }

    internal bool CheckIfGuardLoops()
    {
        //clean the path
        List<(int x, int y, int d)> path = new List<(int x, int y, int d)>();
        while (GuardHasLeft == false)
        {
            path.Add((Guard.x, Guard.y, Direction));
            MakeStep();
            if (GuardHasLeft) break;
            //check pattern loop
            if (path.Contains((Guard.x, Guard.y, Direction)))
            {
                //Console.WriteLine($"Loop detected! Steps:{path.Count}");
                return true;
            }
        }
        return false;
    }
}