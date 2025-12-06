namespace AoCSolver._2024.Day07;

public class Day07 : Solver<List<List<long>>, long>
{
    public override List<List<long>> PrepareData(List<string> input) =>
        input.Select(line => line.Replace(":", "").Split(" ").Select(long.Parse).ToList()).ToList();

    public override long Part1(List<List<long>> data) => 
        data.Where(line => Test(line, 1, 0, line[0])).Sum(line => line[0]);

    public override long Part2(List<List<long>> data) =>
        data.Where(line => Test2(line, 1, 0, line[0])).Sum(line => line[0]);

    private bool Test(List<long> data, int index, long acc, long expected)
    {
        if (index == data.Count)
        {
            return acc == expected;
        }
        
        return Test(data, index + 1, acc + data[index], expected) ||
               Test(data, index + 1, acc * data[index], expected);
    }
    
    private bool Test2(List<long> data, int index, long acc, long expected)
    {
        if (index == data.Count)
        {
            return acc == expected;
        }
        
        return Test2(data, index + 1, acc + data[index], expected) ||
               Test2(data, index + 1, acc * data[index], expected) ||
               Test2(data, index + 1, long.Parse(acc.ToString() + data[index].ToString()), expected);
    }
}