using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD1020_Softwaretechnik
{
    class Dijkstra
    {
        /*
         * Start: Wählt schrittweise den nächsten kürzesten Weg aus
         * Dabei kann er Verbesserungen vornehmen
         * Es wird gespeichert: Welcher Knoten Vorgänger ist und wie teuer der Weg ist
         *
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
         *
         */

        public int[] RunDijk2(GraphStructure graph, GraphStructure.Vertex startVert)
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
                        }
                        else
                        {
                            if (connectedVerts[i].weight + costs[currentVert] < costs[id])
                            {
                                costs[id] = connectedVerts[i].weight + costs[currentVert];
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

            return costs;
        }

        public void RunDijk(GraphStructure graph, GraphStructure.Vertex startVert)
        {
            //Initialisierung

            //temp Vars
            int vertAmount = graph.VertexList.Count;
            int startVertId = 0;

            //Kosten Wege zu allen Knoten
            int[] costs = new int[vertAmount];
            for (int i = 0; i < vertAmount; i++)
            {
                costs[i] = -1;
            }

            //Vorgänger
            int[] preds = new int[vertAmount];

            //sammelt alle IDs in einem Array, neue ID ist der Index im Array
            int[] ids = new int[vertAmount];
            for (int i = 0; i < vertAmount; i++)
            {
                ids[i] = graph.VertexList[i].ID;
                //Kosten zum Startknoten = 0
                if (graph.VertexList[i].ID == startVert.ID)
                {
                    costs[i] = 0;
                    startVertId = i;
                }
            }

            //Queue und Done
            List<int> queue = new List<int>();
            queue.Add(startVertId);
            List<int> done = new List<int>();

            //Rumpf

            //Wähle ersten Knoten aus
            //! Schleife mit Abbruchbedingung queue.Count == 0
            int currentVert = queue[0];
            List<(int weight, int connectedVertex)> connectedVerts = graph.ConnectionDictionary[ids[currentVert]];
            for (int i = 0; i < connectedVerts.Count; i++)
            {


                //falsch
                if (costs[currentVert] == -1)
                {
                    //ich glaube das ist nicht richtig
                    costs[currentVert] = connectedVerts[i].weight;
                }
                else
                {
                    int tempCost = costs[currentVert] + connectedVerts[i].weight;
                    if (tempCost < costs[currentVert])
                    {
                        costs[currentVert] = tempCost;
                        preds[currentVert] = connectedVerts[i].connectedVertex;
                    }
                }
            }
        }


        /*
        //Wikipedia
        public void RunDijkstra(Graph, Startknoten)
        {
            //1. initialisieren, Infos

            //für jeden Knoten in vertexList
            //  Abstand = -1
            //  Vorgänger = null
            //Abstand Startknoten = 0
            //Liste mit allen Vertices remainingVert

            //while remainingVert.Count > 0
            //u = knoten mit kleinstem Abstand
            //remove u aus remainingVert
            //für alle Knoten v mit Nachbar u (Edge)
            //falls v in Q

            //Update Distanz (Funktion)
            //temp = abstand[u] + abstand_zwischen(u, v)
            //falls alternativ < abstand[v]
            //  abstand[v] = temp
            //  vorgänger[v] = u

            //return vorgänger[]

        }
        */
    }
}