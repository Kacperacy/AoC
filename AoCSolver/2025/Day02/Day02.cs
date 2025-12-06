namespace AoCSolver._2025.Day02;

public class Day02 : Solver<List<(long, long)>, long>
{
    public override List<(long, long)> PrepareData(List<string> input)
    {
        var result = string.Join("", input)
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(pair => pair.Split('-'))
            .Select(ids => (long.Parse(ids[0]), long.Parse(ids[1])))
            .ToList();

        return result;
    }

    public override long Part1(List<(long, long)> data)
    {
        long result = 0;

        foreach (var pair in data)
        {
            for (long i = pair.Item1; i <= pair.Item2; i++)
            {
                var str = i.ToString();
                
                if (str.Length % 2 != 0) continue;

                if (str[..(str.Length / 2)] == str.Substring(str.Length / 2, str.Length / 2))
                    result += i;
            }
        }
        
        return result;
    }

    public override long Part2(List<(long, long)> data)
    {
        long result = 0;

        foreach (var pair in data)
        {
            for (long i = pair.Item1; i <= pair.Item2; i++)
            {
                var str = i.ToString();
                
                for (int j = 1; j <= str.Length / 2; j++)
                {
                    if (str.Length % j != 0) continue;

                    var shouldAdd = true;
                    var pattern = str.Substring(0, j);

                    for (int k = 0; k < str.Length / j; k++)
                    {
                        if (pattern != str.Substring(k * j, j))
                        {
                            shouldAdd = false;
                        }
                    }

                    if (shouldAdd)
                    {
                        result += i;
                        break;
                    }
                }
            }
        }
        
        return result;
    }
}