using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace DynamicsPlugin
{
    public class PluginAccountPreValidation : IPlugin
    {
        public void Execute(IServiceProvider serviceProvidor)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvidor.GetService(typeof(IPluginExecutionContext));

            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvidor.GetService(typeof(IOrganizationServiceFactory));

            IOrganizationService serviceAdmin = serviceFactory.CreateOrganizationService(null);

            ITracingService trace = (ITracingService)serviceProvidor.GetService(typeof(ITracingService));

            Entity entidadeContexto = null;

            if (context.InputParameters.Contains("Target"))
            {
                entidadeContexto = (Entity)context.InputParameters["Target"];
                trace.Trace("Entidade de Contexto: " + entidadeContexto.Attributes.Count);
                if (entidadeContexto == null)
                {
                    return;
                }
                if (!entidadeContexto.Contains("telephone1"))
                {
                    throw new InvalidPluginExecutionException("Campo Telefone Principal é obrigatório");
                }
            }
        }
    }
}