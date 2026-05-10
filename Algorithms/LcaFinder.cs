using MiniGit.Models;

namespace MiniGit.Algorithms;

public static class LcaFinder
{
    static void DFS(
        CommitNode node,
        HashSet<CommitNode> visited)
    {
        if (visited.Contains(node))
            return;

        visited.Add(node);

        foreach (var p in node.Parents)
            DFS(p, visited);
    }

    public static CommitNode FindLCA(
        CommitNode A,
        CommitNode B)
    {
        HashSet<CommitNode> aAnc = new();
        HashSet<CommitNode> bAnc = new();

        DFS(A, aAnc);
        DFS(B, bAnc);

        return aAnc.Intersect(bAnc).First();
    }
}