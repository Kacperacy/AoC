namespace AoCSolver.Day12;

public class Day12() : Solver<Dictionary<Day12.Point, char>, int>("Day12/input.txt")
{
    public readonly record struct Point(int X, int Y)
    {
        public Point Step(char dir) => dir switch
        {
            'U' => this with { Y = Y - 1 },
            'D' => this with { Y = Y + 1 },
            'L' => this with { X = X - 1 },
            'R' => this with { X = X + 1 },
            _ => this
        };
    }

    public override Dictionary<Point, char> PrepareData(List<string> input)
    {
        var map = new Dictionary<Point, char>();

        for (var y = 0; y < input.Count; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                map[new Point(x, y)] = input[y][x];
            }
        }

        return map;
    }

    public override int Part1(Dictionary<Point, char> farm)
    {
        return CalculatePrice(farm, FencePrice);
    }

    public override int Part2(Dictionary<Point, char> farm)
    {
        return CalculatePrice(farm, FencePriceDiscount);
    }

    private int CalculatePrice(Dictionary<Point, char> farm, Func<Dictionary<Point, char>, HashSet<Point>, char, int> fencePriceFunc)
    {
        var keys = farm.Keys.ToHashSet();
        var price = 0;

        while (keys.Count > 0)
        {
            var flood = Flood(farm, keys.First());
            price += flood.Count * fencePriceFunc(farm, flood, farm[keys.First()]);
            keys.ExceptWith(flood);
        }

        return price;
    }

    private HashSet<Point> Flood(Dictionary<Point, char> farm, Point start)
    {
        var type = farm[start];
        var flood = new HashSet<Point>();
        var q = new Queue<Point>();
        q.Enqueue(start);

        while (q.TryDequeue(out var pos))
        {
            if (!flood.Add(pos))
            {
                continue;
            }

            foreach (var point in "UDLR".Select(pos.Step))
            {
                if (farm.TryGetValue(point, out var c) && c == type)
                {
                    q.Enqueue(point);
                }
            }
        }

        return flood;
    }

    private int FencePrice(Dictionary<Point, char> farm, HashSet<Point> flood, char type)
    {
        var fence = new HashSet<(Point, char)>();

        foreach (var pos in flood)
        {
            foreach (var dir in "UDLR")
            {
                var point = pos.Step(dir);

                if (!farm.TryGetValue(point, out var c) || c != type)
                {
                    fence.Add((pos, dir));
                }
            }
        }

        return fence.Count;
    }

    private int FencePriceDiscount(Dictionary<Point, char> farm, HashSet<Point> flood, char type)
    {
        var fence = new HashSet<(Point, char)>();

        foreach (var pos in flood)
        {
            foreach (var dir in "UDLR")
            {
                var point = pos.Step(dir);

                if (!farm.TryGetValue(point, out var c) || c != type)
                {
                    fence.Add((pos, dir));
                }
            }
        }

        var count = 0;

        foreach (var (p, t) in fence)
        {
            var d = fence.Contains((p.Step('D'), t));
            var r = fence.Contains((p.Step('R'), t));

            if (!(d || r))
            {
                count++;
            }
        }

        return count;
    }
}