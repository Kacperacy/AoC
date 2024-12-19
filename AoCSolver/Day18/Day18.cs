namespace AoCSolver.Day18;

public class Day18() : Solver<List<Day18.Point>, string>("Day18/input.txt")
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

     public override List<Point> PrepareData(List<string> input)
     {
          var data = new List<Point>();
          foreach (var line in input)
          {
               var parts = line.Split(",");
               data.Add(new Point(int.Parse(parts[0]), int.Parse(parts[1])));
          }
          return data;
     }

     public override string Part1(List<Point> data)
     {
          var walls = data.Take(1024).ToHashSet();

          return FindPath(walls, new Point(0, 0), new Point(70, 70)).ToString();
     }
     
     public override string Part2(List<Point> data)
     {
          var start = new Point(0, 0);
          var end = new Point(70, 70);
          for (var i = 1024; i <= data.Count; i++)
          {
               if (FindPath(data.Take(i).ToHashSet(), start, end) == -1)
               {
                    return data[i - 1].ToString();
               }
          }
          
          return "No solution found";
     }
     
     private int FindPath(HashSet<Point> walls, Point start, Point end)
     {
          var queue = new PriorityQueue<Point, int>();
          queue.Enqueue(start, 0);
          var visited = new HashSet<Point>();
          var gScore = new Dictionary<Point, int> { [start] = 0 };

          while (queue.Count > 0)
          {
               var current = queue.Dequeue();
               if (current == end)
               {
                    return gScore[current];
               }

               visited.Add(current);

               foreach (var dir in "UDLR")
               {
                    var next = current.Step(dir);
                    if (next.X < 0 || next.X > 70 || next.Y < 0 || next.Y > 70 || walls.Contains(next) || visited.Contains(next))
                    {
                         continue;
                    }

                    var tentativeGScore = gScore[current] + 1;
                    if (!gScore.ContainsKey(next) || tentativeGScore < gScore[next])
                    {
                         gScore[next] = tentativeGScore;
                         queue.Enqueue(next, tentativeGScore);
                    }
               }
          }

          return -1;
     }
}