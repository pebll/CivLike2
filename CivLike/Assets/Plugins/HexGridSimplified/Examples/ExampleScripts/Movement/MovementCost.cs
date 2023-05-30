using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Wunderwunsch.HexGridSimplified
{
    public struct MovementCost : IEqualityComparer<MovementCost>, IEquatable<MovementCost>, IComparable<MovementCost>
    {
        public readonly int turnCost;
        public readonly int movementPointCost;

        public MovementCost(int turnCost, int movementPointCost)
        {
            this.turnCost = turnCost;
            this.movementPointCost = movementPointCost;
        }

        public int CompareTo(MovementCost other)
        {
            if (this.turnCost < other.turnCost) return -1;
            if (this.turnCost > other.turnCost) return 1;
            //if we are here then turnCost is even
            if (this.movementPointCost < other.turnCost) return -1;
            else return 1;
        }

        public bool Equals(MovementCost other)
        {
            return this.movementPointCost == other.movementPointCost && this.turnCost == other.turnCost;
        }

        public override bool Equals(object obj)
        {
            if (obj is MovementCost)
            {
                MovementCost c = (MovementCost)obj;
                return this.movementPointCost == c.movementPointCost && this.turnCost == c.turnCost;
            }
            else return false;
        }

        public int GetHashCode(MovementCost m)
        {
            unchecked
            {
                return m.turnCost * 31 + m.movementPointCost;
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return turnCost * 31 + movementPointCost;
            }
        }

        public bool Equals(MovementCost f, MovementCost s)
        {
            return f.movementPointCost == s.movementPointCost && f.movementPointCost == s.movementPointCost;
        }

        public static bool operator ==(MovementCost a, MovementCost b)
        {
            if (a.turnCost == b.turnCost && a.movementPointCost == b.movementPointCost) return true;
            return false;
        }

        public static bool operator !=(MovementCost a, MovementCost b)
        {
            if (a.turnCost != b.turnCost || a.movementPointCost != b.movementPointCost) return true;
            return false;
        }

        public static bool operator >(MovementCost a, MovementCost b)
        {
            if (a.turnCost > b.turnCost) return true;
            if (a.turnCost < b.turnCost) return false;
            if (a.movementPointCost > b.movementPointCost) return true; // if we are here then turncost is identicals
            return false;
        }

        public static bool operator <(MovementCost a, MovementCost b)
        {
            if (a.turnCost < b.turnCost) return true;
            if (a.turnCost > b.turnCost) return false;
            if (a.movementPointCost < b.movementPointCost) return true; // if we are here then turncost is identicals
            return false;
        }

        public static bool operator >=(MovementCost a, MovementCost b)
        {
            if (a.turnCost > b.turnCost) return true;
            if (a.turnCost < b.turnCost) return false;

            if (a.movementPointCost >= b.movementPointCost) return true;
            return false;
        }

        public static bool operator <=(MovementCost a, MovementCost b)
        {
            if (a.turnCost < b.turnCost) return true;
            if (a.turnCost > b.turnCost) return false;

            if (a.movementPointCost <= b.movementPointCost) return true;
            return false;
        }

        public override string ToString()
        {
            return "TurnCost: " + turnCost + " MovementCost: " + movementPointCost;
        }
    }
}
