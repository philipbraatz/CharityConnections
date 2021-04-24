
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doorfail.Connections.WebUI.Model
{
    public class MemberProfile<TMember, TEntry>
        where TMember : DataConnection.CrudModel_Json<TEntry>
        where TEntry : class
    {


        public MemberProfile()
        {

        }

    }
}