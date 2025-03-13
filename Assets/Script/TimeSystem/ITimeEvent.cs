namespace Script.TimeSystem {
    public interface ITimeEvent {
        void OnTimeReached(float gameTime);
    }
}