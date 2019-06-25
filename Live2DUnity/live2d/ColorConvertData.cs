namespace live2d
{
    public class ColorConvertData
    {
        public ColorConvertMethod methodType;

        public float mainLineHigh = 1f;

        public float mainLineLow = 0.5f;

        public void setMatrix(float[] m)
        {
        }

        public float[] getMatrix()
        {
            return new float[] { };
        }

        public override string ToString()
        {
            return base.ToString() + $" type:{methodType.ToString()}";
        }
    }
}
