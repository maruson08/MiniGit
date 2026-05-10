namespace MiniGit.Algorithms;

public class DiffOperation
{
    public DiffType Type { get; set; }

    public string Line { get; set; }

    public DiffOperation(
        DiffType type,
        string line)
    {
        Type = type;
        Line = line;
    }
}