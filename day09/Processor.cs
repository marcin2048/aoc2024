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
        //qube.Print();
    }

    internal void ProcessTask1(int algorithm)
    {
        ResetQube();
        //Console.WriteLine($"RESULT: {sum}");
        if (algorithm == 1)
        {
            qube.CleanUpTheMemory();
            long checksum = qube.CalculateChecksum();
            Console.WriteLine($"RESULT: {checksum}");
        }
        else
        {
            qube.CleanUpTheMemory2();
            long checksum = qube.CalculateForBlocks();
            Console.WriteLine($"RESULT: {checksum}");
        }
    }

}

public class Qube
{

    List<int?> Memory = new List<int?>();
    int lastMemPos = -1;
    int firstEmptyPos = -1;
    List<Block> Blocks = new List<Block>(); //blocks description
    int lastBlockToMove = -1;

    internal void Print()
    {
        foreach (int? i in Memory)
        {
            if (i is null)
                Console.Write(".");
            else
                Console.Write($"{i}");
        }
        Console.WriteLine();
    }

    internal void CleanUpTheMemory2()
    {
        bool Continue = true;
        while (Continue)
        {
            //move last to first
            int lastMemPos = FindLastBlockToMove();
            if (lastMemPos != -1)
            {
                int firstEmptyPos = FindFirstAreaToFit(Blocks[lastMemPos].Size);
                if (firstEmptyPos < lastMemPos)
                {
                    ReplaceBlocks(firstEmptyPos, lastMemPos);
                }
                //Print2();
            }

            if (lastMemPos ==-1 ) Continue = false;

        }
    }

    private void Print2()
    {
        //print blocks
        foreach (Block b in Blocks)
        {
            int skip = (b.Skip) ? 1 : 0;
            Console.Write($"[{b.Id}");
            if (b.Sub)
            {
                foreach (Block sb in b.Subs)
                {
                    Console.Write($"[{sb.Id}]");
                }
            }

            Console.Write($"],");
        }
        Console.WriteLine();
    }

    private void ReplaceBlocks(int firstEmptyPos, int lastMemPos)
    {
        if (firstEmptyPos == -1 || lastMemPos == -1) return;
        //check size...
        if (Blocks[lastMemPos].Size == Blocks[firstEmptyPos].Size  && Blocks[firstEmptyPos].Sub == false)
        {
            //replace indexes
            Blocks[firstEmptyPos].Id = Blocks[lastMemPos].Id;
            Blocks[lastMemPos].Id = null;
        }
        else
        {
            Blocks[firstEmptyPos].Subs.Add(new Block() { Size = Blocks[lastMemPos].Size, Id = Blocks[lastMemPos].Id });
            Blocks[firstEmptyPos].Sub = true;
            Blocks[firstEmptyPos].Size = Blocks[firstEmptyPos].Size - Blocks[lastMemPos].Size;
            if (Blocks[firstEmptyPos].Size > 0)
            {
                Blocks[firstEmptyPos].Skip = false;
            }
            Blocks[firstEmptyPos].Id = null;
            Blocks[lastMemPos].Id = null;
        }
    }

    private int FindFirstAreaToFit(int size)
    {
        //find first empty block
        for (int i = 0; i < Blocks.Count; i++)
        {
            if (Blocks[i].Id is null) if (Blocks[i].Skip == false)
                    if (Blocks[i].Size >= size)
            {
                firstEmptyPos = i;
                return i;
            }
        }
        return -1;
    }

    private int FindLastBlockToMove()
    {
        //find last block with Id 
        for (int i = Blocks.Count -1; i >= 0; i--)
        {
            if (Blocks[i].Id is not null) if (Blocks[i].Skip == false)
                {
                    Blocks[i].Skip = true;
                    lastBlockToMove = i;
                    return i;
                }
        }
        return -1;
    }

    internal void CleanUpTheMemory()
    {
        while (checkIfComplete()==false)
        {
            //move last to first
            int lastMemPos = FindLastNotEmpty();
            int firstEmptyPos = FindFirstEmpty();
            //replace indexes
            Memory[firstEmptyPos] = Memory[lastMemPos];
            Memory[lastMemPos] = null;
            //Print();

        }
    }

    private int FindFirstEmpty()
    {
        int startPos = (firstEmptyPos == -1) ? 0 : firstEmptyPos;
        //find first empty
        for (int i = startPos; i < Memory.Count; i++)
        {
            if (Memory[i] is null)
            {
                firstEmptyPos = i;
                return i;
            }
        }
        return -1;
    }

    private int FindLastNotEmpty()
    {
        int startPos = (lastMemPos == -1) ? Memory.Count - 1 : lastMemPos;
        //find last not empty
        for (int i = startPos; i >= 0; i--)
        {
            if (Memory[i] is not null)
            {
                lastMemPos = i;
                return i;
            }
        }
        return -1;
    }

    internal bool checkIfComplete()
    {
        //check is there any null in memory between not null
        bool foundBool = false;
        foreach (int? i in Memory)
        {
            if (i is null) foundBool = true;
            else if (foundBool) return false;
        }
        return true;
    }


    internal void LoadValues(List<string> lines)
    {
        Memory.Clear();
        int blockNo = 0;
        bool Data = true;
        for (int y = 0; y < lines.Count; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                int count = lines[y][x] - '0';
                int? Id = null;
                if (Data)
                {
                    //add count fields to memory
                    for (int i = 0; i < count; i++)
                    {
                        Memory.Add(blockNo);
                    }
                    Id = blockNo;
                    blockNo++;
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        Memory.Add(null);
                    }
                }
                Blocks.Add(new Block() { Size = count, Id = Id });
                Data = !Data;
            }
        }
        Blocks[0].Skip = true;

    }

    internal long CalculateChecksum()
    {
        //for each memory block if not null multiply by position
        long sum = 0;
        for (int i = 0; i < Memory.Count; i++)
        {
            if (Memory[i] is not null)
            {
                sum += (i ) * Memory[i].Value;
            }
        }
        return sum;
    }

    internal long CalculateForBlocks()
    {
        //build...
        List<int?> res = new List<int?>();
        for (int i=0; i<Blocks.Count; i++)
        {
            if (Blocks[i].Id is null && Blocks[i].Sub is false)
            {
                for (int k = 0; k < Blocks[i].Size; k++) res.Add(null);
            }
            else
            {
                //check subs
                for (int j=0; j< Blocks[i].Subs.Count; j++)
                {
                    for (int k = 0; k < Blocks[i].Subs[j].Size; k++) res.Add(Blocks[i].Subs[j].Id);
                }
                for (int k = 0; k < Blocks[i].Size; k++) res.Add(Blocks[i].Id);
            }
        }

        //calculate sum of res
        long sum = 0;
        for (int i = 0; i < res.Count; i++)
        {
            if (res[i] is not null)
            {
                if (res[i] is not null)
                    sum += (i) * (int)res[i]!;
            }
        }

        return sum;
    }
}

internal class Block
{
    internal int Size;
    internal int? Id;
    internal bool Skip = false;
    internal bool Sub = false;
    internal List<Block> Subs = new List<Block>();
}