using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Model
{
    public class PddlProblem
    {
        public string ProblemName { get; set; }
        private const string DomainName = "aaqc";
        
        private const string ObjectsString =
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

        private const string GoalsString =
            "(:goal (and\n" +
            "    (not(temperature-low ti))\n" +
            "    (not(temperature-high ti))\n" +
            "    (not(air-purity-bad ai))\n" +
            "    (not(co2-level-emergency ci))\n" +
            "    (not(on h))\n" +
            "    (not(on ac))\n" +
            "    (not(on ap))\n" +
            "    (not(on v))\n))";

        private readonly Dictionary<string, string> _objects = new Dictionary<string, string>();
        private readonly List<PddlPredicateObjectPair> _initStates = new List<PddlPredicateObjectPair>();
        private readonly List<PddlPredicateObjectPair> _goals = new List<PddlPredicateObjectPair>();

        public PddlProblem()
        {
            ProblemName = "aaqcProblem";
            _objects.Add("v", "ventilation");
            _objects.Add("h", "heater");
            _objects.Add("ac", "air-conditioner");
            _objects.Add("ap", "air-purifier");
            _objects.Add(Constants.TemperatureInObjectName, "temperature-in");
            _objects.Add(Constants.TemperatureOutObjectName, "temperature-out");
            _objects.Add(Constants.HumidityOutObjectName, "humidity-out");
            _objects.Add(Constants.AirPurityInObjectName, "air-purity-in");
            _objects.Add(Constants.AirPurityOutObjectName, "air-purity-out");
            _objects.Add(Constants.Co2LevelInObjectName, "co2-level-in");
        }

        public bool HasInitStates()
        {
            return _initStates.Any();
        }

        public string GetInitStatesAsString()
        {
            var sb = new StringBuilder();
            foreach (var initState in _initStates)
            {
                sb.AppendLine($"({initState.Predicate} {initState.ObjectName})");
            }

            return sb.ToString();
        }
        
        public void AddInitState(string predicate, string objectName)
        {
            _initStates.Add(new PddlPredicateObjectPair
            {
                Predicate = predicate,
                ObjectName = objectName,
            });
        }

        public void AddGoal(PddlPredicateObjectPair goal)
        {
            _goals.Add(goal);
        }

        public string BuildProblemFile()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"(define (problem {ProblemName}) (:domain {DomainName})");

            sb.AppendLine(ObjectsString);

            sb.AppendLine("(:init");
            foreach (var initState in _initStates)
            {
                sb.AppendLine($"    ({initState.Predicate} {initState.ObjectName})");
            }

            sb.AppendLine(")");

            // sb.AppendLine("(:goal (and");
            // foreach (var goal in Goals)
            // {
            //     sb.AppendLine($"    ({goal.Predicate} {goal.ObjectName})");
            // }
            // sb.AppendLine("))");
            sb.AppendLine(GoalsString);

            sb.AppendLine(")");
            return sb.ToString();
        }
    }

    public class PddlPredicateObjectPair
    {
        public string Predicate { get; set; }
        public string ObjectName { get; set; }
    }
}