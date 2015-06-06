using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ElonsRiot.BSPTree
{
     static class  CreateBSP
    {
        private static Leaf rootLeaf;
        private static string currentRoom;
        public static void  CreateLeafs()
        { 
            float[] x = new float[]{59.5f,120.5f};
            float[] z = new float[]{-170.0f,-15.5f};
            rootLeaf = new Leaf(x, z, "Hall"); //pokoj w ktorym zaczynamy

            x = new float[] { -0.5f, 50.0f };
            z = new float[] { -49.9f, 49.9f };
            rootLeaf.RightChild =new Leaf(x, z, "WareHouse"); //magazyn

            x = new float[] { 50.0f, 102.0f };
            z = new float[] { 0.5f, 99.9f };
            rootLeaf.LeftChild = new Leaf(x, z, "ShootingGallery");//strzelnica

            float[] xH = new float[] { 50.0f, 60.0f };
            float[] zH = new float[] { -41.5f, -28.5f };
            Hall WareHouseHall = new Hall(xH, zH, "WareHouseHall"); //korytarz laczacy magazyn z hall
            rootLeaf.AddHall(WareHouseHall);

            xH = new float[] { 80.5f, 92.5f };
            zH = new float[] { -15.0f, 0.0f };
            Hall ShootingHall = new Hall(xH, zH, "ShootingHall"); //korytarz laczacy strzelnice z Hall
            rootLeaf.AddHall(ShootingHall);

            rootLeaf.LeftChild.AddHall(ShootingHall);
            rootLeaf.RightChild.AddHall(WareHouseHall);

            x = new float[] { -0.5f, 50.0f };
            z = new float[] { -150.0f, -50.0f };
            rootLeaf.RightChild.LeftChild = new Leaf(x,z,"Laboratory");  //tajemniczy pokoj
        }
         public static void checkPositionOfPlayer(Vector3 playerPosition)
        {
            currentRoom = rootLeaf.CheckIfPlayerIsInside(playerPosition);
         //   Debug.WriteLine(currentRoom);
        }
    }
}
