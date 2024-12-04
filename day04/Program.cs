using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Processor p = new Processor();


        Console.WriteLine("test data");
        p.ProcessTask1("testdata.txt");
        p.ProcessTask2();


        Console.WriteLine("My data");
        p.ProcessTask1("inputdata.txt");
        p.ProcessTask2();


    }
}