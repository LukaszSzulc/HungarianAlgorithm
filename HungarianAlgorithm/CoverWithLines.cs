using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HungarianAlgorithm
{
    public enum CoverType
    {
        Row,Column
    }

    public class Cover
    {
        public int Index { get; set; }

        public CoverType CoverType { get; set; }
    }

    public class CoverWithLines
    {
        private readonly int[,] originalGraph;

        private readonly int[,] modifiedGraph;

        private readonly int[,] covered;
        public CoverWithLines(int[,] originalGraph)
        {
            this.originalGraph = originalGraph;
            modifiedGraph = CopyToTemporaryLargerMatrix();
            covered = new int[originalGraph.GetLength(0),originalGraph.GetLength(1)];
        }

        public List<Tuple<int, int>> FindMinimumCost()
        {
            int numberOfCovers = 0;
            do
            {
                ResetZeroCount();
                CountZerosInRows();
                CountZerosInColumns();
                numberOfCovers = CoverZeros();
                if (numberOfCovers == originalGraph.GetLength(0))
                {
                    break;
                }

                var minimum = GetMinimumFromCovered();
                AdjustCoverdAndNotCoverdElements(minimum);
            }
            while (true);

            return new List<Tuple<int, int>>();
        }

        private int CoverZeros()
        {
            int iteration = 2;
            int numberOfCovers = 0;
            do
            {
                var oder = GetOrder(iteration);
                int indexToCover;
                switch (oder)
                {
                    case CoverType.Row:
                        indexToCover = GetRowMaximum();
                        CoverRow(indexToCover);
                        break;
                    case CoverType.Column:
                        indexToCover = GetColumnMaximum();
                        CoverColumn(indexToCover);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                numberOfCovers++;
                iteration++;
            }
            while (IsAnyZerosToCover());

            return numberOfCovers;
        }

        private void CountZerosInRows()
        {
            for (int i = 1; i < modifiedGraph.GetLength(0); i++)
            {
                int count = 0;
                for (int j = 1; j < modifiedGraph.GetLength(1); j++)
                {
                    if (modifiedGraph[i, j] == 0)
                    {
                        count++;
                    }
                }

                modifiedGraph[i, 0] = count;
            }
        }

        private void AdjustCoverdAndNotCoverdElements(int minimum)
        {
            for (int i = 0; i < covered.GetLength(0); i++)
            {
                for (int j = 0; j < covered.GetLength(1); j++)
                {
                    if (covered[i, j] == 0)
                    {
                        this.modifiedGraph[i + 1, j + 1]-= minimum;
                    }

                    if (covered[i, j] >= 2)
                    {
                        modifiedGraph[i+1, j+1] += minimum;
                    }
                }
            }
        }

        private void CountZerosInColumns()
        {
            for (int i = 1; i < modifiedGraph.GetLength(0); i++)
            {
                int count = 0;
                for (int j = 1; j < modifiedGraph.GetLength(1); j++)
                {
                    if (modifiedGraph[j, i] == 0)
                    {
                        count++;
                    }
                }

                modifiedGraph[0, i] = count;
            }
        }

        private bool IsAnyZerosToCover()
        {
            for (int i = 1; i < modifiedGraph.GetLength(1); i++)
            {
                if (modifiedGraph[0, i] > 0 || modifiedGraph[i, 0] > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private int GetColumnMaximum()
        {
            int max = modifiedGraph[0, 1];
            int index = 1;
            for (int i = 2; i < modifiedGraph.GetLength(0); i++)
            {
                if (modifiedGraph[0, i] > max)
                {
                    index = i;
                    max = modifiedGraph[0, i];
                }
            }

            return index;
        }

        private int GetRowMaximum()
        {
            int max = modifiedGraph[1, 0];
            int index = 1;
            for (int i = 2; i < modifiedGraph.GetLength(0); i++)
            {
                if (modifiedGraph[i, 0] > max)
                {
                    max = modifiedGraph[i, 0];
                    index = i;
                }
            }

            return index;
        }

        private int[,] CopyToTemporaryLargerMatrix()
        {
            var dimension = originalGraph.GetLength(0);

            var newMatrix = new int[dimension + 1, dimension + 1];

            for (int i = 1; i < originalGraph.GetLength(0) + 1; i++)
            {
                for (int j = 1; j < originalGraph.GetLength(1) + 1; j++)
                {
                    newMatrix[i, j] = originalGraph[i - 1, j - 1];
                }
            }

            return newMatrix;
        }

        private void CoverRow(int index)
        {
            for (int i = 1; i < modifiedGraph.GetLength(1); i++)
            {
                if (modifiedGraph[index, i] == 0)
                {
                    modifiedGraph[0, i]--;
                }

                covered[index - 1, i - 1] ++;
            }

            modifiedGraph[index, 0] = 0;
        }

        private void CoverColumn(int index)
        {
            for (int i = 1; i < modifiedGraph.GetLength(1); i++)
            {
                if (modifiedGraph[i, index] == 0)
                {
                    modifiedGraph[i, 0]--;
                }

                covered[i - 1, index - 1] ++;
            }

            modifiedGraph[0, index] = 0;

        }

        private void ResetZeroCount()
        {
            for (int i = 0; i < modifiedGraph.GetLength(1); i++)
            {
                modifiedGraph[i, 0] = 0;
                modifiedGraph[0, i] = 0;
            }
        }

        private int GetMinimumFromCovered()
        {
            int minimum = modifiedGraph[1, 1];
            for (int i = 0; i < covered.GetLength(0); i++)
            {
                for (int j = 0; j < covered.GetLength(1); j++)
                {
                    if (covered[i, j] == 0)
                    {
                        if (modifiedGraph[i + 1, j + 1] < minimum)
                        {
                            minimum = modifiedGraph[i + 1, j + 1];
                        }
                    }
                }
            }

            return minimum;
        }

        
        private CoverType GetOrder(int iteration)
        {
            return iteration % 2 == 0 ? CoverType.Row : CoverType.Column;
        }
    }
}
