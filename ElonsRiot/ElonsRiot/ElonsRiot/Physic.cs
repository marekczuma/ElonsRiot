using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    class Physic
    {
        BoxBoxCollision boxesCollision;
        List<GameObject> gameObject;
        List<GameObject> PositiveObj = new List<GameObject>();
        List<GameObject> NegativeObj = new List<GameObject>();
        List<GameObject> RotationFar = new List<GameObject>();
        List<GameObject> RotationNear = new List<GameObject>();
        List<GameObject> Items = new List<GameObject>();
        public Physic()
        {
           boxesCollision = new BoxBoxCollision();
            
        }
        public void update(GameTime gameTime,List<GameObject> gameO,Player player)
        {
            this.gameObject = gameO;
            player.Initialize();
            player.createBoudingBox();
            player.RefreshMatrix();
            player.AAbox = new Box(player);
            player.AAbox.CheckWhichCorners();

            List<GameObject> tmp = new List<GameObject>();

            foreach (GameObject obj in gameObject)
            {

                if (obj.Name != player.Name)
                {

                    obj.Initialize();
                    obj.RefreshMatrix();
                    obj.GetCentre();
                    if (obj.ObjectPath == "3D/Ziemia/bigFloor")
                    {
                        obj.createPlane();
                        obj.RefreshMatrix();
                        tmp.Add(obj);

                    }
                    else
                    {
                        obj.createBoudingBox();
                        obj.RefreshMatrix();
                        obj.AAbox = new Box(obj, player);
                        obj.AAbox.CheckWhichCornersForObjects();
                    }
                    if ((obj.AAbox.max.X >= player.AAbox.min.X) && obj.ObjectPath != "3D/Ziemia/bigFloor" && obj.Rotation.Y == 0)
                    {
                        PositiveObj.Add(obj);
                    }
                    if ((obj.AAbox.min.X < player.AAbox.max.X) && obj.ObjectPath != "3D/Ziemia/bigFloor" && obj.Rotation.Y == 0)
                    {
                        NegativeObj.Add(obj);
                    }
                    if (obj.Rotation.Y != 0 && (obj.AAbox.max.Z > player.AAbox.max.Z))
                    {
                        RotationNear.Add(obj);
                    }
                    if (obj.Rotation.Y != 0 && (obj.AAbox.min.Z < player.AAbox.min.Z))
                    {
                        RotationFar.Add(obj);
                    }
                }
                if (obj.ObjectPath == "3D/Box/box")
                {
                    tmp.Add(obj);
                }
                /*if (tmp.Count == 2)
                {
                    Vector3 pos = tmp[1].Position;
                    Vector3 rot = tmp[1].Rotation;
                    tmp[1].ChangePosition(new Vector3(0, -0.005f, 0));
                    tmp[1].ChangeRotation(new Vector3(0.5f, 0, 0));
                    bool tmpe = boxesCollision.TestAABBPlane(tmp[1], tmp[0].plane);
                    if (tmpe == true)
                    {
                        Debug.WriteLine("plane + aabb");
                        tmp[1].Rotation = rot;
                        tmp[1].SetPosition(pos);
                    }
                }*/
            }
               if (boxesCollision.CheckCollision(player, PositiveObj,0))
                {
                    Debug.WriteLine("dziala positive");
                    player.Position = player.oldPosition;
                }
            if (boxesCollision.CheckCollision(player, NegativeObj,1))
            {
                Debug.WriteLine("dziala negative");
                player.Position = player.oldPosition;
            }
            if (boxesCollision.CheckCollision(player, RotationNear, 2))
            {
                Debug.WriteLine("dziala near ");
                player.Position = player.oldPosition;
            }
            if (boxesCollision.CheckCollision(player, RotationFar, 3))
            {
                Debug.WriteLine("dziala far");
                player.Position = player.oldPosition;
            }
        }
    }
}
