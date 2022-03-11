using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

//Cursor Parkplatz

namespace GD1020_Softwaretechnik
{
    public class GraphStructure<T>
    {
        internal Random random = new Random();

        //Dictionary mit T(zurück zu int?) und weight + connected Vert
        private Dictionary<Vertex<T>, List<(int weight, Vertex<T> connectedVertex)>> _connectionDictionary;
        public Dictionary<Vertex<T>, List<(int weight, Vertex<T> connectedVertex)>> ConnectionDictionary
        {
            get => _connectionDictionary;
            private set {}
        }

        public GraphStructure()
        {
            _connectionDictionary = new Dictionary<Vertex<T>, List<(int weight, Vertex<T> connectedVertex)>>();
        }

        public class Vertex<T2>
        {
            public readonly int ID;
            private T container;
            public T Container { get { return container; } set { container = value; } }

            public Vertex(int id)
            {
                ID = id;
            }
        }

        public void PrintGraph()
        {
            foreach (KeyValuePair<Vertex<T>, List<(int weight, Vertex<T> connectedVertex)>> keyValuePair in _connectionDictionary)
            {
                Console.Write($"Vertex: {keyValuePair.Key} || ");
                foreach (var t in keyValuePair.Value)
                {
                    Console.Write($"[Weight: {t.weight}, Vertex: {t.connectedVertex}]; ");
                }
                Console.WriteLine();
            }
        }

        //Cant work since it only generates a root with neighbours which never get used
        public void GenerateRandomGraph(int graphSize, int maximumNeighbors, int maximumWeight)
        {

        }

        public void InsertVertex(int vertexID)
        {
            try
            {
                if (_connectionDictionary.Count > 0 && (from vertex in _connectionDictionary.Keys where vertex.ID.Equals(vertexID) select vertex).First() != null)
                    throw new ArgumentException("Vertex ID already exists");
                _connectionDictionary.Add(new Vertex<T>(vertexID), new List<(int, Vertex<T>)>());
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void InsertVertex(Vertex<T> vertex)
        {
            try
            {
                if (_connectionDictionary.Count > 0 && (from vertexComparison in _connectionDictionary.Keys where vertexComparison.ID.Equals(vertex.ID) select vertexComparison).First() != null)
                    throw new ArgumentException("Vertex ID already exists");
                if (_connectionDictionary.ContainsKey(vertex))
                    throw new ArgumentException("Vertex ID already exists");
                _connectionDictionary.Add(vertex, new List<(int, Vertex<T>)>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void InsertMultipleVertices(int[] verticesID)
        {
            for (int i = 0; i < verticesID.Length; i++)
            {
                InsertVertex(verticesID[i]);
            }
        }

        public void DeleteVertex(int vertexID)
        {
            if(_connectionDictionary.Count > 0) _connectionDictionary.Remove((from vertex in _connectionDictionary.Keys where vertex.ID.Equals(vertexID) select vertex).First());
        }

        public void DeleteVertex(Vertex<T> vertex)
        {
            _connectionDictionary.Remove(vertex);
        }

        public void ConnectVertices(Vertex<T> vertexA, Vertex<T> vertexB, int weight)
        {
            try
            {
                if (!_connectionDictionary.Keys.Contains(vertexA) || !_connectionDictionary.Keys.Contains(vertexB))
                    throw new IndexOutOfRangeException("At least one of the given Vertices does not exist");
                if (_connectionDictionary.Count > 0 && (from vertexItem in _connectionDictionary[vertexA] where vertexItem.connectedVertex.Equals(vertexB) select vertexItem).First().connectedVertex != null)
                    throw new ArgumentException("Connection already exists");
                _connectionDictionary[vertexA].Add((weight, vertexB));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DisconnectVertices(Vertex<T> vertexA, Vertex<T> vertexB)
        {
            try
            {
                if (!_connectionDictionary.Keys.Contains(vertexA) || !_connectionDictionary.Keys.Contains(vertexB))
                    throw new IndexOutOfRangeException("At least one of the given Vertices does not exist");
                if (_connectionDictionary.Count > 0) _connectionDictionary[vertexA].Remove((from vertexItem in _connectionDictionary[vertexA] where vertexItem.connectedVertex.Equals(vertexB) select vertexItem).First());
                if (_connectionDictionary.Count > 0) _connectionDictionary[vertexB].Remove((from vertexItem in _connectionDictionary[vertexB] where vertexItem.connectedVertex.Equals(vertexA) select vertexItem).First());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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