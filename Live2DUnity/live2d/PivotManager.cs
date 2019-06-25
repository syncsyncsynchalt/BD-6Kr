using System;
using System.Collections.Generic;

namespace live2d
{
    public class PivotManager : ISerializableV2
    {

        public void readV2(BReader br)
        {
        }

        public int getParamCount()
        {
            return -1;
        }

        public List<ParamPivots> getParamPivotTableRef()
        {
            return new List<ParamPivots> { };
        }
    }
}
