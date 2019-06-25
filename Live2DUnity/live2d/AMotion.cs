namespace live2d
{
    public abstract class AMotion
    {
        protected int fadeInMsec;

        protected int fadeOutMsec;

        protected float weight;

        public AMotion()
        {
        }

        public void setFadeIn(int fadeInMsec)
        {
        }

        public void setFadeOut(int fadeOutMsec)
        {
        }

        public void setWeight(float weight)
        {
        }

        public int getFadeOut()
        {
            return -1;
        }

        public int getFadeIn()
        {
            return -1;
        }

        public float getWeight()
        {
            return -1;
        }

        public virtual int getDurationMSec()
        {
            return -1;
        }

        public virtual int getLoopDurationMSec()
        {
            return -1;
        }

        public static float getEasing(float time, float totalTime, float accelerateTime)
        {
            return -1;
        }

        public void updateParam(ALive2DModel model, MotionQueueEnt motionQueueEnt)
        {
        }

        public abstract void updateParamExe(ALive2DModel model, long timeMSec, float weight, MotionQueueEnt motionQueueEnt);
    }
}
