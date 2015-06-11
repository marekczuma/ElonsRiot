using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ElonsRiot
{
    static class HUD
    {
        public static SpriteFont font;
        public static Texture2D healthBar;
        public static Texture2D healthValue;
        public static Texture2D healthBackground;
        public static Vector2 scale;
        public static Texture2D gunElon;
        public static Texture2D gunPalo;
        public static Texture2D progress;
        public static Texture2D crossHair;
        public static Texture2D newItem1;
        public static Texture2D newItem2;
        public static Texture2D skills;
        public static Texture2D learning;
        public static Vector2 scalePalo;

        public static void LoadHUD(ContentManager content, float health)
        {
            font = content.Load<SpriteFont>("HUD/HUDFont");
            healthBar = content.Load<Texture2D>("HUD/healthBar");
            healthValue = content.Load<Texture2D>("HUD/health");
            healthBackground = content.Load<Texture2D>("HUD/healthEmpty");
            gunElon = content.Load<Texture2D>("HUD/Gun/gunElon");
            gunPalo = content.Load<Texture2D>("HUD/Gun/GunPalo");
            progress = content.Load<Texture2D>("HUD/progress");
            crossHair = content.Load<Texture2D>("HUD/crosshair");
            newItem1 = content.Load<Texture2D>("HUD/item1");
            newItem2 = content.Load<Texture2D>("HUD/item2");
            skills = content.Load<Texture2D>("HUD/skills");
            learning = content.Load<Texture2D>("HUD/nauka");
        }

        public static void DrawHUD(SpriteBatch[] spriteBatch, float healthElon, float healthPalo, GraphicsDevice graphics, Scene myScene, int width)
        {
            scale = new Vector2(0.65f * (healthElon / 100), 0.65f);

            spriteBatch[0].Begin();

            spriteBatch[0].Draw(healthBackground, new Vector2(0, -150), null, Color.White, 0, Vector2.Zero, 0.65f, SpriteEffects.None, 0);
            spriteBatch[0].Draw(healthValue, new Vector2(0, -150), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            spriteBatch[0].Draw(healthBar, new Vector2(0, -150), null, Color.White, 0, Vector2.Zero, 0.65f, SpriteEffects.None, 0);
            spriteBatch[0].DrawString(font, "" + healthElon, new Vector2(150, 5), Color.White);

            spriteBatch[0].End();

            scalePalo = new Vector2(0.65f * (healthPalo / 100), 0.65f);
            spriteBatch[1].Begin();
            spriteBatch[1].Draw(healthBackground, new Vector2(width - 340, -150), null, Color.White, 0, Vector2.Zero, 0.65f, SpriteEffects.None, 0);
            spriteBatch[1].Draw(healthValue, new Vector2(width - 340, -150), null, Color.White, 0, Vector2.Zero, scalePalo, SpriteEffects.None, 0);
            spriteBatch[1].Draw(healthBar, new Vector2(width - 340, -150), null, Color.White, 0, Vector2.Zero, 0.65f, SpriteEffects.None, 0);
            spriteBatch[1].DrawString(font, "" + healthPalo, new Vector2(width - 190, 5), Color.White);
            //spriteBatch[1].DrawString(font, "Visible objects: " + myScene.VisibleGameObjects.Count, new Vector2(300, 15), Color.White);
            //spriteBatch[1].DrawString(font, "OffsetDistance.Z: " + myScene.PlayerObject.camera.offsetDistance.Z, new Vector2(300, 30), Color.White);

            spriteBatch[1].End();

            graphics.DepthStencilState = DepthStencilState.Default;


        }
        public static void DrawHUDGuns(SpriteBatch[] spriteBatch, float ammoElon,float ammoPalo,float ammoMax, GraphicsDevice graphics,int width)
        {
            spriteBatch[0].Begin();
            spriteBatch[0].Draw(gunElon, new Vector2(5, 50), null, Color.White, 0, Vector2.Zero, 0.2f, SpriteEffects.None, 0);
            spriteBatch[0].DrawString(font, "" + ammoElon + "/" + ammoMax, new Vector2(160, 50), Color.White);
            spriteBatch[0].End();
            spriteBatch[1].Begin();
            spriteBatch[1].Draw(gunPalo, new Vector2(width - 170, 50), null, Color.White, 0, Vector2.Zero, 0.2f, SpriteEffects.None, 0);
            spriteBatch[1].DrawString(font, "" + ammoPalo + "/" + ammoMax, new Vector2(width - 220, 50), Color.White);
            spriteBatch[1].End();
            graphics.DepthStencilState = DepthStencilState.Default;

        }

        public static void DrawProgress(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(progress, new Vector2(10, 123), null, Color.White, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
            spriteBatch.End();
            graphics.DepthStencilState = DepthStencilState.Default;
        }

        public static void DrawCrosshair(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(crossHair, new Vector2(graphics.Viewport.Width/2 -24, graphics.Viewport.Height/2 -24), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.End();
            graphics.DepthStencilState = DepthStencilState.Default;
        }

        public static void DrawItem1(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(newItem1, new Vector2(graphics.Viewport.Width / 2 - 120, 50), null, Color.White, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
            spriteBatch.End();
            graphics.DepthStencilState = DepthStencilState.Default;
        }

        public static void DrawItem2(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(newItem2, new Vector2(graphics.Viewport.Width / 2 - 120, 50), null, Color.White, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
            spriteBatch.End();
            graphics.DepthStencilState = DepthStencilState.Default;
        }

        public static void DrawString(SpriteBatch spriteBatch,String message, GraphicsDevice graphic)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "" + message, new Vector2(5, graphic.Viewport.Height - 30), Color.White);
            spriteBatch.End();
            graphic.DepthStencilState = DepthStencilState.Default;
        }

        public static void DrawStringForInformation(SpriteBatch spriteBatch, String message, GraphicsDevice graphic)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "" + message, new Vector2(20, graphic.Viewport.Height - 200), Color.White);
            spriteBatch.End();
            graphic.DepthStencilState = DepthStencilState.Default;
        }

        public static void DrawSkills(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(skills, new Vector2(graphics.Viewport.Width - 230, 123), null, Color.White, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
            spriteBatch.End();
            graphics.DepthStencilState = DepthStencilState.Default;
        }

        public static void DrawLearningIcon(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(learning, new Vector2(graphics.Viewport.Width / 2 , 50), null, Color.White, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
            spriteBatch.End();
            graphics.DepthStencilState = DepthStencilState.Default;
        }
    }
}
