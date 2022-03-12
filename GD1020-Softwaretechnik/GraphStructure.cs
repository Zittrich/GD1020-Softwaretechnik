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
                Console.Write($"Vertex: {keyValuePair.Key.ID} || ");
                foreach (var t in keyValuePair.Value)
                {
                    Console.Write($"[Weight: {t.weight}, Vertex: {t.connectedVertex.ID}]; ");
                }
                Console.WriteLine();
            }
        }

        //TODO sometimes maximumNeighbors+1 neighbors
        public void GenerateRandomGraph(int graphSize, int maximumNeighbors, int maximumWeight, int minimumWeight)
        {
            Dictionary<Vertex<T>, int> possibleNeighbours = new Dictionary<Vertex<T>, int>();
            List<Vertex<T>> unused = new List<Vertex<T>>();
            List<Vertex<T>> all = new List<Vertex<T>>();
            Vertex<T> current = null;
            //Generate every vertex
            for (int i = 0; i < graphSize; i++)
            {
                current = InsertVertex(i);
                possibleNeighbours.Add(current, maximumNeighbors-1);
                unused.Add(current);
                all.Add(current);
            }
            current = unused.First();
            unused.Remove(current);
            while (current != null)
            {
                Vertex<T> next = null;
                int randomNeighbourCount = possibleNeighbours[current] <= 0 ? 0 : random.Next(possibleNeighbours[current])+1;
                HashSet<Vertex<T>> uniqueNeighbours = new HashSet<Vertex<T>>();
                uniqueNeighbours.Add(current);
                for (int i = 0; i < randomNeighbourCount; i++)
                {
                    Vertex<T> newNeighbour = null;
                    do
                    {
                        newNeighbour = all[random.Next(all.Count)];
                    } while (uniqueNeighbours.Contains(newNeighbour) || possibleNeighbours[newNeighbour] <= 0);

                    uniqueNeighbours.Add(newNeighbour);
                    int randomWeight = minimumWeight + ((maximumWeight - minimumWeight) == 0 ? 0 : random.Next(maximumWeight-minimumWeight));
                    if (unused.Contains(newNeighbour)) next = newNeighbour;
                    if(ConnectVertices(current, newNeighbour, randomWeight))
                    possibleNeighbours[current]--;
                    if (ConnectVertices(newNeighbour, current, randomWeight))
                    possibleNeighbours[newNeighbour]--;
                }
                if (next == null)
                {
                    if (unused.Any())
                    {
                        int randomWeight = minimumWeight + ((maximumWeight - minimumWeight) == 0 ? 0 : random.Next(maximumWeight - minimumWeight));
                        if (uniqueNeighbours.Count > 1)
                        {
                            DisconnectVertices(uniqueNeighbours.ElementAt(1), current);
                            possibleNeighbours[current]++;
                            DisconnectVertices(current, uniqueNeighbours.ElementAt(1));
                            possibleNeighbours[uniqueNeighbours.ElementAt(1)]++;
                        }
                        do
                        {
                            next = unused[random.Next(unused.Count)];
                        } while (possibleNeighbours[next] + 1 <= 0);
                        ConnectVertices(current, next, randomWeight);
                        possibleNeighbours[current]--;
                        ConnectVertices(next, current, randomWeight);
                        possibleNeighbours[next]--;
                        current = next;
                    }
                    else
                    {
                        current = null;
                    }
                    
                }
                unused.Remove(current);
            }
        }

        public void GenerateRandomGridGraph(int graphSize, int maximumNeighbors, int maximumWeight)
        {

        }

        public Vertex<T> InsertVertex(int vertexID)
        {
            Vertex<T> current = null;
            try
            {
                if ((from vertex in _connectionDictionary.Keys where vertex.ID.Equals(vertexID) select vertex).Any())
                    throw new ArgumentException("Vertex ID already exists");
                current = new Vertex<T>(vertexID);
                _connectionDictionary.Add(current, new List<(int, Vertex<T>)>());
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return current;
        }

        public void InsertVertex(Vertex<T> vertex)
        {
            try
            {
                if ((from vertexComparison in _connectionDictionary.Keys where vertexComparison.ID.Equals(vertex.ID) select vertexComparison).Any())
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
            IEnumerable<Vertex<T>> sequence = (from vertex in _connectionDictionary.Keys where vertex.ID.Equals(vertexID) select vertex);
            if (sequence.Any()) _connectionDictionary.Remove(sequence.First());
        }

        public void DeleteVertex(Vertex<T> vertex)
        {
            _connectionDictionary.Remove(vertex);
        }

        public bool ConnectVertices(Vertex<T> vertexA, Vertex<T> vertexB, int weight)
        {
            try
            {
                if (!_connectionDictionary.Keys.Contains(vertexA) || !_connectionDictionary.Keys.Contains(vertexB))
                    throw new IndexOutOfRangeException("At least one of the given Vertices does not exist");
                if ((from vertexItem in _connectionDictionary[vertexA] where vertexItem.connectedVertex.Equals(vertexB) select vertexItem).Any())
                    throw new ArgumentException("Connection already exists");
                _connectionDictionary[vertexA].Add((weight, vertexB));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public void DisconnectVertices(Vertex<T> vertexA, Vertex<T> vertexB)
        {
            try
            {
                if (!_connectionDictionary.Keys.Contains(vertexA) || !_connectionDictionary.Keys.Contains(vertexB))
                    throw new IndexOutOfRangeException("At least one of the given Vertices does not exist");
                IEnumerable<(int,Vertex<T>)> sequence = (from vertexItem in _connectionDictionary[vertexA] where vertexItem.connectedVertex.Equals(vertexB) select vertexItem);
                if (sequence.Any()) _connectionDictionary[vertexA].Remove(sequence.First());
                sequence = (from vertexItem in _connectionDictionary[vertexB] where vertexItem.connectedVertex.Equals(vertexA) select vertexItem);
                if (sequence.Any()) _connectionDictionary[vertexB].Remove(sequence.First());
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