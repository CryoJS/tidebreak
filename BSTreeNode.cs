// Author:          Jason Sun
// File Name:       BSTreeNode.cs
// Project Name:    Tidebreak
// Creation Date:   May 13, 2026
// Modified Date:   June 8, 2026
// Description:     Node inside BST, has helper values: left, right, parent

class BSTreeNode<Value> : Node<Value>
{
    // Store information for binary search tree
    public BSTreeNode<Value> Left { get; set; } = null;
    public BSTreeNode<Value> Right { get; set; } = null;
    public BSTreeNode<Value> Parent { get; set; } = null;

    /// <summary>
    /// Constructs a node for the BST
    /// </summary>
    /// <param name="val">The value stored in the node</param>
    /// <param name="parent">The parent node of this node</param>
    public BSTreeNode(Value val, BSTreeNode<Value> parent = null) : base(val)
    {
        Parent = parent;
    }
}