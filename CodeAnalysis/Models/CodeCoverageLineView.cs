namespace CodeAnalysis.Models
{
    using System.Collections.Generic;

    public class CodeCoverageLineView
    {
        public CodeCoverageLineView()
        {
            Children = new List<CodeCoverageLineView>();
        }

        public string Scope { get; set; }
        public string Project { get; set; }
        public string Namespace { get; set; }
        public string Type { get; set; }
        public string Member { get; set; }

        public double? CoveredLinePercentageDifference { get; set; }
        public double? CoveredLineNumberDifference { get; set; }

        public CodeCoverageLineModel CodeCoverageTrunk { get; set; }
        public CodeCoverageLineModel CodeCoverageBranche { get; set; }

        public List<CodeCoverageLineView> Children { get; set; }
    }
}