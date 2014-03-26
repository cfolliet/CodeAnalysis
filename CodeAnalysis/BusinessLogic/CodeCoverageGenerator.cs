namespace CodeAnalysis.BusinessLogic
{
    using System.IO;

    /// <summary>
    /// This class compares two code coverage files
    /// </summary>
    public class CodeCoverageGenerator
    {
        public static void Generate(StreamReader codeCoverageTrunkXml, StreamReader codeCoverageBrancheXml)
        {
            string line;

            while ((line = codeCoverageTrunkXml.ReadLine()) != null)
            {
                // treat file here
            }

            codeCoverageTrunkXml.Close();
            codeCoverageBrancheXml.Close();
        }
    }
}