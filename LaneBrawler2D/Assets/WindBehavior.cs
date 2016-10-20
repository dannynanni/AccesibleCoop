using UnityEngine;
using System.Collections;

public class WindBehavior : MonoBehaviour {

    public LayerMask lM;
    public float speed;
    private Vector3 moveVec;

    public void DirOfWind (dir direction)
    {
        switch (direction)
        {
            case (dir.down):
                moveVec = Vector3.down;
                break;
            case (dir.up):
                moveVec = Vector3.up;
                break;
            case (dir.left):
                moveVec = Vector3.left;
                break;
            case (dir.right):
                moveVec = Vector3.right;
                break;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

public enum dir {up, down, left, right}
