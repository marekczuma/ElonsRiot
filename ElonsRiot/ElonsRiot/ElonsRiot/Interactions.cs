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
        box,
        stairs
    }
    public static class Methods
    {
        static Player referencePlayer;
        static List<GameObject> gameObjects;
        static List<GameObject> referenceBoxes;
        static List<GameObject> referenceCharacters;
        static GameObject referenceFloor;
        static bool isColliding;
        static float massDifference;
       public static void setPlayer(Player p,List<GameObject>objests)
       {
           referencePlayer = p;
           gameObjects = objests;
           referenceBoxes = new List<GameObject>();
           referenceCharacters = new List<GameObject>();
           foreach (GameObject obj in objests)
           {
               if(obj.Name.Contains("box"))
               {
                   referenceBoxes.Add(obj);
               }
               if (obj.Name.Contains("terrain"))
               {
                   referenceFloor = obj;
               }
               if(obj.Name.Contains("Elon") || obj.Name.Contains("Palo"))
               {
                   referenceCharacters.Add(obj);
               }
           }
       }
      
        internal static void MoveBox(GameObject gameObject)
        {
            massDifference = 0;
            Vector3 tmp = new Vector3(0, 0, 0);
            isColliding = false;
            BoxBoxCollision bbcol = new BoxBoxCollision();
            foreach (GameObject character in referenceCharacters)
            {
                isColliding = false;
                if (bbcol.TestAABBAABB(character, gameObject))
                {
                    isColliding = true;
                    massDifference = character.mass - gameObject.mass;
                       tmp = character.Position - character.oldPosition;
                      /*  foreach (GameObject gameBox in referenceBoxes)
                        {
                            if (bbcol.TestAABBAABB(gameBox, gameObject) == true && gameBox.Name != gameObject.Name)
                            {
                                isColliding = true;
                                character.Position = character.oldPosition;
                            }

                        }*/
                       if(massDifference < 0)
                       {
                           if (bbcol.TestAABBAABBTMP(character, gameObject))
                           {
                            character.Position = character.oldPosition;
                           }
                       }
                }
            }
            if (isColliding == true && massDifference >= 0)
            {
                gameObject.ChangeRelativePosition(tmp);
                isColliding = false;
            }
            
            
        }
       
        internal static void MoveDoor(GameObject gameObject)
        {
            gameObject.ChangePosition(new Vector3(0f, 0f, 0.2f));
        }
    }
}
