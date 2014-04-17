﻿namespace CodeAnalysis.BusinessLogic
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CodeAnalysis.Models;
    using OfficeOpenXml;

    /// <summary>
    /// This class compares two code metrics files
    /// </summary>
    public static class CodeMetricsExcelGenerator
    {
        public static IEnumerable<CodeMetricsLineView> Generate(string codeMetricsTrunkFilePath, string codeMetricsBrancheFilePath)
        {
            List<CodeMetricsLineModel> codeMetricsTrunk;
            using (var codeMetricsTrunkFile = new StreamReader(codeMetricsTrunkFilePath))
            {
                codeMetricsTrunk = InitCodeMetrics(codeMetricsTrunkFile);
            }

            List<CodeMetricsLineModel> codeMetricsBranche;
            using (var codeMetricsBrancheFile = new StreamReader(codeMetricsBrancheFilePath))
            {
                codeMetricsBranche = InitCodeMetrics(codeMetricsBrancheFile);
            }

            IEnumerable<CodeMetricsLineView> codeMetrics = InitCodeMetricsDifferences(codeMetricsTrunk, codeMetricsBranche);

            return InitCodeMetricsTree(codeMetrics);
        }

        /// <summary>
        /// Creates a list of CodeMetricsLineModel with information from the excel file
        /// </summary>
        private static List<CodeMetricsLineModel> InitCodeMetrics(StreamReader file)
        {
            var codeMetrics = new List<CodeMetricsLineModel>();

            using (var excelPackage = new ExcelPackage(file.BaseStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];

                for (int row = 2; worksheet.Cells[row, 1].Value != null; row++)
                {
                    var line = new CodeMetricsLineModel
                    {
                        Scope = ConvertString(worksheet.Cells[row, 1]),
                        Project = ConvertString(worksheet.Cells[row, 2]),
                        Namespace = ConvertString(worksheet.Cells[row, 3]),
                        Type = ConvertString(worksheet.Cells[row, 4]),
                        Member = ConvertString(worksheet.Cells[row, 5]),
                        MaintainabilityIndex = ConvertDouble(worksheet.Cells[row, 6]),
                        CyclomaticComplexity = ConvertDouble(worksheet.Cells[row, 7]),
                        DepthOfInheritance = ConvertDouble(worksheet.Cells[row, 8]),
                        ClassCoupling = ConvertDouble(worksheet.Cells[row, 9]),
                        LinesOfCode = ConvertDouble(worksheet.Cells[row, 10]),
                    };

                    if (!line.Project.Contains("Test"))
                    {
                        // Clean project row
                        line.Project = line.Project.Replace(" (Debug)", string.Empty);
                        line.Project = line.Project.Split('\\')[line.Project.Split('\\').Length - 1];
                        line.Project = line.Project.Replace("iTS.", "");

                        // Clean namespace row
                        line.Namespace = line.Namespace.Replace("iTS.", "");
                        line.Namespace = line.Namespace.Replace(line.Project + ".", "");
                        line.Namespace = line.Namespace.Replace("EF.", "");

                        codeMetrics.Add(line);
                    }
                }
            }

            return codeMetrics;
        }

        /// <summary>
        /// Converts an excel range to a string or null
        /// </summary>
        private static string ConvertString(ExcelRange cell)
        {
            return cell.Value != null ? cell.Value.ToString() : string.Empty;
        }

        /// <summary>
        /// Converts an excel range to a double? or null
        /// </summary>
        private static double? ConvertDouble(ExcelRange cell)
        {
            return cell.Value != null ? (double?)cell.Value : null;
        }

        /// <summary>
        /// Creates a list of CodeMetricsLineView containing difference of metrics between two lists of CodeMetricsLineModel
        /// </summary>
        private static IEnumerable<CodeMetricsLineView> InitCodeMetricsDifferences(IEnumerable<CodeMetricsLineModel> codeMetricsTrunk, List<CodeMetricsLineModel> codeMetricsBranche)
        {
            var codeMetrics = new List<CodeMetricsLineView>();

            foreach (CodeMetricsLineModel lineCodeMetricsTrunk in codeMetricsTrunk)
            {
                // Get the same line from the other project
                CodeMetricsLineModel lineCodeMetricsBranche = codeMetricsBranche
                    .FirstOrDefault(cm => cm.Scope == lineCodeMetricsTrunk.Scope
                                    && cm.Project == lineCodeMetricsTrunk.Project
                                    && cm.Namespace == lineCodeMetricsTrunk.Namespace
                                    && cm.Type == lineCodeMetricsTrunk.Type
                                    && cm.Member == lineCodeMetricsTrunk.Member);

                if (lineCodeMetricsBranche != null)
                {
                    codeMetrics.Add(new CodeMetricsLineView()
                    {
                        Scope = lineCodeMetricsBranche.Scope,
                        Project = lineCodeMetricsTrunk.Project,
                        Namespace = lineCodeMetricsTrunk.Namespace,
                        Type = lineCodeMetricsTrunk.Type,
                        Member = lineCodeMetricsTrunk.Member,

                        MaintainabilityIndexDifference = -(lineCodeMetricsTrunk.MaintainabilityIndex - lineCodeMetricsBranche.MaintainabilityIndex),
                        CyclomaticComplexityDifference = lineCodeMetricsTrunk.CyclomaticComplexity - lineCodeMetricsBranche.CyclomaticComplexity,
                        DepthOfInheritanceDifference = lineCodeMetricsTrunk.DepthOfInheritance - lineCodeMetricsBranche.DepthOfInheritance,
                        ClassCouplingDifference = lineCodeMetricsTrunk.ClassCoupling - lineCodeMetricsBranche.ClassCoupling,
                        LinesOfCodeDifference = lineCodeMetricsTrunk.LinesOfCode - lineCodeMetricsBranche.LinesOfCode,

                        CodeMetricsTrunk = lineCodeMetricsTrunk,
                        CodeMetricsBranche = lineCodeMetricsBranche
                    });
                }
            }

            return codeMetrics;
        }

        /// <summary>
        /// Init the tree of code metrics
        /// </summary>
        private static IEnumerable<CodeMetricsLineView> InitCodeMetricsTree(IEnumerable<CodeMetricsLineView> codeMetrics)
        {
            var list = new List<CodeMetricsLineView>();

            foreach (CodeMetricsLineView line in codeMetrics)
            {
                switch (line.Scope)
                {
                    case "Project":
                        list.Add(line);
                        break;

                    case "Namespace":
                        list.Last().Children.Add(line);
                        break;

                    case "Type":
                        list.Last().Children.Last().Children.Add(line);
                        break;

                    case "Member":
                        list.Last().Children.Last().Children.Last().Children.Add(line);
                        break;
                }
            }

            return list;
        }
    }
}