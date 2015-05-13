﻿using Microsoft.Xna.Framework;
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
        static List<GameObject> Stairs;
        static GameObject floor;
        static GameObject Palo;
        static Player Elon;
        static bool isFirstTimeInitialization, isStart;
        static int flag2 = 1;

        public static void setElements()
        {
            boxesCollision = new BoxBoxCollision();
            isFirstTimeInitialization = false;
            isStart = true;
            InteractiveGameObject = new List<GameObject>();
            NotInteractiveGameObject = new List<GameObject>();
            Boxes = new List<GameObject>();
            Stairs = new List<GameObject>();
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

                    if (gObj.Name == "Palo")
                    {
                        Palo = gObj;
                    }
                    if (gObj.Name == "Elon")
                    {
                        Elon = (Player)gObj;
                    }
                    if (gObj.Name.Contains("box"))
                    {
                        Boxes.Add(gObj);
                    }
                    if (gObj.Name.Contains("stairs"))
                    {
                        Stairs.Add(gObj);
                    }
                }
                isStart = false;
            }
            //aktywowanie gravitacji
            //    ActivateGravity(player, floor.plane);
            //    ActivateGravity(Palo, floor.plane);

            //aktualizowanie AAboxów dla Palo i Elona
            player.Initialize();
            player.RefreshMatrix();
            player.GetCentre();
            player.AAbox = new Box(player);
            player.AAbox.createBoudingBox();
            player.AAbox.CheckWhichCorners();
            player.AAbox.createBoudingBoxes();
            Palo.Initialize();
            Palo.RefreshMatrix();
            Palo.GetCentre();
            Palo.AAbox = new Box(Palo, player);
            Palo.AAbox.createBoudingBox();
            Palo.AAbox.CheckWhichCorners();

            //inicjalizacja dla obiektów nieruchomych 
            if (isFirstTimeInitialization == false && NotInteractiveGameObject.Count != 0)
            {
                foreach (GameObject gObj in NotInteractiveGameObject)
                {
                    gObj.Initialize();
                    gObj.GetCentre();
                    gObj.RefreshMatrix();
                    gObj.AAbox = new Box(gObj, player);
                  
                }
                //inicjalizjacja Plane dla podłogi
                floor.Initialize();
                floor.GetCentre();
                floor.RefreshMatrix();
                new MyPlane(floor);
                floor.RefreshMatrix();
                isFirstTimeInitialization = true;
            }
            //inicjalizja / aktualizacja obiektów ruchomych
            if (InteractiveGameObject.Count != 0)
            {
                foreach (GameObject gObj in InteractiveGameObject)
                {
                    gObj.Initialize();
                    gObj.GetCentre();
                    gObj.RefreshMatrix();
                    gObj.AAbox = new Box(gObj, player);
                    gObj.AAbox.CheckWhichCornersForObjects();
                    gObj.AAbox.GetRadius();
                    if (gObj.Name == "stairs")
                    {
                        gObj.AAbox.createAAPlane();
                    }
                }
            }
            //aktualuzacja punktów kolidujących w obiektach interaktywnych
            foreach (GameObject gObj in NotInteractiveGameObject)
            {
                gObj.AAbox.CheckWhichCornersForObjects();
                gObj.AAbox.GetRadius();
            }
            //kolija Palo i Elona
            if (boxesCollision.TestAABBAABB(player, Palo))
            {
                player.Position = player.oldPosition;
            }
            //kolizja obiektów interaktywnych
           /* foreach (GameObject gObj in InteractiveGameObject)
            {

                if (boxesCollision.TestAABBAABB(player, gObj))
                {
                    player.Position = player.oldPosition;
                }


            }*/
            //kolizja elemetów nie interaktywnych,które nie są schodami z Elonem i Palo
            foreach (GameObject gObj in NotInteractiveGameObject)
            {
                if (gObj.Name != "stairs")
                {
                    if (boxesCollision.TestAABBAABB(player, gObj))
                    {
                        player.Position = player.oldPosition;
                    }


                }
             /*   else
                {

                    if (boxesCollision.TestAABBAABB(player, gObj))
                    {
                        flag2 = 0;
                    }

                    if (boxesCollision.TestAABBPlane(player, gObj.AAbox.plane) && flag2 == 0)
                    {
                        player.gravity = 0;
                        float step = (Math.Abs(gObj.AAbox.max.Y - gObj.AAbox.min.Y)) / 200;
                        do
                        {
                            player.ChangePosition(new Vector3(0, step, 0));
                            player.camera.position.Y += step;
                        } while (player.Position.Y < gObj.AAbox.max.Y);


                    }
                }*/

            }

            if (boxesCollision.TestAABBPlane(player, floor.plane))
            {

                player.Position = new Vector3(player.Position.X, player.oldPosition.Y, player.Position.Z);
            }

            ChceckBoxesCollision(Boxes);
            ChceckStairsCollision(Stairs);
        }


        public static  void ActivateGravity(GameObject gameObj, Plane floor)
        {

            if (boxesCollision.TestAABBPlane(gameObj, floor))
            {
                gameObj.ChangePosition(new Vector3(0, gameObj.gravity, 0));
            }

        }




        public static void ChceckBoxesCollision(List<GameObject> Boxes)
        {
            
            foreach (GameObject box in Boxes)
            {
                 Interactions interactionsClass = new Interactions(box.interactionType, box);
                        //  MyScene.GameObjects[i].ChangePosition(new Vector3(0f, 0f, 0.2f));
                        interactionsClass.Add();
                        interactionsClass.CallInteraction();
               
            }
        }
        public static void ChceckStairsCollision(List<GameObject> Stairs)
        {
            foreach (GameObject stairs in Stairs)
            {
                Interactions interactionsClass = new Interactions(stairs.interactionType, stairs);
                //  MyScene.GameObjects[i].ChangePosition(new Vector3(0f, 0f, 0.2f));
                interactionsClass.Add();
                interactionsClass.CallInteraction();
            }
        }
    }
}
