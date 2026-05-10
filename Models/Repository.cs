using System.IO;
using MiniGit.Algorithms;

namespace MiniGit.Models;

public class Repository
{
    public List<CommitNode> Commits = new();

    public CommitNode Commit(
        string filePath,
        string message,
        CommitNode? parent = null)
    {
        string content =
            File.ReadAllText(filePath);

        CommitNode commit = new(
            Guid.NewGuid().ToString()[..8],
            message,
            content,
            parent != null
                ? new List<CommitNode> { parent }
                : new()
        );

        Commits.Add(commit);

        return commit;
    }

    public CommitNode Merge(
        CommitNode A,
        CommitNode B)
    {
        CommitNode lca =
            LcaFinder.FindLCA(A, B);

        string merged =
            MergeEngine.Merge(
                lca.Content,
                A.Content,
                B.Content
            );

        CommitNode mergeCommit = new(
            "merge-" + Guid.NewGuid().ToString()[..6],
            "Merge Commit",
            merged,
            new() { A, B }
        );

        Commits.Add(mergeCommit);

        return mergeCommit;
    }
}