namespace AoCSolver.Day10;

public class Day10() : Solver<List<List<char>>, int>("Day10/input.txt")
{
    public override List<List<char>> PrepareData(List<string> input) =>
        input.Select(line => line.ToList()).ToList();

    public override int Part1(List<List<char>> map)
    {
        var trailheads = map.SelectMany((row, y) => row.Select((cell, x) => (cell, x, y)))
            .Where(cell => cell.cell == '0')
            .Select(cell => (cell.x, cell.y))
            .ToList();
        var sum = 0;

        foreach (var trailhead in trailheads)
        {
            sum += CountHiking(map, trailhead, 0, new HashSet<(int, int)>());
        }

        return sum;
    }

    public override int Part2(List<List<char>> map)
    {
        var trailheads = map.SelectMany((row, y) => row.Select((cell, x) => (cell, x, y)))
            .Where(cell => cell.cell == '0')
            .Select(cell => (cell.x, cell.y))
            .ToList();
        var sum = 0;

        foreach (var trailhead in trailheads)
        {
            sum += CountHikingRating(map, trailhead, 0);
        }

        return sum;
    }

    private int CountHiking(List<List<char>> map, (int, int) pos, int next, HashSet<(int, int)> visited)
    {
        var (x, y) = pos;
        if (x < 0 || x >= map[0].Count || y < 0 || y >= map.Count || visited.Contains((x, y))) return 0;

        if (next == 9 && map[y][x] == '9' && visited.Add(pos))
        {
            return 1;
        }

        if (map[y][x] == next.ToString()[0])
        {
            return CountHiking(map, (x + 1, y), next + 1, visited) +
                   CountHiking(map, (x - 1, y), next + 1, visited) +
                   CountHiking(map, (x, y + 1), next + 1, visited) +
                   CountHiking(map, (x, y - 1), next + 1, visited);
        }

        return 0;
    }
    
    private int CountHikingRating(List<List<char>> map, (int, int) pos, int next)
    {
        var (x, y) = pos;
        if (x < 0 || x >= map[0].Count || y < 0 || y >= map.Count) return 0;

        if (next == 9 && map[y][x] == '9')
        {
            return 1;
        }

        if (map[y][x] == next.ToString()[0])
        {
            return CountHikingRating(map, (x + 1, y), next + 1) +
                   CountHikingRating(map, (x - 1, y), next + 1) +
                   CountHikingRating(map, (x, y + 1), next + 1) +
                   CountHikingRating(map, (x, y - 1), next + 1);
        }

        return 0;
    }
}