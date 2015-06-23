using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ElonsRiot.BSPTree
{
    static class CreateBSP
    {
        private static Leaf rootLeaf;
        private static string currentRoom;

        public static string CurrentRoom
        {
            get { return CreateBSP.currentRoom; }
        }
        public static void CreateLeafs(List<GameObject> gameObjects)
        {
            float[] x = new float[] { 63.3f, 117.5f };
            float[] z = new float[] { -146.5f, -17.6f };
            rootLeaf = new Leaf(x, z, "Hall", "A"); //pokoj w ktorym zaczynamy
            rootLeaf.SortObjects(gameObjects);

            x = new float[] { 3f, 47.5f };
            z = new float[] { -47f, 47.5f };
            rootLeaf.RightChild = new Leaf(x, z, "WareHouse", "B"); //magazyn
            rootLeaf.RightChild.SortObjects(gameObjects);

            x = new float[] { 52.8f, 100f };
            z = new float[] { 2.8f, 98.6f };
            rootLeaf.LeftChild = new Leaf(x, z, "ShootingGallery", "C");//strzelnica
            rootLeaf.LeftChild.SortObjects(gameObjects);

            float[] xH = new float[] { 47.4f, 63.6f };
            float[] zH = new float[] { -38f, -32f };
            Hall WareHouseHall = new Hall(xH, zH, "WareHouseHall", "E"); //korytarz laczacy magazyn z hall
            WareHouseHall.SortObjects(gameObjects);
            rootLeaf.AddHall(WareHouseHall);


            xH = new float[] { 81.9f, 89.6f };
            zH = new float[] { -19f, 2.5f };
            Hall ShootingHall = new Hall(xH, zH, "ShootingHall", "F"); //korytarz laczacy strzelnice z Hall
            ShootingHall.SortObjects(gameObjects);
            rootLeaf.AddHall(ShootingHall);

            rootLeaf.LeftChild.AddHall(ShootingHall);
            rootLeaf.RightChild.AddHall(WareHouseHall);

            x = new float[] { 3f, 47.5f };
            z = new float[] { -150.0f, -50f };
            rootLeaf.RightChild.LeftChild = new Leaf(x, z, "Laboratory", "D");  //tajemniczy pokoj
            rootLeaf.RightChild.LeftChild.SortObjects(gameObjects);
        }
        public static void checkPositionOfPlayer(Vector3 playerPosition)
        {
            currentRoom = rootLeaf.CheckIfPlayerIsInside(playerPosition);
        }

        public static List<GameObject> ListOfVisibleObj()
        {
            return rootLeaf.returnListOfVisibleGameObject(CurrentRoom);
        }

    }
}
