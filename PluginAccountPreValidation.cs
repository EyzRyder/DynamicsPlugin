using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace DynamicsPlugin
{
    public class PluginAccountPreValidation : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

                IOrganizationService serviceAdmin = serviceFactory.CreateOrganizationService(null);

                ITracingService trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

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
            catch (InvalidPluginExecutionException ex)
            {
                throw new InvalidPluginExecutionException("Error ocorrido: " + ex.Message);
            }
        }
    }
}