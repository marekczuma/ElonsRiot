using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SkinnedModel
{
    public class Keyframe
    {
        public Keyframe(int bone, TimeSpan time, Matrix transform)
        {
            Bone = bone;
            Time = time;
            Transform = transform;
        }

        private Keyframe()
        {
        }

        [ContentSerializer]
        public int Bone { get; private set; }

        [ContentSerializer]
        public TimeSpan Time { get; private set; }

        [ContentSerializer]
        public Matrix Transform { get; private set; }
    }
}
