using Core.Model;
using Flurl;
using Flurl.Http;

namespace Core.AiPlanning.ExternalPddlSolver
{
    public class OnlinePddlSolver : IExternalPddlSolver
    {
        private const string Url = "http://solver.planning.domains";
        private const string SolverApiPath = "solve-and-validate";

        public string CreatePlanForProblem(PddlProblem problem)
        {
            return Url
                .AppendPathSegment(SolverApiPath)
                .PostJsonAsync(new
                {
                    domain = PddlDomain.Domain,
                    problem = problem.BuildProblemFile(),
                }).ReceiveString().Result;;
        }
    }
}