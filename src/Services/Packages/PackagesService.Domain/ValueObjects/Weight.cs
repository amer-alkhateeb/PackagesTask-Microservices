namespace PackagesService.Domain.ValueObjects
{
    public sealed record Weight
    {
        public double Kilograms { get;  }
        private const double MinWeight = 0.1;

        private Weight() { }
        private Weight(double kilograms)
        {
            if (kilograms < MinWeight)
                throw new ArgumentException($"Weight must be greater than {MinWeight} kg.", nameof(kilograms));
            Kilograms = kilograms;
        }

        public static Weight Of(double kilograms)
        {
            if (kilograms < MinWeight)
                throw new ArgumentException($"Weight must be greater than {MinWeight} kg.", nameof(kilograms));
            return new Weight(kilograms);
        }
    }
}
