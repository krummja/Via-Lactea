using System;
using System.Collections.Generic;

namespace GalaxyLib
{
    public abstract class GalaxySpec
    {
        protected internal abstract IEnumerable<Star> Generate(Random random);
    }
}
