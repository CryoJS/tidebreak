class BSTreeNode<Value> : Node<Value>
{
    // Store information for binary search tree
    public BSTreeNode<Value> left { get; set; } = null;
    public BSTreeNode<Value> right { get; set; } = null;
    public BSTreeNode<Value> parent { get; set; } = null;

    public BSTreeNode(Value val, BSTreeNode<Value> parent = null) : base(val)
    {
        this.parent = parent;
    }
}