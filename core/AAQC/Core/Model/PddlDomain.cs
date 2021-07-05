using System.IO;

namespace Core.Model
{
    public static class PddlDomain
    {
        public static string Domain = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "../Core/PddlAssets/aaqc-domain.pddl"));
    }
}