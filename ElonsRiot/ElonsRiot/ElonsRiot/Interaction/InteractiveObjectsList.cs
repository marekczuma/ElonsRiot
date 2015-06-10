﻿using Microsoft.Xna.Framework;
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
            InteractiveObjects.Add(Door1);
            Door Door2 = new Door { Name = "Drzwi 2", ObjectPath = "3D/placeholders/Wall5", Interactive = true, Position = new Vector3(86, 0, 0), Rotation = new Vector3(0, 180, 0), Scale = new Vector3(1.2f, 0.75f, 1) };
            InteractiveObjects.Add(Door2);
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