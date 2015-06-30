using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Learning
{
    public class LearningManager
    {
        public PaloCharacter Palo { get; set; }
        public Scene Scene { get; set; }
        public ShootingLearning ShootingLearning { get; set; }
        public float Timer { get; set; }
        public float TimerStatement { get; set; }
        SpriteBatch spriteBatchStatement;
        public LearningManager(Scene _scene)
        {
            Scene = _scene;
            Timer = 0;
            TimerStatement = 0;
            ShootingLearning = new ShootingLearning(Scene);
            //spriteBatchStatement = new SpriteBatch(Scene.GraphicsDevice);
        }

        public void DrawStatement(string _statement)
        {
            float timeInMS = Scene.time.ElapsedGameTime.Milliseconds;
            if(TimerStatement >0)
            {
                
            }
        }

        public void AddObjectToScene()
        {
            ShootingLearning.AddObjectsToScene();
        }

        public void LearningUpdate()
        {
            if ((Palo.PaloLearningState == LearningState.Learning) && (Vector3.Distance(Scene.PlayerObject.Position, ShootingLearning.PosA) <= 5))
            {
                if (!ShootingLearning.IsStarted)
                {
                    ShootingLearning.MoveElonAndPalo();
                    ShootingLearning.IsStarted = true;
                    Palo.PaloState = FriendState.shoot;
                }
                else
                {
                    if(ShootingLearning.ElonShoot)
                    {
                        if (ShootingLearning.ElonAfterFirst)
                        {
                            Scene.PlayerObject.ammo = 1;
                            ShootingLearning.ElonAfterFirst = false;
                        }
                        if(Scene.PlayerObject.ammo <1)
                        {
                            ShootingLearning.ElonShoot = false;
                            Timer = 3000;
                        }
                    }else
                    {
                        float timeInMS = Scene.time.ElapsedGameTime.Milliseconds;
                        Timer -= timeInMS;
                        if (Timer <= 0)
                        {
                            Tin targetTin = new Tin();
                            foreach(var elem in ShootingLearning.Tins)
                            {
                                if(elem.isAlive && (!elem.IsPlayer))
                                {
                                    targetTin = elem;
                                    break;
                                }
                            }
                            if (targetTin != null)
                            {
                                Palo.PaloShooting.Shoot(targetTin);
                                ShootingLearning.ElonShoot = true;
                                ShootingLearning.ElonAfterFirst = true;
                                ShootingLearning.IsStarted = false;
                            }
                        }                                                                                                                          
                    }
                }
            }
        }


    }
}
