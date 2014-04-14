namespace CodeAnalysis.BusinessLogic
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using CodeAnalysis.Models;

    /// <summary>
    /// This class compares two code coverage files
    /// </summary>
    public class CodeCoverageGenerator
    {
        public static IEnumerable<CodeCoverageLineView> Generate(StreamReader codeCoverageTrunkFile, StreamReader codeCoverageBrancheFile)
        {
            List<CodeCoverageLineModel> codeCoverageTrunk = InitCodeMetrics(codeCoverageTrunkFile);
            codeCoverageTrunkFile.Close();

            List<CodeCoverageLineModel> codeCoverageBranche = InitCodeMetrics(codeCoverageBrancheFile);
            codeCoverageBrancheFile.Close();

            return new List<CodeCoverageLineView>();
        }

        /// <summary>
        /// Creates a list of CodeCoverageLineModel with information from the file
        /// </summary>
        private static List<CodeCoverageLineModel> InitCodeMetrics(StreamReader file)
        {
            var codeCoverage = new List<CodeCoverageLineModel>();
            var modules = XDocument.Parse(file.ReadLine());

            foreach (var module in modules.Descendants("Module"))
            {
            }

            return codeCoverage;
        }
    }
}