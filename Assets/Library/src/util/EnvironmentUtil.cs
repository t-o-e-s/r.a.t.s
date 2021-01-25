using System;

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
        public const string TAG_MOVEMENT_TILE = "movement_tile";
        
        //pathfinding info
        public const float STOPPING_DISTANCE = 0.3f;
        
        //animator fields
        public const string REWIND = "reversing";
        public const string BRAWL = "brawling";
        public const string MOVEMENT = "move";
        public const string SLASH = "slashing";
        public const string TURNING = "turning";

    }
}