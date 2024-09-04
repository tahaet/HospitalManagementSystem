namespace Hospital.Utility
{
    public static class SD
    {
        public static class Days
        {
            public const string Monday = "Monday";
            public const string Tuestday = "Tuestday";
            public const string Wednesday = "Wednesday";
            public const string Thursday = "Thursday";
            public const string Friday = "Friday";
            public const string Saturday = "Saturday";
            public const string Sunday = "Sunday";
        }

        public static class Gender
        {
            public const string Male = "Male";
            public const string Femal = "Femal";
        }
        public static class DayTime
        {

            public const string Evening = "Evening";
            public const string Morning = "Morning";
            public const string Midday = "Midday";
        }
       public static class StatusPatient
       {
            public const string Cured = "Cured";
            public const string UnderTreatment = "Under Treatment";
       }
       
        public static class PatientType
        {
            public const string InPatient = "In Patient";
            public const string OutPatient = "Out Patient";
        }

        public static class MedicineType
        {
            public const string Inj = "Inj";
            public const string Capsule = "Capsule";
            public const string Tablet = "Tablet";
        }

        public static class VaccineType
        {
            public const string Intramuscular = "Intramuscular";
            public const string Intravenous = "Intravenous";
            public const string Subcutaneous = "Subcutaneous";

        }
        public static class PaymentStatus
        {
            public const string Paid = "Paid";
            public const string UnPaid = "Un-Paid";

        }

        public static class Roles
        {
            public const string Admin = "admin";
            public const string Employee = "employee";
            public const string Doctor = "doctor";
            public const string patient = "patient";
            public const string nurse = "patient";

        }

    }
}
