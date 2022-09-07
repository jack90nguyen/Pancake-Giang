
namespace Onetez.ZRunning
{
  public class Models
  {
    public class Sheet
    {
      public string Id { get; set; }

      public string Phone { get; set; }
    }

    public class Create
    {
      public bool status { get; set; }
    }

    public class Refresh
    {
      public bool status { get; set; }

      public string message { get; set; }
    }
  }
}