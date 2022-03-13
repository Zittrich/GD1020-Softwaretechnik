using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GD1020_Softwaretechnik
{
    public class Dijkstra<T>
    {
        /*
         * Initialisierung:
         * Weg zum Startknoten: 0 Kosten
         * Andere Wege: -1 Kosten, unbekannt
         * Startknoten kommt in Warteschlange
         *
         * Dann:
         * Erster Knoten in Warteschlange wird ausgewählt
         * Alle Nachbarsknoten werden betrachtet
         * Ist der Knoten in der Warteschlange enthalten?
         * Ist der neue Weg billiger als bisher?
         *
         * Falls ja:
         * Kosten werden verringert
         * Ist der Knoten bereits in der Warteschlange?
         * Falls nein, füge ihn in Warteschlange hinzu + neue Kosten = Vorgängerknoten + Kosten Vorgängerknoten/Knoten Verbindung
         */

        //Liste aller Vertices im Graphen
        private List<GraphStructure<T>.Vertex<T>> _vertexList = new List<GraphStructure<T>.Vertex<T>>();
        public List<GraphStructure<T>.Vertex<T>> VertexList
        {
            get => _vertexList;
            private set { }
        }

        public void DefaultIDs(GraphStructure<T> graph)
        {
            foreach (GraphStructure<T>.Vertex<T> key in graph.ConnectionDictionary.Keys.ToList())
            {
                VertexList.Add(key);
            }
        }

        public (int[] costs, int[] preds) RunDijk(GraphStructure<T> graph, GraphStructure<T>.Vertex<T> startVert)
        {
            //Initialisierung
            int vertAmount = graph.VertexCount();
            int startVertId = 0;

            //Kosten
            int[] costs = new int[vertAmount];
            for (int i = 0; i < vertAmount; i++)
            {
                costs[i] = -1;
            }

            //DefaultIDs
            DefaultIDs(graph);

            //Vorgänger
            int[] preds = new int[vertAmount];

            //Startknoten
            for (int i = 0; i < vertAmount; i++)
            {
                if (VertexList[i].ID == startVert.ID)
                {
                    costs[i] = 0;
                    preds[i] = -1;
                    startVertId = i;
                }
            }

            //Queue und Done
            List<int> queue = new List<int> {startVertId};
            List<int> done = new List<int>();

            //Schleife
            while (queue.Count > 0)
            {
                int currentVert = queue[0];
                List<(int weight, GraphStructure<T>.Vertex<T> connectedVertex)> connectedVerts = graph.ConnectionDictionary[graph.FindVertById(currentVert)];
                //List<(int weight, int connectedVertex)> connectedVerts = graph.ConnectionDictionary[currentVert];
                for (int i = 0; i < connectedVerts.Count; i++)
                {
                    int id = connectedVerts[i].connectedVertex.ID;
                    if (!done.Contains(id))
                    {
                        if (costs[id] == -1)
                        {
                            costs[id] = connectedVerts[i].weight + costs[currentVert];
                            preds[id] = currentVert;
                        }
                        else
                        {
                            if (connectedVerts[i].weight + costs[currentVert] < costs[id])
                            {
                                costs[id] = connectedVerts[i].weight + costs[currentVert];
                                preds[id] = currentVert;
                            }
                        }

                        if (!queue.Contains(id))
                        {
                            queue.Add(id);
                        }
                    }
                }
                done.Add(currentVert);
                queue.Remove(currentVert);
            }

            return (costs, preds);
        }

        public List<int> BuildPathId((int[] costs, int[] preds) input, GraphStructure<T>.Vertex<T> toVert)
        {
            if (input.costs[toVert.ID] == -1)
            {
                throw new Exception("Could not build path from Start Vertex to End Vertex");
            }

            List<int> output = new List<int>();
            int currentVert = toVert.ID;
            output.Add(currentVert);
            while (input.preds[currentVert] != -1)
            {
                currentVert = input.preds[currentVert];
                output.Add(currentVert);
            }

            output.Reverse();

            return output;
        }

        /// <summary>
        /// Converts BuildPathId to Vertices
        /// </summary>
        /// <param name="graphStructure"></param>
        /// <param name="idList"></param>
        /// <returns>Transformed List</returns>
        public List<GraphStructure<T>.Vertex<T>> PathIdToVertices(GraphStructure<T> graphStructure, List<int> idList)
        {
            List<GraphStructure<T>.Vertex<T>> output = new List<GraphStructure<T>.Vertex<T>>(); 
            foreach (int i in idList)
            {
                output.Add(graphStructure.FindVertById(i));
            }

            return output;
        }

        public void PrintDijk(GraphStructure<T> graphStructure, int startId = 0, int endId = 0)
        {
            Console.Clear();

            (int[] costs, int[] preds) output = RunDijk(graphStructure, graphStructure.FindVertById(startId));
            Console.WriteLine("Kostenliste:");
            foreach (int cost in output.costs)
            {
                Console.WriteLine(cost);
            }

            Console.WriteLine("\nVorgängerliste (-1 entspricht Start):");
            foreach (int pred in output.preds)
            {
                Console.WriteLine(pred);
            }

            Console.WriteLine("\nWeg:");
            List<int> path = BuildPathId(output, graphStructure.FindVertById(endId));
            foreach (int i in path)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("\nKosten:");
            Console.WriteLine(output.costs[endId]);

            Console.WriteLine();
        }
    }
}