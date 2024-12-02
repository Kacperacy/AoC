namespace AoCSolver.Day02;

public class Day02() : Solver<List<List<int>>, int>("Day02/input.txt")
{
    public override List<List<int>> PrepareData(List<string> input) =>
        input.Select(line => line.Split(" ").Select(int.Parse).ToList()).ToList();
        
    // 287
    public override int Part1(List<List<int>> data) =>
        data.Count(x =>
            x.Zip(x.Skip(1), (a, b) => b - a >= 1 && b - a <= 3).All(b => b) ||
            x.Zip(x.Skip(1), (a, b) => a - b >= 1 && a - b <= 3).All(b => b));

    // 354
    public override int Part2(List<List<int>> data) =>
        data.Count(x => Enumerable.Range(0, x.Count).Any(i => 
            x.Take(i).Concat(x.Skip(i + 1)).Zip(x.Take(i).Concat(x.Skip(i + 1)).Skip(1), (a, b) => b - a >= 1 && b - a <= 3).All(b => b) || 
            x.Take(i).Concat(x.Skip(i + 1)).Zip(x.Take(i).Concat(x.Skip(i + 1)).Skip(1), (a, b) => a - b >= 1 && a - b <= 3).All(b => b)));
}