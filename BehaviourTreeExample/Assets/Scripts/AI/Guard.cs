using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(FieldOfView))]
public class Guard : MonoBehaviour
{
    private BTBaseNode tree;
    private NavMeshAgent agent;
    private Animator animator;
    private BlackBoard BlackBoard;
    private FieldOfView fov;
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        BlackBoard = this.GetComponent<BlackBoard>();
        BlackBoard.OnInitialize();
        fov = GetComponent<FieldOfView>();
    }

    private void Start()
    {
        Vector3[] patrol = new[] {new Vector3(0,0,0), new Vector3(6,0,6), new Vector3(-6, 0, 6), new Vector3(-6, 0, -6), new Vector3(6, 0, -6), };
        //new BTNodes.BTPatrol(this.gameObject,patrol,animator, "Run Forward");
        tree = 
            new BTNodes.BTSelectorNode(new BTNodes.Attack(fov.validTargets, this.gameObject, animator, "Kick", GameObject.Find("Player")),new BTNodes.BTInverterNode(new BTNodes.BTGetObject(this.gameObject, animator, "Run Forward")), new BTNodes.BTPatrol(this.gameObject, patrol, animator, "Run Forward"));



        //  new BTNodes.BTParallelNode(1, 0, new BTNodes.Attack(), new BTNodes.PlayAnimation(animator, "Idle")),
        //   new BTNodes.BTParallelNode(2,0, new BTNodes.BTSequenceNode(),new BTNodes.DetectPosition(agent,new Vector3(0,0,0))));


    }

    private void FixedUpdate()
    {
        fov.FindVisableTargets();
        tree?.Run();
    }


    //add event die aangeeft dat de nodes geinterupt worden
    //vervolgens reset je de hele node tree
    //

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Handles.color = Color.yellow;
    //    Vector3 endPointLeft = viewTransform.position + (Quaternion.Euler(0, -ViewAngleInDegrees.Value, 0) * viewTransform.transform.forward).normalized * SightRange.Value;
    //    Vector3 endPointRight = viewTransform.position + (Quaternion.Euler(0, ViewAngleInDegrees.Value, 0) * viewTransform.transform.forward).normalized * SightRange.Value;

    //    Handles.DrawWireArc(viewTransform.position, Vector3.up, Quaternion.Euler(0, -ViewAngleInDegrees.Value, 0) * viewTransform.transform.forward, ViewAngleInDegrees.Value * 2, SightRange.Value);
    //    Gizmos.DrawLine(viewTransform.position, endPointLeft);
    //    Gizmos.DrawLine(viewTransform.position, endPointRight);

    //}
}

//patrol tree
//  new BTNodes.BTSequenceNode(new BTNodes.BTParallelNode(2, 0, new BTNodes.Pickup(this.gameObject, new Vector3(10, 0, 10), this.transform.position), new BTNodes.PlayAnimation(animator, "Run Forward")));
// obj pickup tree
//new BTNodes.BTSelectorNode(new BTNodes.BTParallelNode(1, 0, new BTNodes.Pickup(this.gameObject,new Vector3(10,0,10),this.gameObject.transform.position), new BTNodes.PlayAnimation(animator, "Run Forward")));
// attack node
//new BTNodes.Attack(fov.validTargets,this.gameObject,animator,"Kick",GameObject.Find("Player"));