namespace AoCSolver._2024.Day02;

public class Day02 : Solver<List<List<int>>, int>
{
    public override List<List<int>> PrepareData(List<string> input) =>
        input.Select(line => line.Split(" ").Select(int.Parse).ToList()).ToList();
        
    // 287
    public override int Part1(List<List<int>> data) =>
        data.Count(IsValid);

    // 354
    public override int Part2(List<List<int>> data) =>
        data.Count(x => Enumerable.Range(0, x.Count).Any(i =>
            IsValid(x.Take(i).Concat(x.Skip(i + 1)).ToList())));
    
    private bool IsValid(List<int> list) =>
        list.Zip(list.Skip(1), (a, b) => b - a >= 1 && b - a <= 3).All(b => b) ||
        list.Zip(list.Skip(1), (a, b) => a - b >= 1 && a - b <= 3).All(b => b);
}