namespace AoCSolver.Day1;

public class Day01() : Solver<List<List<int>>, int>("Day01/input.txt")
{
    public override List<List<int>> PrepareData(List<string> input)
    {
        var columns = input
            .Select(line => line.Split("   ")
                .Select(int.Parse).ToList())
            .Aggregate((col1: new List<int>(), col2: new List<int>()), (acc, row) =>
            {
                acc.col1.Add(row[0]);
                acc.col2.Add(row[1]);
                return acc;
            });

        columns.col1.Sort();
        columns.col2.Sort();

        return [columns.col1, columns.col2];
    }

    public override int Part1(List<List<int>> data)
    {
        return data[0].Zip(data[1], (a, b) => Math.Abs(a - b)).Sum();
    }

    public override int Part2(List<List<int>> data)
    {
        return data[0].Sum(x => x * data[1].Count(c => c == x));
    }
}