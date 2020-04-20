using ICSharpCode.AvalonEdit.Highlighting;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace ParserawkaWPF.Utils
{
    public static class HighlightRules
    {
        public static HighlightingRule SimpleRule()
        {
            var newHighlightRule = new HighlightingRule();
            newHighlightRule.Color = new HighlightingColor()
            {
                FontWeight = FontWeights.Bold,
                Foreground = new SimpleHighlightingBrush(Color.FromArgb(244, 0, 51, 0))
            };

            newHighlightRule.Regex = new Regex(@"\b(?>procedure|call)\b");
            return newHighlightRule;
        }
    }
}
