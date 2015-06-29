using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ElonsRiot.Particles
{
    class LaserParticleSystem : ParticleSystem
    {
        public LaserParticleSystem(Game1 game, ContentManager content)
            : base(game, content)
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "Particle/laser";

            settings.MaxParticles = 11000;

            settings.Duration = TimeSpan.FromSeconds(6f);
            settings.DurationRandomness = 0;

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 0;

            settings.MinVerticalVelocity = 0;
            settings.MaxVerticalVelocity = 0;

            settings.Gravity = new Vector3(0.3f, 0, 0);

            settings.EndVelocity = 0;

            settings.MinColor = new Color(new Vector3(1, 0.2f, 0.2f));
            settings.MaxColor = Color.Red;

            settings.MinRotateSpeed = 1;
            settings.MaxRotateSpeed = 2;

            settings.MinStartSize = 0.5f;
            settings.MaxStartSize = 0.5f;

            settings.MinEndSize = 0.5f;
            settings.MaxEndSize = 0.5f;

            settings.BlendState = BlendState.Additive;
        }
    }
}
