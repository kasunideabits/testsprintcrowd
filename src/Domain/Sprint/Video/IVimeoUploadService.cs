namespace SprintCrowd.BackEnd.Domain.Sprint.Video
{

    using System.Threading.Tasks;
    public interface IVimeoUploadService
    {
        Task<object> GetVimeoUploadLink(int fileSize);
    }
}