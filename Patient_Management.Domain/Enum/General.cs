namespace Patient_Management.Domain.Enum
{

    public enum UserRole
    {
        Administrator = 1,
        Patient = 2,
        Doctor = 3,
    }

    public enum UserStatus
    {
        Active = 1,
        Inactive = 2
    }
    public enum AppointmentStatus
    {
        Scheduled = 1,
        Cancelled = 2,
        Completed = 3,

    }

}
