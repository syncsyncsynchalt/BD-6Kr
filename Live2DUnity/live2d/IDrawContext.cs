namespace live2d
{
    public class IDrawContext
    {
        public IDrawData srcPtr;

        public int partsIndex;

        public int interpolatedDrawOrder;

        public float interpolatedOpacity;

        public bool[] paramOutside = new bool[1];

        public float partsOpacity;

        public bool available = true;

        public float baseOpacity = 1f;

        public bool isParamOutside()
        {
            return false;
        }

        public bool isAvailable()
        {
            return false;
        }

        public IDrawData getSrcPtr()
        {
            return null;
        }
    }
}
