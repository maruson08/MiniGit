using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

using MiniGit.Models;

namespace MiniGit;

public partial class MainWindow : Window
{
    Repository repo = new();

    Dictionary<string, CommitNode?> branches
        = new();

    string currentBranch = "main";

    string? trackedFilePath;

    public MainWindow()
{
    InitializeComponent();

    branches["main"] = null;

    BranchSelector.Items.Add("main");

    MergeBranchSelector.Items.Add("main");

    BranchSelector.SelectedIndex = 0;

    MergeBranchSelector.SelectedIndex = 0;

    BranchSelector.SelectionChanged +=
        BranchSelector_SelectionChanged;
    
    
}

    private void OpenFile_Click(
        object sender,
        RoutedEventArgs e)
    {
        OpenFileDialog dialog = new();

        dialog.Filter =
            "Text Files (*.txt)|*.txt";

        if (dialog.ShowDialog() == true)
        {
            trackedFilePath =
                dialog.FileName!;

            EditorBox.Text =
                File.ReadAllText(
                    trackedFilePath
                );
        }
    }

    private void Commit_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (trackedFilePath == null)
        {
            MessageBox.Show(
                "Open file first"
            );

            return;
        }

        File.WriteAllText(
            trackedFilePath,
            EditorBox.Text
        );

        CommitNode? parent =
            branches[currentBranch];

        CommitNode commit =
            repo.Commit(
                trackedFilePath,
                $"Commit on {currentBranch}",
                parent
            );

        branches[currentBranch] =
            commit;

        MessageBox.Show(
            $"Commit: {commit.Id}"
        );
    }

private void CreateBranch_Click(
    object sender,
    RoutedEventArgs e)
{
    string branchName =
        $"branch-{branches.Count}";

    branches[branchName] =
        branches[currentBranch];

    BranchSelector.Items.Add(branchName);

    MergeBranchSelector.Items.Add(branchName);

    MessageBox.Show(
        $"Created {branchName}"
    );
}

    private void BranchSelector_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e)
    {
        if (BranchSelector.SelectedItem == null)
            return;

        currentBranch =
            BranchSelector.SelectedItem.ToString()!;
    }
}