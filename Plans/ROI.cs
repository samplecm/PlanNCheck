using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_n_Check.Plans
{
    public class ROI
    {
        string name = "";
        List<Constraint> constraints = new List<Constraint>();
        public ROI()
        {
            this.Name = name;
            this.Constraints = constraints;
            this.Type = "";
            this.Critical = false;
        } 


        public bool Critical { get; set; }
        public string Name { get; set; }
        public List<Constraint> Constraints { get; set;}
        public string Type { get; set; }

    }
}
