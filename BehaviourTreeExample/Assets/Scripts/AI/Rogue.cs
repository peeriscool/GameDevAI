using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class Rogue : MonoBehaviour
{
    //TODO:
    /*
     * player takes damege = ally zoekt een boom (of ander soort cover) om achter te verschuilen,
     * om vanaf daar een rookbom naar de Enemy te gooien. De rookbom blokkeert het zicht van de Enemy,
     * waardoor de speler kan ontsnappen.
     * 
     * 
     * ally zoekt cover met navmesh
     * ally zoekt naar muur en probeert aan de andere kant te komen weg van de enemy
     * geef ally fov script voor het detecteren van obstacles
     * 
     * rookbom gaat fov van ai naar 0 zetten
     * rookbom gaat met raycast 
     * */
    private BTBaseNode tree;
    private NavMeshAgent agent;
    private Animator animator;
    public GameObject Throwable;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        tree = new BTNodes.BTSelectorNode(new BTNodes.BTSelectorNode(new BTNodes.Throw(Throwable, GameObject.Find("AI_Guard").transform, this.gameObject.GetComponent<FieldOfView>().validCovers, this.gameObject),new BTNodes.RunForCover(this.gameObject, this.gameObject.GetComponent<FieldOfView>().validCovers,GameObject.Find("AI_Guard").GetComponent<FieldOfView>().validTargets, GameObject.Find("Player"))), new BTNodes.BTSequenceNode(new BTNodes.BTParallelNode(1, 0, new BTNodes.DetectPosition(agent, GameObject.Find("Player"),this.gameObject), new BTNodes.PlayAnimation(animator, "Run Forward"))));
    }

    private void FixedUpdate()
    {
        this.gameObject.GetComponent<FieldOfView>().FindVisablecovers();
        tree?.Run();
    }

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
