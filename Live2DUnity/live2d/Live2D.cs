namespace live2d
{
    public class Live2D
    {
        public enum DrawMethodVersion
        {
            FORCE_2_0,
            FORCE_2_1,
            DEFAULT_2_0,
            DEFAULT_2_1
        }

        public const string __L2D_VERSION_STR__ = "2.1.00_beta_11";

        public const int __L2D_VERSION_NO__ = 201000000;

        public const int L2D_RENDER_DRAW_MESH_NOW = 0;

        public const int L2D_RENDER_DRAW_MESH = 1;

        public const int L2D_NO_ERROR = 0;

        public const int L2D_ERROR_LIVE2D_INIT_FAILED = 1000;

        public const int L2D_ERROR_FILE_LOAD_FAILED = 1001;

        public const int L2D_ERROR_MEMORY_ERROR = 1100;

        public const int L2D_ERROR_MODEL_DATA_VERSION_MISMATCH = 2000;

        public const int L2D_ERROR_MODEL_DATA_EOF_ERROR = 2001;

        public const int L2D_ERROR_MODEL_DATA_UNKNOWN_FORMAT = 2002;

        public const int L2D_ERROR_DDTEXTURE_SETUP_TRANSFORM_FAILED = 4000;

        public static readonly string __L2D_PLATFORM_STR__;

        public static bool TEST_____ABC;

        static Live2D()
        {
        }

        public static void init()
        {
        }

        public static void dispose()
        {
        }

        public static string getVersionStr()
        {
            return "2.1.00_beta_11";
        }

        public static int getVersionNo()
        {
            return 201000000;
        }

        public static int getError()
        {
            return -1;
        }

        public static void setDrawMethodVersion(DrawMethodVersion version)
        {
        }

        public static DrawMethodVersion getDrawMethodVersion()
        {
            return new DrawMethodVersion();
        }

        public static void setClippingMaskBufferSize(int size)
        {
        }

        public static int getClippingMaskBufferSize()
        {
            return -1;
        }
    }
}
