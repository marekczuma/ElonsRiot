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

        public Projectile(ParticleSystem explosionParticles, Vector3 pos, Quaternion rotation)
        {
            this.explosionParticles = explosionParticles;

       //     position = Vector3.Transform(pos, rotation);
            position = Vector3.Transform(position, Matrix.CreateTranslation(new Vector3(pos.X, pos.Y + 5, pos.Z + 5)));
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
