namespace SprintCrowd.BackEnd.Common
{
  public class SuccessResponse<T>
  {
    public SuccessResponse(T data)
    {
      this.Data = data;
    }
    public T Data { get; set; }
  }
}