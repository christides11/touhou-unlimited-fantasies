namespace TAPI.Entities
{
    public enum EntityStates
    {
        IDLE = 0,
        WALK = 1,
        RUN = 2,
        JUMP_SQUAT = 3,
        JUMP = 4,
        FALL = 5,
        DASH = 6,
        AIR_DASH = 7,
        FLOAT = 8,
        AIR_JUMP = 9,
        ATTACK = 10,
        // Hit Reactions
        FLINCH = 11,
        FLINCH_AIR = 12,
        TUMBLE = 13,
        TRIP = 14,
        KNOCKDOWN = 15,
        WAKEUP = 16,
        GROUND_BOUNCE = 17,
        ENEMY_STEP = 18
    }
}