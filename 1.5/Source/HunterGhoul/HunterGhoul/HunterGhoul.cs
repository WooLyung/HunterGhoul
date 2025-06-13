using HarmonyLib;
using System.Reflection;
using Verse;

namespace HunterGhoul
{
    [StaticConstructorOnStartup]
    public class HunterGhoul
    {
        private static Harmony harmony;

        static HunterGhoul()
        {
            harmony = new Harmony("ng.lyu.hunterghoul");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}