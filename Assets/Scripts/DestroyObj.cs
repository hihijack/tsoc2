using UnityEngine;
using System.Collections;

public class DestroyObj : MonoBehaviour
{

    public void Destroy() 
    {
        DestroyObject(gameObject);
    }
}
