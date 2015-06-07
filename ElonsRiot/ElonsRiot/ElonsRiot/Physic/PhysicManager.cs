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
                        if (gObj.Name.Contains("wall") || gObj.Name.Contains("filar"))
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
                      enemy.AAbox = new Box(enemy, player);
                      enemy.AAbox.GetCorners();
                      enemy.AAbox.createBoudingBox();
                      enemy.AAbox.createBoudingBoxes();

                  }
                  //inicjalizjacja Plane dla podłogi
                  floor.Initialize();
                  floor.RefreshMatrix();
                  floor.AAbox = new Box(player);
                  floor.AAbox.createBoudingBox();
                  new MyPlane(floor);
                  floor.RefreshMatrix();
                //inicjalizjacja nieaktywnych
                  foreach (GameObject gObj in NotInteractiveGameObject)
                  {
                      gObj.Initialize();
                      gObj.RefreshMatrix();
                      gObj.AAbox = new Box(gObj, player);
                      gObj.AAbox.createBoudingBox();
                      gObj.AAbox.createPlanes();
                      gObj.AAbox.GetCorners();
                      gObj.AAbox.createPointsOfCollision();
                      gObj.AAbox.setpointOfChangeWall();

                  }
                //wyszukanie wszystkich sąsiadów 
                foreach(GameObject gameObj in NotInteractiveGameObject)
                {
                    foreach(GameObject neighbor in NotInteractiveGameObject)
                    {
                        if (gameObj.Name.Contains("Hall") == false &&  gameObj.Position != neighbor.Position) { 
                            if(boxesCollision.TestAABBAABB(gameObj,neighbor) && neighbor.Name.Contains("Hall") == false)
                            {
                               gameObj.neighbors.Add(neighbor);
                            }
                         }
                        
                    }
                }
                //inicjalizacja aktywnych
                  foreach (GameObject gObj in InteractiveGameObject)
                  {
                      gObj.Initialize();
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
                character.AAbox.GetCenter();
                character.AAbox.UpdateBoundingBox();
                character.AAbox.GetCorners();
                character.AAbox.GetRefrecneObjectAndPlayer(character, player);
                character.AAbox.createPointsOfCollision();
            }
            //atualizacja wrogów
            foreach (GameObject enemy in Enemies)
            {

                enemy.AAbox.CreateRadiuses();
                enemy.AAbox.GetCenter();
                enemy.AAbox.GetCorners();
                enemy.AAbox.UpdateBoundingBox();
                enemy.AAbox.GetRefrecneObjectAndPlayer(enemy, player);
            }
            //aktualuzacja boxów
            foreach (GameObject box in Boxes)
            {
                box.AAbox.CreateRadiuses();
                box.AAbox.GetCenter();
                box.AAbox.GetCorners();
                box.AAbox.UpdateBoundingBox();
                box.AAbox.GetRefrecneObjectAndPlayer(box, player);
            }
            //aktualuzacja aktywnych
            foreach (GameObject interactive in InteractiveGameObject)
            {
                interactive.AAbox.CreateRadiuses();
                interactive.AAbox.GetCenter();
                interactive.AAbox.GetCorners();
                interactive.AAbox.UpdateBoundingBox();
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
                        Plane plane = checkPointOfChange(character, gObj);
                          if (boxesCollision.TestAABBPlane(character, plane))
                            {
                                    Vector3 direction = character.newPosition - character.oldPosition;
                                    Vector3 invNormal = plane.Normal;

                                   if (plane == gObj.AAbox.planes[0] || plane == gObj.AAbox.planes[2]) //filar i wall1,wall3,wall4 mają tutaj same == i dla 0 i 2
                                   {                                                                    // dla wall2 to 1 i 3 i też ==
                                        invNormal.X *= -1;
                                        invNormal.Y *= -1;
                                        invNormal.Z *= -1;
                                    }

                                    invNormal = invNormal * (direction * plane.Normal).Length();
                                    Vector3 wallDir = direction - invNormal;
                                  //trzeba sprawidzic sasiadów, bo inaczej przechodzi przez rogi ścian ze sobą sasiadujacych
                                   foreach(GameObject neighbor in gObj.neighbors){
                                    
                                        if (boxesCollision.TestAABBAABB(character, neighbor))
                                        {
                                            character.Position = character.oldPosition;
                                            break;
                                        }
                                        else
                                        {
                                            character.Position = character.oldPosition + wallDir;
                                        }
                                  }
                                  if (gObj.neighbors.Count == 0)
                                  {
                                      character.Position = character.oldPosition + wallDir;
                                  }
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
        public static Plane checkPointOfChange(GameObject character, GameObject gObj)
        {
            List<Plane> planes = new List<Plane>();
            List<Vector3> centerOfPlane = new List<Vector3>();
            if(gObj.message == "x")
            {
               if(character.Position.X < gObj.AAbox.pointOfChangeWall[0].X || character.Position.X > gObj.AAbox.pointOfChangeWall[1].X)
                {
                    planes.Add(gObj.AAbox.planes[0]);
                    planes.Add(gObj.AAbox.planes[1]);
                    centerOfPlane.Add(gObj.AAbox.centersOfWalls[0]);
                    centerOfPlane.Add(gObj.AAbox.centersOfWalls[1]);
                }
                else
               {
                   planes.Add(gObj.AAbox.planes[2]);
                   planes.Add(gObj.AAbox.planes[3]);
                   centerOfPlane.Add(gObj.AAbox.centersOfWalls[2]);
                   centerOfPlane.Add(gObj.AAbox.centersOfWalls[3]);
               }
            }
            else
            {
                if (character.Position.Z < gObj.AAbox.pointOfChangeWall[0].Z || character.Position.Z > gObj.AAbox.pointOfChangeWall[1].Z)
                {
                    planes.Add(gObj.AAbox.planes[2]);
                    planes.Add(gObj.AAbox.planes[3]);
                    centerOfPlane.Add(gObj.AAbox.centersOfWalls[2]);
                    centerOfPlane.Add(gObj.AAbox.centersOfWalls[3]);
                }
                else
                {
                    planes.Add(gObj.AAbox.planes[0]);
                    planes.Add(gObj.AAbox.planes[1]);
                    centerOfPlane.Add(gObj.AAbox.centersOfWalls[0]);
                    centerOfPlane.Add(gObj.AAbox.centersOfWalls[1]);
                }
            }
           return findClosestPlane(planes,character,centerOfPlane);
        }

        public static Plane findClosestPlane(List<Plane> planes,GameObject player,List<Vector3>centersOfPlanes)
        {
            float smallestDistance = float.MaxValue;
            int whichWall = 0;
            for (int i = 0; i < 2; i++)
            {
                float distance = (float)Math.Sqrt(Math.Pow(player.AAbox.center2.X - centersOfPlanes[i].X, 2) + Math.Pow(player.AAbox.center2.Y - centersOfPlanes[i].Y, 2) + Math.Pow(player.AAbox.center2.Z - centersOfPlanes[i].Z, 2));
                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    whichWall = i;
                }
            }
            return planes[whichWall];
        }
    }
}
