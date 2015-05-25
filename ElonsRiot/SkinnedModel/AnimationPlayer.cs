using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SkinnedModel
{
    public class AnimationPlayer
    {
        AnimationClip currentClipValue;
        TimeSpan currentTimeValue;
        int currentKeyframe;
        bool done;
        Matrix[] boneTransfroms;
        Matrix[] worldTransforms;
        Matrix[] skinTransforms;

        SkinningData skinningDataValue;

        public AnimationPlayer(SkinningData skinningData)
        {
            if (skinningData == null)
                throw new ArgumentNullException("skinningData");

            skinningDataValue = skinningData;

            boneTransfroms = new Matrix[skinningData.BindPose.Count];
            worldTransforms = new Matrix[skinningData.BindPose.Count];
            skinTransforms = new Matrix[skinningData.BindPose.Count];
        }

        public void StartClip(AnimationClip clip)
        {
            if (clip == null)
                throw new ArgumentNullException("clip");

            currentClipValue = clip;
            currentTimeValue = TimeSpan.Zero;
            currentKeyframe = 0;

            //CurrentClip = skinningDataValue.AnimationClips[clip];
            //CurrentTime = TimeSpan.FromSeconds(0);
            //currentKeyframe = 0;
            //done = false;

            skinningDataValue.BindPose.CopyTo(boneTransfroms, 0);
        }

        public void StartClip(AnimationClip clip, int startFrame, int endFrame, bool loop)
        {

        }

        public void Update(TimeSpan time, bool relativeToCurrentTime, Matrix rootTransform)
        {
            UpdateBoneTransforms(time, relativeToCurrentTime);
            UpdateWorldTransforms(rootTransform);
            UpdateSkinTransforms();
        }

        public void UpdateBoneTransforms(TimeSpan time, bool relativeToCurrentTime)
        {
            if (currentClipValue == null)
                throw new InvalidOperationException("AnimationPlayer.Update was called before StartClip");

            if(relativeToCurrentTime)
            {
                time += currentTimeValue;

                while (time >= currentClipValue.Duration)
                {
                    time -= currentClipValue.Duration;
                }
            }

            if ((time < TimeSpan.Zero) || (time >= currentClipValue.Duration))
                throw new ArgumentOutOfRangeException("time");

            if (time < currentTimeValue)
            {
                currentKeyframe = 0;
                skinningDataValue.BindPose.CopyTo(boneTransfroms, 0);
            }

            currentTimeValue = time;

            IList<Keyframe> keyframes = currentClipValue.Keyframes;

            while (currentKeyframe < keyframes.Count)
            {
                Keyframe keyframe = keyframes[currentKeyframe];

                if (keyframe.Time > currentTimeValue)
                    break;

                boneTransfroms[keyframe.Bone] = keyframe.Transform;

                currentKeyframe++;
            }
        }

        public void UpdateWorldTransforms(Matrix rootTransform)
        {
            worldTransforms[0] = boneTransfroms[0] * rootTransform;

            for (int bone = 1; bone < worldTransforms.Length; bone++)
            {
                int parentBone = skinningDataValue.SkeletonHierarchy[bone];

                worldTransforms[bone] = boneTransfroms[bone] * worldTransforms[parentBone];
            }
        }

        public void UpdateSkinTransforms()
        {
            for (int bone = 0; bone <skinTransforms.Length; bone++)
            {
                skinTransforms[bone] = skinningDataValue.InverseBindPose[bone] * worldTransforms[bone];
            }
        }

        public Matrix[] GetBoneTransforms() { return boneTransfroms; }
        public Matrix[] GetWorldTransforms() { return worldTransforms; }
        public Matrix[] GetSkinTransforms() { return skinTransforms; }

        public AnimationClip CurrentClip { get { return currentClipValue; } set { currentClipValue = value; } }

        public TimeSpan CurrentTime { get { return currentTimeValue; } set { currentTimeValue = value; } }
        public SkinningData SkinningData { get { return skinningDataValue; } set { skinningDataValue = value; } }
    }
}
