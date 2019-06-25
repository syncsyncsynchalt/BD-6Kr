namespace live2d
{
    public struct LDPointF
    {
        public float x;

        public float y;

        public LDPointF(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public LDPointF(LDPointF pt)
        {
            x = pt.x;
            y = pt.y;
        }

        public void setPoint(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public void setPoint(LDPointF pt)
        {
            x = pt.x;
            y = pt.y;
        }
    }
}
