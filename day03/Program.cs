internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Processor p = new Processor();


        Console.WriteLine("First part");
        p.ProcessTask1("testdata2.txt");
        p.ProcessTask1("inputdata.txt");



    }
}