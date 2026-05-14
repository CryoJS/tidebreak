using System;

class Queue<Value>
{
    // Store data about the collection // REVIEW can i implement queue but with integrated linked list and therefore renamed head tail to front back?
    private QueueNode<Value> front = null;
    private QueueNode<Value> back = null;
    public int count { get; private set; }

    public Queue()
    {
        count = 0;
    }

    public bool IsEmpty()
    {
        return count == 0;
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
            back.next = new QueueNode<Value>(val, back);
            back = back.next;
        }

        // Increment count
        count++;
    }

    public Value Dequeue()
    {
        // Only remove from queue if it is not empty
        if (!IsEmpty())
        {
            // Store the value stored at the front node before removing it
            Value val = front.val;

            // Set the front to the next value
            front = front.next;

            // Remove deleter references of old front, if queue is now empty reset back too
            if (front != null) front.prev = null;
            else back = null;

            // Decrement count and return
            count--;
            return val;
        }

        // Tell the user that a logic error has occured and return
        Console.WriteLine("ERROR - Queue<Value>.Dequeue() called when Queue<Value> was empty");
        return default;
    }
}