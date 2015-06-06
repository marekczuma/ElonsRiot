using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.BSPTree
{
    class Hall
    {
        public float[] x, z;
        public string name;
        public Hall(float[] _x,float[] _z,string _name)
         {
            x = _x;
            z = _z;
            name = _name;
          }
       public bool checkIsPlayerInside(Vector3 playerPosition)
        {
            if (playerPosition.X > x[0] && playerPosition.X < x[1])
            {
                if(playerPosition.Z > z[0] && playerPosition.Z < z[1])
                {
                    return true;
                }
            }

            return false;
        }
    }
}
