﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ElonsRiot
{
    class HUD
    {
        public SpriteFont font;
        public Texture2D healthBar;
        public Texture2D healthValue;
        public Texture2D healthBackground;
        public Vector2 scale;

        public void LoadHUD(ContentManager content, float health)
        {
            font = content.Load<SpriteFont>("HUD/HUDFont");
            healthBar = content.Load<Texture2D>("HUD/healthBar");
            healthValue = content.Load<Texture2D>("HUD/health");
            healthBackground = content.Load<Texture2D>("HUD/healthEmpty");
        }

        public void DrawHUD(SpriteBatch spriteBatch, float health, GraphicsDevice graphics)
        {
            scale = new Vector2(0.4f * (health / 100), 0.4f);
            spriteBatch.Begin();
            spriteBatch.Draw(healthBackground, new Vector2 (33,10), null, Color.White, 0, Vector2.Zero, 0.4f, SpriteEffects.None, 0);
            spriteBatch.Draw(healthValue, new Vector2 (33,10), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            spriteBatch.Draw(healthBar, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 0.4f, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, ""+health, new Vector2(160, 5), Color.White);
            spriteBatch.End();
            graphics.DepthStencilState = DepthStencilState.Default;

        }

    }
}