internal class Processor
{
    List<string> lines = new List<string>();
    List<Equation> eq = new List<Equation>();

    internal void LoadFile(string @filename)
    {
        lines.Clear();
        foreach (string line in File.ReadLines(@filename))
        {
            lines.Add(line);
        }
    }

    internal void Reload()
    {
        eq.Clear();
        foreach (string line in lines)
        {
            eq.Add(new Equation(line));
        }
    }

    internal void ProcessTask1()
    {
        Reload();


        long sum = 0;

        foreach(Equation e in eq)
        {
            bool solvable = e.CheckSolvable();
            if (solvable)
            {
                //Console.WriteLine($"solvable {e.result}");
                sum += e.result;
            }
        }

        Console.WriteLine($"RESULT: {sum}");
    }

    internal void ProcessTask2()
    {
        Reload();


        long sum = 0;

        foreach (Equation e in eq)
        {
            bool solvable = e.CheckSolvable2();
            if (solvable)
            {
                //Console.WriteLine($"solvable {e.result}");
                sum += e.result;
            }
        }

        Console.WriteLine($"RESULT: {sum}");
    }

}

internal class Equation
{
    public long result;
    public List<long> ints = new List<long>();
    internal long calcsum = 0;
    internal long lastVV = 0;
    internal bool solv = false;
    internal List<string> vv = new List<string>();

    public Equation(string line)
    {
        string[] parts = line.Split(": ");
        result = long.Parse(parts[0]);
        string[] vals = parts[1].Split(" ");
        foreach (string val in vals)
        {
            ints.Add(int.Parse(val));
        }
    }

    internal bool CheckSolvable2()
    {
        //long res = 0;
        calcsum = 0;
        solv = false;

        CalculateLoop2(0);


        return solv;

    }

    private void CalculateLoop2(int pos)
    {
        if (pos == 0)
        {
            CalculateLoop2(pos + 1);
        }
        else
        {
            if (pos < ints.Count)
            {
                //check add
                vv.Add("+");
                CalculateLoop2(pos + 1);
                vv.RemoveAt(vv.Count - 1);
                if (solv) return;
                //check mul
                vv.Add("*");
                CalculateLoop2(pos + 1);
                vv.RemoveAt(vv.Count - 1);
                if (solv) return;
                //check concat
                vv.Add("|");
                CalculateLoop2(pos + 1);
                vv.RemoveAt(vv.Count - 1);
                if (solv) return;
            }
            if (pos == ints.Count)
            {
                //calculate 
                calcsum = CalculateBasedOnEq(vv);
                if (calcsum == result)
                {
                    solv = true;
                }
                else
                {
                    solv = false;
                }
            }
        }
    }

    private long CalculateBasedOnEq(List<string> vv)
    {
        string cac = string.Join(" ", vv);
        long res = ints[0];
        for (int i = 0; i < vv.Count; i++)
        {
            if (vv[i] == "+")
            {
                res += ints[i + 1];
            }
            if (vv[i] == "*")
            {
                res *= ints[i + 1];
            }
            if (vv[i] == "|")
            {
                res = long.Parse($"{res}{ints[i + 1]}");
            }
        }
        return res;
    }

    internal bool CheckSolvable()
    {
        calcsum = 0;
        solv = false;
        CalculateLoop(0);
        return solv;
    }



    private void CalculateLoop(int pos)
    {
        if (pos==0)
        {
            calcsum = ints[0];
            CalculateLoop(pos + 1);
        }
        else
        {
            if (pos < ints.Count)
            {
                //check add
                calcsum += ints[pos];
                vv.Add("+");
                CalculateLoop(pos + 1);
                vv.RemoveAt(vv.Count - 1);
                calcsum -= ints[pos];
                if (solv) return;
                //check mul
                calcsum *= ints[pos];
                vv.Add("*");
                CalculateLoop(pos + 1);
                vv.RemoveAt(vv.Count - 1);
                calcsum /= ints[pos];
                if (solv) return;
            }
            if (pos == ints.Count)
            {
                if (calcsum == result)
                {
                    solv = true;
                    //string cac = string.Join(" ", vv);
                    //Console.WriteLine($"Equation: {cac}");
                }
                else
                {
                    solv = false;
                }
            }
        }
    }

    //
}