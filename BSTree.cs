// Author:          Jason Sun
// File Name:       BSTree.cs
// Project Name:    Tidebreak
// Creation Date:   May 13, 2026
// Modified Date:   June 8, 2026
// Description:     Binary search tree collection, can store anything

using System;

class BSTree<Value> where Value : IComparable<Value>
{
    // Store required data
    private BSTreeNode<Value> root;
    public int Count { get; private set; }

    /// <summary>
    /// Constructs BSTree, setting count to 0
    /// </summary>
    public BSTree()
    {
        Count = 0;
    }

    /// <summary>
    /// Checks if the BSTree is empty
    /// </summary>
    /// <returns>Boolean, true is empty, false is not</returns>
    public bool IsEmpty()
    {
        return Count == 0;
    }

    /// <summary>
    /// Gets the leftmost node in the tree, moves to left child continuously starting from root
    /// </summary>
    /// <returns>Leftmost node in the tree</returns>
    public Value GetLeftmost()
    {
        // If the tree is empty, return null (for objects, but default since template Value is used)
        if (root == null) return default;

        // Explore tree starting from root, always going left
        BSTreeNode<Value> cur = root;
        while (cur.Left != null) cur = cur.Left;

        // Return found leftmost node
        return cur.val;
    }

    /// <summary>
    /// Gets the rightmost node in the tree, moves to right child continuously starting from root
    /// </summary>
    /// <returns>Rightmost node in the tree</returns>
    public Value GetRightmost()
    {
        // If the tree is empty, return null (for objects, but default since template Value is used)
        if (root == null) return default;

        // Explore tree starting from root, always going right
        BSTreeNode<Value> cur = root;
        while (cur.Right != null) cur = cur.Right;

        // Return found rightmost node
        return cur.val;
    }

    /// <summary>
    /// Add's a value to the BSTree
    /// </summary>
    /// <param name="val">The value to addd</param>
    /// <returns>True if success, false if failed to add</returns>
    public bool Add(Value val)
    {
        // If BST is empty, create new root
        if (root == null)
        {
            root = new BSTreeNode<Value>(val);
        }
        else
        {
            // Start at root
            BSTreeNode<Value> cur = root;

            // Explore children to check subtrees where value should be added
            while (true)
            {
                // If equal, we don't add, if less than, explore left subtree, else explore right subtree 
                if (val.Equals(cur.val))
                {
                    return false;
                }
                else if (val.CompareTo(cur.val) < 0)
                {
                    // If no left tree, place new node, otherwise explore left
                    if (cur.Left == null)
                    {
                        cur.Left = new BSTreeNode<Value>(val, cur);
                        break;
                    }
                    else
                    {
                        cur = cur.Left;
                    }
                }
                else
                {
                    // If no right tree, place new node, otherwise explore right
                    if (cur.Right == null)
                    {
                        cur.Right = new BSTreeNode<Value>(val, cur);
                        break;
                    }
                    else
                    {
                        cur = cur.Right;
                    }
                }
            }
        }

        // Increment count and return true (as we didn't find the value we were adding)
        Count++;
        return true;
    }

    /// <summary>
    /// Finds the node given the value that the BST is ordered by
    /// </summary>
    /// <param name="val">The value of the node to find</param>
    /// <returns>The node with the given value, or null if not found</returns>
    public BSTreeNode<Value> Find(Value val)
    {
        // Explore tree starting from root
        for (BSTreeNode<Value> cur = root; cur != null;)
        {
            // If equal then found, if less than explore left subtree, else explore right subtree
            if (val.Equals(cur.val)) return cur;
            else if (val.CompareTo(cur.val) < 0) cur = cur.Left;
            else cur = cur.Right;
        }

        return null;
    }

    /// <summary>
    /// Deletes a node with the given value
    /// </summary>
    /// <param name="val">Value of node to delete</param>
    public void Delete(Value val)
    {
        // Store node we want to delete
        BSTreeNode<Value> cur = Find(val);

        // Cancel deletion if no node to delete
        if (cur == null) return;

        // Store important nodes we want to store (as they are accessed frequently)
        BSTreeNode<Value> parent = cur.Parent;
        BSTreeNode<Value> left = cur.Left;
        BSTreeNode<Value> right = cur.Right;

        // Check how many children exists (none, two, one)
        if (left == null && right == null) // No children
        {
            // No children, delete, if no parent, delete easily, otherwise find reference
            if (parent == null)
            {
                root = null;
            }
            else
            {
                // Find where node is referenced by parent and delete it
                if (val.CompareTo(parent.val) < 0) parent.Left = null;
                else parent.Right = null;
            }
        }
        else if (left != null && right != null) // Two children
        {
            // Only two children exist, find rightmost (largest) node in left subtree as replacement
            BSTreeNode<Value> newCur = left;
            while (newCur.Right != null) newCur = newCur.Right;

            // Store current node's new value after replacement and delete replacement node
            Value newCurVal = newCur.val;
            Delete(newCurVal);

            // Replace node to be deleted with replacement node
            cur.val = newCurVal;
            Count++;
        }
        else // Only one child
        {
            // Prepare to store the only child of the node to be deleted
            BSTreeNode<Value> child;

            // Find and store the only child
            if (left != null) child = cur.Left;
            else child = cur.Right;

            // If no parent set new root as current node was root, otherwise give parent the only child 
            if (parent == null)
            {
                root = child;
                child.Parent = null;
            }
            else
            {
                // If child is in delete node's parent's left subtree set left child to delete node's only child (and vice versa) 
                if (parent.Left != null && parent.Left.val.Equals(val)) parent.Left = child;
                else parent.Right = child;
                child.Parent = parent;
            }
        }

        // Decrement count
        Count--;
    }

    /// <summary>
    /// Copies the BST
    /// </summary>
    /// <returns>A deep copy of the BST</returns>
    public BSTree<Value> Copy()
    {
        // Create a new (empty) BST to store the copy
        BSTree<Value> copy = new BSTree<Value>();

        // Copy the subtree starting at the root (entire tree) and return
        CopyNode(copy, root);
        return copy;
    }

    /// <summary>
    /// Recursively copies an entire subtree, helper function for Copy()
    /// </summary>
    /// <param name="copy">The final BST copy</param>
    /// <param name="node">The current node, root of current subtree to copy</param>
    private void CopyNode(BSTree<Value> copy, BSTreeNode<Value> node)
    {
        // If node is empty no copying done
        if (node == null) return;

        // Copy the current node, then continue exploring and copying the left and right subtrees
        copy.Add(node.val);
        CopyNode(copy, node.Left);
        CopyNode(copy, node.Right);
    }

    /// <summary>
    /// Displays the subtree (in-order traversal)
    /// </summary>
    /// <returns>Returns a string of all the nodes printed out (in-order)</returns>
    public string InOrderTreeDisplay()
    {
        // If no root, BST is empty, otherwise display
        if (root == null) return "BST is empty";
        return GetDisplayList(root);
    }

    /// <summary>
    /// Recursively displays the left subtree, the self node, the right subtree 
    /// </summary>
    /// <param name="root">The root of the subtree to display</param>
    /// <returns>The in-order display of the given subtree</returns>
    private string GetDisplayList(BSTreeNode<Value> root)
    {
        // Display left subtree, then display root value, then display right subtree
        return (root.Left == null ? "" : "(" + GetDisplayList(root.Left) + ") ")
                + root.val
                + (root.Right == null ? "" : " (" + GetDisplayList(root.Right) + ")");
    }
}