using System.Collections.Generic;

namespace live2d
{
    public abstract class DrawParam
    {
        public class TextureInfo
        {
        }

        public enum ClippingMethod
        {
            Ignore,
            Alpha,
            BufferSeq,
            BufferPre,
            Mesh
        }

        public const int DEFAULT_FIXED_TEXTURE_COUNT = 32;

        public const int CLIPPING_PROCESS_NONE = 0;

        public const int CLIPPING_PROCESS_OVERWRITE_ALPHA = 1;

        public const int CLIPPING_PROCESS_MULTIPLY_ALPHA = 2;

        public const int CLIPPING_PROCESS_DRAW = 3;

        public const int CLIPPING_PROCESS_CLEAR_ALPHA = 4;

        public int clippingProcess;


        public DrawParam()
        {
        }

        public virtual void releaseDrawParam_testImpl()
        {
        }

        public virtual void setupDraw()
        {
        }

        public abstract void drawTexture(int textureNo, int vertexCount, ushort[] indexArray, float[] vertexArray, float[] uvArray, float opacity, int colorCompositionType);

        public abstract void drawLast();

        public virtual int generateModelTextureNo()
        {
            return -1;
        }

        public virtual void releaseModelTextureNo(int no)
        {
        }

        public void setBaseColor(float alpha, float red, float green, float blue)
        {
        }

        public virtual void setCurrentDDID(DrawDataID id)
        {
        }

        public void setLockCulling(bool b)
        {
        }

        public void setCulling(bool b)
        {
        }

        public bool getCulling()
        {
            return false;
        }

        public void setMatrix(float[] m)
        {
        }

        public float[] getMatrix()
        {
            return new float[] { };
        }

        public void setAnisotropy(int n)
        {
        }

        public int getAnisotropy()
        {
            return -1;
        }

        public void setTextureColor(int textureNo, float r, float g, float b, float scale)
        {
        }

        protected void createTextureInfo(int no)
        {
        }
    }
}
