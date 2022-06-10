using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace CustomAircraftTemplate
{
    class DashVertGauge : DashGauge
    {

        public MeasurementManager measures;
        public FlightInfo info;

        protected override float GetMeteredValue()
        {

            ////Debug//DebugFlightLogger.Log(measures.ConvertedVerticalSpeed(info.verticalSpeed).ToString());
            return measures.ConvertedVerticalSpeed(info.verticalSpeed) / 4000f;
        }



    }
}