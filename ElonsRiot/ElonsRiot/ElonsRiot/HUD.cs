using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace ElonsRiot
{
    static class HUD
    {
        public static SpriteFont font;
        public static Texture2D healthBarElon;
        public static Texture2D healthBarPalo;
        public static Texture2D healthValue;
        public static Vector2 scale;
        public static Texture2D gunElon;
        public static Texture2D dialoguesBackground;
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
            healthBarElon = content.Load<Texture2D>("HUD/elonBar");
            healthBarPalo = content.Load<Texture2D>("HUD/paloBar");
            healthValue = content.Load<Texture2D>("HUD/health");
            gunElon = content.Load<Texture2D>("HUD/Gun/gunElon");
            gunPalo = content.Load<Texture2D>("HUD/Gun/GunPalo");
            progress = content.Load<Texture2D>("HUD/progress");
            crossHair = content.Load<Texture2D>("HUD/crosshair");
            newItem1 = content.Load<Texture2D>("HUD/item1");
            newItem2 = content.Load<Texture2D>("HUD/item2");
            skills = content.Load<Texture2D>("HUD/skills");
            learning = content.Load<Texture2D>("HUD/nauka");
            dialoguesBackground = content.Load<Texture2D>("HUD/background");
        }

        public static void DrawHUD(SpriteBatch[] spriteBatch, float healthElon, float healthPalo, GraphicsDevice graphics, Scene myScene, int width)
        {
            float scaleFloat = 0.12f;
            scale = new Vector2(scaleFloat * (healthElon / 100), scaleFloat);

            spriteBatch[0].Begin();

            spriteBatch[0].Draw(healthBarElon, new Vector2(0, 10), null, Color.White, 0, Vector2.Zero, scaleFloat, SpriteEffects.None, 0);
            spriteBatch[0].Draw(healthValue, new Vector2(0, 10), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
           // spriteBatch[0].DrawString(font, "" + healthElon, new Vector2(150, 5), Color.White);

            spriteBatch[0].End();

            scalePalo = new Vector2(scaleFloat * (healthPalo / 100), scaleFloat);
            spriteBatch[1].Begin();
            spriteBatch[1].Draw(healthBarPalo, new Vector2(width - 320, 10), null, Color.White, 0, Vector2.Zero, scaleFloat, SpriteEffects.None, 0);
            spriteBatch[1].Draw(healthValue, new Vector2(width - 320, 10), null, Color.White, 0, Vector2.Zero, scalePalo, SpriteEffects.None, 0);
           // spriteBatch[1].DrawString(font, "" + healthPalo, new Vector2(width - 190, 5), Color.White);
           // spriteBatch[1].DrawString(font, "position: " + myScene.PlayerObject.Position, new Vector2(300, 15), Color.White);
            //foreach (var elem in myScene.GameObjects)
            //{
            //    if (elem.Name == "gun")
            //        spriteBatch[1].DrawString(font, "gunPos: " + elem.Position, new Vector2(300, 40), Color.White);
            //}
            //spriteBatch[1].DrawString(font, "OffsetDistance.Z: " + myScene.PlayerObject.camera.offsetDistance.Z, new Vector2(300, 30), Color.White);

            spriteBatch[1].End();

            graphics.DepthStencilState = DepthStencilState.Default;


        }
        public static void DrawHUDGuns(SpriteBatch[] spriteBatch, float ammoElon,float ammoPalo,float ammoMax, GraphicsDevice graphics,int width)
        {
            spriteBatch[0].Begin();
            spriteBatch[0].Draw(gunElon, new Vector2(5, 75), null, Color.White, 0, Vector2.Zero, 0.2f, SpriteEffects.None, 0);
            spriteBatch[0].DrawString(font, "" + ammoElon + "/" + ammoMax, new Vector2(160, 75), Color.White);
            spriteBatch[0].End();
          //  spriteBatch[1].Begin();
         //   spriteBatch[1].Draw(gunPalo, new Vector2(width - 170, 75), null, Color.White, 0, Vector2.Zero, 0.2f, SpriteEffects.None, 0);
        //    spriteBatch[1].DrawString(font, "" + ammoPalo + "/" + ammoMax, new Vector2(width - 220, 75), Color.White);
        //    spriteBatch[1].End();
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
            float messageScale = 1f;
            if(message.Count() > 122)
            {
                float count = 1228/message.Count();
                count = count / 10;
                messageScale =count;
            }
            spriteBatch.Begin();
            spriteBatch.Draw(dialoguesBackground, new Vector2(20, graphic.Viewport.Height - 80), null, Color.White, 0, Vector2.Zero, 0.96f, SpriteEffects.None, 0);
           // spriteBatch.DrawString(font, "" + message, new Vector2(25, graphic.Viewport.Height - 60), Color.White);
            spriteBatch.DrawString(font, "" + message, new Vector2(25, graphic.Viewport.Height - 60), Color.White, 0, Vector2.Zero, new Vector2(messageScale,1f), SpriteEffects.None, 0);
            spriteBatch.End();
            graphic.DepthStencilState = DepthStencilState.Default;
            //Debug.WriteLine(message.Count());
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
