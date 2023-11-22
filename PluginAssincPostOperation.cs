using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace DynamicsPlugin
{
    public class PluginAssincPostOperation : IPlugin
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

                    for (int i = 0; i < 10; i++)
                    {
                        var Contact = new Entity("contact");

                        Contact.Attributes["firstname"] = "Contato Assinc vinculado a Conta";
                        Contact.Attributes["lastname"] = entidadeContext["name"];
                        Contact.Attributes["parentcustomerid"] = new EntityReference("account", context.PrimaryEntityId);
                        Contact.Attributes["ownerid"] = new EntityReference("systemuser", context.UserId);

                        trace.Trace("firstname: " + Contact.Attributes["firstname"]);

                        serviceAdmin.Create(Contact);
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