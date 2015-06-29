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
        public static SpriteFont bigFont;
        public static Texture2D healthBarElon;
        public static Texture2D healthBarPalo;
        public static Texture2D healthValue;
        public static Vector2 scale;
        public static Texture2D gunElon;
        public static Texture2D dialoguesBackground;
        public static Texture2D gunPalo;
        public static Texture2D progress1;
        public static Texture2D progress2;
        public static Texture2D crossHair;
        public static Texture2D skillsBackground;
        public static Texture2D skill;
        public static Texture2D engineering;
        public static Texture2D persuasion;
        public static Texture2D usingLearning;
        public static Texture2D shooting;
        public static Texture2D learning;
        public static Texture2D legendP;
        public static Texture2D legendO;
        public static Texture2D legendSpace;
        public static Texture2D legendMouse;
        public static Texture2D legendL;

        public static Vector2 scalePalo;
        public const float offset = 25;

        public static void LoadHUD(ContentManager content, float health)
        {
            font = content.Load<SpriteFont>("HUD/HUDFont");
            bigFont = content.Load<SpriteFont>("HUD/CountdownFont");
            healthBarElon = content.Load<Texture2D>("HUD/elonBar");
            healthBarPalo = content.Load<Texture2D>("HUD/paloBar");
            healthValue = content.Load<Texture2D>("HUD/health");
            gunElon = content.Load<Texture2D>("HUD/Gun/gunElon");
            gunPalo = content.Load<Texture2D>("HUD/Gun/GunPalo");
            progress1 = content.Load<Texture2D>("HUD/Progress/progress1");
            progress2 = content.Load<Texture2D>("HUD/Progress/progress2");
            crossHair = content.Load<Texture2D>("HUD/crosshair");
            skillsBackground = content.Load<Texture2D>("HUD/Skills/skillsBg");
            skill = content.Load<Texture2D>("HUD/Skills/skill");
            learning = content.Load<Texture2D>("HUD/nauka");
            dialoguesBackground = content.Load<Texture2D>("HUD/background");
            legendL = content.Load<Texture2D>("HUD/Legend/legendL");
            legendO = content.Load<Texture2D>("HUD/Legend/legendO");
            legendP = content.Load<Texture2D>("HUD/Legend/legendP");
            legendSpace = content.Load<Texture2D>("HUD/Legend/legendSpace");
            legendMouse = content.Load<Texture2D>("HUD/Legend/legendMouse");

        }

        public static void DrawHUD(SpriteBatch[] spriteBatch, float healthElon, float healthPalo, GraphicsDevice graphics, Scene myScene, int width, float countdownTime)
        {
            float scaleFloat = 0.12f;

            spriteBatch[0].Begin();

            spriteBatch[0].Draw(healthBarElon, new Vector2(0+offset, 10), null, Color.White, 0, Vector2.Zero, scaleFloat, SpriteEffects.None, 0);
       //     spriteBatch[0].Draw(healthValue, new Vector2(0, 10), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
           spriteBatch[0].Draw(healthValue, new Vector2(-1+offset, 10), new Rectangle(0, 0, healthValue.Width - (int)(250-2.5*healthElon), healthValue.Height), Color.White, 0, Vector2.Zero, new Vector2(1.2f, 1.18f), SpriteEffects.None, 0);
            // spriteBatch[0].DrawString(font, "" + healthElon, new Vector2(150, 5), Color.White);

            spriteBatch[0].End();
            
            spriteBatch[1].Begin();
            spriteBatch[1].Draw(healthBarPalo, new Vector2(width - 320, 10), null, Color.White, 0, Vector2.Zero, scaleFloat, SpriteEffects.None, 0);
            spriteBatch[1].Draw(healthValue, new Vector2(width - 321, 10), new Rectangle(0, 0, healthValue.Width - (int)(250 - 2.5 * healthPalo), healthValue.Height), Color.White, 0, Vector2.Zero, new Vector2(1.2f, 1.18f), SpriteEffects.None, 0);
           // spriteBatch[1].DrawString(font, "" + healthPalo, new Vector2(width - 190, 5), Color.White);
            
            if (countdownTime > 2 && countdownTime < 3)
                spriteBatch[1].DrawString(bigFont, "3", new Vector2(width/2-30, 50), Color.Maroon);
            else if (countdownTime > 1 && countdownTime < 2)
                spriteBatch[1].DrawString(bigFont, "2", new Vector2(width / 2 - 30, 50), Color.Maroon);
            else if (countdownTime > 0 && countdownTime < 1)
                spriteBatch[1].DrawString(bigFont, "1", new Vector2(width / 2 - 30, 50), Color.Maroon);

            //foreach (var elem in myScene.GameObjects)
            //{
            //    if (elem.Name == "gun")
            //        spriteBatch[1].DrawString(font, "gunPos: " + elem.Position, new Vector2(300, 40), Color.White);
            //}
            //spriteBatch[1].DrawString(font, "OffsetDistance.Z: " + myScene.PlayerObject.camera.offsetDistance.Z, new Vector2(300, 30), Color.White);

            spriteBatch[1].End();

            graphics.DepthStencilState = DepthStencilState.Default;
        }

        public static void DrawProgress(SpriteBatch[] spriteBatch, GraphicsDevice graphics, Scene myScene, int width, float countdownTime)
        {
            spriteBatch[1].Begin();
            if (countdownTime > 2 && countdownTime < 3)
                spriteBatch[1].DrawString(bigFont, "3", new Vector2(width / 2 - 30, 50), Color.Blue);
            else if (countdownTime > 1 && countdownTime < 2)
                spriteBatch[1].DrawString(bigFont, "2", new Vector2(width / 2 - 30, 50), Color.Blue);
            else if (countdownTime > 0 && countdownTime < 1)
                spriteBatch[1].DrawString(bigFont, "1", new Vector2(width / 2 - 30, 50), Color.Blue);
            spriteBatch[1].End();
        }

        public static void DrawHUDGuns(SpriteBatch[] spriteBatch, float ammoElon,float ammoPalo,float ammoMax, GraphicsDevice graphics,int width)
        {
            spriteBatch[0].Begin();
            spriteBatch[0].Draw(gunElon, new Vector2(5+offset, 75), null, Color.White, 0, Vector2.Zero, 0.2f, SpriteEffects.None, 0);
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
            spriteBatch.Draw(progress1, new Vector2(10+offset, 123), null, Color.White, 0, Vector2.Zero, 0.12f, SpriteEffects.None, 0);
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

        //public static void DrawItem1(SpriteBatch spriteBatch, GraphicsDevice graphics)
        //{
        //    spriteBatch.Begin();
        //    spriteBatch.Draw(newItem1, new Vector2(graphics.Viewport.Width / 2 - 120, 50), null, Color.White, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
        //    spriteBatch.End();
        //    graphics.DepthStencilState = DepthStencilState.Default;
        //}

        //public static void DrawItem2(SpriteBatch spriteBatch, GraphicsDevice graphics)
        //{
        //    spriteBatch.Begin();
        //    spriteBatch.Draw(newItem2, new Vector2(graphics.Viewport.Width / 2 - 120, 50), null, Color.White, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
        //    spriteBatch.End();
        //    graphics.DepthStencilState = DepthStencilState.Default;
        //}

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
            spriteBatch.Draw(dialoguesBackground, new Vector2(20+offset, graphic.Viewport.Height - 80), null, Color.White, 0, Vector2.Zero, 0.96f, SpriteEffects.None, 0);
           // spriteBatch.DrawString(font, "" + message, new Vector2(25, graphic.Viewport.Height - 60), Color.White);
           spriteBatch.DrawString(font, "" + message, new Vector2(25+offset, graphic.Viewport.Height - 60), Color.White, 0, Vector2.Zero, new Vector2(messageScale,1f), SpriteEffects.None, 0);
            spriteBatch.End();
            graphic.DepthStencilState = DepthStencilState.Default;
            //Debug.WriteLine(message.Count());
        }

        public static void DrawStringForInformation(SpriteBatch spriteBatch, String message, GraphicsDevice graphic)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "" + message, new Vector2(20+offset, graphic.Viewport.Height - 200), Color.White);
            spriteBatch.End();
            graphic.DepthStencilState = DepthStencilState.Default;
        }

        public static void DrawStringCenter(SpriteBatch spriteBatch, String message, GraphicsDevice graphic)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "" + message, new Vector2(graphic.Viewport.Width/2 - 50, graphic.Viewport.Height/2), Color.White);
            spriteBatch.End();
            graphic.DepthStencilState = DepthStencilState.Default;
        }

        public static void DrawSkills(SpriteBatch spriteBatch, GraphicsDevice graphics, PaloCharacter palo)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(skillsBackground, new Vector2(graphics.Viewport.Width - 350, 123), null, Color.White, 0, Vector2.Zero, 0.12f, SpriteEffects.None, 0);
            spriteBatch.Draw(skill, new Vector2(graphics.Viewport.Width - 163f, 200), null, Color.White, 0, Vector2.Zero, new Vector2(palo.Skills.Shooting / 100 * 0.12f, 0.12f), SpriteEffects.None, 0);
            spriteBatch.Draw(skill, new Vector2(graphics.Viewport.Width - 163f, 232), null, Color.White, 0, Vector2.Zero, new Vector2(palo.Skills.Engineering / 100 * 0.12f, 0.12f), SpriteEffects.None, 0);
            spriteBatch.Draw(skill, new Vector2(graphics.Viewport.Width - 163f, 263), null, Color.White, 0, Vector2.Zero, new Vector2(palo.Skills.Using/ 100 * 0.12f, 0.12f), SpriteEffects.None, 0);
            spriteBatch.Draw(skill, new Vector2(graphics.Viewport.Width - 163f, 295), null, Color.White, 0, Vector2.Zero, new Vector2(palo.Skills.Persuasion / 100 * 0.12f, 0.12f), SpriteEffects.None, 0);
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

        public static void DrawLegend(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            Vector2 firstPos = new Vector2(graphics.Viewport.Width - 370, graphics.Viewport.Height - 130);

            spriteBatch.Begin();
            spriteBatch.Draw(legendL, firstPos, null, Color.White, 0, Vector2.Zero, 0.09f, SpriteEffects.None, 0);
            spriteBatch.Draw(legendSpace, new Vector2(firstPos.X, firstPos.Y - 35), null, Color.White, 0, Vector2.Zero, 0.09f, SpriteEffects.None, 0);
            spriteBatch.Draw(legendMouse, new Vector2(firstPos.X, firstPos.Y - 70), null, Color.White, 0, Vector2.Zero, 0.09f, SpriteEffects.None, 0);
            spriteBatch.Draw(legendP, new Vector2(firstPos.X, firstPos.Y - 105), null, Color.White, 0, Vector2.Zero, 0.09f, SpriteEffects.None, 0);
            spriteBatch.End();
            graphics.DepthStencilState = DepthStencilState.Default;
        }
    }
}
