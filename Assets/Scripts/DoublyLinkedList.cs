public class DoublyLinkedListNode<T>
{
    public T Data { get; set; }
    public DoublyLinkedListNode<T> Next { get; set; }
    public DoublyLinkedListNode<T> Previous { get; set; }

    public DoublyLinkedListNode(T data)
    {
        this.Data = data;
    }
}

public class DoublyLinkedList<T>
{
    public DoublyLinkedListNode<T> Head { get; private set; }
    public DoublyLinkedListNode<T> Tail { get; private set; }

    public void Add(T data)
    {
        var newNode = new DoublyLinkedListNode<T>(data);
        if (Head == null)
        {
            Head = newNode;
            Tail = newNode;
            Head.Next = Tail;
            Head.Previous = Tail;
            Tail.Next = Head;
            Tail.Previous = Head;
        }
        else
        {
            Tail.Next = newNode;
            newNode.Previous = Tail;
            newNode.Next = Head;
            Head.Previous = newNode;
            Tail = newNode;
        }
    }
}