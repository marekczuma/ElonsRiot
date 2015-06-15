using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.BSPTree
{
    class Leaf
    {

        public float[] x, z; // the position and size of this Leaf
        public List<GameObject> gameObjectInRoom;
        public string id;
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

        public List<Hall> halls; // hallways to connect this Leaf to other Leafs
        public string name;
        public Leaf(float[] _x, float[] _z, string _name, string _id)
        {
            x = _x;
            z = _z;
            name = _name;
            rightChild = null;
            leftChild = null;
            halls = new List<Hall>();
            gameObjectInRoom = new List<GameObject>();
            id = _id;
        }
        public void AddHall(Hall newHall)
        {
            halls.Add(newHall);
        }
        public void RemoveHall(Hall removingHall)
        {
            foreach (Hall hall in halls)
            {
                if (hall == removingHall)
                {
                    halls.Remove(removingHall);
                }
            }
        }

        public string CheckIfPlayerIsInside(Vector3 playerPosition)
        {
            if (halls.Count != 0)
            {
                foreach (Hall hall in halls)
                {
                    if (hall.checkIsPlayerInside(playerPosition))
                    {
                        return hall.name;
                    }
                }
            }
            if (isInX(playerPosition) == false)
            {
                if (rightChild != null)
                {
                    return rightChild.CheckIfPlayerIsInside(playerPosition);
                }
            }
            if (isInZ(playerPosition) == false)
            {
                if (leftChild != null)
                {
                    return leftChild.CheckIfPlayerIsInside(playerPosition);
                }
            }
            return this.name;

        }

        public bool isInX(Vector3 position)
        {
            if (position.X > x[0] && position.X < x[1])
            {
                return true;
            }
            return false;
        }
        public bool isInZ(Vector3 position)
        {
            if (position.Z > z[0] && position.Z < z[1])
            {
                return true;
            }
            return false;
        }
        public void SortObjects(List<GameObject> gameObjects)
        {
            foreach (GameObject gObject in gameObjects)
            {
                if (gObject.id.Contains(this.id))
                {
                    gameObjectInRoom.Add(gObject);
                }
            }
        }
        public List<GameObject> returnListOfVisibleGameObject(string currentRoomName)
        {
            if (halls.Count != 0)
            {
                foreach (Hall hall in halls)
                {
                    if (hall.name == currentRoomName)
                    {
                        return hall.gameObjectInHall;
                    }
                }
            }
            if (this.name == currentRoomName)
            {
                return this.gameObjectInRoom;
            }
            if (rightChild != null)
            {
                if (rightChild.returnListOfVisibleGameObject(currentRoomName) != null)
                {
                    return rightChild.returnListOfVisibleGameObject(currentRoomName);
                }
                else if (leftChild != null)
                {
                    return leftChild.returnListOfVisibleGameObject(currentRoomName);
                }
            }

            return null;
        }
    }
}
