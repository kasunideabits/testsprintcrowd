namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    public enum ErrorCodes
    {
        AlreadyInvited = 1000,
        NotFounInvitation = 1001,
        AlreadyJoined = 1002,
        SprintExpired = 1003,
        MaxUserExceeded = 1004,
        ParticipantNotFound = 1005,
        CanNotRemoveParticipant = 1006,
        NotAllowedOperation = 1007,
        MarkAttendanceEnable = 1008,
    }
}