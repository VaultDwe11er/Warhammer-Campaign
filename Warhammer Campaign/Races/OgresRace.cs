using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Misc;
using System.Xml.Linq;

namespace Races
{
    class OgresRace : Race
    {
        public OgresRace()
            : base()
        {
            RaceType = "Ogres";
            FolderName = "Ogres";
            //Resources.Add(new Resource("Food", 500));
            //Resources.Add(new Resource("Magic", 100));

            ConfigDocPath = @"Races\Ogres\config.xml";
            ConfigDoc = XDocument.Load(ConfigDocPath);

            base.LoadResources();
        }
    }
}
