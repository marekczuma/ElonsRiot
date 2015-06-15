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

        public Projectile(ParticleSystem explosionParticles, Player playerObject)
        {
            this.explosionParticles = explosionParticles;

            if (playerObject.elonState.State == State.walkShoot)
            {
                playerObject.newPosition += Vector3.Transform(Vector3.Right * 1.35f, playerObject.RotationQ);
                position = playerObject.newPosition + 6f * Vector3.Transform(Vector3.Forward, playerObject.RotationQ) + Vector3.Up * 5.2f;
            }
            else if (playerObject.elonState.State == State.idleShoot)
            {
                playerObject.newPosition += Vector3.Transform(Vector3.Right * 1.15f, playerObject.RotationQ);
                position = playerObject.newPosition + 6.5f * Vector3.Transform(Vector3.Forward, playerObject.RotationQ) + Vector3.Up * 5.2f;
            }

            velocity.X = (float)(random.NextDouble() - 0.5) * sidewaysVelocityRange;
            velocity.Y = (float)(random.NextDouble() + 0.5) * verticalVelocityRange;
            velocity.Z = (float)(random.NextDouble() - 0.5) * sidewaysVelocityRange;
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
