namespace SprintCrowd.BackEnd.Common
{
  public class SuccessDTO<T>
  {
    public SuccessDTO(T data)
    {
      this.Data = data;
    }
    public T Data { get; set; }
  }
}