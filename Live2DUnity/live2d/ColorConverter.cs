namespace live2d
{
    public class ColorConverter
    {
        public ColorConvertData data = new ColorConvertData();

        public virtual void clearCache()
        {
        }

        public override string ToString()
        {
            return base.ToString() + " data:" + data.ToString();
        }
    }
}
