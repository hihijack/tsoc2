using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(UISlider))]
public class HPBarHurtEff : MonoBehaviour
{
    public UISlider target;
    /// <summary>
    /// 最大受击间隔时间
    /// </summary>
    public float interTime;
    UISlider slider;

    bool hitting = false;

    float lastHitTime;

    void Awake()
    {
        slider = GetComponent<UISlider>();
        target.onChange.Add(new EventDelegate(OnTargetValChange));
    }

    private void OnTargetValChange()
    {
        lastHitTime = Time.time;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!hitting || (Time.time - lastHitTime) < interTime)
        {
            hitting = true;
        }
        else
        {
            slider.value = target.value;
            hitting = false;
        }
    }
}
