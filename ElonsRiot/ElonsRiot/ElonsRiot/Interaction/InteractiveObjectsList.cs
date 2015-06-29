using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Interaction
{
    public class InteractiveObjectsList
    {
        public List<InteractiveGameObject> InteractiveObjects { get; set; }
        public Scene Scene {get; set;}
        public InteractiveObjectsList(Scene _scene)
        {
            InteractiveObjects = new List<InteractiveGameObject>();
            Scene = _scene;
            FillList();
        }

        public void FillList()
        {
            Door Door1 = new Door { Name = "Drzwi 1", ObjectPath = "3D/placeholders/drzwi", Interactive = true, Position = new Vector3(50.5f, 0, -35), Rotation = new Vector3(0, 90, 0), Scale = new Vector3(1, 0.75f, 1) };
            Door1.id = "ABCDEF";
            InteractiveObjects.Add(Door1);
            //Door Door2 = new Door { Name = "Drzwi 2", ObjectPath = "3D/placeholders/drzwi", Interactive = true, Position = new Vector3(86, 0, -0.5f), Rotation = new Vector3(0, 180, 0), Scale = new Vector3(1.38f, 0.75f, 1) };
            //Door2.id = "ABCDEF";
            //InteractiveObjects.Add(Door2);
            HardChest BigChest = new HardChest { Name = "boxForMovement", ObjectPath = "3D/placeholders/skrzynka1", Interactive = true, Position = new Vector3(40, 0, 20), Rotation = new Vector3(0, 180, 0), Scale = new Vector3(1f, 2.5f, 1f) };
            BigChest.id = "ABCDEF";
            InteractiveObjects.Add(BigChest);
            BombDoor bombDoor = new BombDoor { Name = "Drzwi 2", ObjectPath = "3D/placeholders/drzwi", Interactive = true, Position = new Vector3(86, 0, -0.5f), Rotation = new Vector3(0, 180, 0), Scale = new Vector3(1.38f, 0.75f, 1) };
            bombDoor.id = "ABCDEF";
            InteractiveObjects.Add(bombDoor);
            Chest JustChest = new Chest { Name = "boxForMovement", ObjectPath = "3D/placeholders/skrzynka1", Interactive = true, Position = new Vector3(15, 0, -10), Rotation = new Vector3(0, 180, 0), Scale = new Vector3(1f, 1f, 1f) };
            JustChest.id = "ABCDEF";
            InteractiveObjects.Add(JustChest);
            Stuff gun1 = new Stuff { Name = "gun1", ObjectPath = "3D/placeholders/gun", Interactive = true, Position = new Vector3(3, 5.5f, 12), Rotation = new Vector3(0, 90, 0), Scale = new Vector3(0.1f, 0.1f, 0.1f) };
            gun1.id = "ABCDEF";
            InteractiveObjects.Add(gun1);
            Stuff gun2 = new Stuff { Name = "gun2", ObjectPath = "3D/placeholders/gun", Interactive = true, Position = new Vector3(3, 5.5f, 10), Rotation = new Vector3(0, 90, 0), Scale = new Vector3(0.1f, 0.1f, 0.1f) };
            gun2.id = "ABCDEF";
            InteractiveObjects.Add(gun2);
            Door Door3 = new Door { Name = "Drzwi 3", ObjectPath = "3D/placeholders/drzwi", Interactive = true, Position = new Vector3(20f, 0, -100), Rotation = new Vector3(0, 180, 0), Scale = new Vector3(1, 0.75f, 1) };
            Door3.id = "ABCDEF";
            InteractiveObjects.Add(Door3);
            EngineeringStuff eStuff = new EngineeringStuff { Name = "laptop", ObjectPath = "3D/placeholders/laptop", Interactive = true, Position = new Vector3(8, 3, -56), Rotation = new Vector3(0, 0, 0), Scale = new Vector3(0.3f, 0.4f, 0.4f) };
            eStuff.id = "ABCDEF";
            InteractiveObjects.Add(eStuff);
            EndStuff eStuff2 = new EndStuff { Name = "superComputer", ObjectPath = "3D/placeholders/komputer_dol", Interactive = true, Position = new Vector3(25, 0, -148), Rotation = new Vector3(0, 90, 0), Scale = new Vector3(1, 0.75f, 0.98f) };
            eStuff2.id = "ABCDEF";
            InteractiveObjects.Add(eStuff2);
            Sensore Sensore1 = new Sensore { Name = "assetSensor", ObjectPath = "3D/placeholders/czujniki", Interactive = true, Position = new Vector3(3, 0, -90), Rotation = new Vector3(0, 270, 0), Scale = new Vector3(0.7f, 0.75f, 0.7f) };
            Sensore1.id = "ABCDEF";
            InteractiveObjects.Add(Sensore1);
            OpeningDoor openingDoor = new OpeningDoor { Name = "openingDoor", ObjectPath = "3D/placeholders/drzwi_suwane_2", Interactive = true, Position = new Vector3(51, 0, -85), Rotation = new Vector3(0, 0, 0), Scale = new Vector3(1, 0.75f, 1f) };
            openingDoor.id = "ABCDEF";
            InteractiveObjects.Add(openingDoor);
            Stuff boom = new Stuff { Name = "stuffBoom", ObjectPath = "3D/placeholders/Bomba", Interactive = true, Position = new Vector3(17, 3.6f, -55), Rotation = new Vector3(0, 90, 0), Scale = new Vector3(1f, 1f, 1f) };
            boom.id = "ABCDEF";
            InteractiveObjects.Add(boom);
        }

        public void AddToScene()
        {
            foreach(var elem in InteractiveObjects)
            {
                Scene.GameObjects.Add(elem);
            }
        }
    }
}
