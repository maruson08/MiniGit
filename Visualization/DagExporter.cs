using MiniGit.Models;
using System.IO;
using System.Drawing;
namespace MiniGit.Visualization;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using System.Collections.Generic;


public static class DagExporter
{
    public static void ExportPng(
        List<CommitNode> commits,
        string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return;
        }

        Directory.CreateDirectory(
            Path.GetDirectoryName(path)!
        );

        Graph graph = new();

        foreach (var commit in commits)
        {
            graph.AddNode(commit.Id);

            foreach (var parent in commit.Parents)
            {
                graph.AddEdge(
                    parent.Id,
                    commit.Id
                );
            }
        }

        GraphRenderer renderer =
            new(graph);

        renderer.CalculateLayout();

        int width = 1200;
        int height = 800;

        using Bitmap bitmap =
            new(width, height);

        renderer.Render(bitmap);

        bitmap.Save(path);
    }
}