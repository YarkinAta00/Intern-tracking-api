using System;
using System.ComponentModel.DataAnnotations;

namespace İnternApi.Model
{
	public class Departments
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public String DepartmentName { get; set; }
       
    }
}

