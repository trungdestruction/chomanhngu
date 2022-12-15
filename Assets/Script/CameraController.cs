using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    [SerializeField] private Transform camTransform;

    public Vector3 Offset;
    [SerializeField] private Vector3 velocity = Vector3.zero;

    private float SmoothTime = 0.1f;

    void Awake()
    {
        Application.targetFrameRate = 60;

    }
    void Start()
    {
        Offset = camTransform.position - target.position;
    }

    void LateUpdate()
    {
        if (target.GetComponent<PlayerController>().isFinished == true)
        {
            Vector3 targetPosition = target.position + Offset;
            camTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
        }
        else
        {
            camTransform.position = new Vector3(transform.position.x, transform.position.y, target.transform.position.z + Offset.z);
        }
    }
}
