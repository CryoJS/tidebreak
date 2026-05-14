class QueueNode<Value> : Node<Value>
{
    // Store information for button queue
    public QueueNode<Value> next { get; set; } = null;
    public QueueNode<Value> prev { get; set; } = null;

    public QueueNode(Value val, QueueNode<Value> prev = null) : base(val)
    {
        this.prev = prev;
    }
}