using System;
using Microsoft.Xna.Framework;

namespace ElonsRiot.Particles
{
    class Projectile
    {
        const int numExplosionParticles = 30;
        const float projectileLifespan = 0f;
        const float sidewaysVelocityRange = 0;
        const float verticalVelocityRange = 0;
        const float gravity = 0;

        ParticleSystem explosionParticles;

        Vector3 position;
        Vector3 velocity;
        float age;

        static Random random = new Random();

        public Projectile(ParticleSystem explosionParticles, Scene scene)
        {
            if (scene.PlayerObject.showShootExplosion)
            {
                this.explosionParticles = explosionParticles;

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
                this.explosionParticles = explosionParticles;
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
                    if (element.Name == "Bomba")
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
                this.explosionParticles = explosionParticles;
                position = new Vector3(scene.currentTinPos.X, scene.currentTinPos.Y+2, scene.currentTinPos.Z);

                velocity.X = (float)(random.NextDouble() - 0.5) * sidewaysVelocityRange;
                velocity.Y = (float)(random.NextDouble() + 0.5) * verticalVelocityRange;
                velocity.Z = (float)(random.NextDouble() - 0.5) * sidewaysVelocityRange;
            }
            
        }

        public bool Update (GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position += velocity * elapsedTime;
            velocity.Y -= elapsedTime * gravity;
            age += elapsedTime;

           if (age > projectileLifespan)
           {
               for (int i = 0; i < numExplosionParticles; i++)
                   explosionParticles.AddParticle(position, velocity);

               return false;
           }
           return true;
        }
    }
}
