class Node<Value>
{
    // Store the cargo that the node holds
    public Value val { get; set; }

    public Node(Value val)
    {
        this.val = val;
    }
}