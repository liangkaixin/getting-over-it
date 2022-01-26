using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().transparencySortMode =
           TransparencySortMode.Orthographic;
    }

    private void FixedUpdate()
    {
        Vector3 moveVec = target.position - GetComponent<Camera>().transform.position;
        moveVec.z = 0;
        GetComponent<Camera>().transform.position += moveVec * Time.fixedDeltaTime * 4.0f;
    }
}
