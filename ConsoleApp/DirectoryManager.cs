var result = new Dictionary<string, List<string>>();

while (true)
{
    var input = Console.ReadLine();
    var commands = input?.Split(' ');
    if (commands is null || commands.Length == 0)
    {
        continue;
    }

    switch (commands[0].ToUpperInvariant())
    {
        case "CREATE":
            if (commands.Length == 2)
            {
                Create(commands[1]);
            }
            break;
        case "MOVE":
            if (commands.Length == 3)
            {
                Move(commands[1], commands[2]);
            }
            break;
        case "DELETE":
            if (commands.Length == 2)
            {
                Delete(commands[1]);
            }
            break;
        case "LIST":
            var baseDirectories = result.Where(x => !x.Key.Contains('/')).OrderBy(x => x.Key).ToList();
            foreach (var directory in baseDirectories)
            {
                Console.WriteLine(directory.Key);
                GetSubDirectories(directory.Key, 1);
            }
            break;
        default: break;
    }
}

void Create(string baseDirectory)
{
    var directories = baseDirectory.Split('/');
    var currentDirectory = directories.First();

    foreach (var directory in directories)
    {
        if (!result.ContainsKey(currentDirectory))
        {
            result[currentDirectory] = new List<string>();
        }

        if (currentDirectory == directory)
        { 
            continue;
        }

        if (!result[currentDirectory].Contains(directory))
        {
            result[currentDirectory].Add(directory);
        }

        currentDirectory += $"/{directory}";
    }
}

void Move(string source, string destination)
{
    var sourceDirectories = source.Split('/');
    var directoryToMove = sourceDirectories.Last();
    var childDirectories = result
        .Where(x => x.Key.StartsWith(sourceDirectories.First()) && x.Key.Contains(directoryToMove))
        .SelectMany(x => x.Value.Select(value => $"{x.Key}/{value}"))
        .ToList();

    Create($"{destination}/{directoryToMove}");
    childDirectories.ForEach(x =>
    {
        var newDirectory = x[x.IndexOf(directoryToMove)..];
        Create($"{destination}/{newDirectory}");
        if (result.ContainsKey(x))
        {
            Delete(x);
        }
    });
    Delete(source);
}

void Delete(string baseDirectory)
{
    var directories = baseDirectory.Split('/');
    var currentDirectory = directories.First();

    for (int i = 0;  i < directories.Length; i++)
    {
        if (!result.ContainsKey(currentDirectory))
        {
            Console.WriteLine($"Cannot delete {baseDirectory} - {currentDirectory} does not exist");
            return;
        }

        if (currentDirectory == directories[i])
        {
            continue;
        }

        if (!result[currentDirectory].Contains(directories[i]))
        {
            Console.WriteLine($"Cannot delete {baseDirectory} - {directories[i]} does not exist");
            return;
        }

        if (i != (directories.Length - 1))
        {
            currentDirectory += $"/{directories[i]}";
        }
    }

    result[currentDirectory].Remove(directories.Last());
    if (result.ContainsKey(currentDirectory) && currentDirectory.Contains('/'))
    {
        result.Remove(currentDirectory);
    }

    if (result.ContainsKey(baseDirectory))
    {
        result.Remove(baseDirectory);
    }
}

void GetSubDirectories(string baseDirectory, int level)
{
    if (!result.ContainsKey(baseDirectory))
    {
        return;
    }

    foreach (var directory in result[baseDirectory].OrderBy(x => x))
    {
        Console.WriteLine($"{new string(' ', level * 2)}{directory}");
        GetSubDirectories($"{baseDirectory}/{directory}", level + 1);
    }
}