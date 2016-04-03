namespace HungarianAlgorithm
{
    using System.Linq;

    public class Reduce
    {
        private readonly int[,] graph;

        public Reduce(int[,] graph)
        {
            this.graph = graph;
        }


        public int[,] ReduceInColumns()
        {
            for (int i = 0; i < graph.GetLength(0); i++)
            {
                int min = graph[0,i];
                for (int j = 0; j < graph.GetLength(1); j++)
                {
                    if (graph[j,i] < min)
                    {
                        min = graph[j,i];
                    }
                }

                SubstractFromColumn(i, min);
            }
            return graph;
        }

        public int[,] ReduceInRows()
        {
            for (int i = 0; i < graph.GetLength(0); i++)
            {
                var min = graph[i, 0];
                for (var j = 0; j < graph.GetLength(1); j++)
                {
                    if (graph[i, j] < min)
                    {
                        min = graph[i, j];
                    }
                }
                SubstractFromRow(i, min);
            }

            return graph;
        }

        private void SubstractFromRow(int row, int value)
        {           
            for (int i = 0; i < graph.GetLength(1); i++)
            {
                graph[row,i] -= value;
            }
        }

        private void SubstractFromColumn(int column, int value)
        {
            for (int i = 0; i < graph.GetLength(0); i++)
            {
                graph[i,column] -= value;
            }
        }

    }
}
