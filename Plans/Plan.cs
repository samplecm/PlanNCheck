﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_n_Check.Plans
{
    public abstract class Plan
    {
        //Plans must have a name, and a list of structures, which will each have a list of constraints.
        public abstract string Name
        { get; set; }
        public abstract List<ROI> ROIs
        { get; set;}
        public abstract double PrescriptionDose
        { get; set; }
        public abstract List<int> PTV_Types
        { get; set; }
        public abstract int Fractions
        { get; set; }
    }
}
