using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace DynamicsPlugin
{
    public class PluginAccountPostOperation : IPlugin
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

                    if (!entidadeContext.Attributes.Contains("telephone1"))
                    {
                        throw new InvalidPluginExecutionException("Campo Telefone principal e obrigarotio!");
                    }

                    var Task = new Entity("task");

                    Task.Attributes["ownerid"] = new EntityReference("systemuser", context.UserId);
                    Task.Attributes["regardingobjectid"] = new EntityReference("account", context.PrimaryEntityId);
                    Task.Attributes["subject"] = "Visite nosso site: " + entidadeContext["websiteurl"];
                    Task.Attributes["description"] = "TASK criada via Plugin Post Operation";

                    serviceAdmin.Create(Task);
                }
            }
            catch (InvalidPluginExecutionException ex)
            {
                throw new InvalidPluginExecutionException("Error ocorrido: " + ex.Message);
            }
        }
    }
}