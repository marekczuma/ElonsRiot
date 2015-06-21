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
        static List<GameObject> Floors;
        public static List<GameObject> Stuffs;
        public static bool isStart;
        public static bool isClimbing;
        static SpriteBatch spriteBatchHUD;
        static GraphicsDevice graphicDevice;
        static GameObject ramp;
        static bool isGoDown;
        public static void setElements(GraphicsDevice graphic)
        {
            boxesCollision = new BoxBoxCollision();
            isStart = true;
            isGoDown = false;
            isClimbing = false;
            InteractiveGameObject = new List<GameObject>();
            NotInteractiveGameObject = new List<GameObject>();
            Boxes = new List<GameObject>();
            Characters = new List<GameObject>();
            Floors = new List<GameObject>();
            Stuffs = new List<GameObject>();
            ramp = new GameObject();
            spriteBatchHUD = new SpriteBatch(graphic);
            graphicDevice = graphic;
        }
        public static void InitializePhysicManager(List<GameObject> gameO, Player player)
        {
            foreach (GameObject gObj in gameO)
            {
                if (gObj.Interactive == true)
                {

                    if (!gObj.Name.Contains("character") && !gObj.Name.Contains("enemy") && !gObj.Name.Contains("box"))
                    {
                        InteractiveGameObject.Add(gObj);
                    }
                }
                else
                {
                    if (gObj.Name.Contains("wall") || gObj.Name.Contains("filar"))
                    {
                        if (!gObj.Name.Contains("character") && !gObj.Name.Contains("stuff")) 
                        {
                            NotInteractiveGameObject.Add(gObj);
                        }
                    }
                }
                if (gObj.Name.Contains("terrain"))
                {
                    Floors.Add(gObj);
                }
                if (gObj.Name.Contains("box"))
                {
                    Boxes.Add(gObj);
                }
                if (gObj.Name.Contains("character") || gObj.Name.Contains("enemy"))
                {
                    Characters.Add(gObj);
                }
                if (gObj.Name.Contains("ramp"))
                {
                    ramp = gObj;
                }
                if (gObj.Name.Contains("stuff"))
                {
                    Stuffs.Add(gObj);
                }
            }

            //tworzenie AAboxów dla bohaterów
            foreach (GameObject character in Characters)
            {
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
            //tworzenie AAboxów dla stuffs
            foreach (GameObject stuff in Stuffs)
            {
                stuff.Initialize();
                stuff.RefreshMatrix();
                stuff.AAbox = new Box(stuff, player);
                stuff.AAbox.GetCorners();
                stuff.AAbox.createBoudingBox();
                stuff.AAbox.createBoudingBoxes();
            }
            //inicjalizjacja Plane dla podłogi
            foreach (GameObject floor in Floors)
            {
                floor.Initialize();
                floor.RefreshMatrix();
                floor.AAbox = new Box(floor, player);
                floor.AAbox.createBoudingBox();
                new MyPlane(floor);
                floor.RefreshMatrix();
            }
            //inicjalizacja rampy
            if (ramp.GameObjectModel != null)
            {
                ramp.Initialize();
                ramp.RefreshMatrix();
                ramp.AAbox = new Box(ramp, player);
                ramp.AAbox.createBoudingBox();
                ramp.AAbox.createPlanes();
                ramp.AAbox.GetCorners();
                ramp.AAbox.rampa();
            }
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
            foreach (GameObject gameObj in NotInteractiveGameObject)
            {
                foreach (GameObject neighbor in NotInteractiveGameObject)
                {
                    if (gameObj.Name.Contains("Hall") == false && gameObj.Position != neighbor.Position)
                    {
                        if (boxesCollision.TestAABBAABB(gameObj, neighbor) && neighbor.Name.Contains("Hall") == false)
                        {
                            gameObj.neighbors.Add(neighbor);
                        }
                    }

                }
            }
            //wyszukiwanie sasiadow obiektow interaktywnych 
            foreach (GameObject gameObj in InteractiveGameObject)
            {
                foreach (GameObject neighbor in NotInteractiveGameObject)
                {
                    if (gameObj.Name == "door1" && neighbor.Name.Contains("4Hall"))
                    {
                        gameObj.neighbors.Add(neighbor);
                        neighbor.neighbors.Add(gameObj);
                    }
                    if (gameObj.Name == "door2" && neighbor.Name.Contains("2Hall"))
                    {
                        gameObj.neighbors.Add(neighbor);
                        neighbor.neighbors.Add(gameObj);
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
                gObj.AAbox.createPlanes();
                gObj.AAbox.createPointsOfCollision();
                gObj.AAbox.setpointOfChangeWall();
            }
        }
       
        public static void update(GameTime gameTime, List<GameObject> gameO, Player player)
        {  
            //atualizacja bohaterów
            foreach (GameObject character in Characters)
            {
                character.AAbox.CreateRadiuses();
                character.AAbox.GetCenter();
                character.AAbox.UpdateBoundingBox();
                character.AAbox.GetCorners();
                character.AAbox.GetRefrecneObjectAndPlayer(character, player);
                character.AAbox.createPointsOfCollision();
                character.AAbox.UpdateBoundingBoxes();

                character.AAbox.createPlanes();
                character.AAbox.GetCorners();
                character.AAbox.createPointsOfCollision();
                character.AAbox.setpointOfChangeWall();
            }
            //aktualuzacja boxów
            foreach (GameObject box in Boxes)
            {
                box.AAbox.CreateRadiuses();
                box.AAbox.GetCenter();
                box.AAbox.GetCorners();
                box.AAbox.UpdateBoundingBox();
                box.AAbox.GetRefrecneObjectAndPlayer(box, player);
                box.AAbox.UpdateBoundingBoxes();
            }
            //aktualuzacja stuffs
            foreach (GameObject stuff in Stuffs)
            {
                stuff.AAbox.CreateRadiuses();
                stuff.AAbox.GetCenter();
                stuff.AAbox.GetCorners();
                stuff.AAbox.UpdateBoundingBox();
                stuff.AAbox.GetRefrecneObjectAndPlayer(stuff, player);
                stuff.AAbox.UpdateBoundingBoxes();
            }
            //aktualuzacja aktywnych
            foreach (GameObject interactive in InteractiveGameObject)
            {
                interactive.AAbox.CreateRadiuses();
                interactive.AAbox.GetCenter();
                interactive.AAbox.GetCorners();
                interactive.AAbox.UpdateBoundingBox();
                interactive.AAbox.GetRefrecneObjectAndPlayer(interactive, player);
                interactive.AAbox.createPlanes();
                interactive.AAbox.createPointsOfCollision();
                interactive.AAbox.setpointOfChangeWall();
            }
            //kolizja aktywnych z charakterami
            foreach (GameObject character in Characters)
            {
                foreach (GameObject interactive in InteractiveGameObject)
                {
                    if (boxesCollision.TestAABBAABB(character, interactive))
                    {
                        character.AAbox.createBoudingBoxes();
                        foreach (BoundingBox box in character.boxes)
                        {
                            if (boxesCollision.TestAABBAABBTMP(character, interactive))
                            {

                                Plane plane = getLongerWallInInteractiveObject(character, interactive);
                                if (boxesCollision.TestAABBPlane(character, plane))
                                {
                                    Vector3 direction = character.newPosition - character.oldPosition;
                                    Vector3 invNormal = plane.Normal;

                                    if (plane == interactive.AAbox.planes[0] || plane == interactive.AAbox.planes[2]) //filar i wall1,wall3,wall4 mają tutaj same == i dla 0 i 2
                                    {                                                                    // dla wall2 to 1 i 3 i też ==
                                        invNormal.X *= -1;
                                        invNormal.Y *= -1;
                                        invNormal.Z *= -1;
                                    }
                                    invNormal = invNormal * (direction * plane.Normal).Length();
                                    Vector3 wallDir = direction - invNormal;
                                    //trzeba sprawidzic sasiadów, bo inaczej przechodzi przez rogi ścian ze sobą sasiadujacych
                                    foreach (GameObject neighbor in interactive.neighbors)
                                    {

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
                                    if (interactive.neighbors.Count == 0)
                                    {
                                        character.Position = character.oldPosition + wallDir;
                                    }

                                }
                            }
                        }
                    }
                }
            }
            //kolija między charakterami
            foreach (GameObject character in Characters)
            {
                foreach (GameObject character2 in Characters)
                    if (character.Name != character2.Name && boxesCollision.TestAABBAABB(character, character2))
                    {
                        if (boxesCollision.TestAABBAABBTMP(character, character2))
                        {
                            foreach (BoundingBox box in character.boxes)
                            {
                                if (boxesCollision.TestAABBAABBTMP(character, character2))
                                {

                                    Plane plane = checkPointOfChange(character, character2);
                                    if (boxesCollision.TestAABBPlane(character, plane))
                                    {
                                        Vector3 direction = character.newPosition - character.oldPosition;
                                        Vector3 invNormal = plane.Normal;

                                        if (plane == character2.AAbox.planes[0] || plane == character2.AAbox.planes[2]) //filar i wall1,wall3,wall4 mają tutaj same == i dla 0 i 2
                                        {                                                                    // dla wall2 to 1 i 3 i też ==
                                            invNormal.X *= -1;
                                            invNormal.Y *= -1;
                                            invNormal.Z *= -1;
                                        }
                                        invNormal = invNormal * (direction * plane.Normal).Length();
                                        Vector3 wallDir = direction - invNormal;

                                        character.Position = character.oldPosition + wallDir;
                                    }
                                }
                            }
                        }
                    }
            }
            //kolizja elemetów nieinteraktywnych z bahataterami 
            foreach (GameObject character in Characters)
            {

                foreach (GameObject gObj in NotInteractiveGameObject)
                {

                    if (boxesCollision.TestAABBAABB(character, gObj))
                    {

                        Plane plane = checkPointOfChange(character, gObj);
                        if (boxesCollision.TestAABBPlane(character, plane))
                        {
                            Vector3 direction = character.newPosition - character.oldPosition;
                            Vector3 invNormal = plane.Normal;

                            if (plane == gObj.AAbox.planes[0] || plane == gObj.AAbox.planes[2])
                            {
                                invNormal.X *= -1;
                                invNormal.Y *= -1;
                                invNormal.Z *= -1;
                            }

                            invNormal = invNormal * (direction * plane.Normal).Length();
                            Vector3 wallDir = direction - invNormal;
                            //trzeba sprawidzic sasiadów, bo inaczej przechodzi przez rogi ścian ze sobą sasiadujacych
                            foreach (GameObject neighbor in gObj.neighbors)
                            {

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
            if (ramp.GameObjectModel != null)
            {
                //kolizja rampy z bahataterami 
                foreach (GameObject character in Characters)
                {
                    if ((int)character.Position.Y >= ramp.boundingBox.Max.Y - 0.3f)
                    {
                        isGoDown = true;
                    }
                    if ((int)character.Position.Y <= ramp.boundingBox.Min.Y - 0.3f)
                    {
                        isGoDown = false;
                    }
                    if (boxesCollision.TestAABBAABB(character, ramp))
                    {

                        Plane plane = ramp.AAbox.planes[4];
                        if (boxesCollision.TestAABBPlane(character, plane))
                        {
                            Vector3 direction = character.newPosition - character.oldPosition;
                            Vector3 invNormal = plane.Normal;
                            if (isGoDown == true) { direction = -direction; }
                            invNormal = invNormal * (direction * plane.Normal).Length();
                            Vector3 wallDir = direction - invNormal;
                            if (isGoDown == false)
                            {
                                character.Position = character.oldPosition + wallDir;
                            }

                            if (isGoDown == true)
                            {
                                character.Position = character.oldPosition - wallDir;

                            }
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
                    if (boxesCollision.TestAABBAABB(character, box))
                    {
                        character.SetPositionY(character.oldPosition.Y);
                    }
                }
            }
            //grawitacja
            foreach (GameObject character in Characters)
            {
                ActivateGravity(character, Floors);
            }
        /*    foreach (GameObject stuffs in Stuffs)
            {
                ActivateGravity(stuffs, Floors);
            }
            //
            foreach(GameObject stuff in Stuffs)
            {
                if(boxesCollision.TestAABBAABB(player,stuff))
                {
                    player.equipment.AddElement(stuff, gameO);
                }
                if(player.equipment.PressButton == true)
                 {
                     player.equipment.DeleteElement(stuff, player, gameO);
                 }
            }*/
            
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

        public static void ActivateGravity(GameObject player, List<GameObject> Floors)
        {

            BoxBoxCollision boxesCollision = new BoxBoxCollision();
            int counter = 0;
            if (ramp.GameObjectModel != null)
            {
                if (boxesCollision.TestAABBAABB(player, ramp))
                {
                    if (boxesCollision.TestAABBPlane(player, ramp.AAbox.planes[4]))
                    {
                        counter++;
                    }
                }
            }
            foreach (GameObject floor in Floors)
            {
                if (boxesCollision.TestAABBAABB(player, floor))
                {
                 //   if (boxesCollision.TestAABBPlane(player, floor.plane))
                 //   {
                        counter++;
                //    }

                }
            }


            if (counter == 0)
            {
                player.ChangePosition(new Vector3(0, player.gravity, 0));
            }
        }
        public static Plane getLongerWallInInteractiveObject(GameObject character, GameObject gObj)
        {
            List<Plane> planes = new List<Plane>();
            List<Vector3> centerOfPlane = new List<Vector3>();
            if (gObj.message == "x")
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
            return findClosestPlane(planes, character, centerOfPlane);
        }
        public static Plane checkPointOfChange(GameObject character, GameObject gObj)
        {
            List<Plane> planes = new List<Plane>();
            List<Vector3> centerOfPlane = new List<Vector3>();
            if (gObj.message == "x")
            {
                //wyszedl poza zasieg
                if (character.Position.X < gObj.AAbox.pointOfChangeWall[0].X || character.Position.X > gObj.AAbox.pointOfChangeWall[1].X)
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
            return findClosestPlane(planes, character, centerOfPlane);
        }

        public static Plane findClosestPlane(List<Plane> planes, GameObject player, List<Vector3> centersOfPlanes)
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
