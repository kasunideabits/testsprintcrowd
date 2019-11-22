namespace SprintCrowd.BackEnd.Domain.Admin.Dashboard
{
  using SprintCrowd.BackEnd.Domain.Device;
  using SprintCrowd.BackEnd.Domain.Sprint;
  public class DashboardDataDto
  {
    public int WeeklyActiveUsers { get; set; }
    public DeviceModal AppDownloads { get; set; }
    public LiveSprintCount LiveSprintsCount { get; set; }
  }
}