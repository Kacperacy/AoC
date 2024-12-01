using System.Reflection;
using AoCSolver;

var solvers = Assembly.GetExecutingAssembly().GetTypes()
    .Where(t => t.BaseType?.IsGenericType == true && t.BaseType.GetGenericTypeDefinition() == typeof(Solver<,>))
    .OrderBy(t => int.Parse(t.Name[^2..]))
    .Select(t => (SolverBase)Activator.CreateInstance(t)!)
    .ToList();

solvers.Last().Solve();
        
        
while (true)
{
    Console.WriteLine("\nPress A to run all solvers, or enter a specific day to run that solver.");
    var input = Console.ReadLine();
    if (input == "A")
    {
        Console.WriteLine("Running all solvers...");
        foreach (var solver in solvers)
        {
            solver.Solve();
        }
    }
    else if (int.TryParse(input, out var day) && day >= 1)
    {
        solvers.First(x => x.GetType().Name[^2..] == day.ToString()).Solve();
    }
    else
    {
        Console.WriteLine("Invalid input.");
    }
}