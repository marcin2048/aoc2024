

internal class Processor
{
    int poz = 0;
    int i1 = 0;
    int i2 = 0;
    int pozdo = 0;
    int pozdont = 0;
    bool sumdo = true;

    internal void Reset()
    {
        poz = 0;
        i1 = 0;
        i2 = 0;
    }
    internal void ResetDo()
    {
        pozdo = 0;
    }
    internal void ResetDont()
    {
        pozdont = 0;
    }

    internal void ProcessTask1(string @filename)
    {
        int sum = 0;
        using StreamReader reader = new StreamReader(@filename);

        int c;
        while ((c = reader.Read()) >= 0)
        {
            switch (c)
            {
                case 'd':
                    if (pozdont == 0) pozdont++;
                    else ResetDont();
                    break;
                case 'o':
                    if (pozdont == 1) pozdont++;
                    else ResetDont();
                    break;
                case 'n':
                    if (pozdont == 2) pozdont++;
                    else ResetDont();
                    break;
                case '\'':
                    if (pozdont == 3) pozdont++;
                    else ResetDont();
                    break;
                case 't':
                    if (pozdont == 4) pozdont++;
                    else ResetDont();
                    break;
                case '(':
                    if (pozdont == 5) pozdont++;
                    else ResetDont();
                    break;
                case ')':
                    if (pozdont == 6)
                    {
                        Console.WriteLine("DON'T found");
                        sumdo = false;
                    }
                    ResetDont();
                    break;
                default:
                    ResetDont();
                    break;
            }

            switch (c)
            {
                case 'd':
                    if (pozdo == 0) pozdo++;
                    else ResetDo();
                    break;
                case 'o':
                    if (pozdo == 1) pozdo++;
                    else ResetDo();
                    break;
                case '(':
                    if (pozdo == 2) pozdo++;
                    else ResetDo();
                    break;
                case ')':
                    if (pozdo == 3)
                    {
                        Console.WriteLine("DO found");
                        sumdo = true;
                    }
                    ResetDo();
                    break;
                default:
                    ResetDo();
                    break;
            }

            switch (c)
            {
                case 'm':
                    if (poz == 0) poz++;
                    else Reset();
                    break;
                case 'u':
                    if (poz == 1) poz++;
                    else Reset();
                    break;
                case 'l':
                    if (poz == 2) poz++;
                    else Reset();
                    break;
                case '(':
                    if (poz == 3) poz++;
                    else Reset();
                    break;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    if (poz == 4)
                    {
                        i1 = i1 * 10 + c - '0';
                    }
                    else if (poz == 5)
                    {
                        i2 = i2 * 10 + c - '0';
                    }
                    else Reset();
                    break;
                case ',':
                    if (poz == 4) poz++;
                    else Reset();
                    break;
                case ')':
                    if (poz == 5)
                    {
                        //finish
                        if (sumdo)
                        {
                            sum += i1 * i2;
                            Console.WriteLine($"i1={i1}, i2={i2}, sum={sum}");
                            Reset();

                        }
                    }
                    Reset();
                    break;
                default:
                    Reset();
                    break;
            }

        };

        Console.WriteLine($"RESULT: {sum}");
    }



}
