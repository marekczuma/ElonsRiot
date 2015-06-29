using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ElonsRiot.Particles
{
    class ExplosionParticleSystem : ParticleSystem
    {
        public ExplosionParticleSystem(Game1 game, ContentManager content) : base(game, content)
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "Particle/explosion";

            settings.MaxParticles = 100;

            settings.Duration = TimeSpan.FromSeconds(0.25f);
            settings.DurationRandomness = 0;

            settings.MinHorizontalVelocity = 1;
            settings.MaxHorizontalVelocity = 1;

            settings.MinVerticalVelocity = 1;
            settings.MaxVerticalVelocity = 1;

            settings.Gravity = new Vector3(0, 0, 0);

            settings.EndVelocity = 0;

            settings.MinColor = Color.DarkGray;
            settings.MaxColor = Color.Gray;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 0.5f;
            settings.MaxStartSize = 0.5f;

            settings.MinEndSize = 1;
            settings.MaxEndSize = 1;

            settings.BlendState = BlendState.Additive;
        }
    }
}
