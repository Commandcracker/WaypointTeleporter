using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace WaypointTeleporter
{
    class CommandWaypointLocate : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "WaypointLocate";
        public string Help => "Tells you the Coordinates of your Waypoint/Marker";
        public string Syntax => "";
        public List<string> Aliases => new List<string>() { "WLocate" };
        public List<string> Permissions => new List<string>() { "waypointlocate" };


        public void Execute(IRocketPlayer caller, params string[] command)
        {
            UnturnedPlayer uCaller = (UnturnedPlayer)caller;

            if (uCaller.Player.quests.isMarkerPlaced)
            {
                UnityEngine.Vector3? markerLocation = GetSurface(uCaller.Player.quests.markerPosition);
                if (markerLocation != null)
                {
                    UnturnedChat.Say(uCaller, $"Marker Location: {markerLocation.Value}.", Color.yellow);
                    Logger.Log($"{markerLocation.Value}", ConsoleColor.Cyan);
                } else {
                    UnturnedChat.Say(uCaller, $"Marker Location: {uCaller.Player.quests.markerPosition}.", Color.yellow);
                }
            }
            else
            {
                UnturnedChat.Say(uCaller, "You need to set a Marker before using this command!", Color.red);
            }
        }

        private Vector3? GetSurface(Vector3 Position)
        {
            if (Physics.Raycast(new Vector3(Position.x, Position.y + 1024, Position.z), Vector3.down, out RaycastHit Hit, Mathf.Infinity))
            {
                return Hit.point;
            }
            else
            {
                return null;
            }
        }
    }
}