using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace live2d
{
    public class BReader
    {
        public BReader(Stream stream)
        {
        }

        public BReader(Stream stream, int version)
        {
        }

        public int getFormatVersion()
        {
            return -1;
        }

        public void setFormatVersion(int version)
        {
        }

        public int readNum()
        {
            return -1;
        }

        public static int bytesToNum(BinaryReader bin)
        {
            return -1;
        }


        public double readDouble()
        {
            return -1;
        }

        public float readFloat()
        {
            return -1;

        }

        public int readInt()
        {
            return -1;

        }

        public byte readByte()
        {
            return 0x00;
        }

        public short readShort()
        {
            return -1;
        }

        public long readLong()
        {
            return -1;
        }

        public bool readBoolean()
        {
            return false;
        }

        public string readUTF8()
        {
            return "";
        }

        public int[] readArrayInt()
        {
            return new int[] { };
        }

        public float[] readArrayFloat()
        {
            return new float[] { };
        }

        public double[] readArrayDouble()
        {
            return new double[] { };
        }

        public object readObject()
        {
            return new object();
        }

        public object readObject_exe1(int cno)
        {
            return new object();
        }

        public object readObject_exe2(int cno)
        {
            return new object();
        }

        public bool readBit()
        {
            return false;
        }
    }
}
