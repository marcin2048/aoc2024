

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
            qube.Process();
            long checksum = qube.CalculateChecksum();
            Console.WriteLine($"RESULT: {checksum}");
            List<int> values = qube.Tiles.Select(t => t.x * 1000 + t.y ).ToList();
            int cnt = values.Distinct().Count() + 1;
            Console.WriteLine($"DISTINCT: {cnt}");
        }
    }

}

public class Qube
{
    int[,] MyMap = new int[0, 0];

    int[,] MapDist = new int[0, 0];

    int MapWidth = 0;
    int MapHeight = 0;
    int sum = 0;
    int lowestSum = 0;

    public Move MinDist = new Move(0, 0, 0);

    public (int x, int y) StartPos;
    public (int x, int y) EndPos;
    public Move Reindeer = new Move(0, 0, 0);

    public List<Move> Tiles = new List<Move>();



    internal void LoadValues(List<string> lines)
    {
        MapHeight = lines.Count;
        MapWidth = lines[0].Length;
        MyMap = new int[MapWidth, MapHeight];
        MapDist = new int[MapWidth, MapHeight];
        //load values to mymap
        for (int y=0; y<MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                MyMap[x, y] = MapPoint(lines[y][x]);
                switch (lines[y][x])
                {
                    case 'S': StartPos = (x, y); break;
                    case 'E': EndPos = (x, y); break;
                }
                MapDist[x, y] = 0;
            }
        }
        Reindeer = new Move(StartPos.x, StartPos.y, 1);
        MinDist = new Move(StartPos.x, StartPos.y, 1);
        sum = 0;
    }

    private int MapPoint(char v)
    {
        return v switch
        {
            '.' => 0,
            '#' => 1,   //wall
            'S' => 0,
            'E' => 0,
            _ => throw new Exception("Invalid character"),
        };
    }

    internal long CalculateChecksum()
    {
        return lowestSum;
    }

    internal void Process()
    {
        //move reindeer
        List<Move> moves = new List<Move>();
        moves.Add(Reindeer);//init Position
        MakeMove(ref moves, true);
    }

    private void MakeMove(ref List<Move> moves, bool checkTurn)
    {
        //PrintDist();
        if (MapDist[Reindeer.x, Reindeer.y] == 0) MapDist[Reindeer.x, Reindeer.y] = sum;// dist;
        if (MapDist[Reindeer.x, Reindeer.y] + 1001 < sum )
        {
            return;
        }
        MapDist[Reindeer.x, Reindeer.y] = sum;

        //check points if already not too much
        if (lowestSum > 0) if (sum > lowestSum)
        {
            return;
        }
        //bezpiecznik
        //66404
        if (sum > 66404)
        {
            return;
        }

        //check END
        if (Reindeer.x == EndPos.x && Reindeer.y == EndPos.y  )
        {
            Console.WriteLine($"This is end!  lowest: {lowestSum} new: {sum}");
            if (lowestSum == 0) lowestSum = sum;
            if (sum < lowestSum)
            {
                lowestSum = sum;
                Tiles.Clear();
            }
            //add points of path
            Tiles.AddRange(moves);
            return;
        }
        //check if we can move forward
        var move = CanMoveForward();
        if (move is not null)
        {
            var beenThere = moves.FirstOrDefault(x=> x.x == move.x && x.y == move.y);
            if (beenThere is null)
            {
                moves.Add(Reindeer);
                Reindeer = move;
                sum += 1;
                MakeMove(ref moves, true);
                //revert
                Reindeer = moves[moves.Count - 1];
                moves.RemoveAt(moves.Count - 1);
                sum -= 1;
            }
        }
        if (checkTurn)
        {
            //check turn right
            moves.Add(Reindeer);
            Reindeer = new Move(Reindeer.x, Reindeer.y, (Reindeer.dir + 1) % 4);
            sum += 1000;
            MakeMove(ref moves, false);
            sum -= 1000;
            //revert
            Reindeer = moves[moves.Count - 1];
            moves.RemoveAt(moves.Count - 1);
            //check turn left
            moves.Add(Reindeer);
            Reindeer = new Move(Reindeer.x, Reindeer.y, (Reindeer.dir + 3) % 4);
            sum += 1000;
            MakeMove(ref moves, false);
            sum -= 1000;
            //revert
            Reindeer = moves[moves.Count - 1];
            moves.RemoveAt(moves.Count - 1);
        }
        //???

    }


    private int ReindeerDist()
    {
        return Math.Abs(Reindeer.x - EndPos.x) + Math.Abs(Reindeer.y - EndPos.y);
    }
    private int LastMinDist()
    { 
        return Math.Abs(MinDist.x - EndPos.x) + Math.Abs(MinDist.y - EndPos.y);
    }

    private void PrintDist()
    {
        //calculate diff
        int diff = ReindeerDist();
        int mindist = LastMinDist();
        if (diff < mindist)
        {
            MinDist = new Move(Reindeer.x, Reindeer.y, Reindeer.dir);
            Console.WriteLine($"MinDist: {MinDist.x}:{MinDist.y}  diff: {diff} SUM: {sum}");
        }
    }

    private void PrintMap()
    {
        Console.WriteLine($"Reindeer ({Reindeer.x}:{Reindeer.y})  cost: {sum}:");
        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                if (Reindeer.x == x && Reindeer.y == y)
                {
                    Console.Write("R");
                }
                else
                {
                    if (MyMap[x, y] == 1)
                        Console.Write("#");
                    else
                        Console.Write(" ");
                }
            }
            Console.WriteLine();
        }
    }

    private Move? CanMoveForward()
    {
        int dir = Reindeer.dir;
        int x = Reindeer.x;
        int y = Reindeer.y;
        if (dir == 0) {
            if (y > 0)
            {
                if (MyMap[x, y - 1] == 0) return new Move(x,y-1,dir);
            }
        }
        if (dir == 1) {
            if (x < MapWidth - 1)
            {
                if (MyMap[x + 1, y] == 0) return new Move(x+1,y,dir);
            }
        }
        if (dir == 2) {
            if (y < MapHeight - 1)
            {
                if (MyMap[x, y + 1] == 0) return new Move(x,y+1,dir);
            }
        }
        if (dir == 3) {
            if (x > 0)
            {
                if (MyMap[x - 1, y] == 0) return new Move (x-1,y,dir);
            }
        }
        return null;
    }
}

public record Move (int x, int y, int dir);

