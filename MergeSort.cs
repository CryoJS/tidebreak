using System;
using System.Collections.Generic;

static class MergeSort
{
    public static List<Value> Sort<Value>(List<Value> list, Comparison<Value> comparison)
    {
        return MergeSortRange(list, 0, list.Count - 1, comparison);
    }

    private static List<Value> MergeSortRange<Value>(List<Value> vals, int left, int right, Comparison<Value> comparison)
    {
        // If list is empty or has one element, already sorted, return the list itself
        if (left > right) return [];
        if (left == right) return [vals[left]];

        // Find midpoint, sort each half, then merge them together
        int mid = (left + right) >> 1;
        return Merge(MergeSortRange(vals, left, mid, comparison), MergeSortRange(vals, mid + 1, right, comparison), comparison);
    }

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