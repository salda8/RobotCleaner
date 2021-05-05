using RobotCleaner.Exceptions;
using RobotCleaner.Interfaces;
using Serilog;

namespace RobotCleaner.Classes
{
    public class Battery : IBattery
    {
        private readonly ILogger logger;
        public int Charge { get; set; }

        public Battery(ILogger logger)
        {
            this.logger = logger;
        }

        public void DecreaseCharge(int chargeToConsume)
        {
            if (Charge < chargeToConsume)
            {
                throw new NotEnoughBatteryException();
            }

            Charge -= chargeToConsume;
            logger.Information($"Battery charge: {Charge}");
        }
    }
}