using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Interaction
{
    public class BombDoor : InteractiveGameObject
    {
        public BombDoor()
        {
            Information = "Kliknij E zeby podlozyc bombe";
            
        }
        public override void Interaction(Scene _scene)
        {
            _scene.PlayerObject.elonState.SetCurrentState(State.interact);
            if (_scene.PlayerObject.isBomb == false)
            {
                //GameObject tmp = new GameObject { Name = "paczka", ObjectPath = "3D/Placeholders/Bomba", Position = new Vector3(86, 5, -1f), Scale = new Vector3(1, 3, 1), id = "ABCDEF", Rotation = new Vector3(90, 0, 0) };
                //tmp.LoadModels(_scene.ContentManager);

                //_scene.GameObjects.Add(tmp);
                foreach (var gObj in _scene.GameObjects)
                {
                    if (gObj.Name == "paczuszka")
                        gObj.Position = new Vector3(86, 5, -1);
                }
                _scene.PlayerObject.isBomb = true;
            }
        }
    }
}
