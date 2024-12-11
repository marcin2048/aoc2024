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

    internal void ProcessTask1(int algorithm, int count)
    {
        ResetQube();
        qube.Blink(count, algorithm);
        long checksum = qube.GetTheResultBlink3();
        Console.WriteLine($"RESULT: {checksum}");
    }

}

public class Qube
{
    List<long> Stones = new List<long>();
    long[] Stones2;
    long blink3res = 0;
    List<ResultItem> Results = new List<ResultItem>();

    internal void OneBlink()
    {
        List<long> NewStones = new List<long>();
        foreach (long stone in Stones)
        {
            //apply one of rules...
            if (stone == 0)
            {
                NewStones.Add(stone+1);
                continue;
            }
            //check if length is even
            if (stone.ToString().Length % 2 == 0)
            {
                (long first, long second) = GetSplitted(stone);
                NewStones.Add(first);
                NewStones.Add(second);
                continue;
            }
            NewStones.Add(stone * 2024);
        }
        Stones = NewStones;
    }

    internal (long first, long second) GetSplitted(long stone)
    {
        string s = stone.ToString();
        int half = s.Length / 2;
        string first = s.Substring(0, half);
        string second = s.Substring(half);
        return (long.Parse(first), long.Parse(second));
    }


    internal long[] NextPattern(long input)
    {
        //apply one of rules...
        if (input == 0)
        {
            return [input + 1];
        } 
        else if (input.ToString().Length % 2 == 0)
        {
            //split the number in half
            string s = input.ToString();
            int half = s.Length / 2;
            string first = s.Substring(0, half);
            string second = s.Substring(half);
            //check if the two halves are the same
            return [long.Parse(first), long.Parse(second)];
        } else {
            return [input * 2024];
        }
    }

    internal void OneBlink3(int lev, long[] stones2)
    {
        for (int o = 0; o < stones2.Length; o++)
        {
            if (lev == -1)
            {
                blink3res++;
                return;
            }
            var val = stones2[o];
            //find if we already have calculated result for this value and level
            var res = Results.Where(x => x.Level == lev && x.Val == val).FirstOrDefault();
            if (res is null)
            {
                //calculate result for this value and level
                long[] stonesNextLev = NextPattern(val);
                long resTemp = blink3res;   //to remeber!
                OneBlink3(lev - 1, stonesNextLev);
                Results.Add(new ResultItem() { 
                        Level = lev, Val = val, Result = blink3res - resTemp 
                    });
            }
            else
            {
                //use already calculated result
                blink3res += res.Result;
            }
        }
    }


    internal long GetTheResultBlink3()
    {
        return blink3res;
    }

    internal void LoadValues(List<string> lines)
    {
        foreach (var line in lines)
        {
            var s = line.Split(" ");
            foreach (var i in s)
                Stones.Add(long.Parse(i));
            LoadFromArray(s);
        }

    }

    private void LoadFromArray(string[] s)
    {
        Stones2 = new long[s.Length];
        for (int i = 0; i < s.Length; i++)
        {
            Stones2[i] = long.Parse(s[i]);
        }
    }

    internal void Blink(int v, int algo)
    {
        if (v < 30)
        {
            Console.WriteLine("FIRST ALGORITHM: this is slow. Only for levels less than 30");
            for (int i = 0; i < v; i++)
            {
                OneBlink();
                Console.WriteLine($"{i} LEN:{Stones.Count}");
            }
            if (v < 10) Print();
        }
        Console.WriteLine("SECOND ALGORITHM: for levels around 75 time is about 60 seconds");
        OneBlink3(v, Stones2);
    }

    private void Print()
    {
        foreach (var stone in Stones)
        {
            Console.Write($"{stone} ");
        }
        Console.WriteLine();
    }

}

internal class ResultItem
{
    internal int Level;         //level of recursion
    internal long Val;          //stone value
    internal long Result;       //number to add
}