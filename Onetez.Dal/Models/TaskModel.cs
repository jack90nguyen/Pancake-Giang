using System.Collections.Generic;

namespace Onetez.Dal.Models
{
    public class TaskModel
    {
        public long id { get; set; }

        public string name { get; set; }

        public string detail { get; set; }

        public string deadline { get; set; }

        public string deadline_day { get; set; }

        public string deadline_time { get; set; }

        public long? parent { get; set; }

        public int status_id { get; set; }

        public string status_icon { get; set; }

        public long? staff_id { get; set; }

        public string staff_name { get; set; }

        public List<string> images { get; set; }

        public List<TaskModel> childs { get; set; }
    }
}
