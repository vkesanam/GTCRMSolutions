using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using System.ServiceModel;
using Microsoft.Crm.Sdk.Messages;
using Xrm;

namespace GenerateQuote
{
    public class Proposal:CodeActivity
    {
        [Input("Opportunity")]
        [RequiredArgument]
        [ReferenceTarget("opportunity")]
        public InArgument<EntityReference> Opportunity { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            IWorkflowContext workflowContext = (IWorkflowContext)context.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)context.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = (IOrganizationService)serviceFactory.CreateOrganizationService(workflowContext.UserId);

            Guid OpportunityGUID = Opportunity.Get<EntityReference>(context).Id;
            GenerateQuotationFromOpportunity(OpportunityGUID, service, context);
        }

        private void GenerateQuotationFromOpportunity(Guid opportunityGUID, IOrganizationService service, CodeActivityContext context)
        {
            try
            {
                var genQuoteFromOppRequest = new GenerateQuoteFromOpportunityRequest
                {
                    OpportunityId = opportunityGUID,
                    ColumnSet = new ColumnSet("quoteid", "name")
                };

                var genQuoteFromOppResponse = (GenerateQuoteFromOpportunityResponse)
                    service.Execute(genQuoteFromOppRequest);

                Quote quote = genQuoteFromOppResponse.Entity.ToEntity<Quote>();
                Guid _quoteId = quote.Id;
            }
            catch (InvalidPluginExecutionException ex)
            {
                if (ex != null)
                {

                    throw new InvalidPluginExecutionException("1) Unable to Create Proposal." + ex.Message + "Contact Your Administrator");
                }
                else
                    throw new InvalidPluginExecutionException("2) Unable to Create Proposal." + ex.Message + "Contact Administrator");
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("3) Unable to Send Create Proposal." + ex.Message + "Contact Your Administrator.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException("4) Unable to Send Create Proposal." + ex.Message + "Contact Your Administrator.", ex);
            }
        }
    }
}
