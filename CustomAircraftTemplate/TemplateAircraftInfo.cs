namespace CustomAircraftTemplate
{
    public class TemplateAircraftInfo : IAircraftInfo
    {
        public string HarmonyId => "Oku.Template.Aircraft";

        public float maxInternalFuel => 11500;

        public int AircraftMPIdentifier => 0xd06f00d;

        public string CustomAircraftPV => "T01Template.asset";

        public string AircraftName => "T-01";

        public string AircraftNickname => "Template";

        public string AircraftDescription => "The Template is a perfect demonstration of how to assemble an aircraft in an easy way.";

        public string PreviewPngFileName => "t-01_template.png";

        public string AircraftAssetbundleName => "t-01";

        public string AircraftPrefabName => "t01.prefab";

        public string AircraftLoadoutConfigurator => "t01-LoadoutConfigurator.prefab";
    }
}
