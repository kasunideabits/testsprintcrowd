namespace SprintCrowd.BackEnd.Domain.Sprint
{
  using System;
  /// <summary>
  /// model for holding event data
  /// </summary>
  public class SprintModel
  {

    /// <inheritdoc />
    public SprintModel(string name, int distance, Boolean locationProvided, DateTime startTime, int sprintType,
      double lattitude, double longitutude)
    {
      this.Name = name;
      this.Distance = distance;
      this.LocationProvided = locationProvided;
      this.StartTime = startTime;
      this.SprintType = sprintType;
      this.Lattitude = lattitude;
      this.Longitutude = longitutude;
    }

    /// <summary>
    /// Event Name
    /// </summary>
    /// <value></value>
    public string Name { get; }
    /// <summary>
    /// Event distance
    /// </summary>
    /// <value></value>
    public int Distance { get; }
    /// <summary>
    /// Wether location is provided or not
    /// </summary>
    /// <value></value>
    public Boolean LocationProvided { get; }
    /// <summary>
    /// Start Time of the event
    /// </summary>
    /// <value></value>
    public DateTime StartTime { get; }
    /// <summary>
    /// public or private
    /// </summary>
    /// <value></value>
    public int SprintType { get; }
    /// <summary>
    /// Latitutude
    /// </summary>
    /// <value></value>
    public double Lattitude { get; }
    /// <summary>
    /// Longitutude
    /// </summary>
    /// <value></value>
    public double Longitutude { get; }
  }
}