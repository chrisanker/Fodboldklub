using System.ComponentModel.DataAnnotations;

namespace Fodboldklub.Models
{
	public class Member
	{
		public int Id { get; set; }
		[Display(Name = "Fornavn")]
		[Required]
		public string FirstName { get; set; }
		[Display(Name = "Efternavn")]
		[Required]
		public string LastName { get; set; }
		[Display(Name = "Adresse")]
		[Required]
		public string Address { get; set; }
		[Display(Name = "Telefonnummer")]
		[Required]
		[DataType(DataType.PhoneNumber)]
		[RegularExpression(@"^[0-9\+\-\(\)]+$")]
		public string Phone { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
	}
}
