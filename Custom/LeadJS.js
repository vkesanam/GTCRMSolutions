/// This function is used to retrieve the contact record
function retrieveContact() {
    try {
        var lookup = new Array();
        lookup = Xrm.Page.getAttribute("parentcontactid").getValue();
        if (lookup[0] != null) {
            var id = lookup[0].id;
            Xrm.WebApi.retrieveRecord("contact", id, "$select=firstname,lastname,gt_salutation,emailaddress1,fullname,telephone1,preferredcontactmethodcode,createdon,_parentcustomerid_value,creditlimit")
        .then(function (data) {
            retrieveContactSuccess(data);
        })
        .fail(function (error) {
            Xrm.Utility.alertDialog(error.message);
        });
        }
    }
    catch (e) {
        Xrm.Utility.alertDialog(e.message);
    }
}

///retrieve success
function retrieveContactSuccess(data) {

    try {
        //get the values 

        Xrm.Page.getAttribute("firstname").setValue(data["firstname"]);
        Xrm.Page.getAttribute("lastname").setValue(data["lastname"]);
        Xrm.Page.getAttribute("gt_salutation").setValue(data["gt_salutation"]);
        Xrm.Page.getAttribute("emailaddress1").setValue(data["emailaddress1"]);

        ////string
        //var fullname = data["fullname"];

        ////optionset
        //var typeCode = data["customertypecode"];

        ////lookup
        //var customerGuid = data["_parentcustomerid_value"];
        //var customerName = data["_parentcustomerid_value@OData.Community.Display.V1.FormattedValue"];
        //var customerEntityLogicalName = data["_parentcustomerid_value@Microsoft.Dynamics.CRM.lookuplogicalname"];

        ////money
        //var creditLimit = data["creditlimit@OData.Community.Display.V1.FormattedValue"];

        ////date
        //var createdonFormattedValue = data["createdon@OData.Community.Display.V1.FormattedValue"]; // gives date in following format 2017-09-30T21:10:19Z

        //var createdon = data["createdon"]; // gives date in following format 10/1/2017 2:40 AM

        ////optionset
        //var preferredConMethod = data["preferredcontactmethodcode@OData.Community.Display.V1.FormattedValue"];

    } catch (e) {
        Xrm.Utility.alertDialog(e.message);
    }
}




/// This function is used to retrieve the contact record
function retrieveAccount() {
    try {
        var lookup = new Array();
        lookup = Xrm.Page.getAttribute("parentaccountid").getValue();
        if (lookup[0] != null) {
            var id = lookup[0].id;
            Xrm.WebApi.retrieveRecord("account", id, "$select=_primarycontactid_value")
        .then(function (data) {
            retrieveAccountSuccess(data);
        })
        .fail(function (error) {
            Xrm.Utility.alertDialog(error.message);
        });
        }
    }
    catch (e) {
        Xrm.Utility.alertDialog(e.message);
    }
}

///retrieve success
function retrieveAccountSuccess(data) {

    try {
        //get the values 



        ////string
        //var fullname = data["fullname"];

        ////optionset
        //var typeCode = data["customertypecode"];

        //lookup
        var contactGuid = data["_primarycontactid_value"];
        var contactName = data["_primarycontactid_value@OData.Community.Display.V1.FormattedValue"];
        var contactEntityLogicalName = data["_primarycontactid_value@Microsoft.Dynamics.CRM.lookuplogicalname"];

        if (contactGuid != null) {
            var value = new Array();
            value[0] = new Object();
            value[0].id = contactGuid;
            value[0].name = contactName;
            value[0].entityType = contactEntityLogicalName;

            Xrm.Page.getAttribute("parentcontactid").setValue(value); //set the lookup value finally

            try {

                Xrm.WebApi.retrieveRecord("contact", contactGuid, "$select=firstname,lastname,gt_salutation,emailaddress1,fullname,telephone1,preferredcontactmethodcode,createdon,_parentcustomerid_value,creditlimit")
            .then(function (data) {
                retrieveContactSuccess(data);
            })
            .fail(function (error) {
                Xrm.Utility.alertDialog(error.message);
            });
            }

            catch (e) {
                Xrm.Utility.alertDialog(e.message);
            }
        }
        ////money
        //var creditLimit = data["creditlimit@OData.Community.Display.V1.FormattedValue"];

        ////date
        //var createdonFormattedValue = data["createdon@OData.Community.Display.V1.FormattedValue"]; // gives date in following format 2017-09-30T21:10:19Z

        //var createdon = data["createdon"]; // gives date in following format 10/1/2017 2:40 AM

        ////optionset
        //var preferredConMethod = data["preferredcontactmethodcode@OData.Community.Display.V1.FormattedValue"];

    } catch (e) {
        Xrm.Utility.alertDialog(e.message);
    }
}