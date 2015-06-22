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
            Door Door1 = new Door { Name = "Drzwi 1", ObjectPath = "3D/placeholders/Wall5", Interactive = true, Position = new Vector3(50, 0, -35), Rotation = new Vector3(0, 90, 0), Scale = new Vector3(1, 0.75f, 1) };
            Door1.id = "ABCDEF";
            InteractiveObjects.Add(Door1);
            Door Door2 = new Door { Name = "Drzwi 2", ObjectPath = "3D/placeholders/Wall5", Interactive = true, Position = new Vector3(86, 0, 0), Rotation = new Vector3(0, 180, 0), Scale = new Vector3(1.2f, 0.75f, 1) };
            Door2.id = "ABCDEF";
            InteractiveObjects.Add(Door2);
            HardChest BigChest = new HardChest { Name = "boxForMovement", ObjectPath = "3D/Boxes/box", Interactive = true, Position = new Vector3(45, 0, 20), Rotation = new Vector3(0, 180, 0), Scale = new Vector3(0.1f, 0.15f, 0.05f) };
            BigChest.id = "ABCDEF";
            InteractiveObjects.Add(BigChest);
            BombDoor bombDoor = new BombDoor { Name = "Drzwi 2", ObjectPath = "3D/placeholders/Wall5", Interactive = true, Position = new Vector3(86, 0, 0), Rotation = new Vector3(0, 180, 0), Scale = new Vector3(1.2f, 0.75f, 1) };
            bombDoor.id = "ABCDEF";
            InteractiveObjects.Add(bombDoor);
            
            Chest JustChest = new Chest { Name = "boxForMovement", ObjectPath = "3D/Boxes/box", Interactive = true, Position = new Vector3(15, 0, -10), Rotation = new Vector3(0, 180, 0), Scale = new Vector3(0.05f, 0.05f, 0.05f) };
            JustChest.id = "ABCDEF";
            InteractiveObjects.Add(JustChest);
            Stuff stuff = new Stuff { Name = "boxForMovement", ObjectPath = "3D/placeholders/stuff", Interactive = true, Position = new Vector3(100, 2, -40), Rotation = new Vector3(0, 0, 0), Scale = new Vector3(0.05f, 0.05f, 0.05f) };
            stuff.id = "ABCDEF";
            InteractiveObjects.Add(stuff);
       //     Wall wallMV = new Wall { Name = "wall2", ObjectPath = "3D/placeholders/Wall2", Interactive = true, Position = new Vector3(25, 0, -50), Rotation = new Vector3(0,90, 0), Scale = new Vector3(1f, 0.75f,1f) };
       //     wallMV.id = "ABCDEF";
//InteractiveObjects.Add(wallMV);
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
