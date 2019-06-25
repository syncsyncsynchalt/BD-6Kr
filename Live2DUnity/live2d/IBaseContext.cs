namespace live2d
{
    public class IBaseContext
    {
        public bool isAvailable()
        {
            return false;
        }

        public void setAvailable(bool available)
        {
        }

        public IBaseContext(IBaseData src)
        {
        }

        public IBaseData getSrcPtr()
        {
            return null;
        }

        public void setPartsIndex(int p)
        {
        }

        public int getPartsIndex()
        {
            return -1;
        }

        public bool isOutsideParam()
        {
            return false;
        }

        public void setOutsideParam(bool isOutsideParam)
        {
        }

        public float getTotalScale()
        {
            return -1;
        }

        public void setTotalScale_notForClient(float totalScale)
        {
        }

        public float getInterpolatedOpacity()
        {
            return -1;
        }

        public void setInterpolatedOpacity(float interpolatedOpacity)
        {
        }

        public float getTotalOpacity()
        {
            return -1;
        }

        public void setTotalOpacity(float totalOpacity)
        {
        }
    }
}
