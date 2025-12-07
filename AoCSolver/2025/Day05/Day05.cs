using SuperLinq;

namespace AoCSolver._2025.Day05;

public class Day05 : Solver<List<long>, long>
{
    private List<(long, long)> _ranges = new();
    
    public override List<long> PrepareData(List<string> input)
    {
        var i = 0;
        while (!string.IsNullOrEmpty(input[i]))
        {
            var range = input[i].Split('-');
            _ranges.Add((long.Parse(range[0]), long.Parse(range[1])));
            i++;
        }
        
        _ranges = _ranges.OrderBy(range => range.Item1).ToList();

        return input.Skip(i + 1).Select(long.Parse).ToList();
    }

    public override long Part1(List<long> data) => data.Count(id => _ranges.Any(range => range.Item1 <= id && id <= range.Item2));

    public override long Part2(List<long> data)
    {
        var merged = new List<(long, long)>();
        var current = _ranges[0];

        foreach (var range in _ranges)
        {
            if (range.Item1 <= current.Item2)
            {
                current = (current.Item1, Math.Max(current.Item2, range.Item2));
            }
            else
            {
                merged.Add(current);
                current = range;
            }
        }
        merged.Add(current);
        
        return merged.Sum(range => range.Item2 - range.Item1 + 1);
    }
}