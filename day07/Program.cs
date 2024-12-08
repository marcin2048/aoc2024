using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        //measure execution time
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Console.WriteLine("Hello, World!");
        Processor p = new Processor();

        Console.WriteLine("Test file");
        p.LoadFile("testdata.txt");
        p.ProcessTask1();
        p.ProcessTask2();

        Console.WriteLine("Input file");
        p.LoadFile("inputdata.txt");
        p.ProcessTask1();
        p.ProcessTask2();

        sw.Stop();
        Console.WriteLine($"Execution time: {sw.ElapsedMilliseconds} ms");

    }
}