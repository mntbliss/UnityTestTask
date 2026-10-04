using System;

namespace _Bludoku.Scripts.Analytics
{
    [Serializable]
    public class AnalyticsKeys
    {
        public AnalyticsKey GameStarted = new AnalyticsKey("game_started", "Player started the game");
        public AnalyticsKey GameOver = new AnalyticsKey("game_over", "Player reached game over");
        public AnalyticsKey ComboReached = new AnalyticsKey("combo_reached", "Player reached a combo");
        public AnalyticsKey ComboBroken = new AnalyticsKey("combo_broken", "Player broke a combo");

        public AnalyticsKey PieceMoveStarted = new AnalyticsKey("piece_move_started", "Player started moving a piece");
        public AnalyticsKey PiecePlaced = new AnalyticsKey("piece_placed", "Player placed a piece");
        public AnalyticsKey PieceDeleted = new AnalyticsKey("piece_deleted", "Player deleted a piece");

        public AnalyticsKey BonusReceived = new AnalyticsKey("bonus_received", "Player received a bonus");
        public AnalyticsKey PowerupUsed = new AnalyticsKey("powerup_used", "Player used a power-up");
    }
}
