using System.Collections.Generic;
using Model.Model;

namespace Model.Interfaces
{
    public interface IExternalPddlSolver
    {
        public IEnumerable<PddlPlanStep> CreatePlanForProblem(PddlProblem problem);
    }
}