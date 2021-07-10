using Model.Model;

namespace Model.Interfaces
{
    public interface IExternalPddlSolver
    {
        public string CreatePlanForProblem(PddlProblem problem);
    }
}