using Photon.Pun;
using UnityEngine;
using static GorillaStats.Scrim;


namespace GorillaStats
{
    public class GUIStuff
    {
        public static Rect windowRect = new Rect(20, 20, 150, 50);

        /*public static GUISkin CreateGUISkin()
        {
            GUISkin customSkin = ScriptableObject.CreateInstance<GUISkin>();

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.fontSize = 14;
            buttonStyle.fontStyle = FontStyle.Bold;
            buttonStyle.alignment = TextAnchor.MiddleCenter;

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.normal.textColor = Color.white;
            labelStyle.fontSize = 12;
            labelStyle.alignment = TextAnchor.MiddleCenter;

            customSkin.button = buttonStyle;
            customSkin.label = labelStyle;

            return customSkin;
        }*/

        public static void GUIWindow(int windowID)
        {
            if (PhotonNetwork.InRoom)
            {
                if (!showStats)
                {
                    if (GUILayout.Button("Show Stats"))
                    {
                        showStats = true;
                        windowRect.width = 350;
                        windowRect.height = 100;
                    }
                }
                else
                {
                    if (playerStats.Count > roundNumber - 1 && playerStats[roundNumber - 1].Count > 0)
                    {
                        foreach (var player in playerStats[roundNumber - 1])
                        {
                            GUILayout.Label($"{player.Value.Name} - Tags: {player.Value.Tags} - Runtime: {Mathf.RoundToInt(player.Value.Runtime)} seconds");
                        }
                    }
                    else
                    {
                        GUILayout.Label("No players have any stats this round.");
                    }

                    if (GUILayout.Button("Hide Stats"))
                    {
                        showStats = false;
                        windowRect.width = 150;
                        windowRect.height = 50;
                    }
                }

                if (!roundActive)
                {
                    if (GUILayout.Button($"Start Round {roundNumber}"))
                    {
                        StartRound();
                    }
                    if (playerStats.Count > 0)
                    {
                        if (GUILayout.Button($"End Scrim"))
                        {
                            EndScrim();
                        }
                    }
                }
                else
                {
                    if (!roundPaused)
                    {
                        if (GUILayout.Button("Pause Round"))
                        {
                            roundPaused = true;
                        }
                        if (GUILayout.Button($"End Round {roundNumber}"))
                        {
                            EndRound();
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Resume Round"))
                        {
                            roundPaused = false;
                        }
                    }
                }
            }
            else
            {
                GUILayout.Label("Please join a room...");
            }

            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }
    }
}