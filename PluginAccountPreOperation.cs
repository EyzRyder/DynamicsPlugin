using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DynamicsPlugin
{
    public class PluginAccountPreOperation : IPlugin
    {
        public void Execute(IServiceProvider serviceProvidor)
        {
            try
            {
                IPluginExecutionContext context = (IPluginExecutionContext)serviceProvidor.GetService(typeof(IPluginExecutionContext));

                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvidor.GetService(typeof(IOrganizationServiceFactory));

                IOrganizationService serviceAdmin = serviceFactory.CreateOrganizationService(null);

                ITracingService trace = (ITracingService)serviceProvidor.GetService(typeof(ITracingService));

                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    Entity entidadeContext = (Entity)context.InputParameters["Target"];

                    if (entidadeContext.LogicalName == "account")
                    {
                        if (entidadeContext.Attributes.Contains("telephone1"))
                        {
                            var phone1 = entidadeContext["telephone1"];
                            string FetchContact = @"<?xml version='1.0'?>" +
                            "<fetch distinct='false' mapping='logical' output-format='xml-platform' version='1.0'>" +
                            "<entity name='contact'>" +
                            "<attribute name='fullname'>" +
                            "<attribute name='telephone1'>" +
                            "<attribute name='contactid'>" +
                            "<order descending='false' attribute='fullname' />" +
                            "<filter type='and'>" +
                            "<condition attribute='telephone1' value='" + phone1 + "' operator='eq'/>" +
                            "</filter>" +
                            "</entity>" +
                            "</fetch>";

                            trace.Trace("FetchContact: " + FetchContact);

                            var primarycontact = serviceAdmin.RetrieveMultiple(new FetchExpression(FetchContact));

                            if (primarycontact.Entities.Count > 0)
                            {
                                foreach (var entityContact in primarycontact.Entities)
                                {
                                    entidadeContext["primarycontactid"] = new EntityReference("contact", entityContact.Id);
                                }
                            }
                        }
                    }
                }
            }
            catch (InvalidPluginExecutionException ex)
            {
                throw new InvalidPluginExecutionException("Error ocorrido: " + ex.Message);
            }
        }
    }
}