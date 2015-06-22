using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Quests
{
    public class QuestManager
    {
        public Scene Scene { get; set; }
        public HiddenPassageAI HiddenPassage { get; set; }
        public QuestManager(Scene _scene)
        {
            Scene = _scene;
            HiddenPassage = new HiddenPassageAI { PosButtonA = new Vector3(7, -4, 30), PosButtonB = new Vector3(43, -4, 10), Scene = Scene };
        }

        public void UpdateQuests()
        {
            HiddenPassageManage();
        }

        public void HiddenPassageManage()
        {
            HiddenPassage.CheckIsIncluded();
            if(HiddenPassage.AIsIncluded && HiddenPassage.BIsIncluded)
            {
                if(HiddenPassage.QuestFinished == false)
                    HiddenPassage.WallIsMoving = true;
            }
            if(HiddenPassage.WallIsMoving)
            {
                foreach(var element in Scene.GameObjects)
                {
                    if (element.Tag == "hiddenPassage")
                    {
                        Vector3 tmpV = element.Position;
                        if (tmpV.X > 10)
                        {
                            tmpV.X -= 0.02f;
                            element.Position = tmpV;
                        }
                        if(element.Position.X <= 10)
                        {
                            HiddenPassage.WallIsMoving = false;
                            HiddenPassage.QuestFinished = true;
                        }
                    }
                }
            } 
        } 
    }
}
