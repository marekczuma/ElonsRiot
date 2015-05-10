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
        box
    }
    public static class Methods
    {
        static Player referencePlayer;
        static List<GameObject> gameObjects;
        static bool isColliding;
       public static void setPlayer(Player p,List<GameObject>objests)
       {
           referencePlayer = p;
           gameObjects = objests;
       }
      
        internal static void MoveBox(GameObject gameObject)
        {
            isColliding = false;
            Vector3 tmp = referencePlayer.newPosition - referencePlayer.oldPosition;
            tmp = Vector3.Transform(tmp, referencePlayer.MatrixWorld);
            tmp.Normalize();
            BoxBoxCollision bbcol = new BoxBoxCollision();
            if (bbcol.TestAABBAABB(referencePlayer, gameObject))
            {
                if (bbcol.TestAABBAABBTMP(referencePlayer, gameObject))
                {
                    if (gameObject.collisionCommunicat == "x")
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
                    else if (gameObject.collisionCommunicat == "z")
                    {
                        if (referencePlayer.newPosition.Z - referencePlayer.oldPosition.Z < 0)
                        {
                            gameObject.ChangeRelativePosition(new Vector3(0, 0, -referencePlayer.velocity));
                        }
                        else
                        {
                            gameObject.ChangeRelativePosition(new Vector3(0, 0, referencePlayer.velocity));
                        }
                    }
                }
               /* foreach(GameObject gameObj in gameObjects)
                {
                    if(gameObj.Name != gameObject.Name && gameObj.Name != "Elon")
                    {
                        if (bbcol.TestAABBAABB(gameObj, gameObject))
                        {
                            isColliding = true;
                        }
                    }
                }*/

            }
        //    Debug.WriteLine(gameObject.Position.ToString());
        }

        internal static void MoveDoor(GameObject gameObject)
        {
            gameObject.ChangePosition(new Vector3(0f, 0f, 0.2f));
        }
    }
}
