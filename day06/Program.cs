using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Processor p = new Processor();

        Console.WriteLine("Test file");
        p.LoadFile("testdata.txt");
        p.ResetQube();
        p.ProcessTask1();
        p.ResetQube();
        p.ProcessTask2();

        Console.WriteLine("Input file");
        p.LoadFile("inputdata.txt");
        p.ResetQube();
        p.ProcessTask1();
        p.ResetQube();
        p.ProcessTask2();

    }
}