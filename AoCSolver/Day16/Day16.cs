namespace AoCSolver.Day16;

public class Day16() : Solver<Day16.Maze, int>("Day16/input.txt")
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

    public record struct Maze
    {
        public List<Point> Walls { get; set; }
        public Point Deer { get; set; }
        public Point Target { get; set; }

        public Maze(List<List<char>> data)
        {
            Walls = new List<Point>();
            for (var y = 0; y < data.Count; y++)
            {
                for (var x = 0; x < data[y].Count; x++)
                {
                    switch (data[y][x])
                    {
                        case '#':
                            Walls.Add(new Point(x, y));
                            break;
                        case 'S':
                            Deer = new Point(x, y);
                            break;
                        case 'E':
                            Target = new Point(x, y);
                            break;
                    }
                }
            }
        }
    }

    public override Maze PrepareData(List<string> input)
    {
        var data = input.Select(line => line.ToList()).ToList();
        return new Maze(data);
    }

    public override int Part1(Maze data)
    {
        const int moveScore = 1;
        const int turnScore = 1_000;

        var visited = new HashSet<Point>();
        var priorityQueue = new SortedSet<(int score, Point pos, char dir)>(
            Comparer<(int, Point, char)>.Create((a, b) =>
                a.Item1 == b.Item1
                    ? (a.Item2 == b.Item2
                        ? a.Item3.CompareTo(b.Item3)
                        : a.Item2.GetHashCode().CompareTo(b.Item2.GetHashCode()))
                    : a.Item1.CompareTo(b.Item1))) { (0, data.Deer, 'R') };

        while (priorityQueue.Count > 0)
        {
            var (score, pos, dir) = priorityQueue.Min;
            priorityQueue.Remove(priorityQueue.Min);
            if (pos == data.Target) return score;
            if (!visited.Add(pos)) continue;

            var nextScore = score + moveScore;
            var nextTurnScore = nextScore + turnScore;

            foreach (var nextDir in new[] { 'U', 'D', 'L', 'R' })
            {
                if (nextDir == 'U' && dir == 'D' || nextDir == 'D' && dir == 'U' ||
                    nextDir == 'L' && dir == 'R' || nextDir == 'R' && dir == 'L') continue;

                var nextPos = pos.Step(nextDir);
                if (data.Walls.Contains(nextPos)) continue;

                if (nextDir == dir)
                {
                    priorityQueue.Add((nextScore, nextPos, nextDir));
                }
                else
                {
                    priorityQueue.Add((nextTurnScore, nextPos, nextDir));
                }
            }
        }

        return -1;
    }

    public override int Part2(Maze data)
    {
        const int moveScore = 1;
        const int turnScore = 1_000;

        var queue = new PriorityQueue<(int score, Point pos, char dir, List<Point> path), int>();
        queue.Enqueue((0, data.Deer, 'R', new List<Point> { data.Deer }), 0);

        var bestPaths = new List<List<Point>>();
        var bestScore = int.MaxValue;
        var minScores = new Dictionary<(Point pos, char dir), int>();

        while (queue.Count > 0)
        {
            var (score, pos, dir, path) = queue.Dequeue();
            if (score > bestScore) continue;
            if (pos == data.Target)
            {
                if (score < bestScore)
                {
                    bestScore = score;
                    bestPaths.Clear();
                    bestPaths.Add(path);
                }
                else if (score == bestScore)
                {
                    bestPaths.Add(path);
                }

                continue;
            }

            var nextScore = score + moveScore;
            var nextTurnScore = nextScore + turnScore;

            foreach (var nextDir in new[] { 'U', 'D', 'L', 'R' })
            {
                if (nextDir == 'U' && dir == 'D' || nextDir == 'D' && dir == 'U' ||
                    nextDir == 'L' && dir == 'R' || nextDir == 'R' && dir == 'L') continue;

                var nextPos = pos.Step(nextDir);
                if (data.Walls.Contains(nextPos) || path.Contains(nextPos)) continue;

                var newPath = new List<Point>(path) { nextPos };
                var newScore = nextDir == dir ? nextScore : nextTurnScore;

                if (!minScores.TryGetValue((nextPos, nextDir), out var minScore) || newScore <= minScore)
                {
                    minScores[(nextPos, nextDir)] = newScore;
                    queue.Enqueue((newScore, nextPos, nextDir, newPath), newScore);
                }
            }
        }

        var uniquePaths = new HashSet<Point>(bestPaths.SelectMany(x => x));
        return uniquePaths.Count;
    }
}