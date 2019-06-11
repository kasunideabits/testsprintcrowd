namespace SprintCrowdBackEnd.Web.Event
{
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Mvc;
  using SprintCrowdBackEnd.Application;
  /// <summary>
  /// event controller
  /// </summary>
  public class EventController
  {
    [HttpPost]
    [Route("create")]
    public async Task<ResponseObject> CreateEvent()
    {

    }
  }
}