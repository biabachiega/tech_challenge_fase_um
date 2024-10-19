using System.ComponentModel.DataAnnotations;

namespace TechChallengeFaseUm.Entities
{
    public class ContatosUpdateRequest
    {
        public string name { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
    }
}
