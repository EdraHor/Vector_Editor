namespace Vector_Editor
{
    public class TNode<T>
    {
        public T Data;
        public TNode<T> Next;

        public TNode(T data)
        {
            Data = data;
        }
    }
}

