
using UnityEngine;

public static class GameConstants
{
    public const int LaneCount = 3;
    public const float LaneWidth = 2.5f;

    public static float LaneToX(int laneIndex)
    {
        float center = (LaneCount - 1) / 2f;
        return (laneIndex - center) * LaneWidth;
    }

    public static int CenterLane => (LaneCount - 1) / 2;
}
