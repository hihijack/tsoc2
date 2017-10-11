using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UIVisibleCtl : MonoBehaviour
{
    public bool showing = false;
    public Vector3 oriPos;

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            oriPos = transform.localPosition;
        }
    }

    public void SetVisible(bool val)
    {
        if (val)
        {
            transform.localPosition = oriPos;
        }
        else
        {
            transform.localPosition = Vector3.right * 3000;
        }
        showing = val;
    }
}
