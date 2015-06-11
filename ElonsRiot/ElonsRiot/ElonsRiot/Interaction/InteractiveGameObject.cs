using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Interaction
{
    public class InteractiveGameObject : GameObject
    {
        public string Information { get; set; }
        
        public virtual void Interaction(Scene _scene)
        {

        }

        public virtual void AfterInteraction(Scene _scene)
        {

        }
    }
}
