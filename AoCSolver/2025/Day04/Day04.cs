namespace AoCSolver._2025.Day04;

public class Day04 : Solver<List<List<char>>, long>
{
    private static readonly (int, int)[] dirs = new[]
    {
        (0, 1),
        (1, 1),
        (1, 0),
        (1, -1),
        (0, -1),
        (-1, -1),
        (-1, 0),
        (-1, 1),
    };

    public override List<List<char>> PrepareData(List<string> input) =>
        input.Select(line => line.ToCharArray().ToList()).ToList();

    public override long Part1(List<List<char>> data)
    {
        var result = 0;
        var width = data[0].Count;
        var height = data.Count;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (data[y][x] == '@')
                {
                    var rolls = 0;
                    foreach (var dir in dirs)
                    {
                        var targetX = x + dir.Item1;
                        var targetY = y + dir.Item2;

                        if (targetX >= 0 && targetX < width && targetY >= 0 && targetY < height &&  data[targetY][targetX] == '@')
                        {
                            rolls++;
                        }
                    }

                    if (rolls < 4) result++;
                }
            }
        }
        
        return result;
    }

    public override long Part2(List<List<char>> data)
    {
        var result = 0;
        var width = data[0].Count;
        var height = data.Count;

        while (data.Any(line => line.Contains('@')))
        {
            var newMap = data.Select(line => new List<char>(line)).ToList();
            var removed = 0;
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (data[y][x] == '@')
                    {
                        var rolls = 0;
                        foreach (var dir in dirs)
                        {
                            var targetX = x + dir.Item1;
                            var targetY = y + dir.Item2;

                            if (targetX >= 0 && targetX < width && targetY >= 0 && targetY < height &&
                                data[targetY][targetX] == '@')
                            {
                                rolls++;
                            }
                        }

                        if (rolls < 4)
                        {
                            newMap[y][x] = '.';
                            removed++;
                        }
                    }
                }
            }
            data = newMap;
            if (removed == 0) break;
            result += removed;
        }
        
        return result;
    }
}