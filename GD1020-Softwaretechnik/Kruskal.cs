using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GD1020_Softwaretechnik
{
    public class Kruskal
    {
        private static void Main(string[] args)
        {
            UndirectedGraphStructure graphStructure = new UndirectedGraphStructure(7);

            graphStructure.AddEdge(0, 1, 7);
            graphStructure.AddEdge(0, 3, 5);
            graphStructure.AddEdge(1, 2, 8);
            graphStructure.AddEdge(1, 3, 9);
            graphStructure.AddEdge(2, 4, 5);
            graphStructure.AddEdge(3, 4, 15);
            graphStructure.AddEdge(3, 5, 6);
            graphStructure.AddEdge(4, 5, 8);
            graphStructure.AddEdge(4, 6, 9);
            graphStructure.AddEdge(5, 6, 11);

            ExecuteKruskal(graphStructure);

            Console.WriteLine(graphStructure.ToString());
            Console.ReadKey();
        }

        //master Methode
        public static UndirectedGraphStructure ExecuteKruskal(UndirectedGraphStructure graphStructure)
        {
            List<int> sortedWeightList = new List<int>();
            sortedWeightList = SortWeights(graphStructure);

            UndirectedGraphStructure returnGraph = new UndirectedGraphStructure(graphStructure.Vertex);

            return MarkShortest(graphStructure, sortedWeightList, returnGraph);
        }

        private static UndirectedGraphStructure MarkShortest(UndirectedGraphStructure graphStructure, List<int> sortedWeights, UndirectedGraphStructure returnGraph)
        {
            //markiert kürzeste
            for (int i = 0; i < graphStructure.Adjacent.Length; i++)
            {
                for (int j = 0; j < graphStructure.Adjacent[i].Count; j++)
                {
                    if (graphStructure.Adjacent[i][j].isMarked == false)
                    {
                        if (graphStructure.Adjacent[i][j].weight == sortedWeights[0])
                        {
                            if(!IsCyclic(graphStructure,i, graphStructure.Adjacent[i][j].connection, i, new List<int>()))
                                graphStructure.MarkEdge(i, graphStructure.Adjacent[i][j].connection);
                            else
                            {
                                graphStructure.RemoveEdge(i, graphStructure.Adjacent[i][j].connection);
                            }
                        }
                    }
                }
            }

            /*
            for (int i = 0; i < graphStructure.Adjacent.Length; i++)
            {
                for (int j = 0; j < graphStructure.Adjacent[i].Count; j++)
                {
                    if (IsCyclic(graphStructure, i, graphStructure.Adjacent[i][j].connection, i, new List<int>()))
                    if(NewIsCyclic(graphStructure))
                    {
                        graphStructure.RemoveEdge(i, graphStructure.Adjacent[i][j].connection);
                    }
                    NewIsCyclic(graphStructure);

                }
            }*/


            sortedWeights.RemoveAt(0);

            if (sortedWeights.Count > 0)
            {
                return MarkShortest(graphStructure, sortedWeights, returnGraph);
            }

            return graphStructure;
        }

        private static List<int> SortWeights(UndirectedGraphStructure graphStructure)
        {
            //erstellt eine sortierte liste aller gewichte
            List<int> sortedWeightList = new List<int>();
            for (int i = 0; i < graphStructure.Adjacent.Length; i++)
            {
                for (int j = 0; j < graphStructure.Adjacent[i].Count; j++)
                {
                    if (!sortedWeightList.Contains(graphStructure.Adjacent[i][j].weight))
                    {
                        sortedWeightList.Add(graphStructure.Adjacent[i][j].weight);
                    }
                }
            }
            sortedWeightList.Sort();
            //möglicherweise effizienteren Sortieralgoritmus?

            return sortedWeightList;
        }

        private static bool IsCyclic(UndirectedGraphStructure graphStructure, int vertexA, int vertexB, int startingVertex, List<int> connectionList)
        {
            //go two steps
            //check if you reach the original one again after going two steps

            //I want to go through each connection of a given vertex and look at the connections of all the connections of that vertex and if that equals the original then its a loop
            // A -> B -> C <= has A in it?

            //sehr ineffizient
            //Zieht nicht größere loops in betracht --> Funktioniert nicht
            //NewIsCyclic()

            //hier einen Depth First Search einbauen?
            

            for (int i = 0; i < graphStructure.Adjacent[vertexA].Count; i++)
            {
                if(graphStructure.Adjacent[vertexA][i].isForward)
                {
                    for (int j = 0; j < graphStructure.Adjacent[graphStructure.Adjacent[vertexA][i].connection].Count; j++)
                    {
                        if(graphStructure.Adjacent[graphStructure.Adjacent[vertexA][i].connection][j].isForward)
                        {
                            for (int l = 0; l < graphStructure.Adjacent[graphStructure.Adjacent[graphStructure.Adjacent[vertexA][i].connection][j].connection].Count; l++)
                            {
                                if(graphStructure.Adjacent[graphStructure.Adjacent[graphStructure.Adjacent[vertexA][i].connection][j].connection][l].isForward)
                                {
                                    if (graphStructure.Adjacent[graphStructure.Adjacent[graphStructure.Adjacent[vertexA][i].connection][j].connection][l].connection == vertexA)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private static UndirectedGraphStructure NewIsCyclic(UndirectedGraphStructure graphStructure, UndirectedGraphStructure returnGraph)
        {
            //neuer isCyclic checker
            //Funktioniert nicht
            //mit bool

            int vertexAmount = graphStructure.Vertex;
            bool[] visited = new bool[vertexAmount];

            for (int i = 0; i < vertexAmount; i++)
            {
                visited[i] = false;
            }

            for (int u = 0; u < vertexAmount; u++)
            {
                if (!visited[u])
                {
                    (bool isCyclic, int connection, int weight) thisCyclicCheck = isCyclicUtil(graphStructure, u, visited, -1);
                    if (!thisCyclicCheck.isCyclic)
                    {
                        returnGraph.AddEdge(u, thisCyclicCheck.connection, thisCyclicCheck.weight);

                        //graphStructure.RemoveEdge(u, isCyclicUtil(graphStructure, u, visited, -1).connectedVertex);
                    }
                }
            }
            return returnGraph;
        }

        private static (bool isCyclic, int connectedVertex, int weight) isCyclicUtil(UndirectedGraphStructure graphStructure, int startingVertex, bool[] visited, int parent)
        {
            visited[startingVertex] = true;

            foreach ((int connection, int weight, bool isMarked, bool isForward) valueTuple in graphStructure.Adjacent[startingVertex])
            {
                if (!visited[valueTuple.connection])
                {
                    if (isCyclicUtil(graphStructure, valueTuple.connection, visited, startingVertex).isCyclic)
                    {
                        return (true, valueTuple.connection, valueTuple.weight);
                    }
                }
                else if (valueTuple.connection != parent)
                {
                    return (true, valueTuple.connection, valueTuple.weight);

                }
                return (false, valueTuple.connection, valueTuple.weight);
            }

            return (false, 0, 0);
        }

        //  -- Versuch mit gerichteten Graphen --

        /*private static void Main(string[] args)
        {
            GraphStructure graphStructure = new GraphStructure(new Dictionary<int, List<(int weight, int connectedVertex)>>(), new List<GraphStructure.Vertex>());

            graphStructure.InsertMultipleVertices(new []{0,1,2,3,4,5,6});

            graphStructure.ConnectVertices(0, 1, 7);
            graphStructure.ConnectVertices(0, 3, 5);
            graphStructure.ConnectVertices(1, 2, 8);
            graphStructure.ConnectVertices(1, 3, 9);
            graphStructure.ConnectVertices(2, 4, 5);
            graphStructure.ConnectVertices(3, 4, 15);
            graphStructure.ConnectVertices(3, 5, 6);
            graphStructure.ConnectVertices(4, 5, 8);
            graphStructure.ConnectVertices(4, 6, 9);
            graphStructure.ConnectVertices(5, 6, 11);

            ExecuteKruskal(graphStructure);
            Console.ReadKey();
        }

        public static GraphStructure ExecuteKruskal(GraphStructure graphStructure)
        {
            GraphStructure maxGraphStructure =
                new GraphStructure(new Dictionary<int, List<(int weight, int connectedVertex)>>(),
                    graphStructure.VertexList);

            return FindShortest(graphStructure, maxGraphStructure);
        }

        private static GraphStructure FindShortest(GraphStructure graphStructure, GraphStructure maxGraphStructure)
        {
            Dictionary<int, List<(int weight, int connectedVertex)>> shortestTuples = new Dictionary<int, List<(int, int)>>();
            for (int i = 0; i < graphStructure.VertexList.Count; i++)
            {
                shortestTuples.Add(i, new List<(int weight, int connectedVertex)>());
            }
            shortestTuples[0].Add(graphStructure.ConnectionDictionary[0][0]);

            foreach (KeyValuePair<int, List<(int weight, int connectedVertex)>> keyValuePair in graphStructure.ConnectionDictionary)
            {
                for (int i = 0; i < keyValuePair.Value.Count; i++)
                {
                    if (keyValuePair.Value[i].weight == shortestTuples[0][0].weight)
                    {
                        if (CheckCyclic(shortestTuples, maxGraphStructure, (keyValuePair.Key, keyValuePair.Value[i].weight, keyValuePair.Value[i].connectedVertex)))
                        {
                            graphStructure.DisconnectVertices(keyValuePair.Key, keyValuePair.Value[i].connectedVertex);
                            continue;
                        }

                        (graphStructure, shortestTuples) = AddVertexToList(graphStructure, keyValuePair, shortestTuples, i);
                    }

                    else if ( keyValuePair.Value[i].weight < shortestTuples[0][0].weight)
                    {
                        if (CheckCyclic(shortestTuples, maxGraphStructure, (keyValuePair.Key, keyValuePair.Value[i].weight, keyValuePair.Value[i].connectedVertex)))
                        {
                            graphStructure.DisconnectVertices(keyValuePair.Key, keyValuePair.Value[i].connectedVertex);
                            continue;
                        }

                        shortestTuples.Clear();
                        (graphStructure, shortestTuples) = AddVertexToList(graphStructure, keyValuePair, shortestTuples, i);
                    }
                }
            }

            foreach (KeyValuePair<int, List<(int weight, int connectedVertex)>> shortestTuple in shortestTuples)
            {
                for (int i = 0; i < shortestTuple.Value.Count; i++)
                {
                    graphStructure.DisconnectVertices(shortestTuple.Key, shortestTuple.Value[i].connectedVertex);
                }
            }

            foreach (KeyValuePair<int, List<(int weight, int connectedVertex)>> shortestTuple in shortestTuples)
            {
                for (int i = 0; i < shortestTuples.Count; i++)
                {

                    maxGraphStructure.ConnectionDictionary[shortestTuple.Key].Add(shortestTuple.Value[i]);

                }
            }

            if (graphStructure.ConnectionDictionary.Count > 0)
                return FindShortest(graphStructure, maxGraphStructure);

            return maxGraphStructure;
        }

        private static (GraphStructure graphStructure, Dictionary<int, List<(int weight, int connectedVertex)>> shortestTuples) AddVertexToList
        (GraphStructure graphStructure, KeyValuePair<int, List<(int weight, int connectedVertex)>> keyValuePair, 
                Dictionary<int, List<(int weight, int connectedVertex)>> shortestTuples, int position)
        {
            shortestTuples[keyValuePair.Key].Add(keyValuePair.Value[position]);

            return (graphStructure, shortestTuples);
        }

        private static bool CheckCyclic(Dictionary<int, List<(int weight, int connectedVertex)>> shortestTuples, GraphStructure maxGraphStructure,
            (int vertexA, int vertexB, int weight) tupleToAdd)
        {

            if (maxGraphStructure.ConnectionDictionary.Count < tupleToAdd.vertexA)
            {
                shortestTuples.Add(tupleToAdd.vertexA, new List<(int weight, int connectedVertex)>());
                shortestTuples[tupleToAdd.vertexA].Add((tupleToAdd.weight, tupleToAdd.vertexB));
            }
            else
            {
                shortestTuples[tupleToAdd.vertexA].Add((tupleToAdd.weight, tupleToAdd.vertexB));
            }

            foreach (KeyValuePair<int, List<(int weight, int connectedVertex)>> shortestTuple in shortestTuples)
            {
                maxGraphStructure.ConnectionDictionary[shortestTuple.Key] = shortestTuple.Value;
            }

            return IsCyclic(maxGraphStructure, tupleToAdd.vertexA, tupleToAdd.vertexB, tupleToAdd.vertexA, new List<int>());

        }

        private static bool IsCyclic(GraphStructure maxGraphStructure, int vertexA, int vertexB, int startingVertex, List<int> connectionList)
        {
            startingVertex += 1;
            if (startingVertex == vertexA + 1)
            {
                foreach (int i in connectionList)
                {
                    if (i == vertexB)
                        return true;
                }

                return false;
            }

            foreach ((int weight, int connectedVertex) valueTuple in maxGraphStructure.ConnectionDictionary[vertexA])
            {
                connectionList.Add(valueTuple.connectedVertex);
            }

            return IsCyclic(maxGraphStructure,vertexA,vertexB,startingVertex, connectionList);
        }*/
    }
}
