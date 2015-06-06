using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.BSPTree
{
    class Leaf
    {
     
    public float[] x,z; // the position and size of this Leaf

    private Leaf leftChild; // the Leaf's left child Leaf
    internal Leaf LeftChild
    {
        get { return leftChild; }
        set { leftChild = value; }
    }
    private Leaf rightChild; // the Leaf's right child Leaf
    internal Leaf RightChild
    {
        get { return rightChild; }
        set { rightChild = value; }
    }

    public List<Hall>halls; // hallways to connect this Leaf to other Leafs
    public string name;
    public  Leaf(float[] _x,float[] _z,string _name)
    {
        x = _x;
        z = _z;
        name = _name;
        rightChild = null;
        leftChild = null;
        halls = new List<Hall>();
    }
     public void AddHall(Hall newHall)
     {
         halls.Add(newHall);
     }
     public void RemoveHall(Hall removingHall)
     {
         foreach(Hall hall in halls)
         {
             if(hall == removingHall)
             {
                 halls.Remove(removingHall);
             }
         }
     }

        public string CheckIfPlayerIsInside(Vector3 playerPosition)
         {
            if(halls.Count != null)
            {
                foreach(Hall hall in halls)
                {
                    if( hall.checkIsPlayerInside(playerPosition))
                    {
                        return hall.name;
                    }
                }
            }
            if(playerPosition.X < x[0] || playerPosition.X > x[1])
            {
               if(rightChild != null)
               {
                   return rightChild.CheckIfPlayerIsInside(playerPosition);
               }
            }
            if(playerPosition.Z < z[0] || playerPosition.Z > z[1])
            {
                if(leftChild != null)
                 {
                     return leftChild.CheckIfPlayerIsInside(playerPosition);
                 }
            }
                return this.name;
            
         }

    }
}
