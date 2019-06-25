namespace live2d
{
    public struct LDRectF
    {
        public float x;

        public float y;

        public float width;

        public float height;

        public LDRectF(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public float getCenterX()
        {
            return x + 0.5f * width;
        }

        public float getCenterY()
        {
            return y + 0.5f * height;
        }

        public float getRight()
        {
            return x + width;
        }

        public float getBottom()
        {
            return y + height;
        }

        public void setRect(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public void setRect(LDRectF r)
        {
            x = r.x;
            y = r.y;
            width = r.width;
            height = r.height;
        }

        public bool contains(float x, float y)
        {
            if (this.x <= x && this.y <= y && x <= this.x + width)
            {
                return y <= this.y + height;
            }
            return false;
        }

        public void add(float x1, float y1)
        {
            if (x < x1)
            {
                if (x + width < x1)
                {
                    width = x1 - x;
                }
            }
            else
            {
                width = x + width - x1;
                x = x1;
            }
            if (y < y1)
            {
                if (y + height < y1)
                {
                    height = y1 - y;
                }
            }
            else
            {
                height = y + height - y1;
                y = y1;
            }
        }

        public new string ToString()
        {
            return x + " , " + y + " , " + width + " , " + height;
        }

        public void expand(float w, float h)
        {
            x -= w;
            y -= h;
            width += w * 2f;
            height += h * 2f;
        }
    }
}
