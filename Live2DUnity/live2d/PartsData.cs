using System;
using System.Collections.Generic;

namespace live2d
{
    public class PartsData : ISerializableV2
    {
        public class PartsDataContext : IContextData
        {
            public PartsDataContext(PartsData src)
            {
            }

            public float getPartsOpacity()
            {
                return -1;
            }

            public void setPartsOpacity(float v)
            {
            }
        }

        public PartsData()
        {
        }

        public void readV2(BReader br)
        {
        }

        public PartsDataContext init(ModelContext mdc)
        {
            return new PartsDataContext(new PartsData());
        }

        public void addBaseData(IBaseData baseData)
        {
        }

        public void addDrawData(IDrawData drawData)
        {
        }

        public void setBaseData(List<IBaseData> _baseDataList)
        {
        }

        public void setDrawData(List<IDrawData> _drawDataList)
        {
        }

        public bool isVisible()
        {
            return false;
        }

        public bool isLocked()
        {
            return false;
        }

        public void setVisible(bool v)
        {
        }

        public void setLocked(bool v)
        {
        }

        public List<IBaseData> getBaseData()
        {
            return new List<IBaseData> { };
        }

        public List<IDrawData> getDrawData()
        {
            return new List<IDrawData> { };
        }

        public PartsDataID getPartsDataID()
        {
            return new PartsDataID();
        }

        public void setPartsDataID(PartsDataID id)
        {
        }

        public string toStringAllDrawable()
        {
            return "";
        }
    }
}
