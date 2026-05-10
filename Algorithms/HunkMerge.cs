namespace MiniGit.Algorithms;

public static class HunkMerge
{
    public static string Merge(
        List<DiffOperation> aOps,
        List<DiffOperation> bOps)
    {
        List<string> result = new();

        int max =
            Math.Max(aOps.Count, bOps.Count);

        for (int i = 0; i < max; i++)
        {
            DiffOperation? a =
                i < aOps.Count
                ? aOps[i]
                : null;

            DiffOperation? b =
                i < bOps.Count
                ? bOps[i]
                : null;

            // 둘 다 없음
            if (a == null && b == null)
                continue;

            // A만 존재
            if (b == null)
            {
                if (a!.Type != DiffType.Delete)
                    result.Add(a.Line);

                continue;
            }

            // B만 존재
            if (a == null)
            {
                if (b.Type != DiffType.Delete)
                    result.Add(b.Line);

                continue;
            }

            // 둘 다 동일
            if (a.Line == b.Line &&
                a.Type == b.Type)
            {
                if (a.Type != DiffType.Delete)
                    result.Add(a.Line);

                continue;
            }

            // A만 변경
            if (a.Type != DiffType.Equal &&
                b.Type == DiffType.Equal)
            {
                if (a.Type != DiffType.Delete)
                    result.Add(a.Line);

                continue;
            }

            // B만 변경
            if (b.Type != DiffType.Equal &&
                a.Type == DiffType.Equal)
            {
                if (b.Type != DiffType.Delete)
                    result.Add(b.Line);

                continue;
            }

            // 충돌
            result.Add("<<<<<<< A");

            if (a.Type != DiffType.Delete)
                result.Add(a.Line);

            result.Add("=======");

            if (b.Type != DiffType.Delete)
                result.Add(b.Line);

            result.Add(">>>>>>> B");
        }

        return string.Join("\n", result);
    }
}