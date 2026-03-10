using HutongGames.PlayMaker;
using MSCLoader;
using UnityEngine;

namespace MSCLightsOn
{
    public class MSCLightsOn : Mod
    {
        public override string ID => "MSCLightsOn";
        public override string Name => "Turn on the Lights";
        public override string Author => "teamteppy";
        public override string Version => "1.0";
        public override string Description => "Turn home lights on when loading in";
        public override Game SupportedGames => Game.MySummerCar;

        private readonly string[] houseSwitchPaths = new string[]
        {
            "YARD/Building/Dynamics/LightSwitches/switch_kitchen",
            "YARD/Building/Dynamics/LightSwitches/switch_hallway",
            "YARD/Building/Dynamics/LightSwitches/switch_bedroomParents",
            "YARD/Building/Dynamics/LightSwitches/switch_bedroomBoy",
            "YARD/Building/Dynamics/LightSwitches/switch_bathroom",
            "YARD/Building/Dynamics/LightSwitches/switch_entry",
            "YARD/Building/Dynamics/LightSwitches/switch_garage",
            "YARD/Building/Dynamics/LightSwitches/switch_wc",
        };

        public override void ModSetup()
        {
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.ModSettings, Mod_Settings);
        }

        private void Mod_Settings()
        {
        }

        private void Mod_OnLoad() {
            foreach (string path in houseSwitchPaths)
            {
                TurnLightOn(path);
            }
        }

        private GameObject FindByPath(string path)
        {
            string[] parts = path.Split('/');
            GameObject root = GameObject.Find(parts[0]);
            if (root == null)
            {
                return null;
            }
            Transform t = root.transform;
            for (int i = 1; i < parts.Length; i++)
            {
                t = t.Find(parts[i]);
                if (t == null)
                {
                    return null;
                }
            }
            return t.gameObject;
        }

        private void TurnLightOn(string switchPath)
        {
            GameObject switchObj = FindByPath(switchPath);
            PlayMakerFSM useFsm = PlayMakerFSM.FindFsmOnGameObject(switchObj, "Use");
            FsmBool switchVar = useFsm.FsmVariables.GetFsmBool("Switch");
            FsmGameObject lightsVar = useFsm.FsmVariables.GetFsmGameObject("Lights");

            // Sync FSM state
            if (switchVar != null)
            {
                switchVar.Value = true;
            }

            // Turn on the light
            lightsVar.Value.SetActive(true);

            // Physically flip the switch button to ON position (x = -12)
            GameObject lightSwitch = switchObj.transform.Find("light_switch").gameObject;
            GameObject button = lightSwitch.transform.Find("light_switch_button").gameObject;
            button.transform.localEulerAngles = new Vector3(-12f, 0f, 0f);
        }
    }
}