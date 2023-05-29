using System;
using System.ComponentModel.DataAnnotations;

namespace İnternApi.Model
{
	public class RoleMapping
	{
        [Key]
        public int UserRoleMappingID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
    }
}

