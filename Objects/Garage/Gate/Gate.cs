using CourseWork.Common;

namespace CourseWork.Objects.Garage.Gate
{
    public enum GateState
    {
        Opening,
        Opened,
        Closing,
        Closed
    }

    public abstract class Gate : Model, IUpdateable
    {
        public float OpeningSpeed { get; set; } = 0.1f;
        public float ClosingSpeed { get; set; } = 0.1f;
        public float OpenPercentage { get; set; }
        public GateState State { get; set; } = GateState.Closed;

        public void Open()
        {
            if (State != GateState.Opened)
                State = GateState.Opening;
        }

        public void Close()
        {
            if (State != GateState.Closed)
                State = GateState.Closing;
        }

        public virtual void Update(double dt)
        {
            float updatePercentage = 0;
            if (State == GateState.Opening)
                updatePercentage = OpeningSpeed;
            else if (State == GateState.Closing)
                updatePercentage = -ClosingSpeed;

            OpenPercentage += updatePercentage * (float) dt * 100;

            if (OpenPercentage >= 100)
            {
                OpenPercentage = 100;
                State = GateState.Opened;
            }

            if (OpenPercentage <= 0)
            {
                OpenPercentage = 0;
                State = GateState.Closed;
            }
        }
    }
}