using System.ComponentModel.DataAnnotations;

namespace TechChallengeFaseUm.Entities
{
    public class ContatosUpdateRequest
    {
        public string? name { get; set; }
        [EmailAddress(ErrorMessage = "Email em formato inválido.")]
        public string? email { get; set; }
        [RegularExpression(@"^\(\d{2}\) \d{4,5}-\d{4}$", ErrorMessage = "Telefone em formato inválido. Exemplo: (11) 91234-5678")]
        public string? telefone { get; set; }
    }
}
