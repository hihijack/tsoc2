using UnityEngine;
using System.Collections;

public class ProjCtl : MonoBehaviour
{
    public Transform tf;

    ProjModeBase mode;

    void Awake()
    {
        this.tf = transform;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (mode != null)
        {
            mode.OnUpdate();
        }
    }

    public void SetProjMode(ProjModeBase mode)
    {
        mode.ctl = this;
        this.mode = mode;
    }

    public static ProjCtl ProjAGobj(GameObject gobj)
    {
        if (gobj != null)
        {
            return gobj.AddComponent<ProjCtl>();
        }
        return null;
    }
}
