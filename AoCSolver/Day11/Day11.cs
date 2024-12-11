using SuperLinq;

namespace AoCSolver.Day11;

public class Day11() : Solver<List<long>, long>("Day11/input.txt")
{
    private Dictionary<(long, int), long> cache = new Dictionary<(long, int), long>();

    public override List<long> PrepareData(List<string> input) =>
        input[0].Split(" ").Select(long.Parse).ToList();

    public override long Part1(List<long> data)
    {
        var sum = 0L;
        foreach (var stone in data)
        {
            sum += GetStoneValue(stone, 0, 25);
        }

        return sum;
    }

    public override long Part2(List<long> data)
    {
        var sum = 0L;
        foreach (var stone in data)
        {
            sum += GetStoneValue(stone, 0, 75);
        }

        return sum;
    }

    private long GetStoneValue(long stone, long stoneAmount , int depth)
    {
        if (depth <= 0)
        {
            return 1;
        }
        
        var key = (stone, depth);
        if (cache.TryGetValue(key, out var cachedValue))
        {
            return cachedValue;
        }
        
        var stoneStr = stone.ToString();
        if (stone == 0)
        {
            stoneAmount += GetStoneValue(1, 0, depth - 1);
        } else if (stoneStr.Length % 2 == 0)
        {
            stoneAmount += GetStoneValue(long.Parse(stoneStr[0..(stoneStr.Length / 2)]), 0, depth - 1) + 
                   GetStoneValue(long.Parse(stoneStr[(stoneStr.Length / 2)..]), 0, depth - 1);
        } else
        {
            stoneAmount += GetStoneValue(stone * 2024, 0, depth - 1);
        }

        cache[key] = stoneAmount;
        return stoneAmount;
    }
}