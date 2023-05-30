using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

namespace Wunderwunsch.HexGridSimplified
{

    public class PathNode : FastPriorityQueueNode //FastPriorityQueue needs the Elements in it to inherit from FastPriorityQueueNode
    {
        //2*24 bit, should be still okay a struct but check if defining it as class might help. Also we could use axis coordinated to change it to 2*16 bit or we even use short or sbyte instead?
        public readonly Vector3Int position; //readonly might hurt performance, I read it doesn't affect runtime cost but then it apparently actually does in some situations? just test it!
        public readonly PathNode previousNode;
        public readonly MovementCost costToThisNode;

        public PathNode(Vector3Int position, PathNode previousNode, MovementCost costToThisNode)
        {
            this.position = position;
            this.previousNode = previousNode;
            this.costToThisNode = costToThisNode;
        }

        public override string ToString()
        {
            string prevNodePosition;
            if (previousNode == null) prevNodePosition = "NONE";
            else prevNodePosition = previousNode.position.ToString();
            return ("posSelf: " + position + " | posPrevious: " + prevNodePosition + " | costToThisNode: " + costToThisNode);
        }
    }
}
