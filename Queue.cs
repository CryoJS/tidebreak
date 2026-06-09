// Author:          Jason Sun
// File Name:       Queue.cs
// Project Name:    Tidebreak
// Creation Date:   April 27, 2026
// Modified Date:   June 8, 2026
// Description:     A queue collection built with a doubly linked list

using System;

class Queue<Value>
{
    // Store data about the collection
    private QueueNode<Value> front = null;
    private QueueNode<Value> back = null;
    public int Count { get; private set; }

    /// <summary>
    /// Constructs a queue object, resetting count
    /// </summary>
    public Queue()
    {
        Count = 0;
    }

    /// <summary>
    /// Checks if the queue is empty or not
    /// </summary>
    /// <returns>True is empty, false if not</returns>
    public bool IsEmpty()
    {
        return Count == 0;
    }

    /// <summary>
    /// Adds a value into the back of the queue
    /// </summary>
    /// <param name="val">Value to put into queue</param>
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

    /// <summary>
    /// Deletes the front value and returns it
    /// </summary>
    /// <returns>The front value</returns>
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