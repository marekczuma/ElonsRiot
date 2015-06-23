using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Learning
{
    public class PaloSkills
    {
        public PaloCharacter Palo { get; set; }
        public float Engineering { get; set; }
        public float Shooting { get; set; }
        public float Using { get; set; }
        public float Persuasion { get; set; }

        public PaloSkills()
        {
            Engineering = 34;
            Shooting = 0;
            Using = 16;
            Persuasion = 73;
        }
    }
}
