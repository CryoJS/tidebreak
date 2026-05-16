using System;

class Queue<Value>
{
    // Store data about the collection // REVIEW can i implement queue but with integrated linked list and therefore renamed head tail to front back?
    private QueueNode<Value> front = null;
    private QueueNode<Value> back = null;
    public int Count { get; private set; }

    public Queue()
    {
        Count = 0;
    }

    public bool IsEmpty()
    {
        return Count == 0;
    }

    public void Enqueue(Value val)
    {
        // If empty, new button is front and back, otherwise add after back
        if (IsEmpty())
        {
            front = back = new QueueNode<Value>(val);
        }
        else
        {
            // Add to back and update new back
            back.Next = new QueueNode<Value>(val, back);
            back = back.Next;
        }

        // Increment count
        Count++;
    }

    public Value Dequeue()
    {
        // Only remove from queue if it is not empty
        if (!IsEmpty())
        {
            // Store the value stored at the front node before removing it
            Value val = front.val;

            // Set the front to the next value
            front = front.Next;

            // Remove deleter references of old front, if queue is now empty reset back too
            if (front != null) front.Prev = null;
            else back = null;

            // Decrement count and return
            Count--;
            return val;
        }

        // Tell the user that a logic error has occured and return
        Console.WriteLine("ERROR - Queue<Value>.Dequeue() called when Queue<Value> was empty");
        return default;
    }
}