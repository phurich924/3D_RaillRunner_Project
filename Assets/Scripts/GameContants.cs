using UnityEngine;

public static class GameConstants
{
    public const int LaneCount = 5;
    public const float LaneWidth = 1f;

    public static float LaneToX(int laneIndex)
    {
        float center = (LaneCount - 1) / 2f;
        return (laneIndex - center) * LaneWidth;
    }

    public static int CenterLane => (LaneCount - 1) / 2;
}