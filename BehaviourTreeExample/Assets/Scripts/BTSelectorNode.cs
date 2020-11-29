using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
               if(current != TaskStatus.Failed)
                {
                    return current;
                }

            }
            return TaskStatus.Failed;
        }
    }
    
    public class BTSequenceNode : BTBaseNode
    {
        private BTBaseNode[] nodes;
        public BTSequenceNode(params BTBaseNode[] inputNodes)
        {
            nodes = inputNodes;
        }

        public override TaskStatus Run()
        {
            foreach (BTBaseNode node in nodes)
            {
                TaskStatus result = node.Run();
                switch (result)
                {
                    case TaskStatus.Failed: return TaskStatus.Failed;
                    case TaskStatus.Success: continue;
                    case TaskStatus.Running: return TaskStatus.Running;
                }
            }
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
                else if(current == TaskStatus.Failed)
                {
                    return TaskStatus.Success;
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
            if(numRequiredToSucceed > 0 && numChildrenSuceeded>= numRequiredToFail)
            {
                return TaskStatus.Success;
            }
            if(numRequiredToFail > 0 && numChildrenFailed >= numRequiredToFail)
            {
                return TaskStatus.Failed;
            }
            return TaskStatus.Running;
        }
    }




    public class PatrolLeaf : BTBaseNode
    {
        public override TaskStatus Run()
        {
            throw new System.NotImplementedException();
        }
    }

    public class PatrolPickup : BTBaseNode
    {
        public override TaskStatus Run()
        {
            throw new System.NotImplementedException();
        }
    }

    public class PatrolAttack : BTBaseNode
    {
        public override TaskStatus Run()
        {
            throw new System.NotImplementedException();
        }
    }

}
