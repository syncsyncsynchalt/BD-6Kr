using System.Collections.Generic;

namespace live2d
{
    public class ModelImpl : ISerializableV2
    {
        public float getCanvasWidth()
        {
            return -1;
        }

        public float getCanvasHeight()
        {
            return -1;
        }

        public void readV2(BReader br)
        {
        }

        public void addPartsData(PartsData parts)
        {
        }

        public List<PartsData> getPartsDataList()
        {
            return new List<PartsData> { };
        }

        public ParamDefSet getParamDefSet()
        {
            return new ParamDefSet();
        }

        public string toStringAllDrawable()
        {
            return "";
        }
    }
}
