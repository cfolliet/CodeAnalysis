namespace CodeAnalysis.Models
{
    using System.Collections.Generic;

    public class CodeCoverageLineView
    {
        public CodeCoverageLineView()
        {
            Children = new List<CodeCoverageLineView>();
        }

        public List<CodeCoverageLineView> Children { get; set; }
    }
}