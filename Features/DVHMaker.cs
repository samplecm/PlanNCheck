using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plan_n_Check.Plans;
using Plan_n_Check.Calculate;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Axes;

namespace Plan_n_Check.Features
{
    public static class DVHMaker
    {
        public static void DVHPlot(Structure structure, ScriptContext scriptContext)
        {
            var model = new OxyPlot.PlotModel { Title = $"DVH Plot" };
        }

        public static DVHData CalculateDVH(PlanSetup plan, Structure structure)
        {
            return plan.GetDVHCumulativeData(structure, DoseValuePresentation.Relative, VolumePresentation.Relative, 0.01);
        }
        public static OxyPlot.Series.Series CreateDVHSeries(DVHData dvh)
        {
            var series = new LineSeries();
            var points = CreateDataPoints(dvh);
            series.Points.AddRange(points);
            return series;
        }

        public static List<DataPoint> CreateDataPoints(DVHData dvh)
        {
            var points = new List<DataPoint>();
            foreach (var dvhPoint in dvh.CurveData)
            {
                var point = CreateDataPoint(dvhPoint);
                points.Add(point);
            }
            return points;
        }
        public static DataPoint CreateDataPoint(DVHPoint dvhPoint)
        {
            return new DataPoint(dvhPoint.DoseValue.Dose, dvhPoint.Volume);
        }

    }
}
