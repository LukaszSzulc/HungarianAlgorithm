﻿namespace HungarianAlgorithmTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using HungarianAlgorithm;

    using Xunit;

    public class HungarianTests
    {
        [Fact]
        public void ReduceMinValuesInRowsAndColumns()
        {
            var graph = CreateGraph();
            var reduce = new Reduce(graph);

            reduce.ReduceInRows();
            var result = reduce.ReduceInColumns();

            result.Should().BeEquivalentTo(MatrixAfterReduction());
        }

        [Fact]
        public void CovertWithMinimalLines()
        {
            var graph = CreateGraph();
            var original = CreateGraph();
            var reduce = new Reduce(graph);
            reduce.ReduceInRows();
            var result = reduce.ReduceInColumns();

            var zeros = new CoverWithLines(result);
            var solution = zeros.FindMinimumCost();
            var sum = Sum(solution,original);

            sum.Should().Be(15);
        }

        private int Sum(List<Zero> solution,int[,] graph)
        {
            int sum = 0;
            foreach (var zero in solution)
            {
                sum += graph[zero.Index, zero.Column];
            }

            return sum;
        }

        private int[,] CreateGraph()
        {
            var graph = new int[,] { { 14, 5, 8, 7 }, { 2, 12, 6, 5 }, { 7, 8, 3, 9 }, { 2, 4, 6, 10 } };
            return graph;
        }

        private int[,] MatrixAfterReduction()
        {
            return new[,] { { 9, 0, 3, 0 }, { 0, 10, 4, 1 }, { 4, 5, 0, 4 }, { 0, 2, 4, 6 } };
        }
    }
}
