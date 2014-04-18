namespace CodeAnalysis.BusinessLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;

    using CodeAnalysis.Models;

    public static class CodeMetricsDiffer
    {
        /// <summary>
        /// Creates a list of CodeMetricsLineView containing difference of metrics between two lists of CodeMetricsLineModel
        /// </summary>
        public static IEnumerable<CodeMetricsLineView> InitCodeMetricsDifferences(List<CodeMetricsLineModel> codeMetricsTrunk, List<CodeMetricsLineModel> codeMetricsBranche)
        {
            var codeMetrics = new List<CodeMetricsLineView>();

            foreach (CodeMetricsLineModel lineCodeMetricsBranch in codeMetricsBranche)
            {
                // Get the same line from the other project
                CodeMetricsLineModel lineCodeMetricsTrunk = codeMetricsTrunk
                    .FirstOrDefault(cm => cm.Scope == lineCodeMetricsBranch.Scope
                                          && cm.Project == lineCodeMetricsBranch.Project
                                          && cm.Namespace == lineCodeMetricsBranch.Namespace
                                          && cm.Type == lineCodeMetricsBranch.Type
                                          && cm.Member == lineCodeMetricsBranch.Member);

                var codeMetricsLineView = new CodeMetricsLineView
                {
                    Scope = lineCodeMetricsBranch.Scope,
                    Project = lineCodeMetricsBranch.Project,
                    Namespace = lineCodeMetricsBranch.Namespace,
                    Type = lineCodeMetricsBranch.Type,
                    Member = lineCodeMetricsBranch.Member
                };


                if (lineCodeMetricsTrunk != null)
                {
                    codeMetricsLineView.MaintainabilityIndexDifference = lineCodeMetricsBranch.MaintainabilityIndex - lineCodeMetricsTrunk.MaintainabilityIndex;
                    codeMetricsLineView.CyclomaticComplexityDifference = lineCodeMetricsTrunk.CyclomaticComplexity - lineCodeMetricsBranch.CyclomaticComplexity;
                    codeMetricsLineView.DepthOfInheritanceDifference = lineCodeMetricsTrunk.DepthOfInheritance - lineCodeMetricsBranch.DepthOfInheritance;
                    codeMetricsLineView.ClassCouplingDifference = lineCodeMetricsTrunk.ClassCoupling - lineCodeMetricsBranch.ClassCoupling;
                    codeMetricsLineView.LinesOfCodeDifference = lineCodeMetricsTrunk.LinesOfCode - lineCodeMetricsBranch.LinesOfCode;

                    codeMetricsLineView.CodeMetricsTrunk = lineCodeMetricsTrunk;
                    codeMetricsLineView.CodeMetricsBranche = lineCodeMetricsBranch;
                }
                else
                {
                    codeMetricsLineView.CodeMetricsBranche = lineCodeMetricsBranch;
                    codeMetricsLineView.CodeMetricsTrunk = new CodeMetricsLineModel();
                    codeMetricsLineView.Color = new SolidColorBrush(Colors.Teal);
                }

                codeMetrics.Add(codeMetricsLineView);
            }

            
            var deletedLines = codeMetricsTrunk.Where(dev => !codeMetricsBranche.Any(sprint => 
                dev.Scope == sprint.Scope 
                && dev.Project == sprint.Project 
                && dev.Namespace == sprint.Namespace 
                && dev.Type == sprint.Type 
                && dev.Member == sprint.Member));

            foreach (var lineCodeMetricsTrunk in deletedLines)
            {
                var codeMetricsLineView = new CodeMetricsLineView
                {
                    Scope = lineCodeMetricsTrunk.Scope,
                    Project = lineCodeMetricsTrunk.Project,
                    Namespace = lineCodeMetricsTrunk.Namespace,
                    Type = lineCodeMetricsTrunk.Type,
                    Member = lineCodeMetricsTrunk.Member
                };


                codeMetricsLineView.CodeMetricsBranche = new CodeMetricsLineModel();
                codeMetricsLineView.CodeMetricsTrunk = lineCodeMetricsTrunk;
                codeMetricsLineView.Color = new SolidColorBrush(Colors.IndianRed);

                codeMetrics.Add(codeMetricsLineView);
            }

            return codeMetrics.OrderBy(p => p.Project).ThenBy(p => p.Namespace).ThenBy(p => p.Type).ThenBy(p => p.Member).ToList();
        }
    }
}