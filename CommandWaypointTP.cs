using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace WaypointTeleporter
{
    public class CommandWaypointTP: IRocketCommand
    {
        public string Help
        {
            get { return "Teleports you to your Marker"; }
        }

        public string Name
        {
            get { return "WaypointTP"; }
        }

        public string Syntax
        {
            get { return "<WaypointTP>"; }
        }

        public bool RunFromConsole
        {
            get { return false; }
        }
        public List<string> Aliases
        {
            get 
            { 
                return new List<string>() {"WTP"}; 
            }
        }

        public AllowedCaller AllowedCaller
        {
            get { return Rocket.API.AllowedCaller.Player; }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "waypointteleport" };
            }
        }
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (player.Player.quests.isMarkerPlaced)
            {
                Vector3 teleportLocation = GetSurface(player.Player.quests.markerPosition).Value;
                player.Teleport(new Vector3(teleportLocation.x, teleportLocation.y + 3, teleportLocation.z), player.Player.look.angle);
                UnturnedChat.Say(player, "Successfully teleported to your Marker.", Color.yellow);
                Logger.LogWarning($"{player.DisplayName} has teleported to their Marker {teleportLocation}.");
                if (WaypointTeleporter.Instance.Configuration.Instance.RemoveMarkerOnTP)
                {
                    player.Player.quests.askSetMarker(player.CSteamID, false, teleportLocation);
                }
                
            } else
            {
                UnturnedChat.Say(caller, "You need to set a Marker before using this command!", Color.red);
                return;
            }
        }

        private Vector3? GetSurface(Vector3 Position)
        {
            int layerMasks = (RayMasks.BARRICADE | RayMasks.BARRICADE_INTERACT | RayMasks.ENEMY | RayMasks.ENTITY | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.GROUND2 | RayMasks.ITEM | RayMasks.RESOURCE | RayMasks.STRUCTURE | RayMasks.STRUCTURE_INTERACT | RayMasks.WATER);
            if (Physics.Raycast(new Vector3(Position.x, Position.y + 200, Position.z), Vector3.down, out RaycastHit Hit, 250, layerMasks))
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
