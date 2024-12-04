
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

internal class Processor
{
    List<string> lines = new List<string>();

    int[,] letters;


    internal void ProcessTask1(string @filename)
    {
        int sum = 0;
        lines.Clear();
        foreach (string line in File.ReadLines(@filename))
        {
            lines.Add(line);
        }
        //
        int rows = lines.Count;
        int cols = lines[0].Length;
        letters = new int[cols, rows];
        //fill the array
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                letters[i, j] = lines[j][i];
            }
        }

        int sum1 = FindStringInRows("XMAS");
        Console.WriteLine($"SUM : {sum1}");
        int sum2 = FindStringInRows("SAMX");
        Console.WriteLine($"SUM : {sum2}");
        int sum3 = FindStringDiagonal("XMAS");
        Console.WriteLine($"SUM : {sum3}");
        int sum4 = FindStringDiagonal("SAMX");
        Console.WriteLine($"SUM : {sum4}");
        int sum5 = FindStringDiagonal2("XMAS");
        Console.WriteLine($"SUM : {sum5}");
        int sum6 = FindStringDiagonal2("SAMX");
        Console.WriteLine($"SUM : {sum6}");
        int sum7 = FindStringCols("XMAS");
        Console.WriteLine($"SUM : {sum7}");
        int sum8 = FindStringCols("SAMX");
        Console.WriteLine($"SUM : {sum8}");

        sum = sum1 + sum2 + sum3 + sum4 + sum5 + sum6 + sum7 + sum8;

        Console.WriteLine($"RESULT: {sum}");

    }

    private int FindStringCols(string pattern)
    {
        int sum = 0;
        for (int i = 0; i < letters.GetLength(0); i++)
        {
            string line = "";
            for (int j = 0; j < letters.GetLength(1); j++)
            {
                line += (char)letters[i, j];
            }
            int c = FindOccurances(pattern, line);
            sum += c;
        }
        return sum;
    }

    private int FindStringDiagonal2(string pattern)
    {
        int sum = 0;
        //prepare string from diagonal
        string line = "";

        int maxdim = Math.Max(letters.GetLength(0), letters.GetLength(1));

        for (int startcol = 0; startcol < letters.GetLength(0); startcol++)
        {
            line = "";
            for (int i = 0; i < maxdim; i++)
            {
                int col = startcol - i;
                int row = i;
                line += GetValueFromLetters(col, row);
            }
            int c = FindOccurances(pattern, line);
            sum += c;
        }
        for (int startrow = 1; startrow < letters.GetLength(1); startrow++)
        {
            line = "";
            for (int i = 0; i < maxdim; i++)
            {
                int col = letters.GetLength(0) - 1 - i;
                int row = startrow + i;
                line += GetValueFromLetters(col, row);
            }
            int c = FindOccurances(pattern, line);
            sum += c;
        }
        return sum;
    }


    private int FindStringDiagonal(string pattern)
    {
        int sum = 0;
        string line = "";
        int maxdim = Math.Max(letters.GetLength(0), letters.GetLength(1));

        for (int startcol = 0; startcol < letters.GetLength(0); startcol++)
        {
            line = "";
            for (int i = 0; i < maxdim; i++)
            {
                int col = startcol + i;
                int row = i;
                line += GetValueFromLetters(col, row);
            }
            int c = FindOccurances(pattern, line);
            sum += c;
        }
        for (int startrow = 0; startrow < letters.GetLength(1); startrow++)
        {
            line = "";
            for (int i = 1; i < maxdim; i++)
            {
                int col = i;
                int row = startrow + i;
                line += GetValueFromLetters(col, row);
            }
            int c = FindOccurances(pattern, line);
            sum += c;
        }
        return sum;
    }

    private string GetValueFromLetters(int col, int row)
    {
        if (col < 0 || row <0)
            return "";
        if (col >= letters.GetLength(0) || row >= letters.GetLength(1))
            return "";
        char c = (char)letters[col, row];
        return c.ToString();
    }

    private int FindStringInRows(string pattern)
    {
        int sum = 0;
        foreach(string line in lines)
        {
            int count = FindOccurances(pattern,line);
            sum += count;
        }
        return sum;
    }

    private int FindOccurances(string pattern, string line)
    {
        return Regex.Matches(line, pattern).Count;
    }

    internal void ProcessTask2()
    {
        int cols = letters.GetLength(0);
        int rows = letters.GetLength(1);
        cols -= 2;
        rows -= 2;
        int[,] mat1 = new int[3, 3];
        ClearMat(mat1);
        mat1[0, 0] = 'M';
        mat1[2, 0] = 'M';
        mat1[1, 1] = 'A';
        mat1[0, 2] = 'S';
        mat1[2, 2] = 'S';
        int[,] mat2 = new int[3, 3];
        ClearMat(mat2);
        mat2[0, 0] = 'M';
        mat2[2, 0] = 'S';
        mat2[1, 1] = 'A';
        mat2[0, 2] = 'M';
        mat2[2, 2] = 'S';
        int[,] mat3 = new int[3, 3];
        ClearMat(mat3);
        mat3[0, 0] = 'S';
        mat3[2, 0] = 'M';
        mat3[1, 1] = 'A';
        mat3[0, 2] = 'S';
        mat3[2, 2] = 'M';
        int[,] mat4 = new int[3, 3];
        ClearMat(mat4);
        mat4[0, 0] = 'S';
        mat4[2, 0] = 'S';
        mat4[1, 1] = 'A';
        mat4[0, 2] = 'M';
        mat4[2, 2] = 'M';

        int sum1 = 0;
        int sum2 = 0;
        int sum3 = 0;
        int sum4 = 0;
        int sum = 0;

        for (int c = 0; c<cols; c++)
            for (int r = 0; r < rows; r++)
            {
                if (CheckPresence(c, r, mat1))
                    sum1++;
            }
        Console.WriteLine($"mat1: {sum1}");
        for (int c = 0; c < cols; c++)
            for (int r = 0; r < rows; r++)
            {
                if (CheckPresence(c, r, mat2))
                    sum2++;
            }
        Console.WriteLine($"mat2: {sum2}");
        for (int c = 0; c < cols; c++)
            for (int r = 0; r < rows; r++)
            {
                if (CheckPresence(c, r, mat3))
                    sum3++;
            }
        Console.WriteLine($"mat3: {sum3}");
        for (int c = 0; c < cols; c++)
            for (int r = 0; r < rows; r++)
            {
                if (CheckPresence(c, r, mat4)) sum4++;
            }
        Console.WriteLine($"mat4: {sum4}");

        sum = sum1 + sum2 + sum3 + sum4;
        Console.WriteLine($"RESULT: {sum}");

    }

    private void ClearMat(int[,] mat1)
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
            {
                mat1[i, j] = 0;
            }
    }

    private bool CheckPresence(int c, int r, int[,] mat1)
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
            {
                if (mat1[i, j] == 0)
                    continue;
                if (letters[c + i, r + j] != mat1[i, j])
                    return false;
            }
        return true;
    }
}