using System;

class BSTree<Value> where Value : IComparable<Value>
{
    // Store required data
    private BSTreeNode<Value> root;
    public int count { get; private set; }

    public BSTree()
    {
        count = 0;
    }

    public bool IsEmpty()
    {
        return count == 0;
    }

    public Value GetLeftmost()
    {
        // If the tree is empty, return null (for objects, but default since template Value is used)
        if (root == null) return default;

        // Explore tree starting from root, always going left
        BSTreeNode<Value> cur = root;
        while (cur.left != null) cur = cur.left;

        // Return found leftmost node
        return cur.val;
    }

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
                    if (cur.left == null)
                    {
                        cur.left = new BSTreeNode<Value>(val, cur);
                        break;
                    }
                    else
                    {
                        cur = cur.left;
                    }
                }
                else
                {
                    // If no right tree, place new node, otherwise explore right
                    if (cur.right == null)
                    {
                        cur.right = new BSTreeNode<Value>(val, cur);
                        break;
                    }
                    else
                    {
                        cur = cur.right;
                    }
                }
            }
        }

        // Increment count and return true (as we didn't find the value we were adding)
        count++;
        return true;
    }

    public BSTreeNode<Value> Find(Value val)
    {
        // Explore tree starting from root
        for (BSTreeNode<Value> cur = root; cur != null;)
        {
            // If equal then found, if less than explore left subtree, else explore right subtree
            if (val.Equals(cur.val)) return cur;
            else if (val.CompareTo(cur.val) < 0) cur = cur.left;
            else cur = cur.right;
        }

        return null;
    }

    public void Delete(Value val)
    {
        // Store node we want to delete
        BSTreeNode<Value> cur = Find(val);

        // Cancel deletion if no node to delete
        if (cur == null) return;

        // Store important nodes we want to store (as they are accessed frequently)
        BSTreeNode<Value> parent = cur.parent;
        BSTreeNode<Value> left = cur.left;
        BSTreeNode<Value> right = cur.right;

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
                if (val.CompareTo(parent.val) < 0) parent.left = null;
                else parent.right = null;
            }
        }
        else if (left != null && right != null) // Two children
        {
            // Only two children exist, find rightmost (largest) node in left subtree as replacement
            BSTreeNode<Value> newCur = left;
            while (newCur.right != null) newCur = newCur.right;

            // Store current node's new value after replacement and delete replacement node
            Value newCurVal = newCur.val;
            Delete(newCurVal);

            // Replace node to be deleted with replacement node
            cur.val = newCurVal;
            count++;
        }
        else // Only one child (right)
        {
            // Prepare to store the only child of the node to be deleted
            BSTreeNode<Value> child;

            // Find and store the only child
            if (left != null) child = cur.left;
            else child = cur.right;

            // If no parent set new root as current node was root, otherwise give parent the only child 
            if (parent == null)
            {
                root = child;
                child.parent = null;
            }
            else
            {
                // If child is in delete node's parent's left subtree set left child to delete node's only child (and vice versa) 
                if (parent.left.val.Equals(val)) parent.left = child;
                else parent.right = child;
                child.parent = parent;
            }
        }

        // Decrement count
        count--;
    }

    public BSTree<Value> Copy()
    {
        // Create a new (empty) BST to store the copy
        BSTree<Value> copy = new BSTree<Value>();

        // Copy the subtree starting at the root (entire tree) and return
        CopyNode(copy, root);

        return copy;
    }

    private void CopyNode(BSTree<Value> copy, BSTreeNode<Value> node)
    {
        // If node is empty no copying done
        if (node == null) return;

        // Copy the current node, then continue exploring and copying the left and right subtrees
        copy.Add(node.val);
        CopyNode(copy, node.left);
        CopyNode(copy, node.right);
    }

    public string InOrderTreeDisplay()
    {
        // If no root, BST is empty, otherwise display
        if (root == null) return "BST is empty";
        return GetDisplayList(root);
    }

    private string GetDisplayList(BSTreeNode<Value> root)
    {
        // Display left subtree, then display root value, then display right subtree
        return (root.left == null ? "" : "(" + GetDisplayList(root.left) + ") ")
                + root.val
                + (root.right == null ? "" : " (" + GetDisplayList(root.right) + ")");
    }
}