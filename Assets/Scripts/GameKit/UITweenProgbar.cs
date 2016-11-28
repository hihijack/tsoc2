using UnityEngine;
using System.Collections;

public class UITweenProgbar : UITweener{

    [Range(0f, 1f)]
    public float from;
    [Range(0f, 1f)]
    public float to;

    UIProgressBar uiProgbar;

    void Awake()
    {
        uiProgbar = GetComponent<UIProgressBar>();
    }

    public float value { get { return uiProgbar.value; } set { uiProgbar.value = value; } }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        value = Mathf.Lerp(from, to, factor);
    }

    static public UITweenProgbar Begin(GameObject go, float duration, float toVal)
    {
        UITweenProgbar comp = UITweener.Begin<UITweenProgbar>(go, duration);
        comp.from = comp.value;
        comp.to = toVal;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    public override void SetStartToCurrentValue() { from = value; }
    public override void SetEndToCurrentValue() { to = value; }
}
