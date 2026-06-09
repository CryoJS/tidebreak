// Author:          Jason Sun
// File Name:       Stack.cs
// Project Name:    Tidebreak
// Creation Date:   April 27, 2026
// Modified Date:   June 8, 2026
// Description:     Stack collection build with a List<>

using System.Collections.Generic;
using System.Linq;

class Stack<Value>
{
    // Store stack elements in a list
    private List<Value> stack;

    /// <summary>
    /// Constructs a stack object
    /// </summary>
    public Stack()
    {
        stack = new List<Value>();
    }

    /// <summary>
    /// Returns the # of items in the stack
    /// </summary>
    /// <returns># of items in stack</returns>
    public int Size()
    {
        return stack.Count;
    }

    /// <summary>
    /// Retursn if stack is empty
    /// </summary>
    /// <returns>If stack empty or not</returns>
    public bool IsEmpty()
    {
        return stack.Count == 0;
    }

    /// <summary>
    /// Returns the value at the top of the stack
    /// </summary>
    /// <returns>Value at top of stack</returns>
    public Value Top()
    {
        // If stack is not empty return last element in list
        if (IsEmpty()) return default;
        else return stack.Last();
    }

    /// <summary>
    /// Pushes value onto stack
    /// </summary>
    /// <param name="val">Value to push</param>
    public void Push(Value val)
    {
        // Add new value to the end of the list and increment size
        stack.Add(val);
    }

    /// <summary>
    /// Pops from top of stack
    /// </summary>
    /// <returns>Value popped</returns>
    public Value Pop()
    {
        // If stack is empty return last element
        if (IsEmpty()) return default;
        
        // Store last element, deletion it, and return it
        Value last = stack.Last();
        stack.RemoveAt(stack.Count - 1);
        return last;
    }

    /// <summary>
    /// Empies out the stack
    /// </summary>
    public void Clear()
    {
        stack.Clear();
    }
}