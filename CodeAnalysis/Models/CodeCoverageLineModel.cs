namespace CodeAnalysis.Models
{
    public class CodeCoverageLineModel
    {
        public string Scope { get; set; }
        public string Project { get; set; }
        public string Namespace { get; set; }
        public string Type { get; set; }
        public string Member { get; set; }

        public double? CoveredLinePercentage { get; set; }
        public double? CoveredLine { get; set; }
    }
}