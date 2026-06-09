// Author:          Jason Sun
// File Name:       MergeSort.cs
// Project Name:    Tidebreak
// Creation Date:   April 27, 2026
// Modified Date:   June 8, 2026
// Description:     Sorts a collection with the Merge Sort algorithm

using System;
using System.Collections.Generic;

static class MergeSort
{
    /// <summary>
    /// Sorts a given list with a custom comparator using merge sort
    /// </summary>
    /// <typeparam name="Value">Values to sort (inside list)</typeparam>
    /// <param name="list">List to sort</param>
    /// <param name="comparison">Custom comparator</param>
    /// <returns>New sorted list</returns>
    public static List<Value> Sort<Value>(List<Value> list, Comparison<Value> comparison)
    {
        return MergeSortRange(list, 0, list.Count - 1, comparison);
    }

    /// <summary>
    /// Recursively sorts a list by sorting halves, then merging them
    /// </summary>
    /// <typeparam name="Value">Value in list to sort</typeparam>
    /// <param name="vals">List of values to sort</param>
    /// <param name="left">Left index of the current range</param>
    /// <param name="right">RIght index of the current range</param>
    /// <param name="comparison">Custom comparator</param>
    /// <returns>The sorted range</returns>
    private static List<Value> MergeSortRange<Value>(List<Value> vals, int left, int right, Comparison<Value> comparison)
    {
        // If list is empty or has one element, already sorted, return the list itself
        if (left > right) return [];
        if (left == right) return [vals[left]];

        // Find midpoint, sort each half, then merge them together
        int mid = (left + right) >> 1;
        return Merge(MergeSortRange(vals, left, mid, comparison), MergeSortRange(vals, mid + 1, right, comparison), comparison);
    }

    /// <summary>
    /// Merges two sorted lists to one sorted list
    /// </summary>
    /// <typeparam name="Value">Value in lists to merge</typeparam>
    /// <param name="left">A list of values</param>
    /// <param name="right">Another list of values</param>
    /// <param name="comparison">Comparator to sort by</param>
    /// <returns>New merged sorted list</returns>
    private static List<Value> Merge<Value>(List<Value> left, List<Value> right, Comparison<Value> comparison)
    {
        // If either list is empty, return the other
        if (left == null) return right;
        else if (right == null) return left;

        // Create a new list of size equal to the sum of the lengths of the two given lists
        List<Value> result = new(left.Count + right.Count);

        // Integers pointing to the currently considered element of each given (left and right) list
        int li = 0;
        int ri = 0;

        // For each element in the merged list, get the next smallest element between the two given lists
        for (int i = 0; i < left.Count + right.Count; i++)
        {
            // If one side is done, take the other, otherwise take smaller element
            if (li == left.Count) result.Add(right[ri++]);
            else if (ri == right.Count) result.Add(left[li++]);
            else if (comparison(left[li], right[ri]) <= 0) result.Add(left[li++]);
            else result.Add(right[ri++]);
        }

        // Return the merged and sorted list
        return result;
    }
}