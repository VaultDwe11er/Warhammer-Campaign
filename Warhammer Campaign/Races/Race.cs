using Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Races
{
    public class Race
    {
        //Static members
        public static String GetConfigDoc(String raceType)
        {
            switch (raceType)
            {
                case "Ogres":
                    return new OgresRace().ConfigDocPath;
                case "High Elves":
                    return new HighElvesRace().ConfigDocPath;
                default:
                    return "";
            }
        }

        //Instance members
        public Race()
        {
            RaceType = "None";
            Resources = new List<Resource>();
        }

        public String RaceType { get; set; }
        public List<Resource> Resources { get; set; }
        public XDocument ConfigDoc { get; set; }
        public String ConfigDocPath { get; set; }
        public String FolderName { get; set; }

        public int ResourceAmount(String resource)
        {
            Resource r = Resources.Single(val => val.Name == resource);

            if (r == null)
            {
                MessageBox.Show("Resource type " + resource + " not found");
            }
            else
            {
                return r.Value;
            }

            return -1;
        }

        public Resource GetResource(String resource)
        {
            Resource r = Resources.Single(val => val.Name == resource);

            if (r == null)
            {
                MessageBox.Show("Resource type " + resource + " not found");
            }

            return r;
        }

        public bool HasResourceType(String resource)
        {
            return Resources.Single(val => val.Name == resource) != null;
        }

        public void Save(XDocument xDoc, String xPath)
        {
            xDoc.XPathSelectElements(xPath).Nodes().Remove();

            foreach (var r in Resources)
            {
                xDoc.XPathSelectElement(xPath).Add(new XElement(r.Name, r.Value));
            }
        }

        public void Pay(String costType)
        {
            try
            {
                var attrs = ConfigDoc.XPathSelectElement("Config/Costs/" + costType).Attributes();

                foreach (var a in attrs)
                {
                    GetResource(a.Name.ToString()).Subtract(int.Parse(a.Value));
                }
            }
            catch (XPathException)
            {
                MessageBox.Show("Race " + RaceType + " does not have a cost type '" + costType + "' defined");
            }
        }
        public bool CanPay(String costType)
        {
            
            bool retVal = true;

            try
            {
                var attrs = ConfigDoc.XPathSelectElement("Config/Costs/" + costType).Attributes();

                foreach (var a in attrs)
                {
                    if (GetResource(a.Name.ToString()).Value < int.Parse(a.Value)) retVal = false;
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Race " + RaceType + " does not have a cost type '" + costType + "' defined");
                return false;
            }

            return retVal;
        }

        public String GetCost(String costType)
        {
            String retVal = "";

            try
            {
                var attrs = ConfigDoc.XPathSelectElement("Config/Costs/" + costType).Attributes();

                foreach (var a in attrs)
                {
                    retVal = retVal + a.Value + " " + a.Name.ToString() + ", ";
                }
                retVal = retVal.TrimEnd(',', ' ');
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Race " + RaceType + " does not have a cost type '" + costType + "' defined");
                return "";
            }

            return retVal;
        }

        protected void LoadResources()
        {
            var attrs = ConfigDoc.XPathSelectElements("Config/StartingResources").Attributes();

            foreach (var a in attrs)
            {
                Resources.Add(new Resource(a.Name.ToString(), int.Parse(a.Value)));
            }
        }
    }
}
