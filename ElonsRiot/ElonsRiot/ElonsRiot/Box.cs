using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    public class Box
    {
        public Vector3 center;
        public Vector3 center2;
        Vector3 halfWidth;
        Player referencePlayer;
        GameObject referenceObject;
        public Vector3[] ActualCorners;
        public Vector3[] actualRadiuses;
        public Vector3[] corners;
        public int length;
        public Box(Player gameObj)
        {
            ActualCorners = new Vector3[6];
            this.referencePlayer = gameObj;
            this.corners = new Vector3[8];
            this.corners = gameObj.boundingBox.GetCorners();
            this.center = gameObj.center;
            this.actualRadiuses = new Vector3[2];

        }
        public Box(GameObject gameObj, Player pla)
        {
            ActualCorners = new Vector3[6];
            this.referenceObject = gameObj;
            this.corners = new Vector3[8];
            this.corners = gameObj.boundingBox.GetCorners();
            this.referencePlayer = pla;
            this.center = gameObj.center;
            this.actualRadiuses = new Vector3[2];
        }


        public void GetRadius()
        {
            center2.X = (corners[1].X + corners[7].X) / 2;
            center2.Y = (corners[1].Y + corners[7].Y) / 2;
            center2.Z = (corners[1].Z + corners[7].Z) / 2;
        }

        public void CheckWhichCorners()
        {
            GetRadius();
            //ruszyl się w lewo (-x)
            if (referencePlayer.oldPosition.X > referencePlayer.newPosition.X)
            {
                if (referencePlayer.oldPosition.Z > referencePlayer.newPosition.Z)
                {
                    //i w tył (+z) ok
                    length = 2;
                    actualRadiuses[0].X = (corners[4].X + corners[6].X) / 2;
                    actualRadiuses[0].Y = (corners[4].Y + corners[6].Y) / 2;
                    actualRadiuses[0].Z = (corners[4].Z + corners[6].Z) / 2;
                    actualRadiuses[1].X = (corners[4].X + corners[3].X) / 2;
                    actualRadiuses[1].Y = (corners[4].Y + corners[3].Y) / 2;
                    actualRadiuses[1].Z = (corners[4].Z + corners[3].Z) / 2;
                }
                else if (referencePlayer.oldPosition.Z < referencePlayer.newPosition.Z)
                {
                    //i w przód (-z) ok
                    length = 2;
                    actualRadiuses[0].X = (corners[1].X + corners[3].X) / 2;
                    actualRadiuses[0].Y = (corners[1].Y + corners[3].Y) / 2;
                    actualRadiuses[0].Z = (corners[1].Z + corners[3].Z) / 2;
                    actualRadiuses[1].X = (corners[4].X + corners[3].X) / 2;
                    actualRadiuses[1].Y = (corners[4].Y + corners[3].Y) / 2;
                    actualRadiuses[1].Z = (corners[4].Z + corners[3].Z) / 2;

                }
                else
                {
                    //tylko w lewo (-x) ok
                    length = 1;
                    actualRadiuses[0].X = (corners[0].X + corners[7].X) / 2;
                    actualRadiuses[0].Y = (corners[0].Y + corners[7].Y) / 2;
                    actualRadiuses[0].Z = (corners[0].Z + corners[7].Z) / 2;
                }
            }
            //ruszyl sie w prawo.. (+x) ok
            else if (referencePlayer.oldPosition.X < referencePlayer.newPosition.X)
            {
                if (referencePlayer.oldPosition.Z > referencePlayer.newPosition.Z)
                {
                    //i w tył (+z) ok
                    length = 2;
                    actualRadiuses[0].X = (corners[5].X + corners[2].X) / 2;
                    actualRadiuses[0].Y = (corners[5].Y + corners[2].Y) / 2;
                    actualRadiuses[0].Z = (corners[5].Z + corners[2].Z) / 2;
                    actualRadiuses[1].X = (corners[5].X + corners[7].X) / 2;
                    actualRadiuses[1].Y = (corners[5].Y + corners[7].Y) / 2;
                    actualRadiuses[1].Z = (corners[5].Z + corners[7].Z) / 2;
                }
                else if (referencePlayer.oldPosition.Z < referencePlayer.newPosition.Z)
                {
                    //i w przód (-z) ok
                    length = 2;
                    actualRadiuses[0].X = (corners[1].X + corners[3].X) / 2;
                    actualRadiuses[0].Y = (corners[1].Y + corners[3].Y) / 2;
                    actualRadiuses[0].Z = (corners[1].Z + corners[3].Z) / 2;
                    actualRadiuses[1].X = (corners[1].X + corners[6].X) / 2;
                    actualRadiuses[1].Y = (corners[1].Y + corners[6].Y) / 2;
                    actualRadiuses[1].Z = (corners[1].Z + corners[6].Z) / 2;
                }
                else
                {
                    //tylko w prawo (+x) ok
                    length = 4;
                    actualRadiuses[0].X = (corners[1].X + corners[6].X) / 2;
                    actualRadiuses[0].Y = (corners[1].Y + corners[6].Y) / 2;
                    actualRadiuses[0].Z = (corners[1].Z + corners[6].Z) / 2;
                }
            }
            else
            {
                //ruszyl sie w tyl (+z) ok
                if (referencePlayer.oldPosition.Z > referencePlayer.newPosition.Z)
                {
                    length = 1;
                    actualRadiuses[0].X = (corners[5].X + corners[7].X) / 2;
                    actualRadiuses[0].Y = (corners[5].Y + corners[7].Y) / 2;
                    actualRadiuses[0].Z = (corners[5].Z + corners[7].Z) / 2;
                }
                //ruszył się w przód (-z) ok
                else if (referencePlayer.oldPosition.Z < referencePlayer.newPosition.Z)
                {
                    length = 1;
                    actualRadiuses[0].X = (corners[1].X + corners[3].X) / 2;
                    actualRadiuses[0].Y = (corners[1].Y + corners[3].Y) / 2;
                    actualRadiuses[0].Z = (corners[1].Z + corners[3].Z) / 2;
                }
                else
                {
                    //wgl się nie ruszył
                }
            }

        }

        public void CheckWhichCornersForObjects()
        {

            //ruszyl się w lewo (-x)
            if (referencePlayer.oldPosition.X > referencePlayer.newPosition.X)
            {
                if (referencePlayer.oldPosition.Z > referencePlayer.newPosition.Z)
                {
                    //i w tył (+z) ok
                    length = 6;

                    actualRadiuses[1].X = (corners[1].X + corners[3].X) / 2;
                    actualRadiuses[1].Y = (corners[1].Y + corners[3].Y) / 2;
                    actualRadiuses[1].Z = (corners[1].Z + corners[3].Z) / 2;
                    actualRadiuses[0].X = (corners[1].X + corners[6].X) / 2;
                    actualRadiuses[0].Y = (corners[1].Y + corners[6].Y) / 2;
                    actualRadiuses[0].Z = (corners[1].Z + corners[6].Z) / 2;
                }
                else if (referencePlayer.oldPosition.Z < referencePlayer.newPosition.Z)
                {
                    //i w przód (-z) ok
                    length = 6;
                    actualRadiuses[1].X = (corners[5].X + corners[2].X) / 2;
                    actualRadiuses[1].Y = (corners[5].Y + corners[2].Y) / 2;
                    actualRadiuses[1].Z = (corners[5].Z + corners[2].Z) / 2;
                    actualRadiuses[0].X = (corners[5].X + corners[7].X) / 2;
                    actualRadiuses[0].Y = (corners[5].Y + corners[7].Y) / 2;
                    actualRadiuses[0].Z = (corners[5].Z + corners[7].Z) / 2;

                }
                else
                {
                    //tylko w lewo (-x) ok
                    length = 4;

                    actualRadiuses[0].X = (corners[1].X + corners[6].X) / 2;
                    actualRadiuses[0].Y = (corners[1].Y + corners[6].Y) / 2;
                    actualRadiuses[0].Z = (corners[1].Z + corners[6].Z) / 2;
                }
            }
            //ruszyl sie w prawo.. (+x) ok
            else if (referencePlayer.oldPosition.X < referencePlayer.newPosition.X)
            {
                if (referencePlayer.oldPosition.Z > referencePlayer.newPosition.Z)
                {
                    //i w tył (+z) ok
                    length = 6;
                    actualRadiuses[1].X = (corners[1].X + corners[3].X) / 2;
                    actualRadiuses[1].Y = (corners[1].Y + corners[3].Y) / 2;
                    actualRadiuses[1].Z = (corners[1].Z + corners[3].Z) / 2;
                    actualRadiuses[0].X = (corners[4].X + corners[3].X) / 2;
                    actualRadiuses[0].Y = (corners[4].Y + corners[3].Y) / 2;
                    actualRadiuses[0].Z = (corners[4].Z + corners[3].Z) / 2;
                }
                else if (referencePlayer.oldPosition.Z < referencePlayer.newPosition.Z)
                {
                    //i w przód (-z) ok
                    length = 6;
                    actualRadiuses[0].X = (corners[4].X + corners[6].X) / 2;
                    actualRadiuses[0].Y = (corners[4].Y + corners[6].Y) / 2;
                    actualRadiuses[0].Z = (corners[4].Z + corners[6].Z) / 2;
                    actualRadiuses[1].X = (corners[4].X + corners[3].X) / 2;
                    actualRadiuses[1].Y = (corners[4].Y + corners[3].Y) / 2;
                    actualRadiuses[1].Z = (corners[4].Z + corners[3].Z) / 2;
                }
                else
                {
                    //tylko w prawo (+x)
                    length = 4;
                    actualRadiuses[0].X = (corners[0].X + corners[7].X) / 2;
                    actualRadiuses[0].Y = (corners[0].Y + corners[7].Y) / 2;
                    actualRadiuses[0].Z = (corners[0].Z + corners[7].Z) / 2;
                }
            }
            else
            {
                //ruszyl sie w tyl (+z)
                if (referencePlayer.oldPosition.Z > referencePlayer.newPosition.Z)
                {
                    length = 4;
                    actualRadiuses[0].X = (corners[1].X + corners[3].X) / 2;
                    actualRadiuses[0].Y = (corners[1].Y + corners[3].Y) / 2;
                    actualRadiuses[0].Z = (corners[1].Z + corners[3].Z) / 2;
                }
                //ruszył się w przód (-z)
                else if (referencePlayer.oldPosition.Z < referencePlayer.newPosition.Z)
                {
                    length = 4;
                    actualRadiuses[0].X = (corners[5].X + corners[7].X) / 2;
                    actualRadiuses[0].Y = (corners[5].Y + corners[7].Y) / 2;
                    actualRadiuses[0].Z = (corners[5].Z + corners[7].Z) / 2;
                }
                else
                {
                    //wgl się nie ruszył
                }
            }

        }


    }
}

