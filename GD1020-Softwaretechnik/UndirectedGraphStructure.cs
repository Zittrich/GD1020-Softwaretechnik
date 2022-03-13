using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD1020_Softwaretechnik
{
    public class UndirectedGraphStructure
    {
        private readonly int _vertex;
        private int _edge;

        private List<(int connection, int weight, bool isMarked, bool isForward)>[] _adjacent;

        public UndirectedGraphStructure(int vertex)
        {
            _vertex = vertex;

            _adjacent = new List<(int connection, int weight, bool isMarked, bool isForward)>[vertex];

            for (int i = 0; i < _adjacent.Length; i++)
                _adjacent[i] = new List<(int connection, int weight, bool isMarked, bool isForward)>();
        }

        public List<(int connection, int weight, bool isMarked, bool isForward)>[] Adjacent
        {
            get => _adjacent;
            private set { }
        }

        public int Vertex 
        {
            get => _vertex;
            private set { }
        }

        public int Edge
        {
            get => _edge;
            private set { }
        }

        public void AddEdge(int edgeA, int edgeB, int weight)
        {
            if (edgeA > _adjacent.Length || edgeB > _adjacent.Length)
                throw new ArgumentException("Invalid node number specified.");

            _adjacent[edgeA].Add((edgeB, weight, false, true));
            _adjacent[edgeB].Add((edgeA, weight, false, false));
            _edge++;
        }

        public List<(int connection, int weight, bool isMarked, bool isForward)> GetAdjacency(int vertex)
        {
            return _adjacent[vertex];
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(_vertex + " vertices, " + _edge + " edges");
            for (int i = 0; i < _vertex; i++)
            {
                builder.Append(i + ": ");
                foreach (var connection in _adjacent[i])
                {
                    builder.Append("(Connection: " + connection.connection + " Weight: " + connection.weight + ") ");
                }
                builder.AppendLine(string.Empty);
            }

            return builder.ToString();
        }

        public void MarkEdge(int edgeA, int edgeB)
        {
            for (int i = 0; i < _adjacent[edgeA].Count; i++)
            {
                if (_adjacent[edgeA][i].connection == edgeB)
                {
                    _adjacent[edgeA][i] = (_adjacent[edgeA][i].connection, _adjacent[edgeA][i].weight,
                        true, _adjacent[edgeA][i].isForward);
                    break;
                }
            }
        }

        public void RemoveEdge(int edgeA, int edgeB)
        {
            for (int i = 0; i < _adjacent[edgeA].Count; i++)
            {
                if (_adjacent[edgeA][i].connection == edgeB)
                {
                    _adjacent[edgeA].Remove((_adjacent[edgeA][i].connection, _adjacent[edgeA][i].weight,
                        _adjacent[edgeA][i].isMarked, _adjacent[edgeA][i].isForward));
                    break;
                }
            }

            for (int i = 0; i < _adjacent[edgeB].Count; i++)
            {
                if (_adjacent[edgeB][i].connection == edgeA)
                {
                    _adjacent[edgeB].Remove((_adjacent[edgeB][i].connection, _adjacent[edgeB][i].weight,
                        _adjacent[edgeB][i].isMarked, _adjacent[edgeB][i].isForward));
                    break;
                }
            }

            _edge -= 1;
        }
    }
}

