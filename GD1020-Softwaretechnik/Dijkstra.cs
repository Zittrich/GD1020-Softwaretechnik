using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GD1020_Softwaretechnik
{
    public class Dijkstra
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

        public (int[] costs, int[] preds) RunDijk(GraphStructure graph, GraphStructure.Vertex startVert)
        {
            //Initialisierung
            int vertAmount = graph.VertexList.Count;
            int startVertId = 0;

            //Kosten
            int[] costs = new int[vertAmount];
            for (int i = 0; i < vertAmount; i++)
            {
                costs[i] = -1;
            }

            //Vorgänger
            int[] preds = new int[vertAmount];

            //Startknoten
            for (int i = 0; i < vertAmount; i++)
            {
                if (graph.VertexList[i].ID == startVert.ID)
                {
                    costs[i] = 0;
                    preds[i] = -1;
                    startVertId = i;
                }
            }

            //Queue und Done
            List<int> queue = new List<int>();
            queue.Add(startVertId);
            List<int> done = new List<int>();

            //Schleife
            while (queue.Count > 0)
            {
                int currentVert = queue[0];
                List<(int weight, int connectedVertex)> connectedVerts = graph.ConnectionDictionary[currentVert];
                for (int i = 0; i < connectedVerts.Count; i++)
                {
                    int id = connectedVerts[i].connectedVertex;
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

        public List<int> BuildPath((int[] costs, int[] preds) input, GraphStructure.Vertex toVert)
        {
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

        public void PrintDijk(GraphStructure graphStructure)
        {
            (int[] costs, int[] preds) output = RunDijk(graphStructure, graphStructure.VertexList[0]);
            Console.WriteLine("Kosten:");
            foreach (int cost in output.costs)
            {
                Console.WriteLine(cost);
            }

            Console.WriteLine("Vorgänger:");
            foreach (int pred in output.preds)
            {
                Console.WriteLine(pred);
            }

            Console.WriteLine("Weg:");
            List<int> path = BuildPath(output, graphStructure.VertexList[4]);
            foreach (int i in path)
            {
                Console.WriteLine(i);
            }
        }
    }
}