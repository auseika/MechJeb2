﻿using System;
using UnityEngine;

namespace MuMech
{
    public class MechJebModuleAscentPEGMenu : MechJebModuleAscentMenuBase
    {
        public MechJebModuleAscentPEGMenu(MechJebCore core)
            : base(core)
        {
            hidden = true;
        }

        public MechJebModuleAscentPEG path { get { return autopilot.ascentPath as MechJebModuleAscentPEG; } }
        public MechJebModuleAscentAutopilot autopilot;

        public override void OnStart(PartModule.StartState state)
        {
            autopilot = core.GetComputerModule<MechJebModuleAscentAutopilot>();
        }

        public override GUILayoutOption[] WindowOptions()
        {
            return new GUILayoutOption[] { GUILayout.Width(300), GUILayout.Height(100) };
        }

        protected override void WindowGUI(int windowID)
        {
            if (path == null)
            {
                GUILayout.Label("Path is null!!!1!!1!1!1111!11eleven");
                base.WindowGUI(windowID);
                return;
            }

            GUILayout.BeginVertical();

            GuiUtils.SimpleTextBox("Target Periapsis:", autopilot.desiredOrbitAltitude, "km");
            GuiUtils.SimpleTextBox("Target Apoapsis:", path.desiredApoapsis, "km");
            if ( path.desiredApoapsis < autopilot.desiredOrbitAltitude )
            {
                GUIStyle s = new GUIStyle(GUI.skin.label);
                s.normal.textColor = Color.yellow;
                GUILayout.Label("Apoapsis < Periapsis: circularizing orbit at periapsis", s);
            }

            GuiUtils.SimpleTextBox("Booster Pitch start:", path.pitchStartTime, "s");
            GuiUtils.SimpleTextBox("Booster Pitch rate:", path.pitchRate, "°/s");
            GuiUtils.SimpleTextBox("Booster Pitch end:", path.pitchEndTime, "s");
            GUILayout.Label(String.Format("ending pitch: {0:F1}°", (path.pitchEndTime - path.pitchStartTime)*path.pitchRate));
            GuiUtils.SimpleTextBox("Terminal Guidance Period:", path.terminalGuidanceSecs, "s");


            GUILayout.EndVertical();
            GUILayout.BeginVertical();

            GUILayout.Label("Burnout Stats");
            GUILayout.Label(String.Format("delta-V: {0:F1}", path.dV));
            GUILayout.Label(String.Format("time: {0:F1}", path.T));
            GUILayout.Label(String.Format("pitch: {0:F1}", path.guidancePitch));
            GUILayout.Label(String.Format("steps: {0:D}", path.convergenceSteps));

            if (path.guidanceEnabled)
            {
                if (GUILayout.Button("Disable PEG Guidance"))
                    path.guidanceEnabled = false;
            }
            else
            {
                if (GUILayout.Button("Enable PEG Guidance"))
                    path.guidanceEnabled = true;
            }

            GuiUtils.SimpleTextBox("Emergency pitch adj.:", path.pitchBias, "°");


            if (autopilot.enabled)
            {
                GUILayout.Label("Autopilot status: " + autopilot.status);
            }


            GUILayout.EndVertical();
            base.WindowGUI(windowID);
        }

        public override string GetName()
        {
            return "Atlas/Centaur PEG Pitch Program";
        }
    }
}