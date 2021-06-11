namespace SprintCrowd.BackEnd.Domain.Sprint.Dtos
{
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    public class SprintReturnDto : Sprint
    {
        public SprintReturnDto(Sprint obj)
       : base(obj)
        { }
        public UserDto influencerDetails { get; set; }
        public UserDto influencerSecondDetails { get; set; }
    }
}