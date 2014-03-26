namespace CodeAnalysis.Models
{
    using System.Collections.Generic;

    public class CodeMetricsLineView : CodeMetricsLineModel
    {
        public CodeMetricsLineView()
        {
            Children = new List<CodeMetricsLineView>();
        }

        public double? MaintainabilityIndexDifference { get; set; }
        public double? CyclomaticComplexityDifference { get; set; }
        public double? DepthOfInheritanceDifference { get; set; }
        public double? ClassCouplingDifference { get; set; }
        public double? LinesOfCodeDifference { get; set; }
        public List<CodeMetricsLineView> Children { get; set; }
    }
}