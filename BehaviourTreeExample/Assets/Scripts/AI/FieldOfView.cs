using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=rQG9aUWarwE
public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range (0,360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<Transform> validTargets = new List<Transform>();
    public List<Transform> validCovers = new List<Transform>();
    public List<Transform> FindVisableTargets()
    {
        validTargets.Clear();
        Collider[] targetInRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        for (int i = 0; i < targetInRadius.Length; i++)
        {
            Transform target = targetInRadius[i].transform;
            Vector3 Direction = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, Direction) < viewAngle/2)
            {
                //valid but is there obj
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if(!Physics.Raycast(transform.position,Direction,distanceToTarget,obstacleMask))
                {
                    //no obstacles
                    validTargets.Add(target);
                }
            }
        }
        return validTargets;
    }
    public List<Transform> FindVisablecovers()
    {
        validCovers.Clear();
        Collider[] targetInRadius = Physics.OverlapSphere(transform.position, viewRadius, obstacleMask);
        for (int i = 0; i < targetInRadius.Length; i++)
        {
            Transform target = targetInRadius[i].transform;
            Vector3 Direction = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, Direction) < viewAngle / 2)
            {
                //valid but is there obj
                //float distanceToTarget = Vector3.Distance(transform.position, target.position);
                //if (!Physics.Raycast(transform.position, Direction, distanceToTarget, obstacleMask))
                //{
                    //no obstacles
                    validCovers.Add(target);
                //}
            }
        }
        return validCovers;
    }
    public Vector3 DirFromAngle(float angleInDegrees, bool AngleIsGlobal)
    {
        if(!AngleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
