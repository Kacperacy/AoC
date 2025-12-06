using System.Reflection;
using AoCSolver;

static int ParseDay(Type t)
{
    var name = t.Name;
    if (name.StartsWith("Day", StringComparison.OrdinalIgnoreCase) &&
        int.TryParse(name.AsSpan(3), out var dayFromType))
        return dayFromType;

    var ns = t.Namespace ?? string.Empty;
    foreach (var seg in ns.Split('.'))
    {
        if (seg.StartsWith("Day", StringComparison.OrdinalIgnoreCase) &&
            int.TryParse(seg.AsSpan(3), out var dayFromNs))
            return dayFromNs;
    }

    return 0;
}

static int ParseYear(Type t)
{
    var ns = t.Namespace ?? string.Empty;
    foreach (var seg in ns.Split('.'))
    {
        if (seg.Length == 5 && seg[0] == '_' && int.TryParse(seg.AsSpan(1), out var y))
            return y;
        if (seg.Length == 4 && int.TryParse(seg, out var y4))
            return y4;
    }
    return 0;
}

var solverType = Assembly.GetExecutingAssembly().GetTypes()
    .Where(t => t.BaseType?.IsGenericType == true &&
                t.BaseType.GetGenericTypeDefinition() == typeof(Solver<,>))
    .Select(t => new { Type = t, Year = ParseYear(t), Day = ParseDay(t) })
    .OrderBy(x => x.Year)
    .ThenBy(x => x.Day)
    .Last().Type;

var solver = (SolverBase)Activator.CreateInstance(solverType)!;
solver.Solve();