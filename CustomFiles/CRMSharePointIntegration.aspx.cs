using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomFiles
{
    public partial class CRMSharePointIntegration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

            ClientCredentials credentials = new ClientCredentials();
            credentials.UserName.UserName = "admin@gtuaed365.onmicrosoft.com";
            credentials.UserName.Password = "gtuae$2018";
            Uri OrganizationUri = new Uri("https://gtuat.api.crm4.dynamics.com/XRMServices/2011/Organization.svc");
            Uri HomeRealUri = null;
            using (OrganizationServiceProxy serviceProxy = new OrganizationServiceProxy(OrganizationUri, HomeRealUri, credentials, null))
            {
                IOrganizationService service = (IOrganizationService)serviceProxy;

                Guid EmailGuid = new Guid("7091C1F0-CEC0-E811-A95B-000D3AB490F3");
                QueryExpression q1 = new QueryExpression();
                q1.EntityName = "activitymimeattachment";
                q1.ColumnSet = new ColumnSet { AllColumns = true };
                FilterExpression fe = new FilterExpression();
                fe.AddCondition(new ConditionExpression("objectid", ConditionOperator.Equal, EmailGuid));
                q1.Criteria = fe;
                EntityCollection ec = service.RetrieveMultiple(q1);
                if(ec.Entities.Count>0)
                {
                    foreach(Entity c in ec.Entities)
                    {

                    }
                }
            }
        }
    }
}