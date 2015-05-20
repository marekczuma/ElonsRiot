using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    public static class Interactions
    {
        public delegate void InteractionsDelegate(GameObject gameObject);
        static event InteractionsDelegate InteractionEvent;
        public static void Add(InterationTypes interactionType)
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
        public static void CallInteraction(GameObject referenceObject)
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
                if (bbcol.TestAABBAABB(character, gameObject) && gameObject.AAbox.max.Y > character.Position.Y)
                {
                    isColliding = true;
                    massDifference = character.mass - gameObject.mass;
                       tmp = character.Position - character.oldPosition;
                        foreach (GameObject gameBox in referenceBoxes)
                        {
                            if (bbcol.TestAABBAABB(gameBox, gameObject) && gameBox.Name != gameObject.Name)
                            {
                                isColliding = false;
                                character.AAbox.createBoudingBoxes();
                                if (bbcol.TestAABBAABBTMP(character, gameObject))
                                {
                                    character.Position = character.oldPosition;
                                }
                                gameObject.Position = gameObject.oldPosition;
                            }

                        } 
                       if(massDifference < 0)
                       {
                           character.AAbox.createBoudingBoxes();
                           if (bbcol.TestAABBAABBTMP(character, gameObject))
                           {
                            character.Position = character.oldPosition;
                           }
                       }
                }
            }
            if (isColliding == true && massDifference >= 0)
            {
                gameObject.ChangeRelativePosition(new Vector3(tmp.X, 0, tmp.Z));
                isColliding = false;
            }
           
            
        }
       internal static void ClimbBox(GameObject gameObject)
        {
            BoxBoxCollision bbcol = new BoxBoxCollision();
            foreach (GameObject character in referenceCharacters)
            {
                if (bbcol.TestAABBAABB(character, gameObject))
                {
                    character.SetPosition(new Vector3(gameObject.Position.X, gameObject.AAbox.max.Y +0.5f, gameObject.Position.Z));
                    CollisionBox(gameObject);
                   
                }
            }
        }
        internal static void CollisionBox(GameObject gameObj)
       {
           BoxBoxCollision bbcol = new BoxBoxCollision();
           foreach (GameObject character in referenceCharacters)
           {
               if (bbcol.TestAABBAABB(character, gameObj))
               {
                   character.Position = character.oldPosition;
                   Interactions.Add(InterationTypes.box);
                //   Interactions.Delete();
               }
           }
       }
        internal static void MoveDoor(GameObject gameObject)
        {
            gameObject.ChangePosition(new Vector3(0.01f, 0f, 0.0f));
        }
    }
}
