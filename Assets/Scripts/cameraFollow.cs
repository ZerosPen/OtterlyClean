using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    [SerializeField]
    private float speedFollow = 2f;
    [SerializeField] private Transform target;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPost = new Vector3(target.position.x, target.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, newPost, speedFollow * Time.deltaTime);
    }
}
