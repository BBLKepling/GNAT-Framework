using System.Collections.Generic;
using Verse;

namespace GNATFramework
{
    [StaticConstructorOnStartup]
    public class ModFeatureDef : Def
    {
        public List<string> features;
    }
}
