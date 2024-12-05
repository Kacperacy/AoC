namespace AoCSolver.Day05;

public class Day05() : Solver<List<List<List<int>>>, int>("Day05/input.txt")
{
    public override List<List<List<int>>> PrepareData(List<string> input)
    {
        var index = input.IndexOf("");
        
        return
        [
            input.Take(index).Select(x => x.Split("|").Select(int.Parse).ToList()).ToList(),
            input.Skip(index + 1).Select(x => x.Split(",").Select(int.Parse).ToList()).ToList()
        ];
    }

    public override int Part1(List<List<List<int>>> data)
    {
        var sum = 0;

        foreach (var line in data[1])
        {
            var valid = true;

            for (var a = 0; a < line.Count - 1; a++)
            {
                for (var b = a; b < line.Count; b++)
                {
                    if (data[0].Any(x => x[0] == line[b] && x[1] == line[a]))
                    {
                        valid = false;
                        break;
                    }
                }
            }
            
            if (valid)
            {
                sum += line[line.Count / 2];
            }
        }
        
        return sum;
    }

    public override int Part2(List<List<List<int>>> data)
    {
        var sum = 0;

        foreach (var line in data[1])
        {
            var valid = true;

            for (var a = 0; a < line.Count - 1; a++)
            {
                for (var b = a; b < line.Count; b++)
                {
                    if (data[0].Any(x => x[0] == line[b] && x[1] == line[a]))
                    {
                        valid = false;
                        (line[b], line[a]) = (line[a], line[b]);
                    }
                }
            }
            
            if (!valid)
            {
                sum += line[line.Count / 2];
            }
        }
        
        return sum;
    }
}