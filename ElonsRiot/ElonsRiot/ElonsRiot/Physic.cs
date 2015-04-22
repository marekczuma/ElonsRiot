using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    class Physic
    {
        List<GameObject> gameObject;
        public Physic(List<GameObject> gobj)
        {
            this.gameObject = gobj;
        }

    }
}
