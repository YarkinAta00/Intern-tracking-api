using System;
using System.ComponentModel.DataAnnotations;

namespace İnternApi.Model
{
	public class Files
	{
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[]? FileData { get; set; }

    }
}

