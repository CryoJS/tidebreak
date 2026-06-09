// Author:          Jason Sun
// File Name:       Node.cs
// Project Name:    Tidebreak
// Creation Date:   April 27, 2026
// Modified Date:   June 8, 2026
// Description:     A basic node that stores a value

class Node<Value>
{
    // Store the cargo that the node holds
    public Value val { get; set; }

    /// <summary>
    /// Creates a node object
    /// </summary>
    /// <param name="val">The value to store in the node</param>
    public Node(Value val)
    {
        this.val = val;
    }
}