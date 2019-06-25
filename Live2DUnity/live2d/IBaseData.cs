namespace live2d
{
    public abstract class IBaseData : ISerializableV2
    {
        public const int BASE_INDEX_NOT_INIT = -2;

        public const int TYPE_BD_AFFINE = 1;

        public const int TYPE_BD_BOX_GRID = 2;

        public IBaseData()
        {
        }

        public virtual void readV2(BReader br)
        {
        }

        protected void readV2_opacity(BReader br)
        {
        }

        public abstract IBaseContext init(ModelContext mdc);

        public abstract void setupInterpolate(ModelContext mdc, IBaseContext _data);

        protected void interpolateOpacity(ModelContext mdc, PivotManager pivotMgr, IBaseContext _data, bool[] ret_paramOutside)
        {
        }

        public abstract void setupTransform(ModelContext mdc, IBaseContext _data);

        public abstract void transformPoints(ModelContext mdc, IBaseContext _data, float[] srcPoints, float[] dstPoints, int numPoint, int pt_offset, int pt_step);

        public abstract int getType();

        public void setTargetBaseDataID(BaseDataID id)
        {
        }

        public void setBaseDataID(BaseDataID id)
        {
        }

        public BaseDataID getTargetBaseDataID()
        {
            return new BaseDataID();
        }

        public BaseDataID getBaseDataID()
        {
            return new BaseDataID();
        }

        public bool needTransform()
        {
            return false;
        }
    }
}
