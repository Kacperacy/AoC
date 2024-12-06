using System.Reflection;
using AoCSolver;

var solvers = Assembly.GetExecutingAssembly().GetTypes()
    .Where(t => t.BaseType?.IsGenericType == true && t.BaseType.GetGenericTypeDefinition() == typeof(Solver<,>))
    .OrderBy(t => int.Parse(t.Name[^2..]))
    .Select(t => (SolverBase)Activator.CreateInstance(t)!)
    .ToList();

solvers.Last().Solve();