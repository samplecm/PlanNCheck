using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using VMS.TPS;

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
            this.HasSubsegments = false;
            this.IsPTV = false;
            this.PTVDose = null;
            this.Weight = 0;
            this.MatchingStructures = null;
            this.OptimizationStructures = null; 
        }


        public bool Critical { get; set; }
        public string Name { get; set; }
        public List<Constraint> Constraints { get; set; }
        public string Type { get; set; }
        public bool HasSubsegments { get; set; }
        public bool IsPTV { get; set; }
        public int? PTVDose { get; set; }
        public double Score { get; set; }
        public int Weight { get; set; }
        public List<Structure> MatchingStructures { get; set; }

        public List<Structure> OptimizationStructures { get; set; }

    }
}
