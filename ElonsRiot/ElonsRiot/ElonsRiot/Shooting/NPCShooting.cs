using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Shooting
{
    public class NPCShooting
    {
        public GameObject Character { get; set; }
        public Scene Scene { get; set; }
        public GameObject Target { get; set; }
        public NPCShooting (Scene _scene)
        {
            Scene = _scene;
        }
        public void TinShot(Learning.Tin _tin)
        {
            _tin.Destroy();
        }
    }
}
