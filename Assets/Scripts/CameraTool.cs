using UnityEngine;
using System.Collections;

public class CameraTool : MonoBehaviour {

	void OnDrawGizmos(){
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(transform.position, new Vector3(1280f / 720f * GetComponent<Camera>().orthographicSize * 2, GetComponent<Camera>().orthographicSize * 2, 0f));
	}
}
