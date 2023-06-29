using System;
using System.ComponentModel.DataAnnotations;

namespace İnternApi.Model
{
	public class Tasks
	{
        [Key]
        public int TaskId { get; set; }
        public int UserID { get; set; }
        public String TaskName { get; set; }
        public String TaskDetails { get; set; }
        public bool IsCompleted { get; set; }
        public bool CheckProgres { get; set; }
        public String TaskSender { get; set; }
        public DateTime TaskStartTime { get; set; }
        public DateTime TaskEndTime { get; set; }

    }
}

