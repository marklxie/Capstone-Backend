﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone_Backend.Models {
	public class Requestline {
		public int Id { get; set; }
		public int RequestId { get; set; }
		public virtual Request Request { get; set; }
		public int ProductId { get; set; }
		public virtual Product Product { get; set; }
		public int Quantity { get; set; }
	}
}
