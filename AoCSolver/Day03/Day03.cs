using System.Text.RegularExpressions;

namespace AoCSolver.Day03;

public class Day03() : Solver<List<List<int>>, int>("Day03/input.txt")
{
    public override List<List<int>> PrepareData(List<string> input)
    {
        var ret = new List<List<int>>();
        var regex = new Regex(@"mul\((\d+),(\d+)\)");
        var doRegex = new Regex(@"(don't|do)");
        var flag = "do";

        foreach (var line in input)
        {
            var matches = regex.Matches(line);
            var doMatch = doRegex.Matches(line);

            foreach (Match match in matches)
            {
                flag = doMatch.LastOrDefault(x => x.Index < match.Index)?.Value ?? flag;
                ret.Add(new List<int> { int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), flag == "do" ? 1 : 0 });
            }
            flag = doMatch.Last()?.Value ?? flag;
        }

        return ret;
    }

    public override int Part1(List<List<int>> data) =>
        data.Sum(x => x[0] * x[1]);

    public override int Part2(List<List<int>> data) =>
        data.Where(x => x[2] == 1).Sum(x => x[0] * x[1]);
}