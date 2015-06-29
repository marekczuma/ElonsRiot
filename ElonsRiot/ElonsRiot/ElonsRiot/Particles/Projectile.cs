using System;
using Microsoft.Xna.Framework;

namespace ElonsRiot.Particles
{
    class Projectile
    {
        const float numLaserParticles = 1;
        const int numExplosionParticles = 30;
        const float projectileLifespan = 0f;
        const float sidewaysVelocityRange = 0;
        const float verticalVelocityRange = 0;
        const float gravity = 0;

        ParticleSystem explosionParticles;
        ParticleSystem laserParticles;

        Vector3 position;
        Vector3 laserPosition;
        Vector3 velocity;
        Vector3 laserVelocity;
        float age;

        static Random random = new Random();

        public Projectile(ParticleSystem explosionParticles, ParticleSystem laserParticles, Scene scene)
        {
            this.explosionParticles = explosionParticles;
            this.laserParticles = laserParticles;

            if (scene.PlayerObject.showShootExplosion)
            {
           //     this.explosionParticles = explosionParticles;

                if (scene.PlayerObject.elonState.State == State.walkShoot)
                {
                    scene.PlayerObject.newPosition += Vector3.Transform(Vector3.Right * 1.25f, scene.PlayerObject.RotationQ);
                    position = scene.PlayerObject.newPosition + 4.2f * Vector3.Transform(Vector3.Forward, scene.PlayerObject.RotationQ) + Vector3.Up * 6.9f;
                }
                else if (scene.PlayerObject.elonState.State == State.idleShoot)
                {
                    scene.PlayerObject.newPosition += Vector3.Transform(Vector3.Right * 1.05f, scene.PlayerObject.RotationQ);
                    position = scene.PlayerObject.newPosition + 4.7f * Vector3.Transform(Vector3.Forward, scene.PlayerObject.RotationQ) + Vector3.Up * 6.9f;
                }

                velocity.X = (float)(random.NextDouble() - 0.5) * sidewaysVelocityRange;
                velocity.Y = (float)(random.NextDouble() + 0.5) * verticalVelocityRange;
                velocity.Z = (float)(random.NextDouble() - 0.5) * sidewaysVelocityRange;
            }
            if (scene.PlayerObject.showBigExplosion)
            {
             //   this.explosionParticles = explosionParticles;
                position = new Vector3(86,4,-7);

                foreach (var element in scene.PlayerObject.Scene.GameObjects)
                {
                    if (element.Name == "Drzwi 2")
                    {
                        element.ChangePosition(new Vector3(12, 0, 0));
                        scene.PlayerObject.Scene.GameObjects.Remove(element);
                        break;
                    }
                }
                foreach (var element in scene.PlayerObject.Scene.GameObjects)
                {
                    if (element.Name == "paczuszka")
                    {
                        scene.PlayerObject.Scene.GameObjects.Remove(element);
                        break;
                    }
                }

                velocity.X = (float)(random.NextDouble() - 5) * sidewaysVelocityRange;
                velocity.Y = (float)(random.NextDouble() + 5) * verticalVelocityRange;
                velocity.Z = (float)(random.NextDouble() - 5) * sidewaysVelocityRange;
            }
            if (scene.PlayerObject.showTinExplosion)
            {
            //    this.explosionParticles = explosionParticles;
                position = new Vector3(scene.currentTinPos.X, scene.currentTinPos.Y+2, scene.currentTinPos.Z);

                velocity.X = (float)(random.NextDouble() - 0.5) * sidewaysVelocityRange;
                velocity.Y = (float)(random.NextDouble() + 0.5) * verticalVelocityRange;
                velocity.Z = (float)(random.NextDouble() - 0.5) * sidewaysVelocityRange;
            }
            if (scene.PlayerObject.showLaser)
            {
                laserPosition = new Vector3(2, 3.5f, -90);

                laserVelocity.X = 15f;

                //laserEmitter = new ParticleEmitter(explosionParticles, trailLaserPerSecond, position);
            }
            //if (scene.NPCs[0].showShootExplosion)
            //{
            //    if (scene.NPCs[0].State == GuardState.shoot)
            //    {
            //        scene.NPCs[0].Position += Vector3.Transform(Vector3.Right * 1.05f, scene.NPCs[0].RotationQ);
            //        position = scene.NPCs[0].Position + 4.4f * Vector3.Transform(Vector3.Forward, scene.NPCs[0].RotationQ) + Vector3.Up * 7.6f;
            //    }

            //    velocity.X = (float)(random.NextDouble() - 0.5) * sidewaysVelocityRange;
            //    velocity.Y = (float)(random.NextDouble() + 0.5) * verticalVelocityRange;
            //    velocity.Z = (float)(random.NextDouble() - 0.5) * sidewaysVelocityRange;
            //}
            //if (scene.NPCs[1].showShootExplosion)
            //{
            //    if (scene.NPCs[1].State == GuardState.shoot)
            //    {
            //        scene.NPCs[1].Position += Vector3.Transform(Vector3.Right * 1.05f, scene.NPCs[1].RotationQ);
            //        position = scene.NPCs[1].Position + 4.4f * Vector3.Transform(Vector3.Forward, scene.NPCs[1].RotationQ) + Vector3.Up * 7.6f;
            //    }

            //    velocity.X = (float)(random.NextDouble() - 0.5) * sidewaysVelocityRange;
            //    velocity.Y = (float)(random.NextDouble() + 0.5) * verticalVelocityRange;
            //    velocity.Z = (float)(random.NextDouble() - 0.5) * sidewaysVelocityRange;
            //}
            //if (scene.NPCs[2].showShootExplosion)
            //{
            //    if (scene.NPCs[2].State == GuardState.shoot)
            //    {
            //        scene.NPCs[2].Position += Vector3.Transform(Vector3.Right * 1.05f, scene.NPCs[2].RotationQ);
            //        position = scene.NPCs[2].Position + 4.4f * Vector3.Transform(Vector3.Forward, scene.NPCs[2].RotationQ) + Vector3.Up * 7.6f;
            //    }

            //    velocity.X = (float)(random.NextDouble() - 0.5) * sidewaysVelocityRange;
            //    velocity.Y = (float)(random.NextDouble() + 0.5) * verticalVelocityRange;
            //    velocity.Z = (float)(random.NextDouble() - 0.5) * sidewaysVelocityRange;
            //}
            
        }

        public bool Update (GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position += velocity * elapsedTime;
            velocity.X += elapsedTime * gravity;
            age += elapsedTime;

            //laserEmitter.Update(gameTime, position);

           if (age > projectileLifespan)
           {
               for (int i = 0; i < numExplosionParticles; i++)
                   explosionParticles.AddParticle(position, velocity);

               for (int i = 0; i < numLaserParticles; i++)
                   laserParticles.AddParticle(laserPosition, laserVelocity);

                   return false;
           }
           return true;
        }
    }
}
