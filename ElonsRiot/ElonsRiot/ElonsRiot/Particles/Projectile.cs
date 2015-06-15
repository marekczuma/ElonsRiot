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
 //       ParticleSystem bigExplosionParticles;

        Vector3 position;
        Vector3 velocity;
        float age;

        static Random random = new Random();

        public Projectile(ParticleSystem explosionParticles, Player playerObject)
        {
            if (playerObject.showShootExplosion)
            {
                this.explosionParticles = explosionParticles;

                if (playerObject.elonState.State == State.walkShoot)
                {
                    playerObject.newPosition += Vector3.Transform(Vector3.Right * 1.25f, playerObject.RotationQ);
                    position = playerObject.newPosition + 4.2f * Vector3.Transform(Vector3.Forward, playerObject.RotationQ) + Vector3.Up * 6.9f;
                }
                else if (playerObject.elonState.State == State.idleShoot)
                {
                    playerObject.newPosition += Vector3.Transform(Vector3.Right * 1.05f, playerObject.RotationQ);
                    position = playerObject.newPosition + 4.7f * Vector3.Transform(Vector3.Forward, playerObject.RotationQ) + Vector3.Up * 6.9f;
                }

                velocity.X = (float)(random.NextDouble() - 0.5) * sidewaysVelocityRange;
                velocity.Y = (float)(random.NextDouble() + 0.5) * verticalVelocityRange;
                velocity.Z = (float)(random.NextDouble() - 0.5) * sidewaysVelocityRange;
            }
            if (playerObject.showBigExplosion)
            {
                this.explosionParticles = explosionParticles;
                position = new Vector3(86,4,-7);
                foreach(var element in playerObject.Scene.GameObjects)
                {
                    if (element.Name == "Drzwi 2")
                    {
                        element.ChangePosition(new Vector3(12, 0, 0));
                        playerObject.Scene.GameObjects.Remove(element);
                        break;
                    }
                }
                foreach (var element in playerObject.Scene.GameObjects)
                {
                    if (element.Name == "Bomba")
                    {
                        playerObject.Scene.GameObjects.Remove(element);
                        break;
                    }
                }

                velocity.X = (float)(random.NextDouble() - 5) * sidewaysVelocityRange;
                velocity.Y = (float)(random.NextDouble() + 5) * verticalVelocityRange;
                velocity.Z = (float)(random.NextDouble() - 5) * sidewaysVelocityRange;
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
