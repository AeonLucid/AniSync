using System.ComponentModel.DataAnnotations;

namespace AniSync.ViewModels.Config
{
    public class TautulliViewModel
    {
        [Required]
        public bool Enabled { get; set; }

        [Required]
        public string Endpoint { get; set; }

        [Required]
        public string ApiKey { get; set; }
    }
}
