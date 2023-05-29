using System;
using System.ComponentModel.DataAnnotations;
namespace İnternApi.Model
{
	public class Roles
	{
        [Key]
        public int RoleID { get; set; }
        public int UserID { get; set; }
        public String RoleName { get; set; }
        public int IsActive { get; set; }
        public int IsDeleted { get; set; }
    }
}

