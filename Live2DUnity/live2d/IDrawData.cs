namespace live2d
{
    public interface IDrawData : ISerializableV2
    {
        IDrawContext init(ModelContext mdc);

        void setupInterpolate(ModelContext mdc, IDrawContext cdata);

        void setupTransform(ModelContext mdc, IDrawContext cdata);

        DrawDataID getDrawDataID();

        void setDrawDataID(DrawDataID id);

        float getOpacity(ModelContext mdc, IDrawContext cdata);

        int getDrawOrder(ModelContext mdc, IDrawContext cdata);

        void setDrawOrder(int[] orders);

        BaseDataID getTargetBaseDataID();

        void setTargetBaseDataID(BaseDataID id);

        bool needTransform();

        void draw(DrawParam dp, ModelContext mdc, IDrawContext cdata);

        void preDraw(DrawParam dp, ModelContext mdc, IDrawContext cdata);

        int getType();

        void setZ_TestImpl(ModelContext mdc, IDrawContext _cdata, float z);
    }
}
