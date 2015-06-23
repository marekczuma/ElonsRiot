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
            Door Door2 = new Door { Name = "Drzwi 2", ObjectPath = "3D/placeholders/drzwi", Interactive = true, Position = new Vector3(86, 0, -0.5f), Rotation = new Vector3(0, 180, 0), Scale = new Vector3(1.38f, 0.75f, 1) };
            Door2.id = "ABCDEF";
            InteractiveObjects.Add(Door2);
            HardChest BigChest = new HardChest { Name = "boxForMovement", ObjectPath = "3D/placeholders/skrzynka1", Interactive = true, Position = new Vector3(45, 0, 20), Rotation = new Vector3(0, 180, 0), Scale = new Vector3(1f, 2.5f, 1f) };
            BigChest.id = "ABCDEF";
            InteractiveObjects.Add(BigChest);
            BombDoor bombDoor = new BombDoor { Name = "Drzwi 2", ObjectPath = "3D/placeholders/drzwi", Interactive = true, Position = new Vector3(86, 0, -0.5f), Rotation = new Vector3(0, 180, 0), Scale = new Vector3(1.38f, 0.75f, 1) };
            bombDoor.id = "ABCDEF";
            
            Chest JustChest = new Chest { Name = "boxForMovement", ObjectPath = "3D/placeholders/skrzynka1", Interactive = true, Position = new Vector3(15, 0, -10), Rotation = new Vector3(0, 180, 0), Scale = new Vector3(1f, 1f, 1f) };
            JustChest.id = "ABCDEF";
            InteractiveObjects.Add(JustChest);
            Stuff stuff = new Stuff { Name = "stuff", ObjectPath = "3D/placeholders/Bomba", Interactive = true, Position = new Vector3(3,5.5f, 11), Rotation = new Vector3(0, 0, 0), Scale = new Vector3(1f, 2f, 1f) };
            stuff.id = "ABCDEF";
            InteractiveObjects.Add(stuff);
            Door Door3 = new Door { Name = "Drzwi 3", ObjectPath = "3D/placeholders/drzwi", Interactive = true, Position = new Vector3(20f, 0, -100), Rotation = new Vector3(0, 180, 0), Scale = new Vector3(1, 0.75f, 1) };
            Door3.id = "ABCDEF";
            InteractiveObjects.Add(Door3);
            EngineeringStuff eStuff = new EngineeringStuff { Name = "laptop", ObjectPath = "3D/placeholders/laptop", Interactive = true, Position = new Vector3(8, 3, -60), Rotation = new Vector3(0, 0, 90), Scale = new Vector3(0.3f, 0.4f, 0.5f) };
            eStuff.id = "ABCDEF";
            InteractiveObjects.Add(eStuff);
            EngineeringStuff eStuff2 = new EngineeringStuff { Name = "superComputer", ObjectPath = "3D/placeholders/komputer_dol", Interactive = true, Position = new Vector3(25, 0, -148), Rotation = new Vector3(0, 90, 0), Scale = new Vector3(1, 0.75f, 0.98f) };
            eStuff2.id = "ABCDEF";
            InteractiveObjects.Add(eStuff2);
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
