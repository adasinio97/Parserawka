using Microsoft.Win32;
using ParserawkaCore.Interfaces;
using ParserawkaCore.PQL;
using ParserawkaCore.PQL.AstElements;
using ParserawkaCore.PQL.Model;
using ParserawkaCore.Utils;
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
            ResultButton.IsEnabled = true;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                FileNameTextBlock.Text = dialog.FileName;
                programCode = File.ReadAllText(dialog.FileName);
                CodeTextBlock.Text = programCode;
                AnalyzeButton.IsEnabled = true;
            }
        }

        private void ResultButton_Click(object sender, RoutedEventArgs e)
        {
            string query = QueryTextBox.Text;
            PqlLexer lexer = new PqlLexer(query);
            PqlParser parser = new PqlParser(lexer);
            PqlAst pqlAst = parser.Parse();
            PqlEvaluator pqlEvaluator = new PqlEvaluator(PKB, pqlAst);
            PqlOutput pqlOutput = pqlEvaluator.Evaluate();
            ResultTextBlock.Text = pqlOutput.ToString();
        }
    }
}
