namespace CustomAircraftTemplate
{
    /// <summary>
    /// The primary runner class for an aircraft mod. Utilizes a base runner that is updated separately.<para/>
    /// Feel free to rename/refactor the class, but be sure to do it with an IDE!
    /// </summary>
    public class Main : IAircraftMod<Main>
    {
        public override void ModLoaded()
        {
            var aircraftInfo = new TemplateAircraftInfo();

            base.AircraftModLoaded(this, aircraftInfo);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            // do extra stuff here
        }
    }
}
