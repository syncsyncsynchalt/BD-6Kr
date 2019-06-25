using System;
using System.Collections.Generic;
using System.IO;

namespace live2d
{
    public abstract class ALive2DModel
    {
        public enum ModelDrawMethodVersion
        {
            NOT_SPECIFIED,
            DRAW_2_0,
            DRAW_2_1
        }

        public const int FILE_LOAD_EOF_ERROR = 1;

        public const int FILE_LOAD_VERSION_ERROR = 2;

        protected static int INSTANCE_COUNT;

        protected ModelImpl modelImpl;

        protected ModelContext modelContext;

        protected int errorFlags;

        protected bool loading;

        public ALive2DModel()
        {
        }

        public void setModelImpl(ModelImpl m)
        {
        }

        public ModelImpl getModelImpl()
        {
            return new ModelImpl();
        }

        public float getCanvasWidth()
        {
            return -1;
        }

        public float getCanvasHeight()
        {
            return -1;
        }

        public float getParamFloat(string paramID)
        {
            return -1;
        }

        public void setParamFloat(string paramID, float value)
        {
        }

        public void setParamFloat(string paramID, float value, float weight)
        {
        }

        public void addToParamFloat(string paramID, float value)
        {
        }

        public void addToParamFloat(string paramID, float value, float weight)
        {
        }

        public void multParamFloat(string paramID, float mult)
        {
        }

        public void multParamFloat(string paramID, float mult, float weight)
        {
        }

        public void loadParam()
        {
        }

        public void saveParam()
        {
        }

        public void init()
        {
        }

        public virtual void update()
        {
        }

        public int generateModelTextureNo()
        {
            return -1;
        }

        public void releaseModelTextureNo(int no)
        {
        }

        public abstract void deleteTextures();

        public abstract void draw();

        public static void loadModel_exe(ALive2DModel ret, string filepath)
        {
        }

        public static void loadModel_exe(ALive2DModel ret, Stream bin)
        {
        }

        public int getParamIndex(string paramID)
        {
            return -1;
        }

        public float getParamFloat(int paramIndex)
        {
            return -1;
        }

        public void setParamFloat(int paramIndex, float value)
        {
        }

        public void setParamFloat(int paramIndex, float value, float weight)
        {
        }

        public void addToParamFloat(int paramIndex, float value)
        {
        }

        public void addToParamFloat(int paramIndex, float value, float weight)
        {
        }
        public void multParamFloat(int paramIndex, float mult)
        {
        }

        public void multParamFloat(int paramIndex, float mult, float weight)
        {
        }

        public ModelContext getModelContext()
        {
            return new ModelContext();
        }

        public int getErrorFlags()
        {
            return -1;
        }

        public void setPartsOpacity(string partsID, float opacity)
        {
        }

        public void setPartsOpacity(int partsIndex, float opacity)
        {
        }

        public float getPartsOpacity(string partsID)
        {
            return -1;
        }

        public int getPartsDataIndex(string partsID)
        {
            return -1;
        }

        public int getPartsDataIndex(PartsDataID partsID)
        {
            return -1;
        }

        public float getPartsOpacity(int partsIndex)
        {
            return -1;
        }

        public abstract DrawParam getDrawParam();

        public int getDrawDataIndex(string drawDataID)
        {
            return -1;
        }

        public IDrawData getDrawData(int drawIndex)
        {
            return null;
        }

        public float[] getTransformedPoints(int drawIndex)
        {
            return new float[] { };
        }

        public ushort[] getIndexArray(int drawIndex)
        {
            return new ushort[] { };
        }

        public void setAvatarParts(string partsID, AvatarPartsItem avatarPartsItem)
        {
        }

        public void releaseAvatarParts(string partsID)
        {
        }

        public void setAnisotropy(int n)
        {
        }

        public int getAnisotropy()
        {
            return -1;
        }

        public string toStringAllDrawable()
        {
            return "";
        }

        public void setTextureMap(int srcIndex, int dstIndex, float scaleX, float scaleY, float offsetX, float offsetY, bool transposition = false)
        {
        }

        public void resetTextureMap()
        {
        }

        public void setModelDrawMethodVersion(ModelDrawMethodVersion userSetting)
        {
        }

        public ModelDrawMethodVersion getModelDrawMethodVersion()
        {
            return new ModelDrawMethodVersion();
        }

        public void setColorConvertHSL(int colorGroupNo, float[] convertMat)
        {
        }

        public void setColorConvertMainLine(int colorGroupNo, float low, float high)
        {
        }
    }
}
