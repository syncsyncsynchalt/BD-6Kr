using System;
using System.Collections.Generic;

namespace live2d
{
    public class MotionQueueManager
    {
        public MotionQueueManager()
        {
        }

        public int startMotion(AMotion motion, bool autoDelete = false)
        {
            return -1;
        }

        public virtual bool updateParam(ALive2DModel model)
        {
            return false;
        }

        public bool isFinished()
        {
            return false;
        }

        public bool isFinished(int _motionQueueEntNo)
        {
            return false;
        }

        public void stopAllMotions()
        {
        }

        public void setMotionDebugMode(bool f)
        {
        }
    }
}
