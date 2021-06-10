using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Windows.Forms;
using System.Windows;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using Plan_n_Check;

namespace Plan_n_Check
{
    public class Constraint
    {

        public string Type  // V or D 
        { get; set; }
        public string Subscript  //mean, max 95, 50... etc
        { get; set; }

        public string EqualityType  //geq, leq
        { get; set; }

        public double Value  //Dose amount or volume amount
        { get; set; }

        public string Format //Relative or Absolute
        { get; set; }
        public string Status  //is constraint on or off for checking
        { get; set; }
        public int Priority
        { get; set; }
        public List<int> PriorityRange //minimum and maximum priority that the constraint can set
        { get; set; }
        public Constraint(string type, string subscript, string equalityType, double value, string format, int priority, List<int> priorityRange, string status = "ON")
        {
            Type = type;
            Subscript = subscript;
            EqualityType = equalityType;
            Value = value;
            Format = format;
            Priority = priority;
            PriorityRange = priorityRange;
            Status = "ON";

        }

    }
}
