using SprintCrowdBackEnd.Enums;

namespace SprintCrowdBackEnd.Models
{
    public class ResponseObject
    {
        public int StatusId = (int)ResponseStatus.AllOk;
        public dynamic Data;
    }
}