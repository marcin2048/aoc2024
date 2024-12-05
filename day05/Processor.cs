using System.Data;

internal class Processor
{
    List<Rule> rules = new List<Rule>();
    List<Order> orders = new List<Order>();

    internal void ProcessTask1(string @filename)
    {
        int sum = 0;
        int sum2 = 0;
        rules.Clear();
        orders.Clear();

        foreach (string line in File.ReadLines(@filename))
        {
            bool orderOrRule = line.Contains("|");
            if (orderOrRule)
            {
                var rule = line.Split('|');
                var rule1 = int.Parse(rule[0]);
                var rule2 = int.Parse(rule[1]);
                rules.Add(new Rule(rule1,rule2));

            }
            else
            {
                if (line.Length > 0)
                orders.Add(new Order(line));
            }
        }

        Console.WriteLine($"Rules: {rules.Count} Orders: {orders.Count}");

        foreach (var order in orders)
        {
            bool skip = false;
            //check rules against orders
            Console.WriteLine($"CHECKING: {order.line}  ");
            foreach (var rule in rules)
            {
                if (order.pages.Contains(rule.rule1) && order.pages.Contains(rule.rule2))
                {
                    //get the position of rule1 and rule2 in the order
                    var pos1 = order.pages.IndexOf(rule.rule1);
                    var pos2 = order.pages.IndexOf(rule.rule2);
                    if (pos1 >= pos2)
                    {
                        skip = true;
                        break;
                    }
                }
            }
            if (!skip) sum += order.GetMiddleValue();
            Console.WriteLine($"   = Result: ; {!skip}  SUM: {sum}");
            if (skip) order.SetAsNotSorted();
        }
        Console.WriteLine($"RESULT: {sum}");

        ///checking only not sorted
        foreach (Order order in orders) if (order.sorted == false)
        {
            Console.WriteLine($"CHECKING: {order.line}  ");
            while (order.sorted == false)
            {
                bool retry = false;
                foreach (Rule rule in  rules)
                {
                    if (order.pages.Contains(rule.rule1) && order.pages.Contains(rule.rule2))
                    {
                        var pos1 = order.pages.IndexOf(rule.rule1);
                        var pos2 = order.pages.IndexOf(rule.rule2);
                        if (pos1 >= pos2)
                        {
                            order.Switch(pos1, pos2);
                            retry = true;
                        }
                    }
                }
                if (!retry) order.sorted = true;
            }
            int val = order.GetMiddleValue();
            sum2 += val;
        }
        Console.WriteLine($"RESULT2: {sum2}");
    }


}

internal class Order
{
    internal List<int> pages;// = new List<int>();
    internal string line;
    internal bool sorted = true;

    public Order(string line)
    {
        pages =line.Split(",").Select(int.Parse).ToList();
        this.line = line;
    }

    internal int GetMiddleValue()
    {
        //get the middle value
        int len = pages.Count - 1;
        return pages[len / 2];
    }

    internal void SetAsNotSorted()
    {
        sorted = false;
    }

    internal void Switch(int pos1, int pos2)
    {
        var temp = pages[pos1];
        pages[pos1] = pages[pos2];
        pages[pos2] = temp;
    }
}

internal class Rule
{
    public Rule(int rule1, int rule2)
    {
        this.rule1 = rule1;
        this.rule2 = rule2;
    }
    internal int rule1;
    internal int rule2;
}