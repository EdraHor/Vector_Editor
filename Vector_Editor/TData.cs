using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Vector_Editor
{
    public class Node<T>
    {
        public T Data;
        public Node<T> Next;

        public Node(T data)
        {
            Data = data;
        }
    }
}
