using System.Threading.Tasks.Sources;

internal class Processor2
{
    public Qube2 qube = new Qube2();
    List<string> maplines = new List<string>();
    List<string> movelines = new List<string>();

    internal void LoadFile(string @filename)
    {
        maplines.Clear();
        bool map = true;
        foreach (string line in File.ReadLines(@filename))
        {
            if (line == "")
            {
                map = false;
                continue;
            }
            if (map)
                maplines.Add(line);
            else
                movelines.Add(line);
        }
    }

    internal void ResetQube()
    {
        qube = new Qube2();
        qube.LoadValues(maplines, movelines);
    }

    internal void ProcessTask1(int algorithm)
    {
        ResetQube();
        if (algorithm == 1)
        {
            qube.RunTheMoves();
            long checksum = qube.CalculateChecksum();
            Console.WriteLine($"RESULT: {checksum}");
        }
    }

}

public class Qube2
{
    int[,] Mapa = new int[0, 0];
    int MapWidth = 0;
    int MapHeight = 0;
    List<int> Moves = new List<int>();

    public (int x, int y) RobPos;

    int[,] MapaSnap = new int[0, 0];
    (int x, int y) RobPosSnap;
    List<(int x, int y)> Pushed = new List<(int x, int y)>();

    int sum = 0;

    internal void LoadValues(List<string> maplines, List<string> movelines)
    {
        MapHeight = maplines.Count;
        MapWidth = maplines[0].Length;
        Mapa = new int[MapWidth * 2, MapHeight];
        //load values
        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                int type = maplines[y][x] switch
                {
                    '#' => 0,
                    '.' => 1,
                    'O' => 2,
                    '@' => 1,
                    _ => throw new Exception("Invalid character in map")
                };
                if ((type == 0) || (type == 1))
                {
                    Mapa[x * 2, y] = type;
                    Mapa[x * 2 + 1, y] = type;
                }
                if (maplines[y][x] == '@')
                {
                    RobPos.x = x * 2;
                    RobPos.y = y;
                }
                //NEW BLOCK TYPE
                if (type == 2)
                {
                    Mapa[x * 2, y] = 2;
                    Mapa[x * 2 + 1, y] = 3;
                }
            }
        }
        //update maxwidth
        MapWidth *= 2;
        //load moves
        foreach (string line in movelines)
        {
            foreach (char c in line)
            {
                Moves.Add(c switch
                {
                    '^' => 0,
                    'v' => 2,
                    '<' => 3,
                    '>' => 1,
                    _ => throw new Exception("Invalid character in moves")
                });
            }
        }


    }

    internal long CalculateChecksum()
    {
        sum = 0;
        //foreach item in map
        for (int x = 0; x < MapWidth; x++)
            for (int y = 0; y < MapHeight; y++)
            {
                if (Mapa[x, y] == 2)
                    sum += y * 100 + x;
            }
        return sum;
    }

    internal void RunTheMoves()
    {
        PrintMap();
        //foreach move
        foreach (var move in Moves)
        {
            //check if move is possible
            //remember the previous state
            CreateMapaAndRoPosSnap();
            var res = PushRock(move, RobPos.x, RobPos.y, 0);
            if (res == -1) RevertMapaAndRobPosFromSnap();
            //PrintMap();
            //break;
        }
        PrintMap();

    }

    private void RevertMapaAndRobPosFromSnap()
    {
        for (int x = 0; x < MapWidth; x++)
            for (int y = 0; y < MapHeight; y++)
                Mapa[x, y] = MapaSnap[x, y];
        RobPos = RobPosSnap;

    }

    private void CreateMapaAndRoPosSnap()
    {
        MapaSnap = new int[MapWidth, MapHeight];
        for (int x = 0; x < MapWidth; x++)
            for (int y = 0; y < MapHeight; y++)
                MapaSnap[x, y] = Mapa[x, y];
        RobPosSnap = RobPos;
    }

    private int PushRock(int dir, int x, int y, int iter)
    {
        if (iter==0) Pushed.Clear();
        if (Pushed.Contains((x, y))) return 1;
        //calculate new x and y based on dir
        int newx = x;
        int newy = y;
        switch (dir)
        {
            case 0:
                newy--;
                break;
            case 1:
                newx++;
                break;
            case 2:
                newy++;
                break;
            case 3:
                newx--;
                break;
        }
        //check if new x and y are in map
        if (newx < 0 || newx >= MapWidth || newy < 0 || newy >= MapHeight)
            return -1;
        //if not empty, push
        if ((Mapa[newx, newy] == 2) || (Mapa[newx, newy] == 3))
        {
            //check next based on direction
            if ((dir == 1) || (dir == 3))
            {
                var r = PushRock(dir, newx, newy, iter + 1);
                if (r == -1) return -1;
            }
            //UP or down
            if ((dir == 0) || (dir ==2))
            {
                var r = PushRock(dir, newx, newy, iter + 1);
                if (r == -1) return -1;
                //check left part of block
                if (Mapa[newx - 1, newy] == 2)
                {
                    var r2 = PushRock(dir, newx - 1, newy, iter + 1);
                    if (r2 == -1) return -1;
                }
                //check right part of block
                if (Mapa[newx + 1, newy] == 3)
                {
                    var r2 = PushRock(dir, newx + 1, newy, iter + 1);
                    if (r2 == -1) return -1;
                }
            }
        }
        //check newx and newy is empty
        if (Mapa[newx, newy] == 1)
        {
            //return 1;
            //
            if (iter == 0)
            {
                //moving the robot
                RobPos.y = newy;
                RobPos.x = newx;
                return 1;   //OK
            }
            else
            {
                //moving the rock
                //if (!Pushed.Contains((newx,newy))) { 
                    Mapa[newx, newy] = Mapa[x,y];
                    Mapa[x, y] = 1;
                    Pushed.Add((newx, newy));
                //};   
                return 1;
            }

        }

        return -1;
    }

    internal void PrintMap()
    {
        //print map
        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                if (x == RobPos.x && y == RobPos.y)
                    Console.Write('@');
                else
                    Console.Write(Map(Mapa[x, y]));
                //Console.Write(Map(Mapa[x, y]));
            }
            Console.WriteLine();
        }
    }

    private char Map(int v)
    {
        return v switch
        {
            0 => '#',
            1 => '.',
            2 => '[',
            3 => ']',
            _ => ' '
        };
    }
}

