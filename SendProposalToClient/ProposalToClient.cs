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
using System.Linq;
using System;

namespace SendProposalToClient
{
    public class ProposalToClient:CodeActivity
    {
         [Input("SourceEmail")]

        [ReferenceTarget("email")]

        public InArgument<EntityReference> SourceEmail { get; set; }
        protected override void Execute(CodeActivityContext executionContext)

        {

            // Get workflow context

            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();

            //Create service factory

            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();

            // Create Organization service

            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            // Get the target entity from the context

            Entity Proposal = (Entity)service.Retrieve("quote", context.PrimaryEntityId, new ColumnSet(new string[] { "quoteid" }));

            AddAttachmentToEmailRecord(service, Proposal.Id, SourceEmail.Get<EntityReference>(executionContext));

        }

        private void AddAttachmentToEmailRecord(IOrganizationService service, System.Guid SourceQuoteID, EntityReference SourceEmailID)

        {

            try
            {
                //create email object

                Entity _ResultEntity = service.Retrieve("email", SourceEmailID.Id, new ColumnSet(true));

                QueryExpression _QueryNotes = new QueryExpression("annotation");

                _QueryNotes.ColumnSet = new ColumnSet(new string[] { "subject", "mimetype", "filename", "documentbody" });

                _QueryNotes.Criteria = new FilterExpression();

                _QueryNotes.Criteria.FilterOperator = LogicalOperator.And;

                _QueryNotes.Criteria.AddCondition(new ConditionExpression("objectid", ConditionOperator.Equal, SourceQuoteID));

                EntityCollection _MimeCollection = service.RetrieveMultiple(_QueryNotes);

                if (_MimeCollection.Entities.Count > 0)

                {  //we need to fetch first attachment

                    Entity _NotesAttachment = _MimeCollection.Entities.First();

                    //Create email attachment

                    Entity _EmailAttachment = new Entity("activitymimeattachment");

                    if (_NotesAttachment.Contains("subject"))

                        _EmailAttachment["subject"] = _NotesAttachment.GetAttributeValue<string>("subject");

                    _EmailAttachment["objectid"] = new EntityReference("email", _ResultEntity.Id);

                    _EmailAttachment["objecttypecode"] = "email";

                    if (_NotesAttachment.Contains("filename"))

                        _EmailAttachment["filename"] = _NotesAttachment.GetAttributeValue<string>("filename");

                    if (_NotesAttachment.Contains("documentbody"))

                        _EmailAttachment["body"] = _NotesAttachment.GetAttributeValue<string>("documentbody");

                    if (_NotesAttachment.Contains("mimetype"))

                        _EmailAttachment["mimetype"] = _NotesAttachment.GetAttributeValue<string>("mimetype");

                    service.Create(_EmailAttachment);

                }

                // Sending email

                SendEmailRequest SendEmail = new SendEmailRequest();

                SendEmail.EmailId = _ResultEntity.Id;

                SendEmail.TrackingToken = "";

                SendEmail.IssueSend = true;

                SendEmailResponse res = (SendEmailResponse)service.Execute(SendEmail);
            }
            catch (InvalidPluginExecutionException ex)
            {
                if (ex != null)
                {

                    throw new InvalidPluginExecutionException("1) Unable to Send Email To Customer." + ex.Message + "Contact Your Administrator");
                }
                else
                    throw new InvalidPluginExecutionException("2) Unable to Send Email To Customer." + ex.Message + "Contact Administrator");
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("3) Unable to Send Email To Customer." + ex.Message + "Contact Your Administrator.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException("4) Unable to Send Email To Customer." + ex.Message + "Contact Your Administrator.", ex);
            }
        }
    }
}
