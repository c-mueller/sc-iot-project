using System.Runtime.Serialization;

namespace Model.Model
{
    [DataContract]
    public class PddlPlanStep
    {
        [DataMember(Name = "action")]
        public string Action { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}