using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    public class Equipment
    {
        private List<GameObject> stuffs;
        private bool pressButton;

        public bool PressButton
        {
            get { return pressButton; }
            set { pressButton = value; }
        }
        public Equipment()
        {
            stuffs = new List<GameObject>();
            pressButton = false;
        }
        public void AddElement(GameObject newObject,List<GameObject> allObj)
        {
            stuffs.Add(newObject);
            allObj.Remove(newObject);
        }
        public void DeleteElementByName(string name)
        {
            for (int i = 0; i < stuffs.Count;i++ )
            {
                if (stuffs[i].Name == name)
                {
                    stuffs.RemoveAt(i);
                }
            }
        }
        public void DeleteElement(GameObject obj, Player player, List<GameObject> allObj)
        {
            stuffs.Remove(obj);
            //TODO: inny warunek
            if (player.newPosition.Z - player.oldPosition.Z > 0)
            {
                obj.SetPosition(new Vector3(player.Position.X, player.Position.Y, player.Position.Z + 5));
            }
            else
            {
                obj.SetPosition(new Vector3(player.Position.X, player.Position.Y, player.Position.Z - 5));
            }
            allObj.Add(obj);
            pressButton = false;
        }
       

    }
}
