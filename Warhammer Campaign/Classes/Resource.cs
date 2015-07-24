using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Misc
{
    public class Resource
    {
        private String _name;
        private int _val;

        public Resource(String name, int val)
        {
            _name = name;
            _val = val;
        }

        public String Name
        {
            get {return _name;}
        }

        public int Value
        {
            get { return _val; }
            set { _val = value; }
        }

        public void Add(int i)
        {
            _val += i;
        }

        public void Subtract(int i)
        {
            if (_val - i < 0)
            {
                MessageBox.Show("Can't reduce by " + i.ToString() + ", value is currently " + _val.ToString() + ". No work done.");
            }
            else
            {
                _val -= i;
            }
        }
    }
}
