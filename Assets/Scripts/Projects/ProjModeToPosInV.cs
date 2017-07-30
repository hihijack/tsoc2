using UnityEngine;
using System.Collections;
using System;

public class ProjModeToPosInV : ProjModeBase
{
    public float v;
    public Vector3 tarPos;
    public Action<ProjCtl> onEnd;

    public ProjModeToPosInV(float v, Vector3 tarPos, Action<ProjCtl> onEnd)
    {
        this.v = v;
        this.tarPos = tarPos;
        this.onEnd = onEnd;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        ctl.tf.right = (tarPos - ctl.tf.position).normalized;
        float dis = (tarPos - ctl.tf.position).magnitude;
        float vToMove = v * Time.deltaTime;
        bool end = false;
        if (vToMove > dis)
        {
            vToMove = dis;
            end = true;
        }
        ctl.tf.Translate(vToMove, 0f, 0f, Space.Self);
        if (end)
        {
            onEnd(ctl);
        }
    }
}
