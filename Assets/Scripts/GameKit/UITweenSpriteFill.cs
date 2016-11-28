using UnityEngine;
using System.Collections;

public class UITweenSpriteFill : UITweener {

    [Range(0f, 1f)] public float from;
    [Range(0f, 1f)] public float to;

    UISprite uiSprite;

    void Awake()
    {
        uiSprite = GetComponent<UISprite>();
    }
    
    public float value { get { return uiSprite.fillAmount; } set { uiSprite.fillAmount = value; } }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        value = Mathf.Lerp(from, to, factor);
    }

    static public UITweenSpriteFill Begin(GameObject go, float duration, float toVal)
    {
        UITweenSpriteFill comp = UITweener.Begin<UITweenSpriteFill>(go, duration);
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
