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
        List<GameObject> InteractiveGameObject;
        List<GameObject> NotInteractiveGameObject;
        GameObject floor;
        GameObject Palo;

        int flag, flag2;
        bool isFirstTimeInitialization;
        public Physic()
        {
            boxesCollision = new BoxBoxCollision();
            flag = 1;
            flag2 = 1;
            isFirstTimeInitialization = false;
            InteractiveGameObject = new List<GameObject>();
            NotInteractiveGameObject = new List<GameObject>();
        }
        public void update(GameTime gameTime, List<GameObject> gameO, Player player)
        {
           foreach ( GameObject gObj in gameO)
           {
               if(gObj.Interactive == true)
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
               if(gObj.Name == "terrain")
               {
                   floor = gObj;
               }
               
               if (gObj.Name == "Palo")
               {
                   Palo = gObj;
               }
           }
            //aktualizowanie AAboxów dla Palo i Elona
            player.Initialize();
            player.RefreshMatrix();
            player.GetCentre();
            player.AAbox = new Box(player);
            player.AAbox.createBoudingBox();
            player.AAbox.CheckWhichCorners();
            Palo.Initialize();
            Palo.RefreshMatrix();
            Palo.GetCentre();
            Palo.AAbox = new Box(Palo, player);
            
            //inicjalizacja dla obiektów nieruchomych 
            if (isFirstTimeInitialization == false && NotInteractiveGameObject.Count != 0)
            {
                foreach (GameObject gObj in NotInteractiveGameObject)
                {
                    gObj.Initialize();
                    gObj.GetCentre();
                    gObj.RefreshMatrix();
                    gObj.AAbox = new Box(gObj, player);
                    if(gObj.Name == "stairs")
                    {
                        gObj.AAbox.createAAPlane();
                    }
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
            if (InteractiveGameObject.Count !=0)
            {
                foreach(GameObject gObj in InteractiveGameObject)
                {
                    gObj.Initialize();
                    gObj.GetCentre();
                    gObj.RefreshMatrix();
                    gObj.AAbox = new Box(gObj, player);
                    gObj.AAbox.CheckWhichCornersForObjects();
                    gObj.AAbox.GetRadius();
                }
            }
            //aktualuzacja punktów kolidujących w obiektach interaktywnych
            foreach(GameObject gObj in NotInteractiveGameObject)
            {
                gObj.AAbox.CheckWhichCornersForObjects();
                gObj.AAbox.GetRadius();
            }
            //kolizja elemetów nie interaktywnych,które nie są schodami z Elonem i Palo
            foreach(GameObject gObj in NotInteractiveGameObject)
            {
                if (gObj.Name != "stairs")
                {
                    if (boxesCollision.TestAABBAABB(player, gObj))
                    {
                        player.Position = player.oldPosition;
                    }

                }
                else {

                        if (boxesCollision.TestAABBAABB(player, gObj))
                        {
                            flag2 = 0;
                        }
                        
                         if(boxesCollision.TestAABBPlane(player,gObj.AAbox.plane) && flag2 == 0)
                         {
                             
                             float step = (Math.Abs(gObj.AAbox.max.Y - gObj.AAbox.min.Y)) /200;
                             if (player.Position.Y < gObj.AAbox.max.Y)
                             {
                                 player.ChangePosition(new Vector3(0, step, 0));
                                 player.camera.position.Y += step;
                             }
                             

                        }
                    }

            }

            if(boxesCollision.TestAABBPlane(player,floor.plane))
            {
                
                player.Position = new Vector3(player.Position.X, player.oldPosition.Y, player.Position.Z);
            }


      
        }



    }
}