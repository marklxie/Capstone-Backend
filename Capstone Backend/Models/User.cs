﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient.Server;

namespace Capstone_Backend.Models {
	public class User {
		public int Id { get; set; }
		[Required]
		[StringLength(30)]
		public string Username { get; set; }
		[Required]
		[StringLength(30)]
		public string Password { get; set; }
		[Required]
		[StringLength(30)]
		public string Firstname { get; set; }
		[Required]
		[StringLength(30)]
		public string Lastname { get; set; }
		
		[StringLength(12)]
		public string Phone { get; set; }
		[StringLength(255)]
		public string Email { get; set; }
		public bool IsReviewer { get; set; }
		public bool IsAdmin { get; set; }
		public User() { }
	}
}
