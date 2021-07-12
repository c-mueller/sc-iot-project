using System.Collections.Generic;
using Model.Model;

namespace Model.Interfaces
{
    public interface IExternalPddlSolver
    {
        public List<PddlPlanStep> CreatePlanForProblem(PddlProblem problem);
    }
}