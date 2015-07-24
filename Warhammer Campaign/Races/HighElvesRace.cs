using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Misc;
using System.Xml.Linq;

namespace Races
{
    class HighElvesRace : Race
    {
        public HighElvesRace()
            : base()
        {
            RaceType = "High Elves";
            FolderName = "HighElves";
            //Resources.Add(new Resource("Gold", 300));
            //Resources.Add(new Resource("Magic", 200));

            ConfigDocPath = @"Races\HighElves\config.xml";
            ConfigDoc = XDocument.Load(ConfigDocPath);

            base.LoadResources();
        }
    }
}
