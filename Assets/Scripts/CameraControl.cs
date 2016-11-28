using UnityEngine;
using System.Collections;

public enum ECameraState
{
    Normal,
    Battle
}

public class CameraControl : MonoBehaviour {

    public GameObject target;
    public float distance;
    public Vector3 posInBattle;
    public Vector3 angleInBattle;
    private ECameraState state;

    public float sizeNormal = 4.4f;
    public float sizeBattle = 2.2f;
    Camera thisCamera;
    public ECameraState State
    {
        get { return state; }
        set 
        {
            state = value;
            if (state == ECameraState.Normal)
            {
               thisCamera.orthographicSize  = sizeNormal;
            }
            else if(state == ECameraState.Battle)
            {
                thisCamera.orthographicSize = sizeBattle;
            }
        }
    }

    void Awake() 
    {
        thisCamera = GetComponent<Camera>();
    }

    // Use this for initialization
	void Start () {
        State = ECameraState.Normal;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (target != null)
        {
            if (State == ECameraState.Normal)
            {
                Vector3 pos = target.transform.position;
                pos.z = distance;
                transform.position = pos;
                
            }
            else if (State == ECameraState.Battle)
            {
                transform.position = posInBattle;
            }
        }      
	}
}
