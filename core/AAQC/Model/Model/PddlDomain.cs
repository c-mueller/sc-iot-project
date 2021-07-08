using System.IO;

namespace Model.Model
{
    public static class PddlDomain
    {
        public static readonly string Domain = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "../Core/PddlAssets/aaqc-domain.pddl"));
    }
}