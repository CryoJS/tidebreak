class BSTreeNode<Value> : Node<Value>
{
    // Store information for binary search tree
    public BSTreeNode<Value> Left { get; set; } = null;
    public BSTreeNode<Value> Right { get; set; } = null;
    public BSTreeNode<Value> Parent { get; set; } = null;

    public BSTreeNode(Value val, BSTreeNode<Value> parent = null) : base(val)
    {
        Parent = parent;
    }
}