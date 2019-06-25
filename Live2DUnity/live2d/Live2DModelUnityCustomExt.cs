using System;
using System.IO;
using UnityEngine;

namespace live2d
{
    public class Live2DModelUnityCustomExt : ALive2DModel
    {
        public Live2DModelUnityCustomExt()
        {
        }

        public override void deleteTextures()
        {
        }

        public void releaseModel()
        {
        }

        public void setMatrix(Matrix4x4 mat)
        {
        }

        public void setMatrix(float[] mat)
        {
        }

        public override void draw()
        {
        }

        public void setTexture(int textureNo, Texture2D texture)
        {
        }

        public static Live2DModelUnityCustom loadModel(string filepath)
        {
            return new Live2DModelUnityCustom();
        }

        public static Live2DModelUnityCustom loadModel(Stream bin)
        {
            return new Live2DModelUnityCustom();
        }

        public static Live2DModelUnityCustom loadModel(byte[] bin)
        {
            return new Live2DModelUnityCustom();
        }

        public override DrawParam getDrawParam()
        {
            return null;
        }

        public void enableCullingSetting(bool b)
        {
        }

        public void setCulling(bool b)
        {
        }

        public void setTextureColor(int textureNo, float r, float g, float b)
        {
        }

        public void setConvertTexture(int colorGroupNo, Texture2D texture)
        {
        }

        public void setNetPatternTexture(int level, Texture2D texture)
        {
        }

        public void setNetPatternLevel(int level)
        {
        }

        public bool existsNetPattern(int level)
        {
            return false;
        }
    }
}
