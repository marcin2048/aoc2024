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

    internal void ResetQube(bool addmillions)
    {
        qube = new Qube();
        qube.LoadValues(lines, addmillions);
    }

    internal void ProcessTask1(int algorithm)
    {
        Console.WriteLine($"Calculating algorithm: {algorithm}");
        ResetQube(algorithm==2);
        qube.CleanUpTheMemory();
        long checksum = qube.CalculateChecksum();
        Console.WriteLine($"RESULT: {checksum}");
    }

}

public class Qube
{
    List<PrizeItem> prizes = new List<PrizeItem>();
    long sum;

    internal void CleanUpTheMemory()
    {
    }

    internal void LoadValues(List<string> lines, bool addmillions)
    {
        prizes.Clear();
        string buttona = "";
        string buttonb = "";
        string prize = "";
        for (int i = 0; i < lines.Count; i++)
        {
            string line = lines[i];
            if (line.Contains("Button A:"))
            {
                buttona = line.Substring(9);
            }
            if (line.Contains("Button B:"))
            {
                buttonb = line.Substring(9);
            }
            if (line.Contains("Prize:"))
            {
                prize = line.Substring(6);
                //X + 94, Y + 34
                //X + 22, Y + 67
                //X = 8400, Y = 5400
                var buttonAp = buttona.Split(", ");
                var buttonAX = int.Parse(buttonAp[0].Trim().Substring(2));
                var buttonAY = int.Parse(buttonAp[1].Trim().Substring(2));
                //the same for button B
                var buttonBp = buttonb.Split(", ");
                var buttonBX = int.Parse(buttonBp[0].Trim().Substring(2));
                var buttonBY = int.Parse(buttonBp[1].Trim().Substring(2));
                //prize
                var prizep = prize.Split(", ");
                long add = addmillions?10000000000000:0;
                var prizeX = int.Parse(prizep[0].Trim().Substring(2)) + add;
                var prizeY = int.Parse(prizep[1].Trim().Substring(2)) + add;

                PrizeItem item = new PrizeItem(buttonAX, buttonAY, buttonBX, buttonBY, prizeX, prizeY);
                prizes.Add(item);
            }
        }
        //print array
        //foreach (var item in prizes)
        //{
        //    Console.WriteLine($"AX:{item.ax} AY:{item.ay} BX:{item.bx} BY:{item.by} PX:{item.px} PY{item.py}");
        //}
    }

    internal long CalculateChecksum()
    {
        sum = 0;
        //3 tokens button A
        //1 token button B
        foreach (PrizeItem item in prizes)
        {
            //find cheapest values
            var res = item.GetVAlues();

            long cost = res.a * 3 + res.b;
            //Console.WriteLine($"SOLVED::::Cost: {cost}");
            sum += cost;
        }
        return sum;
    }

}

internal class PrizeItem
{
    //??
    public long ax, ay, bx, by, px, py;

    public PrizeItem(long ax, long ay, long bx, long by, long px, long py)
    {
        this.ax = ax;
        this.ay = ay;
        this.bx = bx;
        this.by = by;
        this.px = px;
        this.py = py;
    }

    internal (long a, long b) GetVAlues()
    {
        long amax = Math.Min(px / ax, py / ay);
        long bmax = Math.Min(px / bx, py / by);
        long amin = Math.Max((px - bmax * bx) / ax, (py - bmax * by) / ay);
        long bmin = Math.Max((px - amax * ax) / bx, (py - amax * ay) / by);
        long ppy = 0;
        long bbxdiff = bx;
        long bbxdiffctrl = 0;
        long ppydiff = 0;
        long ppydiffctl = 0;
        long bezpiecznik = 0;
        //Console.WriteLine($"Starting ax:{ax}, [{amin}-{amax}], bx:{bx}, [{bmin}-{bmax}]; ay:{ay}, by:{by}, bbxdiff:{bbxdiff}");
        for (long bbx = bmax * bx; bbx >= bmin * bx ; bbx-= bbxdiff)
        {
            if (bezpiecznik++ > 100000)
            {
                return (0, 0);
            }
            //check the rest
            long aax = px - bbx;
            long a_mod = aax % ax;
            //
            long rufus = a_mod + 10;
            if (a_mod == 0)
            {
                if (bbxdiffctrl == 0)
                {
                    bbxdiffctrl = bbx;
                } else if (bbxdiffctrl > 0)
                {
                    bbxdiff = bbxdiffctrl - bbx;
                    bbxdiffctrl = -1;
                    //Console.WriteLine($"Detected diff: {bbxdiff}");
                }

                //Console.Write(".");
                //now check py?
                long b = bbx / bx;
                long a = aax / ax;
                //calc the ppy
                ppy = ay * a + by * b;
                if (ppy ==py)
                {
                    //Console.WriteLine($"HIT: a={a} b={b}");
                    return (a, b);
                }
                //ppydiff
                if (ppydiffctl ==0)
                {
                    ppydiffctl = ppy;
                } else if (ppydiffctl > 0)
                {
                    ppydiff = ppydiffctl - ppy;
                    long  hops = (ppy-py) / ppydiff;
                    ppydiffctl = -1;
                    //Console.WriteLine($"Detected diff PPY : {ppydiff}, hops: {hops}");
                    bbx -= (hops-1) * bbxdiff;
                    bmin = (bbx - 10 * bbxdiff) / bx;
                }

            }

        }
        return (0, 0);

    }
}