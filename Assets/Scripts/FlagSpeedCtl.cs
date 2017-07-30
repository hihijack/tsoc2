using UnityEngine;
using System.Collections;

public class FlagSpeedCtl : MonoBehaviour
{
    public SpriteRenderer txu;
    
    public void RefreshState(int val)
    {
        if (val > 0)
        {
            txu.color = Color.green;
        }
        else if(val <= 0)
        {
            txu.color = Color.red;
        }
    }

    public void SetVisible(bool visible)
    {
       gameObject.SetActive(visible);
    }
}
