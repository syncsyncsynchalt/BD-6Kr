using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace live2d
{
    public class Live2DMotion : AMotion
    {
        internal class Motion
        {
            public int fadeInMsec = -1;

            public int fadeOutMsec = -1;

            internal Motion()
            {
            }
        }

        public Live2DMotion()
        {
        }

        public static Live2DMotion loadMotion(string path)
        {
            return new Live2DMotion();
        }

        public static Live2DMotion loadMotion(Stream bin)
        {
            return new Live2DMotion();
        }

        public static Live2DMotion loadMotion(byte[] str)
        {
            return new Live2DMotion();
        }

        public override int getDurationMSec()
        {
            return -1;
        }

        public override int getLoopDurationMSec()
        {
            return -1;
        }

        public override void updateParamExe(ALive2DModel model, long timeMSec, float _weight, MotionQueueEnt motionQueueEnt)
        {
        }

        public bool isLoop()
        {
            return false;
        }

        public void setLoop(bool loop)
        {
        }

        public float getFPS()
        {
            return -1;
        }

        public void setFPS(float fps)
        {
        }
    }
}
