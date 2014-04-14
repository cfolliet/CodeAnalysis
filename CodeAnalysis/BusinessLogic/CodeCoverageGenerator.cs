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
        public static IEnumerable<CodeCoverageLineView> Generate(StreamReader codeCoverageTrunkFile, StreamReader codeCoverageBrancheFile)
        {
            string line;

            while ((line = codeCoverageTrunkFile.ReadLine()) != null)
            {
                // treat file here
            }

            codeCoverageTrunkFile.Close();
            codeCoverageBrancheFile.Close();

            return new List<CodeCoverageLineView>();
        }
    }
}