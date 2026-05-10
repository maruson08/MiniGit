namespace MiniGit.Algorithms;

public static class MyersDiff
{
    static List<string> SplitLines(string text)
    {
        return text
            .Replace("\r\n", "\n")
            .Split('\n')
            .Where(x => x != "")
            .ToList();
    }

    public static List<DiffOperation> Diff(
        string oldText,
        string newText)
    {
        List<string> A =
            SplitLines(oldText);

        List<string> B =
            SplitLines(newText);

        int n = A.Count;
        int m = B.Count;

        int[,] dp =
            new int[n + 1, m + 1];

        // LCS DP 계산
        for (int i = n - 1; i >= 0; i--)
        {
            for (int j = m - 1; j >= 0; j--)
            {
                if (A[i] == B[j])
                {
                    dp[i, j] =
                        dp[i + 1, j + 1] + 1;
                }
                else
                {
                    dp[i, j] =
                        Math.Max(
                            dp[i + 1, j],
                            dp[i, j + 1]
                        );
                }
            }
        }

        List<DiffOperation> ops = new();

        int x = 0;
        int y = 0;

        while (x < n && y < m)
        {
            if (A[x] == B[y])
            {
                ops.Add(
                    new DiffOperation(
                        DiffType.Equal,
                        A[x]
                    )
                );

                x++;
                y++;
            }
            else if (dp[x + 1, y] >=
                     dp[x, y + 1])
            {
                ops.Add(
                    new DiffOperation(
                        DiffType.Delete,
                        A[x]
                    )
                );

                x++;
            }
            else
            {
                ops.Add(
                    new DiffOperation(
                        DiffType.Insert,
                        B[y]
                    )
                );

                y++;
            }
        }

        while (x < n)
        {
            ops.Add(
                new DiffOperation(
                    DiffType.Delete,
                    A[x]
                )
            );

            x++;
        }

        while (y < m)
        {
            ops.Add(
                new DiffOperation(
                    DiffType.Insert,
                    B[y]
                )
            );

            y++;
        }

        return ops;
    }
}