using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

//Cursor Parkplatz

namespace GD1020_Softwaretechnik
{
    public class Program
    {
        static void Main(string[] args)
        {
            GraphStructure graphStructure = new GraphStructure(new Dictionary<int, List<(int weight, int connectedVertex)>>(), new List<GraphStructure.Vertex>());
            graphStructure.InsertVertex(0);
            graphStructure.InsertVertex(1);
            graphStructure.InsertVertex(2);
            graphStructure.InsertVertex(3);
            graphStructure.InsertVertex(4);
            graphStructure.ConnectVertices(0, 1, 100);
            graphStructure.ConnectVertices(0, 3, 50);
            graphStructure.ConnectVertices(1, 2, 100);
            graphStructure.ConnectVertices(1, 4, 250);
            graphStructure.ConnectVertices(2, 4, 50);
            graphStructure.ConnectVertices(3, 1, 100);
            graphStructure.ConnectVertices(3, 4, 250);

            Dijkstra dijk = new Dijkstra();
            int[] output = dijk.RunDijk2(graphStructure, graphStructure.VertexList[0]);
            foreach (int cost in output)
            {
                Console.WriteLine(cost);
            }

            graphStructure.PrintGraph();
            Console.ReadKey();
        }
    }
    public class GraphStructure
    {
        internal Random random = new Random();

        //Dictionary mit T(zurück zu int?) und weight + connected Vert
        private Dictionary<int, List<(int weight, int connectedVertex)>> _connectionDictionary;
        public Dictionary<int, List<(int weight, int connectedVertex)>> ConnectionDictionary
        {
            get => _connectionDictionary;
            private set {}
        }

        //Liste aller Vertices im Graphen
        private List<Vertex> _vertexList;
        public List<Vertex> VertexList
        {
            get => _vertexList;
            private set {}
        }

        public GraphStructure(Dictionary<int, List<(int weight, int connectedVertex)>> connectionDictionary, List<Vertex> vertexList)
        {
            _connectionDictionary = connectionDictionary;
            _vertexList = vertexList;
        }

        public class Vertex
        {
            public  readonly int ID;

            public Vertex(int id)
            {
                ID = id;
            }
        }

        public void PrintGraph()
        {
            foreach (KeyValuePair<int, List<(int weight, int connectedVertex)>> keyValuePair in _connectionDictionary)
            {
                Console.Write($"Vertex: {keyValuePair.Key} || ");
                foreach (var t in keyValuePair.Value)
                {
                    Console.Write($"[Weight: {t.weight}, Vertex: {t.connectedVertex}]; ");
                }
                Console.WriteLine();
            }
        }

        public GraphStructure GenerateRandomGraph(int graphSize, int maximumNeighbors, int maximumWeight)
        {
            GraphStructure graphStructure = new GraphStructure(new Dictionary<int, List<(int weight, int connectedVertex)>>(), new List<Vertex>());
            for (int i = 0; i < graphSize; i++)
            {
                graphStructure._vertexList.Add(new Vertex(i));

                bool isDirty = false;
                int thisNeighborAmount = random.Next(1, maximumNeighbors);
                List<(int weight, int connectedVertex)> thisList = new List<(int, int)>();
                for (int j = 0; j <= thisNeighborAmount; j++)
                {
                    int weightRandom = random.Next(1, maximumWeight);
                    int connectedVertex = Math.Min(i + j, graphSize - 1);

                    for (int k = 0; k <= thisList.Count; k++)
                    {
                        try
                        {
                            if (thisList[k] == (thisList[k].weight, connectedVertex))
                            {
                                isDirty = true;
                                break;
                            }
                        }
                        catch
                        {
                            if (connectedVertex == i)
                            {
                                isDirty = true;
                                break;
                            }
                        }
                    }

                    if(!isDirty)
                        thisList.Add((weightRandom, connectedVertex));

                    isDirty = false;
                }
                graphStructure._connectionDictionary.Add(i, thisList);
            }

            return graphStructure;
        }

        public void MakeNull()
        {
            _vertexList = null;
            _connectionDictionary = null;
        }

        public void InsertVertex(int vertexID)
        {
            try
            {
                if (_connectionDictionary.ContainsKey(vertexID))
                    throw new ArgumentException("Vertex ID already exists");
            }
            finally
            {
                _vertexList.Add(new Vertex(vertexID));
                _connectionDictionary.Add(vertexID, new List<(int, int)>());
            }
        }

        public void DeleteVertex(int vertexID)
        {
            if (!_connectionDictionary.ContainsKey(vertexID))
                throw new ArgumentException("Vertex ID does not exist");

            foreach(Vertex vertex in _vertexList)
            {
                if(vertex.ID == vertexID)
                {
                    _vertexList.Remove(vertex);
                    break;
                }
            }

            _connectionDictionary.Remove(vertexID);

            foreach (KeyValuePair<int, List<(int,int)>> keyValuePair in _connectionDictionary)
            {
                foreach((int weight, int connectedVertex) valuePair in keyValuePair.Value)
                {
                    if (valuePair == (valuePair.weight, vertexID))
                    {
                        _connectionDictionary.Remove(keyValuePair.Key);
                    }
                }
                //for schleife statt for each
            }
        }

        public void ConnectVertices(int vertexA, int vertexB, int weight)
        {
            if (_vertexList.Count < vertexA || _vertexList.Count < vertexB)
                throw new IndexOutOfRangeException("At least one of the given Vertices does not exist");

            try
            {
                if (_connectionDictionary[vertexA].Contains((weight, vertexB)))
                    throw new ArgumentException("Connection already exists");
            }
            finally
            {
                _connectionDictionary[vertexA].Add((weight, vertexB));
            }
        }

        public void DisconnectVertices(int vertexA, int vertexB, int weight)
        {
            if (_vertexList.Count < vertexA || _vertexList.Count < vertexB)
                throw new IndexOutOfRangeException("At least one of the given Vertices does not exist");

            try
            {
                if (!_connectionDictionary[vertexA].Contains((weight, vertexB)))
                    throw new ArgumentException("Connection does not exist");
            }
            finally
            {
                _connectionDictionary[vertexA].Remove((weight, vertexB));
            }
        }

        //changeVertexInformation

        //member_vertex
        //member_edge
        //edge_weight
        //mark_vertex
        //mark_edge
    }
}