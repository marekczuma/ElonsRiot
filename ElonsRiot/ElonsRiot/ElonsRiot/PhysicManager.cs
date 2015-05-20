using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        static List<GameObject> Enemies;
        static GameObject floor;
        static GameObject Palo;
        static Player Elon;
        static bool isStart;
        public static bool isClimbing;
        static SpriteBatch spriteBatchHUD;
        static GraphicsDevice graphicDevice;
        public static void setElements(GraphicsDevice graphic)
        {
            boxesCollision = new BoxBoxCollision();
            isStart = true;
            isClimbing = false;
            InteractiveGameObject = new List<GameObject>();
            NotInteractiveGameObject = new List<GameObject>();
            Boxes = new List<GameObject>();
            Characters = new List<GameObject>();
            Enemies = new List<GameObject>();

            spriteBatchHUD = new SpriteBatch(graphic);
            graphicDevice = graphic;
        }

        public static void update(GameTime gameTime, List<GameObject> gameO, Player player)
        {

            if (isStart == true)
            {
                foreach (GameObject gObj in gameO)
                {
                    if (gObj.Interactive == true)
                    {

                        if (gObj.Name != "terrain" && !gObj.Name.Contains("character") && !gObj.Name.Contains("enemy") && !gObj.Name.Contains("box"))
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
                    if (gObj.Name.Contains("box"))
                    {
                        Boxes.Add(gObj);
                    }
                    if (gObj.Name.Contains("character"))
                    {
                        Characters.Add(gObj);
                    }
                    if (gObj.Name.Contains("enemy"))
                    {
                        Enemies.Add(gObj);
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
                  //tworzenie AAboxów dla boxów
                  foreach (GameObject box in Boxes)
                  {
                      box.Initialize();
                      box.RefreshMatrix();
                      box.GetCentre();
                      box.AAbox = new Box(box, player);
                      box.AAbox.GetCorners();
                      box.AAbox.createBoudingBox();
                      box.AAbox.createBoudingBoxes();

                  }
                  //tworzenie AAboxów dla wrogów
                  foreach (GameObject enemy in Enemies)
                  {
                      enemy.Initialize();
                      enemy.RefreshMatrix();
                      enemy.GetCentre();
                      enemy.AAbox = new Box(enemy, player);
                      enemy.AAbox.GetCorners();
                      enemy.AAbox.createBoudingBox();
                      enemy.AAbox.createBoudingBoxes();

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
                      gObj.AAbox.GetCorners();
                      gObj.AAbox.createBoudingBox();

                  }
                //inicjalizacja aktywnych
                  foreach (GameObject gObj in InteractiveGameObject)
                  {
                      gObj.Initialize();
                      gObj.GetCentre();
                      gObj.RefreshMatrix();
                      gObj.AAbox = new Box(gObj, player);
                      gObj.AAbox.GetCorners();
                      gObj.AAbox.createBoudingBox();

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
                character.AAbox.GetRefrecneObjectAndPlayer(character, player);
            }
            //atualizacja wrogów
            foreach (GameObject enemy in Enemies)
            {

                enemy.AAbox.CreateRadiuses();
                enemy.AAbox.GetRadius();
                enemy.AAbox.GetCorners();
                enemy.AAbox.createBoudingBox();
                enemy.AAbox.GetRefrecneObjectAndPlayer(enemy, player);
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
            //aktualuzacja aktywnych
            foreach (GameObject interactive in InteractiveGameObject)
            {
                interactive.AAbox.CreateRadiuses();
                interactive.AAbox.GetRadius();
                interactive.AAbox.GetCorners();
                interactive.AAbox.createBoudingBox();
                interactive.AAbox.GetRefrecneObjectAndPlayer(interactive, player);
            }
            //kolizja aktywnych z charakterami
            foreach(GameObject character in Characters)
            {
                foreach (GameObject interactive in InteractiveGameObject)
                    if (boxesCollision.TestAABBAABB(character, interactive))
                    {
                        character.AAbox.createBoudingBoxes();
                        if (boxesCollision.TestAABBAABBTMP(character, interactive))
                        {
                            character.Position = character.oldPosition;
                        }
                    }
            }
            //kolija między charakterami
           foreach (GameObject character in Characters)
            {
                foreach(GameObject character2 in Characters)
                 if (character.Name != character2.Name && boxesCollision.TestAABBAABB(character, character2))
                {
                    character.AAbox.createBoudingBoxes();
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
                        character.AAbox.createBoudingBoxes();
                        if(boxesCollision.TestAABBAABBTMP(character,gObj))
                        {
                            character.Position = character.oldPosition;
                        }
                    }
                  }
            }
            //kolizja elemetów interaktywnych z bahataterami 
            foreach (GameObject character in Characters)
            {
                foreach (GameObject gObj in InteractiveGameObject)
                {
                    if (boxesCollision.TestAABBAABB(character, gObj))
                    {
                        character.AAbox.createBoudingBoxes();
                        if (boxesCollision.TestAABBAABBTMP(character, gObj))
                        {
                            character.Position = character.oldPosition;
                        }
                    }
                }
            }
          
            ChceckBoxesCollision(Boxes);
         //kolizja bohaterów z boxami 
            foreach (GameObject character in Characters)
            {
                foreach (GameObject box in Boxes)
                {
                    if(boxesCollision.TestAABBAABB(character,box))
                    {
                        character.SetPositionY(character.oldPosition.Y);
                    }
                }
            } 
            //kolizka bohaterów z wrogami
            foreach (GameObject character in Characters)
            {
                foreach (GameObject enemy in Enemies)
                {
                    if (boxesCollision.TestAABBAABB(character, enemy))
                    {
                         character.AAbox.createBoudingBoxes();
                         if (boxesCollision.TestAABBAABBTMP(character, enemy))
                         {
                             character.Position = character.oldPosition;
                             enemy.Position = enemy.oldPosition;
                         }
                    }
                }
            } 
            //kolizja wrogów z elementami nieaktywnymi
            foreach (GameObject enemy in Enemies)
            {
                foreach (GameObject gObj in NotInteractiveGameObject)
                {
                    if (boxesCollision.TestAABBAABB(enemy, gObj))
                    {
                            enemy.Position = enemy.oldPosition;
                        
                    }
                }
            }
            foreach(GameObject character in Characters)
            {
                ActivateGravity(character, gameO, floor.plane);
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
             //      HUD.DrawString(spriteBatchHUD, "message new nanana", graphicDevice);
                    isFirst = true;
                }
                Interactions.CallInteraction(box);
                
            }
        }
      
        public static void ClimbBox(GameObject character)
        {
                foreach (GameObject box in Boxes)
                {
                    if (boxesCollision.TestAABBAABBWithError(character, box))
                    {
                        character.SetPosition(new Vector3(box.Position.X, box.AAbox.max.Y, box.Position.Z));
                    }
                }
        }
       
        public static void ActivateGravity(GameObject player, List<GameObject> GameObjs,Plane floor)
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
                player.ChangePosition(new Vector3(0, player.gravity, 0));
            }
        }
    }
}
