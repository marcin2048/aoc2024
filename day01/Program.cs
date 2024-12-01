using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Processor p = new Processor();


        p.ProcessTask1("testdata.txt");
        p.ProcessTask1("inputdata.txt");


        p.ProcessTask2("testdata.txt");
        p.ProcessTask2("inputdata.txt");


    }
}