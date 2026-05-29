using System.Collections.Generic;
using System.Linq;
using CsvHelper.Configuration.Attributes;

class Stack<Value>
{
    // Store stack elements in a list
    private List<Value> stack;

    public Stack()
    {
        stack = new List<Value>();
    }

    public int Size()
    {
        return stack.Count;
    }

    public bool IsEmpty()
    {
        return stack.Count == 0;
    }

    public Value Top()
    {
        // If stack is not empty return last element in list
        if (IsEmpty()) return default;
        else return stack.Last();
    }

    public void Push(Value val)
    {
        // Add new value to the end of the list and increment size
        stack.Add(val);
    }

    public Value Pop()
    {
        // If stack is empty return last element
        if (IsEmpty()) return default;
        
        // Store last element, deletion it, and return it
        Value last = stack.Last();
        stack.RemoveAt(stack.Count - 1);
        return last;
    }

    public void Clear()
    {
        stack.Clear();
    }
}