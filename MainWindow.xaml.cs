using System.Windows;
using System.IO;
using MiniGit.Models;
using MiniGit.Visualization;

namespace MiniGit;

public partial class MainWindow : Window
{
    Repository repo = new();

Dictionary<string, CommitNode?> branches = new();

string currentBranch = "main";
public MainWindow()
{
    InitializeComponent();

    EditorBox.Text =
        File.ReadAllText("tracked.txt");

    branches["main"] = null;

    BranchSelector.Items.Add("main");
    MergeBranchSelector.Items.Add("main");

    BranchSelector.SelectedIndex = 0;
    MergeBranchSelector.SelectedIndex = 0;

    BranchSelector.SelectionChanged +=
        BranchSelector_SelectionChanged;
}

private void BranchSelector_SelectionChanged(
    object sender,
    System.Windows.Controls.SelectionChangedEventArgs e)
{
    if (BranchSelector.SelectedItem != null)
    {
        currentBranch =
            BranchSelector.SelectedItem.ToString()!;
    }
}

    private void Commit_Click(
    object sender,
    RoutedEventArgs e)
{
    File.WriteAllText(
        "tracked.txt",
        EditorBox.Text
    );

    CommitNode? parent =
        branches[currentBranch];

    CommitNode commit =
        repo.Commit(
            "tracked.txt",
            $"Commit on {currentBranch}",
            parent
        );

    branches[currentBranch] = commit;

    RenderDag();
}
private void CreateBranch_Click(
    object sender,
    RoutedEventArgs e)
{
    string newBranch =
        $"branch-{branches.Count}";

    branches[newBranch] =
        branches[currentBranch];

    BranchSelector.Items.Add(newBranch);

    MergeBranchSelector.Items.Add(newBranch);

    MessageBox.Show(
        $"Created {newBranch}"
    );
}


    private void Merge_Click(
    object sender,
    RoutedEventArgs e)
{
    if (MergeBranchSelector.SelectedItem == null)
    {
        MessageBox.Show(
            "Select merge branch"
        );

        return;
    }

    string mergeFrom =
        MergeBranchSelector
            .SelectedItem
            .ToString()!;

    // 자기 자신 merge 방지
    if (mergeFrom == currentBranch)
    {
        MessageBox.Show(
            "Cannot merge same branch"
        );

        return;
    }

    CommitNode? current =
        branches[currentBranch];

    CommitNode? other =
        branches[mergeFrom];

    if (current == null || other == null)
    {
        MessageBox.Show(
            "Both branches need commits"
        );

        return;
    }

    CommitNode merged =
        repo.Merge(current, other);

    // 현재 branch HEAD 이동
    branches[currentBranch] = merged;

    MergeResultBox.Text =
        merged.Content;

    RenderDag();
}

    private void RenderDag()
{
    DagExporter.ExportPng(
        repo.Commits,
        "Output/dag.png"
    );

    if (File.Exists("Output/dag.png"))
    {
        // 기존 이미지 제거
        DagImage.Source = null;

        var bitmap =
            new System.Windows.Media.Imaging.BitmapImage();

        bitmap.BeginInit();

        bitmap.CacheOption =
            System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;

        // 이미지 캐시 무시
        bitmap.CreateOptions =
            System.Windows.Media.Imaging.BitmapCreateOptions.IgnoreImageCache;

        bitmap.UriSource =
            new Uri(
                Path.GetFullPath("Output/dag.png")
            );

        bitmap.EndInit();

        // 파일 잠금 방지
        bitmap.Freeze();

        DagImage.Source = bitmap;
    }
}
}