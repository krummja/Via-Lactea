using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;


namespace GalaxyLib
{
    public class Galaxy
    {
        public IEnumerable<Star> Stars { get; private set; }

        public bool Generated { get; private set; }

        public Galaxy(IEnumerable<Star> stars)
        {
            Stars = stars;
            Generated = true;
        }

        public static async Task<Galaxy> Generate(GalaxySpec spec, Random rand)
        {
            IEnumerable<Star> stars = await Task
                .Factory
                .StartNew(() => spec.Generate(rand));

            return new Galaxy(stars);
        }
    }
}
