using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace DynamicsPlugin
{
    public class FirstAction : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            IOrganizationService serviceAdmin = serviceFactory.CreateOrganizationService(null);

            ITracingService trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            trace.Trace("Minha primeira Action executada com sucesso e criando Lead no Dataverse!");

            Entity entLead = new Entity("lead");
            entLead["subject"] = "Lead criado via Action";
            entLead["firstname"] = "Primeiro Nome";
            entLead["lastname"] = "Lastname Nome";
            entLead["mobilephone"] = "920231122";
            entLead["ownerid"] = new EntityReference("systemuser", context.UserId);

            Guid guidLead = serviceAdmin.Create(entLead);
            trace.Trace("Lead Criando: " + guidLead.ToString());
        }
    }
}