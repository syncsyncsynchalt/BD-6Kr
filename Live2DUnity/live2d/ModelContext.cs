using System;
using System.Collections.Generic;

namespace live2d
{
    public class ModelContext
    {
        public int getDrawDataIndex(DrawDataID id)
        {
            return -1;
        }

        public IDrawData getDrawData(DrawDataID id)
        {
            return null;
        }

        public IDrawData getDrawData(int drawIndex)
        {
            return null;
        }

        public void init()
        {
        }

        public void preDraw(DrawParam dp)
        {
        }

        public void draw(DrawParam dp)
        {
        }

        public int getParamIndex(ParamID paramID)
        {
            return -1;
        }

        public int getBaseIndex(BaseDataID baseID)
        {
            return -1;
        }

        public int getBaseDataIndex(BaseDataID baseID)
        {
            return -1;
        }

        public int addFloatParam(ParamID id, float value, float min, float max)
        {
            return -1;
        }

        public void setBaseData(int baseDataIndex, IBaseData baseData)
        {
        }

        public void setParamFloat(int paramIndex, float value)
        {
        }

        public void loadParam()
        {
        }

        public void saveParam()
        {
        }

        public int getInitVersion()
        {
            return -1;
        }

        public bool requireSetup()
        {
            return false;
        }

        public IBaseData getBaseData(int baseDataIndex)
        {
            return null;
        }

        public float getParamFloat(int paramIndex)
        {
            return -1;
        }

        public float getParamMax(int paramIndex)
        {
            return -1;
        }

        public float getParamMin(int paramIndex)
        {
            return -1;
        }

        public void setPartsOpacity(int partsIndex, float opacity)
        {
        }

        public float getPartsOpacity(int partsIndex)
        {
            return -1;
        }

        public int getPartsDataIndex(PartsDataID partsID)
        {
            return -1;
        }

        public IBaseContext getBaseContext(int baseDataIndex)
        {
            return null;
        }

        public IDrawContext getDrawContext(int drawDataIndex)
        {
            return null;
        }

        public PartsData.PartsDataContext getPartsContext(int partsDataIndex)
        {
            return new PartsData.PartsDataContext(new PartsData());
        }

        public PartsData getPartsData(int partsDataIndex)
        {
            return new PartsData();
        }

        public void setTextureMap(int srcIndex, int dstIndex, float scaleX, float scaleY, float offsetX, float offsetY, bool transposition)
        {
        }

        public bool isDrawMethodVersion_2_0()
        {
            return false;
        }

        public ALive2DModel getModel()
        {
            return null;
        }
    }
}
