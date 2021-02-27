using System;
using UnityEngine;

namespace Library.src.util
{
    public static class EnvironmentUtil
    {
        //brawl info
        public const int BRAWL_DISTANCE = 5;
        public const int BRAWL_HEIGHT = 3;
        public const int BRAWL_MAX_PARTICIPANT = 6;
        public const long BRAWL_RESOLUTION_INTERVAL = 1000L;

        //tags
        public const string TAG_PLAYER = "player_unit";
        public const string TAG_AI = "enemy_unit";
        public const string TAG_LOOT = "loot";
        
        //pathfinding info
        public const float STOPPING_DISTANCE = 0.3f;
        
        //animator fields
        public const string ANIM_BRAWL = "inBrawl";
        public const string ANIM_MOVE = "move";
        public const string ANIM_SLASH = "isSlashing";
        public const string ANIM_TURNING = "turning";

        //resource paths
        public const string RESOURCE_PLAYER_UNIT = "Config/Units/PlayerConfiguration";
        public const string RESOURCE_HOSTILE_UNIT = "Config/Units/HostileConfiguration";
        public const string RESOURCE_WEAPONS = "Config/Weapons/Weapons";
        
        //defaults
        public const string DEFAULT_UNIT = "default";
    }
}