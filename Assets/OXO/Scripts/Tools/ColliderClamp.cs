using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderClamp : MonoBehaviour
{
    // You can use for quick test.

    #region Fields

    public Collider collider;
    public float speed = 10f;
    private Vector3 currentPoint;
    private Vector3 bestPoint;

    #endregion

    void Update()
    {
        bestPoint = transform.position;

        #region ExampleMovement

        Vector3 Movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.Translate(Movement * speed * Time.deltaTime);

        #endregion
        #region ClampArea

        float bestDistance = float.MaxValue;

        currentPoint = collider.ClosestPoint(transform.position);
        float currentDistance = Vector3.Distance(currentPoint, transform.position);
        if (currentDistance < bestDistance)
        {
            bestDistance = currentDistance;
            bestPoint = currentPoint;
            transform.position = bestPoint; // Set pos
        }

        #endregion
    }

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(currentPoint, .3f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(bestPoint, .3f);
    }

    #endregion
}