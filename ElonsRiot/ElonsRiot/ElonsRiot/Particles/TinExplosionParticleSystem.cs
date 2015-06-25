using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ElonsRiot.Particles
{
    class TinExplosionParticleSystem : ParticleSystem
    {
        public TinExplosionParticleSystem(Game1 game, ContentManager content)
            : base(game, content)
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "Particle/explosion";

            settings.MaxParticles = 100;

            settings.Duration = TimeSpan.FromSeconds(0.25f);
            settings.DurationRandomness = 0;

            settings.MinHorizontalVelocity = 1;
            settings.MaxHorizontalVelocity = 3;

            settings.MinVerticalVelocity = 1;
            settings.MaxVerticalVelocity = 3;

            settings.EndVelocity = 0;

            settings.MinColor = Color.DarkGray;
            settings.MaxColor = Color.Gray;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 5f;
            settings.MaxStartSize = 10f;

            settings.MinEndSize = 5;
            settings.MaxEndSize = 10;

            settings.BlendState = BlendState.Additive;
        }
    }
}