using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCamera : MonoBehaviour {

    public Transform Target;
    public float FollowSpeed = 2f;

    private float initialZ;

    private void Start()
    {
        initialZ = transform.position.z;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position, Time.deltaTime * 2f);
        transform.position = new Vector3(transform.position.x, transform.position.y, initialZ);
    }

}
