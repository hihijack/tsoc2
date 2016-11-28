using UnityEngine;
using System.Collections;

public class TestIGame{
    protected int a;
}

public class TTest : TestIGame
{
    public int A
    {
        set
        {
            this.a = value;
        }
        get
        {
            return this.a;
        }
    }
}