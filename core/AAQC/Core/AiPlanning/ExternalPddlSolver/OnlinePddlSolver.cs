using Flurl;
using Flurl.Http;
using Model.Interfaces;
using Model.Model;
using Serilog;

namespace Core.AiPlanning.ExternalPddlSolver
{
    public class OnlinePddlSolver : IExternalPddlSolver
    {
        private const string Url = "http://solver.planning.domains";
        private const string SolverApiPath = "solve-and-validate";

        public string CreatePlanForProblem(PddlProblem problem)
        {
            Log.Information("[AI Planner] Creating plan for PDDL Problem");
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