using UnityEngine;
using BepInEx;
using Photon.Pun;
using ExitGames.Client.Photon;
using static GorillaStats.GUIStuff;
using static GorillaStats.Scrim;
using UnityEngine.InputSystem;

namespace GorillaStats
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        //GUISkin customSkin;
        public bool showGUI = true;


        void Start()
        {
            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
            HarmonyPatches.ApplyHarmonyPatches();

            //customSkin = CreateGUISkin();

            Debug.Log("[GorillaStats] Plugin started");
        }

        void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == GorillaTagManager.ReportInfectionTagEvent && PhotonNetwork.InRoom && roundActive && !roundPaused)
            {
                Debug.Log("[GorillaStats]---------------------------------TAG EVENT RECEIVED-------------------------------------------");
                if (photonEvent.CustomData is object[] data)
                {
                    string taggingPlayerId = data[0] as string;

                    if (!playerStats[roundNumber - 1].ContainsKey(taggingPlayerId))
                    {
                        playerStats[roundNumber - 1][taggingPlayerId] = new();
                    }

                    playerStats[roundNumber - 1][taggingPlayerId].Tags++;
                    playerStats[roundNumber - 1][taggingPlayerId].Name = GetRigById(taggingPlayerId).Creator.NickName;
                }
                else
                {
                    Debug.LogError("[GorillaStats] Failed to parse custom data in tag event.");
                }

            }
        }

        void OnDestroy()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
            HarmonyPatches.RemoveHarmonyPatches();
            Debug.Log("[GorillaStats] Plugin stopped");
        }

        void Update()
        {
            if (PhotonNetwork.InRoom && roundActive && !roundPaused)
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigDict.Values)
                {
                    bool isTagged = GorillaTagManager.instance.GetComponent<GorillaTagManager>().IsInfected(rig.Creator);
                    if (!isTagged)
                    {
                        if (!playerStats[roundNumber - 1].ContainsKey(rig.Creator.UserId))
                        {
                            playerStats[roundNumber - 1][rig.Creator.UserId] = new();
                        }
                        playerStats[roundNumber - 1][rig.Creator.UserId].Runtime += Time.deltaTime;
                        playerStats[roundNumber - 1][rig.Creator.UserId].Name = rig.Creator.NickName;
                    }
                }
            }

            if (Keyboard.current.nKey.wasPressedThisFrame)
            {
                showGUI = !showGUI;
            }
        }

        VRRig GetRigById(string playerId)
        {
            if (playerId == "")
            {
                return null;
            }
            foreach (VRRig rig in GorillaParent.instance.vrrigDict.Values)
            {
                if (rig.Creator.UserId == playerId)
                {
                    return rig;
                }
            }
            return null;
        }

        void OnGUI()
        {
            //GUI.skin = customSkin;
            if (showGUI)
            {
                windowRect = GUILayout.Window(0, windowRect, GUIWindow, "GorillaStats-By _pugg", GUILayout.MinWidth(150), GUILayout.MinHeight(50));
            }
        }
    }
}