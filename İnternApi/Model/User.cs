using System;
using System.ComponentModel.DataAnnotations;

namespace İnternApi.Model
{
	public class User
	{
        [Key]
        public int ID { get; set; }
        //foreign key
        public int roleID { get; set; }
        //foreign key
        public int depID { get; set; }
        //foreign key
        public int TaskID { get; set; }


        public bool? hasPermition { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public char Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string phoneNumber { get; set; }
        public string Address { get; set; }

        public bool? isDeleted { get; set; }
        public bool? isActive { get; set; }
        public bool? isIntern{ get; set; }

        public string CurrentTask { get; set; }
        public string LatesTask { get; set; }
       
        public DateTime StartDate { get; set; }
       
        public DateTime EndDate { get; set; }

    }
}

