using System.Collections.Generic;

namespace Core.Model
{
    public class PddlProblem
    {
        public string ProblemName { get; set; }
        public string DomainName = "aaqc";
        public string ObjectsString =
            "(:objects\n" +
            "    v - ventilation\n" +
            "    h - heater\n" +
            "    ac - air-conditioner\n" +
            "    ap - air-purifier\n" +
            "    ti - temperature-in\n" +
            "    to - temperature-out\n" +
            "    ho - humidity-out\n" +
            "    ai - air-purity-in\n" +
            "    ao - air-purity-out\n" +
            "    ci - co2-level-in\n" +
            ")";
        public Dictionary<string, string> Objects { get; set; }
        public List<PddlPredicateObjectPair> InitStates { get; set; }
        public List<PddlPredicateObjectPair> Goals { get; set; }

        public PddlProblem()
        {
            Objects.Add("v", "ventilation");
            Objects.Add("h", "heater");
            Objects.Add("ac", "air-conditioner");
            Objects.Add("ap", "air-purifier");
            Objects.Add("ti", "temperature-in");
            Objects.Add("to", "temperature-out");
            Objects.Add("ho", "humidity-out");
            Objects.Add("ai", "air-purity-in");
            Objects.Add("ao", "air-purity-out");
            Objects.Add("ci", "co2-level-in");
        }

        public void AddInitState(PddlPredicateObjectPair state)
        {
            InitStates.Add(state);
        }

        public void AddGoal(PddlPredicateObjectPair goal)
        {
            Goals.Add(goal);
        }
    }
    
    public class PddlPredicateObjectPair
    {
        private string Predicate { get; set; }
        private string ObjectName { get; set; }
    }
}