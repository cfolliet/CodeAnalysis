namespace CodeAnalysis.BusinessLogic
{
    using CodeAnalysis.Models;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// This class compares two code coverage files
    /// </summary>
    public class CodeCoverageGenerator
    {
        public static IEnumerable<CodeCoverageLineView> Generate(StreamReader codeCoverageTrunkXml, StreamReader codeCoverageBrancheXml)
        {
            string line;

            while ((line = codeCoverageTrunkXml.ReadLine()) != null)
            {
                // treat file here
            }

            codeCoverageTrunkXml.Close();
            codeCoverageBrancheXml.Close();

            return new List<CodeCoverageLineView>();
        }
    }
}