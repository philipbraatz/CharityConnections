using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CC.Connections.BL;

namespace CC.Connections.API.Controllers
{
    public class HelpingActionController : ApiController
    {
        // GET: api/HelpingAction
        public AbsHelpingActionCollection Get()
        {
            AbsHelpingActionCollection HelpingActions = new AbsHelpingActionCollection();
            HelpingActions.LoadAll();
            return HelpingActions;
        }

        // GET: api/HelpingAction/5
        public AbsHelpingAction Get(Guid id)
        {
            AbsHelpingAction HelpingAction = new AbsHelpingAction { ID = id };
            HelpingAction.LoadId();
            return HelpingAction;
        }

        // POST: api/HelpingAction
        public void Post([FromBody]AbsHelpingAction HelpingAction)
        {
            HelpingAction.Insert();
        }

        // PUT: api/HelpingAction/5
        public void Put(Guid id, [FromBody]AbsHelpingAction HelpingAction)
        {
            HelpingAction.Update();
        }

        // DELETE: api/HelpingAction/5
        public void Delete(Guid id)
        {
            AbsHelpingAction HelpingAction = new AbsHelpingAction { ID = id };
            HelpingAction.Delete();
        }
    }
}
