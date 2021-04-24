using Doorfail.DataConnection;

namespace Doorfail.Connections.BL
{
    public class Test : CrudModel_Sql<Test>
    {
        public string getT() => (string)base.getProperty("t");
    }
}