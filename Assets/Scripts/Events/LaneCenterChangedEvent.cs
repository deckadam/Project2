namespace Player.Events
{
    public class LaneCenterChangedEvent
    {
        public float newLineCenter { get; }

        public LaneCenterChangedEvent(float newLineCenter)
        {
            this.newLineCenter = newLineCenter;
        }
    }
}