using System.Threading.Tasks.Sources;

internal class Processor
{
    public Qube qube = new Qube();
    List<string> maplines = new List<string>();
    List<string> movelines = new List<string>();

    internal void LoadFile(string @filename)
    {
        maplines.Clear();
        bool map = true;
        foreach (string line in File.ReadLines(@filename))
        {
            if (line == "") {
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
        qube = new Qube();
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

public class Qube
{
    int[,] Mapa = new int[0, 0];
    int MapWidth = 0;
    int MapHeight = 0;
    List<int> Moves = new List<int>();

    public (int x, int y) RobPos;

    int sum = 0;

    internal void LoadValues(List<string> maplines, List<string> movelines)
    {
        MapHeight = maplines.Count;
        MapWidth = maplines[0].Length;
        Mapa = new int[MapWidth, MapHeight];
        //load values
        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                Mapa[x, y] = maplines[y][x] switch
                {
                    '#' => 0,
                    '.' => 1,
                    'O' => 2,
                    '@' => 1,
                    _ => throw new Exception("Invalid character in map")
                };
                if (maplines[y][x] == '@')
                {
                    RobPos.x = x;
                    RobPos.y = y;
                }
            }
        }
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
        for (int x=0; x<MapWidth; x++)
            for (int y=0; y<MapHeight; y++)
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
            var res = PushRock(move, RobPos.x, RobPos.y, 0);
            //PrintMap();
            //Console.WriteLine("next move");
        }
        PrintMap();

    }

    private int PushRock (int dir, int x, int y, int iter)
    {
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
        if (Mapa[newx, newy] == 2)
        {
            //check next
            var r = PushRock(dir, newx, newy, iter + 1);
            if (r == -1) return -1;
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
                Mapa[newx, newy] = 2;
                Mapa[x, y] = 1;
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
            2 => 'O',
            _ => ' '
        };
    }
}

