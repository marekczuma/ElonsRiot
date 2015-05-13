using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    public  class Interactions
    {
        InterationTypes interactionType;
        GameObject referenceObject;
        public Interactions(InterationTypes interType, GameObject gameObj)
        {
            this.interactionType = interType;
            this.referenceObject = gameObj;
        }


        public delegate void InteractionsDelegate(GameObject gameObject);
        public  event InteractionsDelegate InteractionEvent;
        private int p;
        private GameObject gameObject;
        public void Add()
        {
            if (interactionType == InterationTypes.door)
            {
                InteractionEvent += Methods.MoveDoor;
            }
            else if (interactionType == InterationTypes.box)
            {
                InteractionEvent += Methods.MoveBox;
            }
            else if (interactionType == InterationTypes.stairs)
            {
                InteractionEvent += Methods.ClimbStairs;
            }
        }
        public void CallInteraction()
        {
            if (InteractionEvent != null)
                InteractionEvent(referenceObject);
        }
    }
    public enum InterationTypes
    {
        door,
        box,
        stairs
    }
    public static class Methods
    {
        static Player referencePlayer;
        static List<GameObject> gameObjects;
        static List<GameObject> referenceBoxes;
        static bool isColliding;
       public static void setPlayer(Player p,List<GameObject>objests)
       {
           referencePlayer = p;
           gameObjects = objests;
           referenceBoxes = new List<GameObject>();
           foreach (GameObject obj in objests)
           {
               if(obj.Name.Contains("box"))
               {
                   referenceBoxes.Add(obj);
               }
           }
       }
      
        internal static void MoveBox(GameObject gameObject)
        {
            isColliding = false;
            BoxBoxCollision bbcol = new BoxBoxCollision();
            if (bbcol.TestAABBAABB(referencePlayer, gameObject))
            {
                if (bbcol.TestAABBAABBTMP(referencePlayer, gameObject))
                {
                    Vector3 tmp = referencePlayer.newPosition - referencePlayer.oldPosition;
                    Debug.WriteLine(tmp.ToString());
                    foreach(GameObject gameBox in referenceBoxes)
                    {
                        if (bbcol.TestAABBAABB(gameBox, gameObject) == true && gameBox.Name != gameObject.Name)
                        {
                            isColliding = true;
                            referencePlayer.Position = referencePlayer.oldPosition;
                        }
                        
                    }
                    if (isColliding == false)
                    {
                        gameObject.ChangeRelativePosition(tmp);
                    }
                    
                  /*  if (gameObject.collisionCommunicat.Contains("x"))
                    {
                        if (referencePlayer.newPosition.X - referencePlayer.oldPosition.X < 0)
                        {
                            gameObject.ChangeRelativePosition(new Vector3(-referencePlayer.velocity, 0, 0));
                        }
                        else
                        {
                            gameObject.ChangeRelativePosition(new Vector3(referencePlayer.velocity, 0, 0));
                        }
                    }
                    else if (gameObject.collisionCommunicat.Contains("z"))
                    {
                        if (referencePlayer.newPosition.Z - referencePlayer.oldPosition.Z < 0)
                        {
                            gameObject.ChangeRelativePosition(new Vector3(0, 0, -referencePlayer.velocity));
                        }
                        else
                        {
                            gameObject.ChangeRelativePosition(new Vector3(0, 0, referencePlayer.velocity));
                        }
                    }*/
                }
            }
        }
        internal static void ClimbStairs(GameObject stairs)
        {
            BoxBoxCollision bbcol = new BoxBoxCollision();
            if (bbcol.TestAABBAABB(referencePlayer, stairs))
            {

                if (bbcol.TestAABBPlane(referencePlayer, stairs.AAbox.plane))
                {
                    float moving = Math.Abs((stairs.AAbox.plane.Normal.X * referencePlayer.Position.X) + (stairs.AAbox.plane.Normal.Y * referencePlayer.Position.Y) +
                    (stairs.AAbox.plane.Normal.Z * referencePlayer.Position.Z)) / (float)Math.Sqrt(Math.Pow(stairs.AAbox.plane.Normal.X, 2) +
                    Math.Pow(stairs.AAbox.plane.Normal.Y, 2) + Math.Pow(stairs.AAbox.plane.Normal.Z, 2));
                    referencePlayer.ChangePosition(new Vector3(0, 3, 0));
                    referencePlayer.camera.Update(referencePlayer.MatrixWorld, referencePlayer.elonState, referencePlayer.Rotation);
                }
            }


          
        }
        internal static void MoveDoor(GameObject gameObject)
        {
            gameObject.ChangePosition(new Vector3(0f, 0f, 0.2f));
        }
    }
}
