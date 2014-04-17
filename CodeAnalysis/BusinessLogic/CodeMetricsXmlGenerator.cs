namespace CodeAnalysis.BusinessLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using CodeAnalysis.Models;

    /// <summary>
    /// This class compares two code metrics files
    /// </summary>
    public static class CodeMetricsXmlGenerator
    {
        public static IEnumerable<CodeMetricsLineView> Generate(string codeMetricsTrunkFilePath, string codeMetricsBrancheFilePath)
        {
            List<CodeMetricsLineModel> codeMetricsTrunk = InitCodeMetrics(codeMetricsTrunkFilePath);
            List<CodeMetricsLineModel> codeMetricsBranche = InitCodeMetrics(codeMetricsBrancheFilePath);
            IEnumerable<CodeMetricsLineView> codeMetrics = InitCodeMetricsDifferences(codeMetricsTrunk, codeMetricsBranche);

            return InitCodeMetricsTree(codeMetrics);
        }

        /// <summary>
        /// Creates a list of CodeMetricsLineModel with information from the xml file
        /// </summary>
        private static List<CodeMetricsLineModel> InitCodeMetrics(string file)
        {
            var codeMetrics = new List<CodeMetricsLineModel>();

            var document = XElement.Load(file);

            LoadLevel(document, null, codeMetrics, "Module", SetProject);
            
            return codeMetrics;
        }

        private static void LoadLevel(XElement node, CodeMetricsLineModel typeLine, List<CodeMetricsLineModel> codeMetrics, string descendantName, Action<CodeMetricsLineModel, CodeMetricsLineModel, string> setModelAction)
        {
            foreach (var subNode in node.Descendants(descendantName))
            {
                var subLine = new CodeMetricsLineModel { Scope = descendantName };

                setModelAction(typeLine, subLine, subNode.Attribute("Name").Value);

                GetMetrics(subNode, subLine);
                CleanNaming(subLine);
                codeMetrics.Add(subLine);

                switch (descendantName)
                {
                    case "Module":
                        LoadLevel(subNode, subLine, codeMetrics, "Namespace", SetNamespace);
                        break;
                    case "Namespace":
                        LoadLevel(subNode, subLine, codeMetrics, "Type", SetType);
                        break;
                    case "Type":
                        LoadLevel(subNode, subLine, codeMetrics, "Member", SetMember);
                        break;
                    case "Member":
                    default:
                        break;
                }
            }
        }

        private static void SetProject(CodeMetricsLineModel line, CodeMetricsLineModel subLine, string subNodeValue)
        {
            subLine.Project = subNodeValue;
        }

        private static void SetNamespace(CodeMetricsLineModel line, CodeMetricsLineModel subLine, string subNodeValue)
        {
            subLine.Project = line.Project;
            subLine.Namespace = subNodeValue;
        }

        private static void SetType(CodeMetricsLineModel line, CodeMetricsLineModel subLine, string subNodeValue)
        {
            subLine.Project = line.Project;
            subLine.Namespace = line.Namespace;
            subLine.Type = subNodeValue;
        }

        private static void SetMember(CodeMetricsLineModel line, CodeMetricsLineModel subLine, string subNodeValue)
        {
            subLine.Project = line.Project;
            subLine.Namespace = line.Namespace;
            subLine.Type = line.Type;
            subLine.Member = subNodeValue;
        }

        private static void CleanNaming(CodeMetricsLineModel projectLine)
        {
            // Clean project row
            projectLine.Project = projectLine.Project.Replace(" (Debug)", string.Empty);
            projectLine.Project = projectLine.Project.Split('\\')[projectLine.Project.Split('\\').Length - 1];
            projectLine.Project = projectLine.Project.Replace("iTS.", string.Empty);
            projectLine.Project = projectLine.Project.Replace(".dll", string.Empty);

            // Clean namespace row
            if (projectLine.Namespace != null)
            {
                projectLine.Namespace = projectLine.Namespace.Replace("iTS.", string.Empty);
                projectLine.Namespace = projectLine.Namespace.Replace(projectLine.Project + ".", string.Empty);
                projectLine.Namespace = projectLine.Namespace.Replace("EF.", string.Empty);
            }
        }

        private static void GetMetrics(XElement node, CodeMetricsLineModel projectLine)
        {
            XElement metrics = node.Element("Metrics");

            foreach (var metric in metrics.Elements())
            {
                var attributeName = metric.Attribute("Name").Value;
                var attributeValue = metric.Attribute("Value").Value;

                switch (attributeName)
                {
                    case "MaintainabilityIndex":
                        projectLine.MaintainabilityIndex = double.Parse(attributeValue);
                        break;
                    case "CyclomaticComplexity":
                        projectLine.CyclomaticComplexity = double.Parse(attributeValue);
                        break;
                    case "ClassCoupling":
                        projectLine.ClassCoupling = double.Parse(attributeValue);
                        break;
                    case "DepthOfInheritance":
                        projectLine.DepthOfInheritance = double.Parse(attributeValue);
                        break;
                    case "LinesOfCode":
                        projectLine.LinesOfCode = double.Parse(attributeValue);
                        break;
                }
            }
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
                    case "Module":
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