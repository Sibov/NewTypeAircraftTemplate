using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomAircraftTemplate
{
    public interface IAircraftInfo
    {
        /// <summary>
        /// A unique patch identifier, in order for your mod to be compatible with other Harmony mods.
        /// </summary>
        string HarmonyId { get; }
        /// <summary>
        /// The maximum internal fuel capacity of the aircraft.
        /// </summary>
        float maxInternalFuel { get; }
        /// <summary>
        /// A static unique identifier for this aircraft, for use in multiplayer. You should not be randomly generating this on load each time.
        /// <para/>You will run into problems if it is the same as another mod or a base game aircraft!
        /// </summary>
        int AircraftMPIdentifier { get; }
        /// <summary> The asset reference name (like <c>"SU-35.asset"</c>) in Unity containing the PlayerVehicle script. </summary>
        string CustomAircraftPV { get; }
        /// <summary> The aircraft's name. </summary>
        /// <remarks><see cref="PlayerVehicle"/></remarks>
        string AircraftName { get; }
        /// <summary> The aircraft's nickname. </summary>
        /// <remarks><see cref="PlayerVehicle"/></remarks>
        string AircraftNickname { get; }
        /// <summary> A description for the aircraft. </summary>
        /// <remarks><see cref="PlayerVehicle"/></remarks>
        string AircraftDescription { get; }
        /// <summary> The file name of the aircraft's preview thumbnail image. </summary>
        string PreviewPngFileName { get; }
        /// <summary> The file name of the aircraft's Unity-exported asset bundle, for loading. </summary>
        string AircraftAssetbundleName { get; }
        /// <summary> The bundle-internal name of the aircraft's prefab, for loading. </summary>
        string AircraftPrefabName { get; }
        /// <summary> The bundle-internal name of the aircraft's loadout configurator prefab, for loading. </summary>
        string AircraftLoadoutConfigurator { get; }
    }
}
