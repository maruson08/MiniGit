namespace MiniGit.Algorithms;

public static class MergeEngine
{
    public static string Merge(
        string baseText,
        string AText,
        string BText)
    {
        List<DiffOperation> aOps =
            MyersDiff.Diff(
                baseText,
                AText
            );

        List<DiffOperation> bOps =
            MyersDiff.Diff(
                baseText,
                BText
            );

        return HunkMerge.Merge(
            aOps,
            bOps
        );
    }
}