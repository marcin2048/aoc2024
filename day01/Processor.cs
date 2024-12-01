internal class Processor
{
    //List<string> lines = new List<string>();
    List<int> c1 = new List<int>();
    List<int> c2 = new List<int>();

    internal void ProcessTask1(string @filename)
    {
        int sum = 0;
        //lines.Clear();
        c2.Clear();
        c1.Clear();
        foreach (string line in File.ReadLines(@filename))
        {
            //lines.Add(line);
            var arr = line.Split("   ");
            int i1 = int.Parse(arr[0]);
            int i2 = int.Parse(arr[1]);
            c1.Add(i1);
            c2.Add(i2);
        }
        
        //sort
        List<int> c1_sorted = c1.OrderBy(x => x).ToList();
        List<int> c2_sorted = c2.OrderBy(x => x).ToList();

        //sum of row substractions
        for (int i = 0; i < c1_sorted.Count; i++)
        {
            sum += Math.Abs(c2_sorted[i] - c1_sorted[i]);
        }


        Console.WriteLine($"RESULT: {sum}");

    }

    internal void ProcessTask2(string @filename)
    {
        int sum = 0;
        //lines.Clear();
        c2.Clear();
        c1.Clear();
        foreach (string line in File.ReadLines(@filename))
        {
            //lines.Add(line);
            var arr = line.Split("   ");
            int i1 = int.Parse(arr[0]);
            int i2 = int.Parse(arr[1]);
            c1.Add(i1);
            c2.Add(i2);
        }

        //sort
        List<int> c1_sorted = c1.OrderBy(x => x).ToList();
        List<int> c2_sorted = c2.OrderBy(x => x).ToList();

        //sum of row substractions
        for (int i = 0; i < c1_sorted.Count; i++)
        {
            sum += Math.Abs(c2_sorted[i] - c1_sorted[i]);
        }

        //get a list of distinct values
        List<int> c1_distinct = c1.Distinct().ToList();
        List<int> c2_distinct = c2.Distinct().ToList();

        List<(int Val, int Count)> c1_occur = new List<(int Val, int Count)>();
        foreach (var val in c1)
        {
            //get the number of occurences of the item in the list
            int count1 = c2.Count(x => x == val);
            //
            c1_occur.Add((val, count1));
        }

        List<(int Val, int Count)> c2_occur = new List<(int Val, int Count)>();
        foreach (var val in c2_distinct)
        {
            //get the number of occurences of the item in the list
            int count1 = c1.Count(x => x == val);
            //
            c2_occur.Add((val, count1));
        }

        //print all values from c1_occur
        foreach (var item in c1_occur)
        {
            Console.WriteLine($"Val: {item.Val} Count: {item.Count}");
        }

        //get the sum of multiplication of the number of occurences of the item in the list
        int sum_sim_c1 = 0;
        for (int i = 0; i < c1_occur.Count; i++)
        {
            sum_sim_c1 += c1_occur[i].Count * c1_occur[i].Val;
        }

        Console.WriteLine($"RESULT: {sum_sim_c1}");
    }

}