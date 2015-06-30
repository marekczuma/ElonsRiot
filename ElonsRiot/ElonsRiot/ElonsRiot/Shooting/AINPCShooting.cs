using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Shooting
{
    public class AINPCShooting
    {
        public Scene Scene { get; set; }
        public GameObject Target { get; set; }
        public GameObject Character { get; set; }
        public float Range { get; set; }

        public AINPCShooting(Scene _scene, GameObject _character)
        {
            Scene = _scene;
            Character = _character;
            Range = 20;
        }

        public void FindEnemies(List<String> _tags) //Lista tagów po których szukamy przeciwników
        {
            float currentDistance = Range+1;
            GameObject tmpTarget = null;
            if (Character is Guard)
            {
                Guard currentGuard = (Guard)Character;
                if ((currentGuard.State != GuardState.dead) && (currentGuard.State != GuardState.chase))
                {
                    foreach (var element in Scene.GameObjects)
                    {
                        if (_tags.Contains(element.Tag))
                        {
                            float tmpDistance = Vector3.Distance(Character.Position, element.Position);
                            if (tmpDistance <= Range)
                            {
                                if (tmpDistance <= currentDistance)
                                {
                                    tmpTarget = element;
                                    currentDistance = tmpDistance;
                                }
                            }
                        }
                    }
                }
            }else if (Character is PaloCharacter)
            {
                PaloCharacter currentGuard = (PaloCharacter)Character;
                if ((currentGuard.PaloState != FriendState.death) && (currentGuard.PaloState != FriendState.decoy) && (currentGuard.Skills.Shooting >= 20))
                {
                    foreach (var element in Scene.GameObjects)
                    {
                        if (_tags.Contains(element.Tag))
                        {
                            float tmpDistance = Vector3.Distance(Character.Position, element.Position);
                            if (tmpDistance <= Range)
                            {
                                if (tmpDistance <= currentDistance)
                                {
                                    tmpTarget = element;
                                    currentDistance = tmpDistance;
                                }
                            }
                        }
                    }
                }
            }
            Target = tmpTarget;
        }
    }
}
