using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : SimpleSingleton<CameraController>
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothSpeed = 0.125f;
    Vector3 originalPosition;
    float shakeDuration;
    float shakeMagnitude;


    private void Awake()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        if (shakeDuration > 0)
        {
            smoothedPosition += Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime;
        }

        transform.position = smoothedPosition;
    }

    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
