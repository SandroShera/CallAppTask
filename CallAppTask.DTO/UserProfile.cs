
using System.ComponentModel.DataAnnotations;

namespace CallAppTask.DTO
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [MinLength(11)]
        [MaxLength(11)]
        public string PersonalNumber { get; set; }
    }
}
