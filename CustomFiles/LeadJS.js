function showConfirmDialog() {
    var type = Xrm.Page.ui.getFormType();
    if (type == 1)
    {
        //Xrm.Page.data.entity.save();
        Xrm.Utility.confirmDialog("Do you want to send an automatic lead creation email?",
     function () {
         var workflowId = "E64B2837-D32A-4F5F-B2C1-080931F2F971";
         var clientURL = Xrm.Page.context.getClientUrl();
         var leadId = Xrm.Page.data.entity.getId().replace('{', ").replace('}', ");

         var requestUri = clientUrl + "/api/data/v9.0/workflows(" + workflowId + ")/Microsoft.Dynamics.CRM.ExecuteWorkflow";

         var xhr = new XMLHttpRequest();
         xhr.open("POST", requestUri, true);
         xhr.setRequestHeader("Accept", "application/json");
         xhr.setRequestHeader("Content-Type", "application/json; charset=utf-8");
         xhr.setRequestHeader("OData-MaxVersion", "4.0");
         xhr.setRequestHeader("OData-Version", "4.0");
         xhr.onreadystatechange = function () {
             if (this.readyState == 4) {
                 xhr.onreadystatechange = null;
                 if (this.status == 200) {
                     var result = JSON.parse(this.response);
                 } else {
                     var error = JSON.parse(this.response).error;
                 }
             }
         };
         xhr.send("{\"EntityId\":\"" + leadId + "\"}");
     },
     function () {
         Xrm.Page.getAttribute("description").setValue("No");
     });
    }
}

function CallWorkflow() {
   
}