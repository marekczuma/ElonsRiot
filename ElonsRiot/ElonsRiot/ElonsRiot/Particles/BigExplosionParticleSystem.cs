using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ElonsRiot.Particles
{
    class BigExplosionParticleSystem : ParticleSystem
    {
        public BigExplosionParticleSystem(Game1 game, ContentManager content)
            : base(game, content)
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "Particle/explosion";

            settings.MaxParticles = 1000;

            settings.Duration = TimeSpan.FromSeconds(0.25f);
            settings.DurationRandomness = 0;

            settings.MinHorizontalVelocity = 10;
            settings.MaxHorizontalVelocity = 20;

            settings.MinVerticalVelocity = 10;
            settings.MaxVerticalVelocity = 20;

            settings.EndVelocity = 0;

            settings.MinColor = Color.DarkGray;
            settings.MaxColor = Color.Gray;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 20f;
            settings.MaxStartSize = 25f;

            settings.MinEndSize = 40;
            settings.MaxEndSize = 50;

            settings.BlendState = BlendState.Additive;
        }
    }
}
