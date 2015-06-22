using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Interaction
{
    class HardChest : InteractiveGameObject
    {

        public HardChest()
        {
            mass = 90;
            Information = "Kliknij E zeby Palo przestawil skrzynke";
        }
        public override void Interaction(Scene _scene)
        {
            if (_scene.PaloObject.Skills.Using >= 30)
            {
                BoxMovementAI tmpAI = new BoxMovementAI(_scene.PlayerObject.Position);
                tmpAI.PointA = _scene.PaloObject.FindPlaceBehindObject(this, _scene.PlayerObject.Position);
                tmpAI.Cube = this;
                tmpAI.CubeMass = this.mass;
                _scene.PaloObject.MoveBoxAI = tmpAI;
                _scene.PaloObject.PaloState = FriendState.moveToBox;
               // _scene.PaloObject.Walk = WalkState.moveBox;
            }
        }
    }
}
