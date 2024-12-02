internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Processor p = new Processor();


        Console.WriteLine("First part");
        p.ProcessTask1("testdata.txt");
        //p.ProcessTask1("inputdata.txt");


        //Console.WriteLine("Second part");
        //p.ProcessTask2("testdata.txt");
        //p.ProcessTask2("inputdata.txt");


    }
}