using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;

namespace DynamicsPlugin
{
    public class WFValidaLimiteInscricoesAluno : CodeActivity
    {
        #region Parameters
        // recebe o usurário do contexto
        [Input("Usuario")]
        [ReferenceTarget("systemuser")]
        public InArgument<EntityReference> usuarioEntrada { get; set; }

        // recebe o context
        [Input("AlunoXCursoDisponivel")]
        [ReferenceTarget("curso_alunoxcursodisponivel")]
        public InArgument<EntityReference> RegistroContext { get; set; }

        [Output("Saida")]
        public OutArgument<string> saida { get; set; }
        #endregion
        protected override void Execute(CodeActivityContext executionContext)
        {
            //Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService trace = executionContext.GetExtension<ITracingService>();

            trace.Trace("curso_alunoxcursodisponivel " + context.PrimaryEntityId);

            Guid guidAlunoXCurso = context.PrimaryEntityId;
            trace.Trace("guidAlunoXCurso" + guidAlunoXCurso);

            String fetchAlunoXCursos = "<fetch distinct='false' mapping='logical' output-format='xml-platform' version='1.0'>";
            fetchAlunoXCursos += "<entity name='curso_alunoxcursodisponivel'>";
            fetchAlunoXCursos += "<attribute name='curso_alunoxcursodisponivelid'/>";
            fetchAlunoXCursos += "<attribute name='curso_name'/>";
            fetchAlunoXCursos += "<attribute name='curso_aluno'/>";
            fetchAlunoXCursos += "<attribute name='createdon'/>";
            fetchAlunoXCursos += "<order descending='curso_alunoxcursodisponivelid' attribute='curso_name'/>";
            fetchAlunoXCursos += "<filter type='and'>";
            fetchAlunoXCursos += "<condition  attribute='curso_alunoxcursodisponivelid' value='"+guidAlunoXCurso+ "' uitype='curso_alunoxcursodisponivel'/>";
            fetchAlunoXCursos += "</filter>";
            fetchAlunoXCursos += "</entity>";
            fetchAlunoXCursos += "</fetch>";

            trace.Trace("fetchAlunoXCursos: " + fetchAlunoXCursos);

            var entityAlunoXCursos = service.RetrieveMultiple(new FetchExpression(fetchAlunoXCursos));
            trace.Trace("entityAlunoXCursos: " + entityAlunoXCursos);

            Guid guidAluno = Guid.Empty;

            foreach(var item in entityAlunoXCursos.Entities)
            {
                string nomeCurso = item.Attributes["curso_name"].ToString();
                trace.Trace("nomeCurso: "+ nomeCurso);

                var entityAluno = ((EntityReference)item.Attributes["curso_aluno"]).Id;
                guidAluno=((EntityReference)item.Attributes["curso_aluno"]).Id;
                trace.Trace("entityAluno: "+ entityAluno);
            }

            String fetchAlunoXCursosQtde = "<fetch distinct='false' mapping='logical' output-format='xml-platform' version='1.0'>";
            fetchAlunoXCursosQtde += "<entity name='curso_alunoxcursodisponivel'>";
            fetchAlunoXCursosQtde += "<attribute name='curso_alunoxcursodisponivelid'/>";
            fetchAlunoXCursosQtde += "<attribute name='curso_name'/>";
            fetchAlunoXCursosQtde += "<attribute name='curso_aluno'/>";
            fetchAlunoXCursosQtde += "<attribute name='createdon'/>";
            fetchAlunoXCursosQtde += "<order descending='false' attribute='curso_name'/>";
            fetchAlunoXCursosQtde += "<filter type='and'>";
            fetchAlunoXCursosQtde += "<condition  attribute='curso_aluno' value='" + guidAluno + "' uitype='curso_alunoxcursodisponivel'/>";
            fetchAlunoXCursosQtde += "</filter>";
            fetchAlunoXCursosQtde += "</entity>";
            fetchAlunoXCursosQtde += "</fetch>";

            trace.Trace("fetchAlunoXCursosQtde: " + fetchAlunoXCursosQtde);
            var entityAlunoXCursosQtde = service.RetrieveMultiple(new FetchExpression(fetchAlunoXCursosQtde));
            trace.Trace("fetchAlunoXCursosQtde: " + entityAlunoXCursosQtde.Entities.Count);
            if(entityAlunoXCursosQtde.Entities.Count>2){
                saida.Set(executionContext, "Aluno excedeu limite de cursos ativos!");
                trace.Trace("Aluno excedeu limite de cursos ativos!");
            throw new NotImplementedException("Aluno excedeu limite de cursos ativos!");
            }
            throw new NotImplementedException();
        }
    }
}