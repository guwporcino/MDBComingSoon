using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDBComingSoon.Models
{
    public class InscricaoViewModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Pessoa")]
        public int PessoaId { get; set; }
        public Pessoa Pessoa { get; set; }
        public List<Modelo> Modelos { get; set; }
    }

    public class Pessoa
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Nome completo")]
        public string Nome { get; set; }
        [DisplayName("E-mail")]
        public string Email { get; set; }
        [DisplayName("Carteira de identidade")]
        public string Identidade { get; set; }
        [DisplayName("Telefone com DDD")]
        public string Telefone { get; set; }
        [DisplayName("Nível do Modelista")]
        public string NivelModelista { get; set; }
    }

    public class Modelo
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Descrição do modelo")]
        public string Descricao { get; set; }
        public string Formato { get; set; }
        public string Fabricante { get; set; }
        [ForeignKey("Tipo")]
        public int TipoId { get; set; }
        public Tipo Tipo { get; set; }
    }

    public class Tipo
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Tipo do Kit")]
        public string Descricao { get; set; }
    }
}