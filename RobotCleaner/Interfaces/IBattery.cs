namespace RobotCleaner.Interfaces
{
    public interface IBattery
    {
        int Charge { get; set; }

        void DecreaseCharge(int chargeToConsume);
    }
}