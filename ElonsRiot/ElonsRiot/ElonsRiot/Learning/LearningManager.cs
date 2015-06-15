using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Learning
{
    public class LearningManager
    {
        public PaloCharacter Palo { get; set; }
        public Scene Scene { get; set; }
        public float Timer { get; set; }
        public float TimerStatement { get; set; }
        SpriteBatch spriteBatchStatement;
        public LearningManager()
        {
            Timer = 0;
            TimerStatement = 0;
            //spriteBatchStatement = new SpriteBatch(Scene.GraphicsDevice);
        }

        public void DrawStatement(string _statement)
        {
            float timeInMS = Scene.time.ElapsedGameTime.Milliseconds;
            if(TimerStatement >0)
            {
                
            }
        }


    }
}
