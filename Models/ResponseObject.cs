using SprintCrowd.Backend.Enums;

namespace SprintCrowd.Backend.Models
{
    public class ResponseObject
    {
        public int StatusId = (int)ResponseStatus.AllOk;
        public dynamic Data;
    }
}