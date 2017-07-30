using UnityEngine;
using System.Collections;

public class AnimListener : MonoBehaviour
{
    Enermy _npc;
    Enermy NPC
    {
        get
        {
            if (_npc == null)
            {
                _npc = transform.parent.gameObject.GetComponent<Enermy>();
            }
            return _npc;
        }
    }
    public void OnAtkBeforeEnd()
    {
        NPC.OnAtkBeforeEnd();
    }

    public void OnAtkAfterEnd()
    {
        NPC.OnAtkAfterEnd();
    }
}
