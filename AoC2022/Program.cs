using System.Reflection;
using Spectre.Console;
using Spectre.Console.Cli;

var app = new CommandApp<PuzzleExecuter>();
return app.Run(args);

internal sealed class PuzzleExecuter : Command
{
    public override int Execute(CommandContext context)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var dayTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(PuzzleDay))).OrderBy(t => t.Name);

        var dayStr = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select [green]puzzle day[/]?")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal days)[/]")
                .AddChoices(dayTypes.Select(t => t.Name)));

        var selectedType = assembly.GetType(dayStr);
        if (Activator.CreateInstance(selectedType) is PuzzleDay day)
        {
            Console.WriteLine($"{dayStr}:");
            Console.WriteLine("Part1:");
            day.Part01();
            Console.WriteLine(Environment.NewLine + "Part2:");
            day.Part02();
        }

        return 0;
    }
}
