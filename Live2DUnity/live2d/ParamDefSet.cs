using System.Collections.Generic;

namespace live2d
{
    public class ParamDefSet : ISerializableV2
    {
        public List<ParamDefFloat> getParamDefFloatList()
        {
            return new List<ParamDefFloat> { };
        }

        public void initDirect()
        {
        }

        public void readV2(BReader br)
        {
        }
    }
}
