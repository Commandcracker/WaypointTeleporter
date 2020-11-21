using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace WaypointTeleporter
{
    class CommandWaypointTP: IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "WaypointTP";
        public string Help => "Teleports you to your Marker";
        public string Syntax => "";
        public List<string> Aliases => new List<string>() { "WTP" };
        public List<string> Permissions => new List<string>() { "waypointteleport" };


        public void Execute(IRocketPlayer caller, params string[] command)
        {
            UnturnedPlayer uCaller = (UnturnedPlayer)caller;

            if (uCaller.Player.quests.isMarkerPlaced)
            {
                var teleportLocation = GetSurface(uCaller.Player.quests.markerPosition);
                if (teleportLocation != null)
                {
                    uCaller.Teleport(new Vector3(teleportLocation.Value.x, teleportLocation.Value.y + 1, teleportLocation.Value.z), uCaller.Player.look.angle);
                    UnturnedChat.Say(uCaller, "Successfully teleported to your Marker.", Color.yellow);
                    Logger.LogWarning($"{uCaller.DisplayName} has teleported to their Marker {teleportLocation.Value}.");
                    if (WaypointTeleporter.Instance.Configuration.Instance.RemoveMarkerOnTP)
                    {
                        uCaller.Player.quests.askSetMarker(uCaller.CSteamID, false, teleportLocation.Value);
                    }
                } else {
                    UnturnedChat.Say(uCaller, "No Location to teleport found!", Color.red);
                }
            } else
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