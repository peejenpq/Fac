using System.ComponentModel.DataAnnotations;

namespace TIROTAPI.Models.SensorViewModels
{
    public class SensorViewModel
    {

        [Required]
        [StringLength(50)]
        [Display(Name = "Sensor ID")]
        public string SensorID { get; set; }

        [Required]
        [Display(Name = "Sensor Value")]
        public string SensorValue { get; set; }
    }
}