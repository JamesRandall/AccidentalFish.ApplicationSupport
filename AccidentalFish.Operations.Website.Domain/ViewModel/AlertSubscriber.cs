using System.ComponentModel.DataAnnotations;

namespace AccidentalFish.Operations.Website.Domain.ViewModel
{
    public class AlertSubscriber
    {
        [Required]
        [MaxLength(256)]
        public string Email { get; set; }

        public string Mobile { get; set; }
    }
}
