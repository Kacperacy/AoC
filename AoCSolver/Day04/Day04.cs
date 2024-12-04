namespace AoCSolver.Day04;

public class Day04() : Solver<List<List<char>>,int>("Day04/input.txt")
{
    public override List<List<char>> PrepareData(List<string> input) =>
        input.Select(x => x.ToCharArray().ToList()).ToList();

    public override int Part1(List<List<char>> data)
    {
        int sum = 0;
        for (int i = 0; i < data.Count; i++)
        {
            for (int j = 0; j < data[0].Count; j++)
            {
                if (data[i][j] != 'X') continue;
                    
                if (i >= 3)
                {
                    if (data[i - 1][j] == 'M' && data[i - 2][j] == 'A' && data[i - 3][j] == 'S')
                    {
                        sum++;
                    }
                }
                if (j >= 3)
                {
                    if (data[i][j - 1] == 'M' && data[i][j - 2] == 'A' && data[i][j - 3] == 'S')
                    {
                        sum++;
                    }
                }
                if (i < data.Count - 3)
                {
                    if (data[i + 1][j] == 'M' && data[i + 2][j] == 'A' && data[i + 3][j] == 'S')
                    {
                        sum++;
                    }
                }
                if (j < data[0].Count - 3)
                {
                    if (data[i][j + 1] == 'M' && data[i][j + 2] == 'A' && data[i][j + 3] == 'S')
                    {
                        sum++;
                    }
                }
                if (i >= 3 && j >= 3)
                {
                    if (data[i - 1][j - 1] == 'M' && data[i - 2][j - 2] == 'A' && data[i - 3][j - 3] == 'S')
                    {
                        sum++;
                    }
                }
                if (i >= 3 && j < data[0].Count - 3)
                {
                    if (data[i - 1][j + 1] == 'M' && data[i - 2][j + 2] == 'A' && data[i - 3][j + 3] == 'S')
                    {
                        sum++;
                    }
                }
                if (i < data.Count - 3 && j >= 3)
                {
                    if (data[i + 1][j - 1] == 'M' && data[i + 2][j - 2] == 'A' && data[i + 3][j - 3] == 'S')
                    {
                        sum++;
                    }
                }
                if (i < data.Count - 3 && j < data[0].Count - 3)
                {
                    if (data[i + 1][j + 1] == 'M' && data[i + 2][j + 2] == 'A' && data[i + 3][j + 3] == 'S')
                    {
                        sum++;
                    }
                }
            }
        }
        return sum;
    }

    public override int Part2(List<List<char>> data)
    {
        int sum = 0;
        for (int i = 0; i < data.Count; i++)
        {
            for (int j = 0; j < data[0].Count; j++)
            {
                if (data[i][j] != 'A') continue;

                var masCount = 0;
                
                if (i >= 1 && j >= 1 && i < data.Count - 1 && j < data[0].Count - 1)
                {
                    if (data[i - 1][j - 1] == 'M' && data[i + 1][j + 1] == 'S')
                    {
                        masCount++;
                    }
                    if (data[i - 1][j + 1] == 'M' && data[i + 1][j - 1] == 'S')
                    {
                        masCount++;
                    }
                    if (data[i + 1][j - 1] == 'M' && data[i - 1][j + 1] == 'S')
                    {
                        masCount++;
                    }
                    if (data[i + 1][j + 1] == 'M' && data[i - 1][j - 1] == 'S')
                    {
                        masCount++;
                    }
                }
                if (masCount == 2)
                {
                    sum++;
                }
            }
        }

        return sum;
    }
}