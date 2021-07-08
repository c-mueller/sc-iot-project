using System.Threading.Tasks;

namespace Core.Model
{
    public interface IExternalPddlSolver
    {
        public string CreatePlanForProblem(PddlProblem problem);
    }
}