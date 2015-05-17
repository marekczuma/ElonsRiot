using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    //monitoruje zdarzenia zachodzące w świecie 
    public static class PhysicManager
    {
        static BoxBoxCollision boxesCollision;
        static List<GameObject> InteractiveGameObject;
        static List<GameObject> NotInteractiveGameObject;
        static List<GameObject> Boxes;
        static List<GameObject> Characters;
        static GameObject floor;
        static GameObject Palo;
        static Player Elon;
        static bool isStart;
        public static bool isClimbing;
        public static void setElements()
        {
            boxesCollision = new BoxBoxCollision();
            isStart = true;
            isClimbing = false;
            InteractiveGameObject = new List<GameObject>();
            NotInteractiveGameObject = new List<GameObject>();
            Boxes = new List<GameObject>();
            Characters = new List<GameObject>();

        }

        public static void update(GameTime gameTime, List<GameObject> gameO, Player player)
        {

            if (isStart == true)
            {
                foreach (GameObject gObj in gameO)
                {
                    if (gObj.Interactive == true)
                    {

                        if (gObj.Name != "terrain" && gObj.Name != "Elon" && gObj.Name != "Palo")
                        {
                            InteractiveGameObject.Add(gObj);
                        }
                    }
                    else
                    {
                        if (gObj.Name != "terrain" && gObj.Name != "Elon" && gObj.Name != "Palo")
                        {
                            NotInteractiveGameObject.Add(gObj);
                        }
                    }
                    if (gObj.Name == "terrain")
                    {
                        floor = gObj;
                    }

                    if (gObj.Name.Contains("Palo"))
                    {
                        Palo = gObj;
                    }
                    if (gObj.Name.Contains("Elon"))
                    {
                        Elon = (Player)gObj;
                    }
                    if (gObj.Name.Contains("box"))
                    {
                        Boxes.Add(gObj);
                    }
                    if (gObj.Name.Contains("character"))
                    {
                        Characters.Add(gObj);
                    }
                }
                //tworzenie AAboxów dla bohaterów
                  foreach(GameObject character in Characters){
                      character.Initialize();
                      character.RefreshMatrix();
                      character.GetCentre();
                      character.AAbox = new Box(character, player);
                      character.AAbox.GetCorners();
                      character.AAbox.createBoudingBox();
                      character.AAbox.createBoudingBoxes();
                      
                     }
                  //inicjalizjacja Plane dla podłogi
                  floor.Initialize();
                  floor.GetCentre();
                  floor.RefreshMatrix();
                  floor.AAbox = new Box(player);
                  floor.AAbox.createBoudingBox();
                  new MyPlane(floor);
                  floor.RefreshMatrix();
                //inicjalizjacja nieaktywnych
                  foreach (GameObject gObj in NotInteractiveGameObject)
                  {
                      gObj.Initialize();
                      gObj.GetCentre();
                      gObj.RefreshMatrix();
                      gObj.AAbox = new Box(gObj, player);

                  }
                //inicjalizacja aktywnych
                  foreach (GameObject gObj in InteractiveGameObject)
                  {
                      gObj.Initialize();
                      gObj.GetCentre();
                      gObj.RefreshMatrix();
                      gObj.AAbox = new Box(gObj, player);

                  }

                isStart = false;
            }
            //atualizacja bohaterów
            foreach (GameObject character in Characters)
            {
                
                character.AAbox.CreateRadiuses();
                character.AAbox.GetRadius();
                character.AAbox.GetCorners();
                character.AAbox.createBoudingBox();
                if (character.Name == "characterElon" || character.Name == "characterPalo")
                {
                    character.AAbox.createBoudingBoxes();
                }
                character.AAbox.GetRefrecneObjectAndPlayer(character, player);
            }
            //aktualuzacja boxów
            foreach (GameObject box in Boxes)
            {
                box.AAbox.CreateRadiuses();
                box.AAbox.GetRadius();
                box.AAbox.GetCorners();
                box.AAbox.createBoudingBox();
                box.AAbox.GetRefrecneObjectAndPlayer(box, player);
            }
            //kolija między charakterami
           foreach (GameObject character in Characters)
            {
                foreach(GameObject character2 in Characters)
                 if (character.Name != character2.Name && boxesCollision.TestAABBAABB(character, character2))
                {
                     if(boxesCollision.TestAABBAABBTMP(character,character2))
                     {
                         character.Position = character.oldPosition;
                     }
                }
            }
           //kolizja elemetów nieinteraktywnych z bahataterami 
            foreach(GameObject character in Characters)
            {
                foreach(GameObject gObj in NotInteractiveGameObject)
                { 
                    if (boxesCollision.TestAABBAABB(character, gObj))
                    {
                        if(boxesCollision.TestAABBAABBTMP(character,gObj))
                        {
                            character.Position = character.oldPosition;
                        }
                    }
                  }
            }
           
            ChceckBoxesCollision(Boxes);
            foreach(GameObject character in Characters)
            {
                ActivateGravity(player, gameO, floor.plane);
            }
           
        }

        static bool isFirst = false;
        public static void ChceckBoxesCollision(List<GameObject> Boxes)
        {
           
            foreach (GameObject box in Boxes)
            {
                if (isFirst == false)
                {
                    Interactions.Add(box.interactionType);
                    isFirst = true;
                }
                Interactions.CallInteraction(box);
            }
        }
      
        public static void ClimbBox()
        {
            foreach (GameObject box in Boxes)
            {
                if (boxesCollision.TestAABBAABB(Elon, box))
                {
                    float x = box.Position.X -  Elon.Position.X;
                    float z = box.Position.Z - Elon.Position.Z;
                    Vector3 climbPosition = new Vector3(0, box.AAbox.max.Y-2, 0);
                   // Elon.ChangePosition(climbPosition);
                    Elon.SetPosition(new Vector3(box.Position.X, box.AAbox.max.Y, box.Position.Z));
                    
                }
            }
        }
        public static void ShowMessage(List<GameObject> ObjectWithMessage)
        {
            foreach(GameObject gameObj in ObjectWithMessage)
            {
            //    HUD.DrawString()
            }
        }
        public static void ActivateGravity(Player player, List<GameObject> GameObjs,Plane floor)
        {
           
            BoxBoxCollision boxesCollision = new BoxBoxCollision();
            int counter = 0;
            foreach(GameObject gobj in GameObjs)
            {
                if (boxesCollision.TestAABBAABB(player,gobj) && gobj.Name !="terrain")
                {
                    counter++;
                }
                else if(boxesCollision.TestAABBPlane(player,floor))
                {
                    counter++;
                }
               
            }
            if(counter == 0)
            {
                player.ChangePosition(new Vector3(0, -0.2f, 0));
            }
        }
    }
}
