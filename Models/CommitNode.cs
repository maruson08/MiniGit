namespace MiniGit.Models;

public class CommitNode
{
    public string Id { get; set; }

    public string Message { get; set; }

    public string Content { get; set; }

    public List<CommitNode> Parents { get; set; }

    public CommitNode(
        string id,
        string message,
        string content,
        List<CommitNode>? parents = null)
    {
        Id = id;
        Message = message;
        Content = content;
        Parents = parents ?? new();
    }
}