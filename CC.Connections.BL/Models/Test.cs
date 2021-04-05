using CC.Abstract;

namespace CC.Connections.BL
{
    public class Test : BaseModel<PL.Test>
    {
        public string getT() => (string)base.getProperty("t");
    }

    public class TestCollection
    : BaseList<Test, PL.Test>
    {
    }
}