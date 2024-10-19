using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TechChallengeFaseUm.Entities
{
    public class ContatosRequest
    {
        [Required(ErrorMessage = "Nome é obrigatório.")]
        public string name { get; set; }

        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email em formato inválido.")]
        public string email { get; set; }

        [Required(ErrorMessage = "Telefone é obrigatório.")]
        [RegularExpression(@"^\(\d{2}\) \d{4,5}-\d{4}$", ErrorMessage = "Telefone em formato inválido. Exemplo: (11) 91234-5678")]
        public string telefone { get; set; }
    }
}
