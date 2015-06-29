using ElonsRiot.Music;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Shooting
{
    public class NPCShooting
    {
        public GameObject Character { get; set; }
        public Scene Scene { get; set; }
        public GameObject Target { get; set; }
        public float RangeOfMistake { get; set; }
        public AINPCShooting AI { get; set; }
        public List<string> enemiesTags { get; set; }
        public float timer;
        public NPCShooting (Scene _scene, GameObject _character)
        {
            Scene = _scene;
            Character = _character;
            AI = new AINPCShooting(Scene, Character);
            timer = 1000; //sekunda
        }



        public void Shoot(GameObject _target)
        {
            Character.LookAt(_target.Position);
            Random rand = new Random();
            float tmpRange = rand.Next((int)RangeOfMistake);
            Character.RotateQuaternions(MathHelper.ToRadians(tmpRange));
            Scene.ShootingManager.Shot(Character, Character.RotationQ);
            MusicManager.PlaySound(1); //Szczela i nawet słychać!
        }


        public void NPCManage()
        {
            float timeInMS = Scene.time.ElapsedGameTime.Milliseconds;
            timer -= timeInMS;
            if (timer <= 0)
            {
                AI.FindEnemies(enemiesTags);
                timer = 1000;
                if(AI.Target != null)
                {
                    Shoot(AI.Target);
                    if(Character is Guard)
                    {
                        Guard tmpGuard = (Guard)Character;
                        tmpGuard.State = GuardState.shoot;
                    }else if (Character is PaloCharacter)
                    {
                        PaloCharacter tmpGuard = (PaloCharacter)Character;
                        tmpGuard.PaloState = FriendState.shoot;
                    }

                }
            }

        }


        //Eee rly?
        public void TinShot(Learning.Tin _tin)
        {
            _tin.Destroy();
            MusicManager.PlaySound(1);
        }


    }
}
