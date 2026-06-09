// Author:          Jason Sun
// File Name:       QueueNode.cs
// Project Name:    Tidebreak
// Creation Date:   April 27, 2026
// Modified Date:   June 8, 2026
// Description:     Helper node for queue, stores next and previous value

class QueueNode<Value> : Node<Value>
{
    // Store information for button queue
    public QueueNode<Value> Next { get; set; } = null;
    public QueueNode<Value> Prev { get; set; } = null;

    /// <summary>
    /// Constructs a queue node object
    /// </summary>
    /// <param name="val">Value to store in the node</param>
    /// <param name="prev">Previous node of this node in the queue</param>
    public QueueNode(Value val, QueueNode<Value> prev = null) : base(val)
    {
        Prev = prev;
    }
}