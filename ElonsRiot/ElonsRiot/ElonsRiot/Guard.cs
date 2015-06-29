using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinnedModel;

namespace ElonsRiot
{
    public enum GuardState { idle, chase, attack, dead, shoot };
    public class Guard : GameObject
    {
        public GameObject Target {get; set;}
        public GuardState State { get; set; }
        public Scene Scene { get; set; }
        public float Velocity { get; set; }
        public Shooting.NPCShooting Shooting { get; set; }
        public AnimationClip clip;
        public AnimationPlayer animationPlayer;
        public SkinningData skinningData;
        GuardState previousGuardState = GuardState.idle;
        public float playSpeed;
        public TimeSpan elapsedTime;
        float guardTime;
        public bool isDead;//, showShootExplosion;
        private List<string> enemiesTags = new List<string>();

        public Guard(Scene _scene)
        {
            Scene = _scene;
            State = GuardState.idle;
            Velocity = 0.18f;
            guardTime = 1.42f;
            isDead = false;
            Shooting = new Shooting.NPCShooting(Scene, this);
            FillEnemies();
            Shooting.RangeOfMistake = 15;
        }
        public void Chase()
        {
            oldPosition = Position;
            State = GuardState.chase;
            WalkToTarget(Target, Velocity, 0.5f);
        }

        public void Die()
        {
            State = GuardState.dead;
        }

        public void FillEnemies()
        {
            enemiesTags.Add("Player");
            enemiesTags.Add("Palo");
            Shooting.enemiesTags = enemiesTags;
        }

        public void LoadAnimation()
        {
            skinningData = GameObjectModel.Tag as SkinningData;
            animationPlayer = new AnimationPlayer(skinningData);
            clip = skinningData.AnimationClips["Take001"];
            animationPlayer.StartClip(clip);
        }

        public void AnimationUpdate(GameTime gameTime)
        {
            elapsedTime = TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalSeconds * playSpeed);


            if (State == GuardState.idle)
            {
                if (previousGuardState != GuardState.dead)
                {
                    playSpeed = 0.7f;

                    clip = skinningData.AnimationClips["Take001"];
                    if (previousGuardState != GuardState.idle)
                        animationPlayer.StartClip(clip);
                    previousGuardState = GuardState.idle;
                }
            }
            else if (State == GuardState.chase)
            {
                if (previousGuardState != GuardState.dead)
                {
                    playSpeed = 0.7f;
                    clip = skinningData.AnimationClips["Take002"];
                    if (previousGuardState != GuardState.chase)
                        animationPlayer.StartClip(clip);
                    previousGuardState = GuardState.chase;
                }
            }
            else if (State == GuardState.dead)
            {
                playSpeed = 2.0f;

                clip = skinningData.AnimationClips["Take003"];
                if (previousGuardState != GuardState.dead)
                    animationPlayer.StartClip(clip);
                previousGuardState = GuardState.dead;

                if (guardTime > 0)
                {
                    guardTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (guardTime <= 0)
                    {
                        Scene.GameObjects.Remove(this);
                        isDead = true;
                    }
                }
            }
            else if (State == GuardState.shoot)
            {
                if (previousGuardState != GuardState.dead)
                {
                    playSpeed = 1f;
                    clip = skinningData.AnimationClips["Take004"];
                    if (previousGuardState != GuardState.shoot)
                        animationPlayer.StartClip(clip);
                    previousGuardState = GuardState.shoot;
                }
            }
        }
    }
}
