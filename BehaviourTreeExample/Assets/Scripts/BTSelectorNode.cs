using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BTNodes
{
    public class BTSelectorNode : BTBaseNode
    {
        private BTBaseNode[] NodeOptions;
        public BTSelectorNode(params BTBaseNode[] inputs)
        {
            NodeOptions = inputs;
        }
        public override TaskStatus Run()
        {
            
            foreach (BTBaseNode node in NodeOptions)
            {
                TaskStatus current = node.Run();
                Debug.Log(current);
                if (current != TaskStatus.Failed)
                {
                    
                    return current;
                }
            }
            return TaskStatus.Failed;
        }
    }
    
    public class BTSequenceNode : BTBaseNode
    {
   
        BTBaseNode currentnode;
        private BTBaseNode[] nodes;
        public BTSequenceNode(params BTBaseNode[] inputNodes)
        {
            nodes = inputNodes;
        }

        public override TaskStatus Run()
        {
            foreach (BTBaseNode node in nodes)
            {  
                if(currentnode != null && currentnode != node) //the node is still running
                {
                    
                    continue;
                }
                TaskStatus result = node.Run();
               
                switch (result)
                {
                    case TaskStatus.Failed:
                        currentnode = null; return TaskStatus.Failed;
                    case TaskStatus.Success:
                        currentnode = null; continue;
                    case TaskStatus.Running:
                        currentnode = node;
                        return TaskStatus.Running;
                }
                
            }
            currentnode = null;
            return TaskStatus.Success;
        }
    }

    public class BTInverterNode : BTBaseNode
    {
        private BTBaseNode[] NodeOptions;
        public BTInverterNode(params BTBaseNode[] inputs)
        {
            NodeOptions = inputs;
        }
        public override TaskStatus Run()
        {
            foreach (BTBaseNode node in NodeOptions)
            {
                TaskStatus current = node.Run();
                if (current == TaskStatus.Failed)
                {
                    return TaskStatus.Success;
                }
                else if(current == TaskStatus.Success)
                {
                    return TaskStatus.Failed;
                }
                else
                {
                    return current;
                }
            }
            return TaskStatus.Failed;
        }
    }

    public class BTParallelNode : BTBaseNode
    {
        private BTBaseNode[] NodeOptions;
        private int numRequiredToSucceed;
        private int numRequiredToFail;
        public BTParallelNode(int NumRequiredToSucceed,int NumRequiredToFail, params BTBaseNode[] inputs)
        {
            NodeOptions = inputs;
            numRequiredToFail = NumRequiredToFail;
            numRequiredToSucceed = NumRequiredToSucceed;
        }
        public override TaskStatus Run()
        {
            var numChildrenSuceeded = 0;
            var numChildrenFailed = 0;
            foreach (BTBaseNode node in NodeOptions)
            {
                TaskStatus current = node.Run();
                switch (current)
                {
                    case TaskStatus.Success: ++numChildrenSuceeded;
                        break;
                    case TaskStatus.Failed: ++numChildrenFailed;
                        break;
                }
            }
            if(numRequiredToSucceed > 0 && numChildrenSuceeded>= numRequiredToSucceed)
            {
                Debug.Log("ParrallelNode Success");
                return TaskStatus.Success; 
            }
            if(numRequiredToFail > 0 && numChildrenFailed >= numRequiredToFail)
            {
                Debug.Log("ParrallelNode fail");
                return TaskStatus.Failed;
            }
            return TaskStatus.Running;
        }
    }




    public class MoveToPosition : BTBaseNode //divide in 2 nodes seek position and movetoposition
    {
        Vector3 Location;
        Vector3[] Locations;
        NavMeshAgent Agent;
        GameObject Moveable;
        bool List;
        int index = 0;
        int indexmax;
        Vector3 ActiveLocation = new Vector3(0,0,0);
        public MoveToPosition(GameObject move, Vector3[] locations)
        {
            Locations = locations;
            Agent = move.GetComponent<NavMeshAgent>();
            Moveable = move;
            List = true;
            indexmax = locations.Length;
            Debug.Log(indexmax);
           // ActiveLocation = Locations[index];
        }
        public MoveToPosition(GameObject move, Vector3 location)
        {
            List = false;
            Location = location;
            Agent = move.GetComponent<NavMeshAgent>();
            Moveable = move; ;
        }

        public override TaskStatus Run()
        {
            if(List)
            {
                //Vector3 a = new Vector3(Mathf.Round(Moveable.transform.position.x), Mathf.Round(Moveable.transform.position.y), Mathf.Round(Moveable.transform.position.z));
                if ( Moveable.transform.position != ActiveLocation)
                {
                    Agent.SetDestination(ActiveLocation);
                    return TaskStatus.Running;
                }
                if (Moveable.transform.position == ActiveLocation)
                {
                    if(index != indexmax -1)
                    {
                        index++;
                        ActiveLocation = Locations[index];
                    }
                    else
                    {
                        index = 0;
                    }
                    return TaskStatus.Running;
                }
            }

            if (!List)
            {
                if (Moveable.transform.position != Location)
                {
                    Agent.SetDestination(Location);
                    return TaskStatus.Running;
                }
                if(Moveable.transform.position == Location)
                {
                    return TaskStatus.Success;
                }
            }
            return TaskStatus.Failed;
        }
    }

    public class Pickup : BTBaseNode
    {
        NavMeshAgent guardmesh;
        GameObject Weapon;
        Vector3 EnemyPos;
        GameObject Guard;
        public Pickup(GameObject guard,Vector3 pickuplocation, Vector3 enemyPos)
        {
            Weapon = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Weapon.transform.localScale /= 4;
            Weapon.transform.position = pickuplocation;
            EnemyPos = enemyPos;
            Guard = guard;
            guardmesh = guard.GetComponent<NavMeshAgent>();

        }
        public override TaskStatus Run()
        {
            EnemyPos = Guard.transform.position;
            guardmesh.SetDestination(Weapon.transform.position);
            //check if boi has weapon
            //elso get weapon
          //  Debug.Log(EnemyPos);
            if(Vector3.Distance(EnemyPos, Weapon.transform.position) <= 0.2f)
            {
                Debug.Log("I've arrived at pickup location");
                Weapon.transform.parent = Guard.transform;
                Weapon.transform.position = Weapon.transform.position;

                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
    }

    public class Attack : BTBaseNode
    {
        List <Transform> playerLocationList;
        GameObject Enemy;
        NavMeshAgent Agent;
        Animator animator;
        string AnimationName;
        Player Playerscript;
        GameObject Player;
        public Attack(List<Transform> playerlocation,GameObject enemy,Animator anim, string animatonName,GameObject player)
        {
            Enemy = enemy;
            Agent = Enemy.GetComponent<NavMeshAgent>();
            animator = anim;
            AnimationName = animatonName;
            playerLocationList = playerlocation;
            Playerscript = player.GetComponent<Player>();
            Player = player;
        }
        public override TaskStatus Run()
        {
            if(playerLocationList.Count == 0)
            {
                return TaskStatus.Failed;
            }
            Agent.SetDestination(playerLocationList[0].position);
           // Debug.Log(Vector3.Distance(playerLocationList[0].position, Enemy.transform.position));
            if (Vector3.Distance(playerLocationList[0].position, Enemy.transform.position) < 2)
            {
                    animator.Play(AnimationName); 
                    Playerscript.TakeDamage(Enemy, 40);
                    return TaskStatus.Success; 
            }

            return TaskStatus.Running;
        }
    }
    public class WeaponCheck : BTBaseNode
    {
        GameObject Owner;
        public WeaponCheck(GameObject owner)
        {
            Owner = owner;
        }
        public override TaskStatus Run()
        {
            foreach (Transform child in Owner.transform)//check if weapon has been obtained
            {
                if(child.name == "Weapon")
                {
                    //weaponFound
                    Debug.Log("Lady's and Gentleman we got em!");
                    return TaskStatus.Success;
                }
            }    
                 return TaskStatus.Running;
               
        }
    }
    public class PlayAnimation : BTBaseNode
    {
        Animator guardanim;
        string animation;
        public PlayAnimation(Animator guard, string animationname)
        {
            guardanim = guard;
            animation = animationname;
        }
        public override TaskStatus Run()
        {
            guardanim.Play(animation);
            return TaskStatus.Failed;
        }
    }
    public class DetectPosition : BTBaseNode //fix new positions
    {
        NavMeshAgent guardmesh;
        GameObject follow;
        GameObject MyLocation;
        public DetectPosition(NavMeshAgent guard, GameObject location,GameObject myLocation)
        {
            follow = location;
            guardmesh = guard;
            MyLocation = myLocation;

        }
        public override TaskStatus Run()
        {
          //  Debug.Log(Vector3.Distance(MyLocation.transform.position, follow.transform.position));
            if (Vector3.Distance(MyLocation.transform.position, follow.transform.position) > 2)
            {
                guardmesh.SetDestination(follow.transform.position);
                return TaskStatus.Running;
            }
            return TaskStatus.Failed;
        }
    }
    public class BTPatrol : BTBaseNode
    {
        GameObject owner;
        Vector3[] position;
        Animator animator;
        BTBaseNode patroltree;
        string AnimationName;
        public BTPatrol(GameObject obj,Vector3[] list,Animator anim, string animationname)
        {
            owner = obj;
            position = list;
            animator = anim;
            AnimationName = animationname;
            patroltree = new BTNodes.BTParallelNode(1, 0, new BTNodes.MoveToPosition(owner, position), new BTNodes.PlayAnimation(animator, AnimationName)); //patrol
        }
        public override TaskStatus Run()
        {
            return (TaskStatus) patroltree?.Run();
             
        }
    }

    public class BTGetObject : BTBaseNode
    {
        BTBaseNode Getobjectree;
        GameObject owner;
        Animator animator;
        string AnimationName;
        public BTGetObject(GameObject obj,Animator anim, string animationname)
        {
            owner = obj;
            animator = anim;
            AnimationName = animationname;
            Getobjectree = new BTNodes.BTParallelNode(1, 0, new BTNodes.Pickup(owner, new Vector3(10, 0, 10), owner.transform.position), new BTNodes.PlayAnimation(animator, AnimationName));
        }
        public override TaskStatus Run()
        {
          return (TaskStatus) Getobjectree?.Run();
        }
    }
        public class Throw : BTBaseNode
        {
        GameObject Object;
        Transform Target;
        Transform Safelocation;
        GameObject Thrower;
         public Throw(GameObject throwable,Transform target, Transform Hidespot,GameObject thrower)
         {
            Object = throwable;
            Target = target;
            Safelocation = Hidespot;
            Thrower = thrower;
         }
         public override TaskStatus Run()
         {
            if(Vector3.Distance(Safelocation.position,Thrower.transform.position) < 2)
            {
                //safe enough to throw
            }
          return TaskStatus.Failed;
         }
        }
    public class RunForCover : BTBaseNode
    {
        NavMeshAgent agent;
        GameObject Runner;
        List<Transform> Vision;
        List<Transform> Compare;
        List<Transform> EnemiesVision;
        GameObject Player;
        public RunForCover(GameObject runner,List <Transform> visablelocation, List<Transform> enemiesVision,GameObject player )
        {
            Runner = runner;
            agent = runner.GetComponent<NavMeshAgent>();
            Vision = visablelocation;
            Compare = new List<Transform>();
            EnemiesVision = enemiesVision;
            Player = player;
        }
        public override TaskStatus Run()
        {
            if(Compare.Count != Vision.Count)
            {
                NavMeshHit hit = new NavMeshHit();
                hit.position = Vision[0].position;
                if(agent.FindClosestEdge(out hit))
                {
                    DrawCircle( Vision[0].position, hit.distance, Color.red);
                    //if player is in sight of enemy
                    //navmesh.setdestiantion
                    foreach(Transform target in EnemiesVision)
                    {
                        if(target == Player.transform)
                        {
                            agent.SetDestination(Vision[0].position);
                            return TaskStatus.Success;
                        }
                    }
                }
            }
            return TaskStatus.Failed;
        }
        void DrawCircle(Vector3 center, float radius, Color color)
        {
            Vector3 prevPos = center + new Vector3(radius, 0, 0);
            for (int i = 0; i < 30; i++)
            {
                float angle = (float)(i + 1) / 30.0f * Mathf.PI * 2.0f;
                Vector3 newPos = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                Debug.DrawLine(prevPos, newPos, color);
                prevPos = newPos;
            }
        }
    }
}