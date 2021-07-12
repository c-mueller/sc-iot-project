using System.Collections.Generic;
using System.Runtime.Serialization;
using Flurl;
using Flurl.Http;
using Model.Interfaces;
using Model.Model;
using Newtonsoft.Json;
using Serilog;

namespace Core.AiPlanning.ExternalPddlSolver
{
    public class OnlinePddlSolver : IExternalPddlSolver
    {
        private const string Url = "http://solver.planning.domains";
        private const string SolverApiPath = "solve-and-validate";

        public List<PddlPlanStep> CreatePlanForProblem(PddlProblem problem)
        {
            Log.Information("[AI Planner] Creating plan for PDDL Problem");
            var response =  Url
                .AppendPathSegment(SolverApiPath)
                .PostJsonAsync(new
                {
                    domain = PddlDomain.Domain,
                    problem = problem.BuildProblemFile(),
                }).ReceiveJson<OnlineSolverResponse>().Result;
            
            // var responseString = Url
            //     .AppendPathSegment(SolverApiPath)
            //     .PostJsonAsync(new
            //     {
            //         domain = PddlDomain.Domain,
            //         problem = problem.BuildProblemFile(),
            //     }).ReceiveString().Result;
            // Log.Information(responseString);
            // var response = JsonConvert.DeserializeObject<OnlineSolverResponse>(responseString);

            if (response.Status.ToLower() == "ok")
            {
                return response.Result.Plan;
            }
            return response.Result.Output.Contains("TRUE") ? new List<PddlPlanStep>() : null;
        }
    }

    [DataContract]
    public class OnlineSolverResponse
    {
        [DataMember(Name = "status")] 
        public string Status { get; set; }
        [DataMember(Name = "result")] 
        public OnlineSolverResponseResult Result { get; set; }
    }

    public class OnlineSolverResponseResult
    {
        [DataMember(Name = "length")] 
        public int Length { get; set; }
        [DataMember(Name = "output")] 
        public string Output { get; set; }
        [DataMember(Name = "parse_status")] 
        public string ParseStatus { get; set; }
        [DataMember(Name = "plan")] 
        public List<PddlPlanStep> Plan { get; set; }
        [DataMember(Name = "type")] 
        public string Type { get; set; }
        [DataMember(Name = "cost")] 
        public int Cost { get; set; }
        [DataMember(Name = "val_stdout")] 
        public string ValStdout { get; set; }
        [DataMember(Name = "val_stderr")] 
        public string ValStderr { get; set; }
        [DataMember(Name = "val_status")] 
        public string ValStatus { get; set; }
        [DataMember(Name = "error")] 
        public string Error { get; set; }
        [DataMember(Name = "planPath")] 
        public string PlanPath { get; set; }
        [DataMember(Name = "logPath")] 
        public string LogPath { get; set; }
    }
}