using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinnedModel;

namespace ElonsRiot
{
    public enum GuardState { idle, chase, attack, dead };
    public class Guard : GameObject
    {
        public GameObject Target {get; set;}
        public GuardState State { get; set; }
        public Scene Scene { get; set; }
        public float Velocity { get; set; }
        public AnimationClip clip;
        public AnimationPlayer animationPlayer;
        public SkinningData skinningData;
        GuardState previousGuardState = GuardState.idle;
        public float playSpeed;
        public TimeSpan elapsedTime;
        float guardTime;
        public bool isDead;

        public Guard()
        {
            State = GuardState.idle;
            Velocity = 0.09f;
            guardTime = 1.42f;
            isDead = false;
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
        }
    }
}
