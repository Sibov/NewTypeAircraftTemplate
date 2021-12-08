using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace CustomAircraftTemplate
{
    class AiSetup : MonoBehaviour
    {
        
        public static void CreateAi(GameObject aiObject)
        {
			

			Disable26MeshAi(aiObject);

			GameObject aircraft = Instantiate<GameObject>(Main.aircraftPrefab);
			aircraft.transform.SetParent(aiObject.transform);
			
			AIPilot aiPilot = aiObject.GetComponentInChildren<AIPilot>();
			GearAnimator gearAnim = aiPilot.gearAnimator;

			AnimationToggle animToggle = AircraftAPI.GetChildWithName(aircraft, "GearAnimator", false).GetComponent<AnimationToggle>();
			gearAnim.OnOpen.AddListener(new UnityAction(animToggle.Retract));
			gearAnim.OnClose.AddListener(new UnityAction(animToggle.Deploy));

			}

		public static void Disable26MeshAi(GameObject go)
		{
			WeaponManager componentInChildren = go.GetComponentInChildren<WeaponManager>(true);
			AircraftAPI.DisableMesh(go, componentInChildren);
			
		}

		public static void CreateControlSurfaces(GameObject aiAircraft, GameObject customAircraft)
		{}

		public static void SetUpRCS(GameObject aiAircraft)
		{
		}

		public static void SetUpRefuelPort(GameObject aiAircraft, GameObject customAircraft)
		{

		}

		public static void SetUpRadarIcon(GameObject aiAircraft)
        {

        }


	}




}
