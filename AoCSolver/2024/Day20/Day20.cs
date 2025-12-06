namespace AoCSolver._2024.Day20;

public class Day20 : Solver<Day20.Maze, long>
{
    public record struct Maze(char[][] Map, (int x, int y) Start, (int x, int y) End);

    private static readonly (int x, int y)[] Neighbors = 
    {
        (0, 1), (1, 0), (0, -1), (-1, 0)
    };

    public override Maze PrepareData(List<string> input)
    {
        var map = input.Select(line => line.ToCharArray()).ToArray();
        var start = map.SelectMany((row, y) => row.Select((cell, x) => (cell, x, y)))
                       .First(p => p.cell == 'S');
        var end = map.SelectMany((row, y) => row.Select((cell, x) => (cell, x, y)))
                     .First(p => p.cell == 'E');
        return new Maze(map, (start.x, start.y), (end.x, end.y));
    }

    public override long Part1(Maze data)
    {
        var paths = GetShortestPaths(data.Start, data.Map);
        return paths.Keys
            .SelectMany(p => Neighbors
                .Select(d =>
                    (
                        p,
                        q: (x: p.x + d.x + d.x, y: p.y + d.y + d.y)
                    )
                )
                .Where(x =>
                    x.q.x.Between(0, data.Map[0].Length - 1)
                    && x.q.y.Between(0, data.Map.Length - 1)
                    && data.Map[x.q.y][x.q.x] != '#'
                )
            )
            .Count(x => paths[x.q].cost - paths[x.p].cost - 2 >= 100);
    }

    public override long Part2(Maze data)
    {
        var paths = GetShortestPaths(data.Start, data.Map);
        var sum = 0L;
        foreach (var (x, y) in paths.Keys)
        {
            var startCost = paths[(x, y)].cost;

            for (var dy = -20; dy <= +20; dy++)
            {
                var mindx = Math.Abs(dy) - 20;
                var maxdx = -mindx;
                for (var dx = mindx; dx <= maxdx; dx++)
                {
                    if (!paths.TryGetValue((x + dx, y + dy), out var value))
                        continue;

                    var deltaCost = value.cost - startCost;
                    deltaCost -= Math.Abs(dy) + Math.Abs(dx);
                    if (deltaCost >= 100)
                        sum++;
                }
            }
        }

        return sum;
    }

    private Dictionary<(int x, int y), (int cost, (int x, int y) prev)> GetShortestPaths((int x, int y) start, char[][] map)
    {
        var paths = new Dictionary<(int x, int y), (int cost, (int x, int y) prev)>();
        var queue = new PriorityQueue<(int x, int y), int>();
        queue.Enqueue(start, 0);
        paths[start] = (0, start);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            var currentCost = paths[current].cost;

            foreach (var neighbor in GetNeighbors(current, map))
            {
                var newCost = currentCost + 1;
                if (!paths.ContainsKey(neighbor) || newCost < paths[neighbor].cost)
                {
                    paths[neighbor] = (newCost, current);
                    queue.Enqueue(neighbor, newCost);
                }
            }
        }

        return paths;
    }

    private IEnumerable<(int x, int y)> GetNeighbors((int x, int y) point, char[][] map)
    {
        foreach (var (dx, dy) in Neighbors)
        {
            var nx = point.x + dx;
            var ny = point.y + dy;
            if (nx >= 0 && nx < map[0].Length && ny >= 0 && ny < map.Length && map[ny][nx] != '#')
            {
                yield return (nx, ny);
            }
        }
    }
}

public static class Extensions
{
    public static bool Between(this int value, int min, int max)
    {
        return value >= min && value <= max;
    }
}