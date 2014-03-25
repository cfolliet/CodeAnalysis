namespace CodeAnalysis.Models
{
    public class CodeMetricsLineView : CodeMetricsLineModel
    {
        public double? MaintainabilityIndexDifference { get; set; }
        public double? CyclomaticComplexityDifference { get; set; }
        public double? DepthOfInheritanceDifference { get; set; }
        public double? ClassCouplingDifference { get; set; }
        public double? LinesOfCodeDifference { get; set; }
    }
}