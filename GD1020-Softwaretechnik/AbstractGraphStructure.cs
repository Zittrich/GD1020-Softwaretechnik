using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD1020_Softwaretechnik
{
    public class AbstractGraphStructure<TConstituent, TResult> //T
    {

        private Func<TConstituent[]> _constructor;

        public AbstractGraphStructure(Func<TConstituent[]> constructor)
        {
            _constructor = constructor;
        }

        /*public TResult GenerateGraph()
        {

        }*/

        public void MakeNull<T>(TResult graph)
        {
            try
            {
                //graph = null;
            }

            catch
            {
                throw new Exception("the graph type is not nullable");
            }
        }

        public void InsertVertex(TConstituent vertex)
        {

        }

        public void DeleteVertex(TConstituent vertex)
        {

        }

        public void ConnectVertices(TConstituent vertexA, TConstituent vertexB)
        {

        }

        public void DisconnectVertices(TConstituent vertexA, TConstituent vertexB)
        {

        }

        //member_vertex
        //member_edge
        //edge_weight
        //mark_vertex
        //mark_edge
    }
}
