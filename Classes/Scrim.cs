using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Reflection;

namespace GorillaStats
{
    public class Scrim
    {
        public static List<Dictionary<string, PlayerStats>> playerStats = new List<Dictionary<string, PlayerStats>>();
        public static int roundNumber = 1;

        public static bool roundActive = false;
        public static bool roundPaused = false;
        public static bool showStats = false;
        public static void StartRound()
        {
            playerStats.Add(new Dictionary<string, PlayerStats>());
            roundActive = true;
        }

        public static void EndRound()
        {
            roundActive = false;
            if (playerStats.Count > roundNumber - 1 && playerStats[roundNumber - 1].Count > 0)
            {
                roundNumber++;
            }
            Debug.Log($"[GorillaStats] Round ended, roundNumber set to {roundNumber}");
            foreach (var player in playerStats[roundNumber - 2])
            {
                Debug.Log($"[GorillaStats] Player {player.Value.Name} [{player.Key}] had {player.Value.Tags} tags and had a runtime of {player.Value.Runtime} seconds");
            }
        }

        public static void EndScrim()
        {
            roundNumber = 1;
            roundActive = false;

            Debug.Log($"[GorillaStats] Scrim ended, roundNumber set to {roundNumber}");

            List<RoundStats> gorillaStats = new List<RoundStats>();

            int roundNum = 1;
            foreach (var stats in playerStats)
            {
                var playerStatsList = new List<PlayerStats>(stats.Values);

                gorillaStats.Add(new RoundStats
                {
                    RoundNumber = roundNum,
                    PlayerStats = playerStatsList
                });

                roundNum++;
            }

            string json = JsonConvert.SerializeObject(new { GorillaStats = gorillaStats }, Formatting.Indented);

            string assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string directoryPath = Path.Combine(assemblyLocation, "SavedGorillaStats");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, $"GorillaStats-{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.json");

            File.WriteAllText(filePath, json);
            Debug.Log($"[GorillaStats] Saved GorillaStats to {filePath}");
            playerStats.Clear();
        }
    }
}