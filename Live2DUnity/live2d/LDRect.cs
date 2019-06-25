namespace live2d
{
    public struct LDRect
    {
        public int x;

        public int y;

        public int width;

        public int height;

        public LDRect(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public LDRect(LDRect r)
        {
            x = r.x;
            y = r.y;
            width = r.width;
            height = r.height;
        }

        public float getCenterX()
        {
            return 0.5f * (float)(x + x + width);
        }

        public float getCenterY()
        {
            return 0.5f * (float)(y + y + height);
        }

        public int getRight()
        {
            return x + width;
        }

        public int getBottom()
        {
            return y + height;
        }

        public void setRect(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public void setRect(LDRect r)
        {
            x = r.x;
            y = r.y;
            width = r.width;
            height = r.height;
        }
    }
}
