namespace live2d
{
    public class ParamDefFloat : ISerializableV2
    {
        public ParamDefFloat()
        {
        }

        public ParamDefFloat(ParamID pid, float min, float max, float defaultV)
        {
        }

        public void readV2(BReader br)
        {
        }

        public float getMinValue()
        {
            return -1;
        }

        public float getMaxValue()
        {
            return -1;
        }

        public float getDefaultValue()
        {
            return -1;
        }

        public ParamID getParamID()
        {
            return new ParamID();
        }
    }
}
