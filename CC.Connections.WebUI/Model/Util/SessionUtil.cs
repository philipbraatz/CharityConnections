using Doorfail.Connections.BL;
using System.Web;
using System.Web.Mvc;

namespace Doorfail.Connections.WebUI.Model
{
    public static class SessionUtil
    {
        //static HttpSessionStateBase Session { get; set; }

        public static Password GetMember(HttpSessionStateBase Session)
        {
            return (Password)Session["Member"];
        }
        public static MemberType GetMemberType(HttpSessionStateBase Session)
        {
            if (Session != null && Session["member"] != null)
                return GetMember(Session).MemberType;
            else return MemberType.GUEST;
        }

        public static TEntity[] GetList<TEntity>(HttpSessionStateBase Session,string name) where TEntity : class
        {
            TEntity[] allCharities;//CharityCollection.INSTANCE;
            if (Session != null && Session[name] != null)
                allCharities =(TEntity[])Session[name];
            else {
                allCharities = apiHelper.getAll<TEntity>();
            }
            return allCharities;
        }


    }
}