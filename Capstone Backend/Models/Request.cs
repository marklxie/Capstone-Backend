using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Capstone_Backend.Models {
	public class Request {
		public int Id { get; set; }
		[Required]
		[StringLength(80)]
		public string Description { get; set; }
		[Required]
		[StringLength(80)]
		public string Justification { get; set; }
		[StringLength(80)]
		public string RejectionReason { get; set; }
		[Required]
		[StringLength(20)]
		[DefaultValue("Pickup")]
		public string DeliveryMode { get; set; } = "Pickup";
		[Required]
		[StringLength(10)]
		[DefaultValue("New")]
		public string Status { get; set; } = "NEW";
		[Column(TypeName = "decimal(11,2)")]
		[DefaultValue(0)]
		public decimal Total { get; set; } = 0;
		public int UserId { get; set; }
		public virtual User User { get; set; }
		public Request() { }
	}
}
