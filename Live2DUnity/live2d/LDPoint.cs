namespace live2d
{
    public struct LDPoint
    {
        public int x;

        public int y;

        public LDPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public LDPoint(LDPoint pt)
        {
            x = pt.x;
            y = pt.y;
        }

        public void setPoint(LDPoint pt)
        {
            x = pt.x;
            y = pt.y;
        }

        public void setPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
