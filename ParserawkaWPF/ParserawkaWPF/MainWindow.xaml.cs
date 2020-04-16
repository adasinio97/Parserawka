using Microsoft.Win32;
using ParserawkaWPF.Interfaces;
using ParserawkaWPF.PQL;
using ParserawkaWPF.PQL.AstElements;
using ParserawkaWPF.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;


namespace ParserawkaWPF
{
    public partial class MainWindow : Window
    {
        private string programCode;
        public IProgramKnowledgeBase PKB { get; }

        public MainWindow()
        {
            InitializeComponent();
            PKB = ImplementationFactory.CreateProgramKnowledgeBase();
        }

        private void AnalyzeButton_Click(object sender, RoutedEventArgs e)
        {
            PKB.LoadData(programCode);
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                FileNameTextBlock.Text = dialog.FileName;
                programCode = File.ReadAllText(dialog.FileName);
            }
        }

        private void ResultButton_Click(object sender, RoutedEventArgs e)
        {
            string query = QueryTextBox.Text;
            Debug.WriteLine(query);
            PqlLexer lexer = new PqlLexer(query);
            PqlParser parser = new PqlParser(lexer);
            PqlAst pqlAst = parser.Parse();
        }
    }
}
