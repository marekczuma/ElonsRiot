using System;
using Microsoft.Xna.Framework;

namespace ElonsRiot.Particles
{
    public class ParticleEmitter
    {
        ParticleSystem particleSystem;
        float timeBetweenParticles;
        Vector3 previousPosition;
        float timeLeftOver;

        public ParticleEmitter(ParticleSystem particleSystem, float particlesPerSecond, Vector3 initialPosition)
        {
            this.particleSystem = particleSystem;
            timeBetweenParticles = 1.0f / particlesPerSecond;
            previousPosition = initialPosition;
        }

        public void Update(GameTime gameTime, Vector3 newPosition)
        {
            if (gameTime == null)
                throw new ArgumentNullException("gameTime");

            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(elapsedTime > 0)
            {
                Vector3 velocity = (newPosition - previousPosition) / elapsedTime;

                float timeToSpend = timeLeftOver + elapsedTime;
                float currentTime = -timeLeftOver;

                while (timeToSpend > timeBetweenParticles)
                {
                    currentTime += timeBetweenParticles;
                    timeToSpend -= timeBetweenParticles;

                    float mu = currentTime / elapsedTime;

                    Vector3 position = Vector3.Lerp(previousPosition, newPosition, mu);

                    particleSystem.AddParticle(position, velocity);
                }

                timeLeftOver = timeToSpend;
            }

            previousPosition = newPosition;
        }
    }
}
