using System;
using System.Collections.Generic;

namespace live2d
{
    public class PhysicsHair
    {
        public enum Src
        {
            SRC_TO_X,
            SRC_TO_Y,
            SRC_TO_G_ANGLE
        }

        public enum Target
        {
            TARGET_FROM_ANGLE,
            TARGET_FROM_ANGLE_V
        }
        public PhysicsHair()
        {
        }

        public PhysicsHair(float _baseLengthM, float _airRegistance, float _mass)
        {
        }

        public void setup(float _baseLengthM, float _airRegistance, float _mass)
        {
        }


        public void setup()
        {
        }

        public void addSrcParam(Src srcType, string paramID, float scale, float weight)
        {
        }

        public void addTargetParam(Target targetType, string paramID, float scale, float weight)
        {
        }

        public void update(ALive2DModel model, long time)
        {
        }

    }
}
