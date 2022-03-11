using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GD1020_Softwaretechnik
{
    public class Kruskal
    {
        //private static void Main(string[] args)
        //{
        //    GraphStructure graphStructure = new GraphStructure(new Dictionary<int, List<(int weight, int connectedVertex)>>(), new List<GraphStructure.Vertex>());

        //    graphStructure.InsertMultipleVertices(new[] { 0, 1, 2, 3, 4, 5, 6 });

        //    graphStructure.ConnectVertices(0, 1, 7);
        //    graphStructure.ConnectVertices(0, 3, 5);
        //    graphStructure.ConnectVertices(1, 2, 8);
        //    graphStructure.ConnectVertices(1, 3, 9);
        //    graphStructure.ConnectVertices(2, 4, 5);
        //    graphStructure.ConnectVertices(3, 4, 15);
        //    graphStructure.ConnectVertices(3, 5, 6);
        //    graphStructure.ConnectVertices(4, 5, 8);
        //    graphStructure.ConnectVertices(4, 6, 9);
        //    graphStructure.ConnectVertices(5, 6, 11);

        //    ExecuteKruskal(graphStructure);
        //    Console.ReadKey();
        //}

    //    public static GraphStructure ExecuteKruskal(GraphStructure graphStructure)
    //    {
    //        GraphStructure maxGraphStructure =
    //            new GraphStructure(new Dictionary<int, List<(int weight, int connectedVertex)>>(),
    //                graphStructure.VertexList);

    //        return FindShortest(graphStructure, maxGraphStructure);
    //    }

    //    private static GraphStructure FindShortest(GraphStructure graphStructure, GraphStructure maxGraphStructure)
    //    {
    //        Dictionary<int, List<(int weight, int connectedVertex)>> shortestTuples = new Dictionary<int, List<(int, int)>>();
    //        for (int i = 0; i < graphStructure.VertexList.Count; i++)
    //        {
    //            shortestTuples.Add(i, new List<(int weight, int connectedVertex)>());
    //        }
    //        shortestTuples[0].Add(graphStructure.ConnectionDictionary[0][0]);

    //        foreach (KeyValuePair<int, List<(int weight, int connectedVertex)>> keyValuePair in graphStructure.ConnectionDictionary)
    //        {
    //            for (int i = 0; i < keyValuePair.Value.Count; i++)
    //            {
    //                if (keyValuePair.Value[i].weight == shortestTuples[0][0].weight)
    //                {
    //                    if (CheckCyclic(shortestTuples, maxGraphStructure, (keyValuePair.Key, keyValuePair.Value[i].weight, keyValuePair.Value[i].connectedVertex)))
    //                    {
    //                        graphStructure.DisconnectVertices(keyValuePair.Key, keyValuePair.Value[i].connectedVertex);
    //                        continue;
    //                    }

    //                    (graphStructure, shortestTuples) = AddVertexToList(graphStructure, keyValuePair, shortestTuples, i);
    //                }

    //                else if (keyValuePair.Value[i].weight < shortestTuples[0][0].weight)
    //                {
    //                    if (CheckCyclic(shortestTuples, maxGraphStructure, (keyValuePair.Key, keyValuePair.Value[i].weight, keyValuePair.Value[i].connectedVertex)))
    //                    {
    //                        graphStructure.DisconnectVertices(keyValuePair.Key, keyValuePair.Value[i].connectedVertex);
    //                        continue;
    //                    }

    //                    shortestTuples.Clear();
    //                    (graphStructure, shortestTuples) = AddVertexToList(graphStructure, keyValuePair, shortestTuples, i);
    //                }
    //            }
    //        }

    //        foreach (KeyValuePair<int, List<(int weight, int connectedVertex)>> shortestTuple in shortestTuples)
    //        {
    //            for (int i = 0; i < shortestTuple.Value.Count; i++)
    //            {
    //                graphStructure.DisconnectVertices(shortestTuple.Key, shortestTuple.Value[i].connectedVertex);
    //            }
    //        }

    //        foreach (KeyValuePair<int, List<(int weight, int connectedVertex)>> shortestTuple in shortestTuples)
    //        {
    //            for (int i = 0; i < shortestTuples.Count; i++)
    //            {

    //                maxGraphStructure.ConnectionDictionary[shortestTuple.Key].Add(shortestTuple.Value[i]);

    //            }
    //        }

    //        if (graphStructure.ConnectionDictionary.Count > 0)
    //            return FindShortest(graphStructure, maxGraphStructure);

    //        return maxGraphStructure;
    //    }

    //    private static (GraphStructure graphStructure, Dictionary<int, List<(int weight, int connectedVertex)>> shortestTuples) AddVertexToList
    //    (GraphStructure graphStructure, KeyValuePair<int, List<(int weight, int connectedVertex)>> keyValuePair,
    //            Dictionary<int, List<(int weight, int connectedVertex)>> shortestTuples, int position)
    //    {
    //        shortestTuples[keyValuePair.Key].Add(keyValuePair.Value[position]);

    //        return (graphStructure, shortestTuples);
    //    }

    //    private static bool CheckCyclic(Dictionary<int, List<(int weight, int connectedVertex)>> shortestTuples, GraphStructure maxGraphStructure,
    //        (int vertexA, int vertexB, int weight) tupleToAdd)
    //    {

    //        if (maxGraphStructure.ConnectionDictionary.Count < tupleToAdd.vertexA)
    //        {
    //            shortestTuples.Add(tupleToAdd.vertexA, new List<(int weight, int connectedVertex)>());
    //            shortestTuples[tupleToAdd.vertexA].Add((tupleToAdd.weight, tupleToAdd.vertexB));
    //        }
    //        else
    //        {
    //            shortestTuples[tupleToAdd.vertexA].Add((tupleToAdd.weight, tupleToAdd.vertexB));
    //        }

    //        foreach (KeyValuePair<int, List<(int weight, int connectedVertex)>> shortestTuple in shortestTuples)
    //        {
    //            maxGraphStructure.ConnectionDictionary[shortestTuple.Key] = shortestTuple.Value;
    //        }

    //        return IsCyclic(maxGraphStructure, tupleToAdd.vertexA, tupleToAdd.vertexB, tupleToAdd.vertexA, new List<int>());

    //    }

    //    private static bool IsCyclic(GraphStructure maxGraphStructure, int vertexA, int vertexB, int startingVertex, List<int> connectionList)
    //    {
    //        startingVertex += 1;
    //        if (startingVertex == vertexA + 1)
    //        {
    //            foreach (int i in connectionList)
    //            {
    //                if (i == vertexB)
    //                    return true;
    //            }

    //            return false;
    //        }

    //        foreach ((int weight, int connectedVertex) valueTuple in maxGraphStructure.ConnectionDictionary[vertexA])
    //        {
    //            connectionList.Add(valueTuple.connectedVertex);
    //        }

    //        return IsCyclic(maxGraphStructure, vertexA, vertexB, startingVertex, connectionList);
    //    }
    }
}
