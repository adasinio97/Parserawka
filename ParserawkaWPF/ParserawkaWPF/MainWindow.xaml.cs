using Microsoft.Win32;
using ParserawkaCore.Interfaces;
using ParserawkaCore.PQL;
using ParserawkaCore.PQL.AstElements;
using ParserawkaCore.PQL.Model;
using ParserawkaCore.Utils;
using System.IO;
using System.Windows;
using ParserawkaWPF.Utils;

namespace ParserawkaWPF
{
    public partial class MainWindow : Window
    {
        private string programCode;
        public IProgramKnowledgeBase PKB { get; }

        public MainWindow()
        {
            InitializeComponent();
            CodeTextBlock.SyntaxHighlighting.MainRuleSet.Rules.Add(HighlightRules.SimpleRule());
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
            ResultTextBox.Text = pqlOutput.ToString();
        }
        
        private void QuickFind(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point mousePoint = System.Windows.Input.Mouse.GetPosition(ResultTextBox);
            int charPosition = ResultTextBox.GetCharacterIndexFromPoint(mousePoint, true);
            if (charPosition > 0)
            {
                ResultTextBox.Focus();
                int index = 0;
                int i = 0;
                string[] strings = ResultTextBox.Text.Split(' ');
                while (index + strings[i].Length < charPosition && i < strings.Length)
                {
                    index += strings[i++].Length + 1;
                }
                ResultTextBox.Select(index, strings[i].Length);
                string statementLineString = "";
                foreach(var character in strings[i])
                {
                    if(char.IsDigit(character)) 
                    {
                        statementLineString += character;
                    }
                }
                if (statementLineString.Length > 1)
                {
                    double vertOffset = (CodeTextBlock.TextArea.TextView.DefaultLineHeight) * (int.Parse(statementLineString) - 1);
                    CodeTextBlock.ScrollToVerticalOffset(vertOffset);
                }
            }
        }
    }
}
