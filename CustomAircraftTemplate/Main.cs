using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomAircraftTemplate
{
    public class Main : IAircraftMod<Main>
    {
        public override void ModLoaded()
        {
            var aircraftInfo = new TemplateAircraftInfo();

            base.AircraftModLoaded(this, aircraftInfo);
        }
    }
}
