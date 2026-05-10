using MiniGit.Models;
using System.Diagnostics;
using System.IO;

namespace MiniGit.Visualization;

public static class DagExporter
{
    public static void ExportPng(
        List<CommitNode> commits,
        string pngPath)
    {
        string dotPath = "Output/dag.dot";

        List<string> lines = new();

        lines.Add("digraph G {");

        // 왼쪽 → 오른쪽 방향
        lines.Add("rankdir=LR;");

        // branch 정렬 개선
        lines.Add("splines=ortho;");

        foreach (var commit in commits)
        {
            string color =
                commit.Parents.Count > 1
                ? "orange"
                : "lightblue";

            lines.Add(
                $"\"{commit.Id}\" " +
                $"[style=filled, fillcolor={color}, shape=circle];"
            );

            foreach (var parent in commit.Parents)
            {
                lines.Add(
                    $"\"{parent.Id}\" -> \"{commit.Id}\";"
                );
            }
        }

        lines.Add("}");

        File.WriteAllLines(dotPath, lines);

        ProcessStartInfo startInfo = new()
        {
            FileName =
    @"C:\Program Files\Graphviz\bin\dot.exe",

            Arguments =
                $"-Tpng \"{dotPath}\" -o \"{pngPath}\"",

            CreateNoWindow = true,
            UseShellExecute = false
        };

        Process process = Process.Start(startInfo)!;

        process.WaitForExit();
    }
}