using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Quests
{
    public class HiddenPassageAI
    {
        public Vector3 PosButtonA { get; set; }
        public Vector3 PosButtonB { get; set; }

        public bool AIsIncluded { get; set; }
        public bool BIsIncluded { get; set; }
        public bool WallIsMoving { get; set; }
        public bool QuestFinished { get; set; }
        
        public GameObject WallTarget { get; set; }

        public Scene Scene { get; set; }

        private float timer = 0;

        public HiddenPassageAI()
        {
            AIsIncluded = false;
            BIsIncluded = false;
            QuestFinished = false;
            WallIsMoving = false;
        }

        public void CheckIsIncluded()
        {
            if(timer <=0)
            {
                foreach (var elem in Scene.GameObjects)
                {
                    if (Vector3.Distance(elem.Position, PosButtonA) <= 7)
                    {
                        AIsIncluded = true;
                        break;
                    }else
                    {
                        AIsIncluded = false;
                    }
                }
                foreach (var elem in Scene.GameObjects)
                {
                    if (Vector3.Distance(elem.Position, PosButtonB) <= 6)
                    {
                        BIsIncluded = true;
                        break;
                    }else
                    {
                        BIsIncluded = false;
                    }
                }
                timer = 1000;
            }
            float timeInMS = Scene.time.ElapsedGameTime.Milliseconds;
            timer -= timeInMS;
        }

    }
}
