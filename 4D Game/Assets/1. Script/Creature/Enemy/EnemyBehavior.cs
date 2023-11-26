using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private RaymarchRenderer raymarchRenderer;
    [SerializeField] private GameObject rotatingBody;
    [SerializeField] private float rotateSpeed = 60f;
    [SerializeField] private float rotateSpeed4D = 10f;
    [SerializeField] private float rotateSpeed4DModX = 1f;
    [SerializeField] private float rotateSpeed4DModY = 1f;
    [SerializeField] private float rotateSpeed4DModZ = 1f;
    [SerializeField] private float rotateSpeed4DMax = 50f;

    private void Update()
    {
        Rotating();
    }

    private void Rotating()
    {
        rotatingBody.transform.Rotate(new Vector3(0, rotateSpeed, 0) * Time.deltaTime);

        raymarchRenderer.rotW.x += rotateSpeed4D * rotateSpeed4DModX * Time.deltaTime;
        raymarchRenderer.rotW.y += rotateSpeed4D * rotateSpeed4DModY * Time.deltaTime;
        raymarchRenderer.rotW.z += rotateSpeed4D * rotateSpeed4DModZ * Time.deltaTime;

        if (Mathf.Abs(raymarchRenderer.rotW.x) > rotateSpeed4DMax)
        {
            raymarchRenderer.rotW.x = rotateSpeed4DMax * rotateSpeed4DModX;
            rotateSpeed4DModX *= -1;
        }

        if (Mathf.Abs(raymarchRenderer.rotW.y) > rotateSpeed4DMax)
        {
            raymarchRenderer.rotW.y = rotateSpeed4DMax * rotateSpeed4DModY;
            rotateSpeed4DModY *= -1;
        }

        if (Mathf.Abs(raymarchRenderer.rotW.z) > rotateSpeed4DMax)
        {
            raymarchRenderer.rotW.z = rotateSpeed4DMax * rotateSpeed4DModZ;
            rotateSpeed4DModZ *= -1;
        }

    }
}
