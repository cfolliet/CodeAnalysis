namespace CodeAnalysis.Models
{
    using System.Collections.Generic;

    public class CodeMetricsLineView
    {
        public CodeMetricsLineView()
        {
            Children = new List<CodeMetricsLineView>();
        }

        public string Scope { get; set; }
        public string Project { get; set; }
        public string Namespace { get; set; }
        public string Type { get; set; }
        public string Member { get; set; }

        public double? MaintainabilityIndexDifference { get; set; }
        public double? CyclomaticComplexityDifference { get; set; }
        public double? DepthOfInheritanceDifference { get; set; }
        public double? ClassCouplingDifference { get; set; }
        public double? LinesOfCodeDifference { get; set; }

        public CodeMetricsLineModel CodeMetricsTrunk { get; set; }
        public CodeMetricsLineModel CodeMetricsBranche { get; set; }

        public List<CodeMetricsLineView> Children { get; set; }
    }
}