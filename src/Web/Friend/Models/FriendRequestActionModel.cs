using System.ComponentModel.DataAnnotations;
namespace SprintCrowd.BackEnd.Web.Friend
{
  /// <summary>
  /// Action request body for friend request
  /// </summary>
  public class FriendRequestActionModel
  {
    /// <summary>
    /// Gets or set unique code
    /// </summary>
   // [Required]
    public string Code { get; set; }
  }
}