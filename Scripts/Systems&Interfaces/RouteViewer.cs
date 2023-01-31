using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteViewer : MonoBehaviour
{
    [SerializeField] private Transform[] ControlPoint;

    private Vector2 GizmoPosition;

    private void OnDrawGizmos()
    {
        for (float t=0; t<= 1; t += 0.05f)
        {
            GizmoPosition = Mathf.Pow(1 - t, 3) * ControlPoint[0].position +
                3 * Mathf.Pow(1 - t, 2) * t * ControlPoint[1].position +
                3 * (1 - t) * Mathf.Pow(t, 2) * ControlPoint[2].position +
                Mathf.Pow(t, 3) * ControlPoint[3].position;

            Gizmos.DrawSphere(GizmoPosition, 0.25f);
        }

        Gizmos.DrawLine(new Vector2(ControlPoint[0].position.x, ControlPoint[0].position.y),
            new Vector2(ControlPoint[1].position.x, ControlPoint[1].position.y));


        Gizmos.DrawLine(new Vector2(ControlPoint[2].position.x, ControlPoint[2].position.y),
            new Vector2(ControlPoint[3].position.x, ControlPoint[3].position.y));
    }
}
