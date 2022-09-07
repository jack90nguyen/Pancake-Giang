
namespace Onetez.Dal.Models
{
  public class UserModel
  {
    public string id { get; set; }

    public string user { get; set; }

    public string name { get; set; }

    public string avatar { get; set; }

    public RoleModel role { get; set; }
  }

  public class RoleModel
  {
    public bool is_role { get; set; }

    public bool is_admin { get; set; }

    public bool is_staff { get; set; }

    public bool is_partner { get; set; }

    public bool is_ads { get; set; }

    public bool is_report { get; set; }
  }
}
