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
        public string Help
        {
            get { return "Tells you the Coordinates of your Waypoint/Marker"; }
        }

        public string Name
        {
            get { return "WaypointLocate"; }
        }

        public string Syntax
        {
            get { return "<WaypointLocate>"; }
        }

        public bool RunFromConsole
        {
            get { return false; }
        }
        public List<string> Aliases
        {
            get 
            { 
                return new List<string>() {"WLocate"}; 
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
                return new List<string>() { "waypointlocate" };
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (player.Player.quests.isMarkerPlaced)
            {
                Vector3 markerLocation = GetSurface(player.Player.quests.markerPosition).Value;
                Logger.Log($"{markerLocation}", ConsoleColor.Cyan);
                UnturnedChat.Say(player, "Marker Location (Also sent to Console):", Color.yellow);
                UnturnedChat.Say(player, $"{markerLocation}", Color.yellow);
            }
            else
            {
                UnturnedChat.Say(player, "You need to set a Marker before using this command!", Color.red);
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
