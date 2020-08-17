using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Mon2ndSite.Models
{
    public class RestoVoteViewModel : IValidatableObject
    {
        public List<RestoCheckBoxViewModel> ListeDesResto { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ListeDesResto.Any(r => r.EstSelectionne))
                yield return new ValidationResult("Vous devez choisir au moins un restaurant", new[] { "ListeDesResto" });
        }
    }
}