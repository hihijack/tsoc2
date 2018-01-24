using UnityEngine;
using System.Collections;
using System;

public class IteratorNPCAtion
{
    int index = -1;
    int maxIndex;
    internal void StartIter(int maxIndex)
    {
        index = -1;
        this.maxIndex = maxIndex;
    }

    internal int GetNextIndex()
    {
        index++;
        if (index > maxIndex)
        {
            index = -1;
        }
        return index;
    }
}
