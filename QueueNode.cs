class QueueNode<Value> : Node<Value>
{
    // Store information for button queue
    public QueueNode<Value> Next { get; set; } = null;
    public QueueNode<Value> Prev { get; set; } = null;

    public QueueNode(Value val, QueueNode<Value> prev = null) : base(val)
    {
        Prev = prev;
    }
}