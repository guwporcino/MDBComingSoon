using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ProjetoCoreDash.Models
{
    public class Inscricao
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Pessoa")]
        public int PessoaId { get; set; }
        [DisplayName("Ano da inscrição")]
        public string AnoInscricao { get; set; }
        public string TipoEvento { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public virtual List<Modelo> Modelos { get; set; }
    }

    public class Pessoa
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Nome completo")]
        public string Nome { get; set; }
        [DisplayName("E-mail")]
        public string Email { get; set; }
        [DisplayName("Cpf")]
        public string Cpf { get; set; }
        [DisplayName("Telefone com DDD")]
        public string Telefone { get; set; }
        [ForeignKey("Nivel")]
        [DisplayName("Nível do modelista")]
        public int NivelId { get; set; }
        public virtual Nivel Nivel { get; set; }
    }

    public class Modelo
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Descrição do modelo")]
        public string Descricao { get; set; }
        [ForeignKey("Escala")]
        public int EscalaId { get; set; }
        public virtual Escala Escala { get; set; }
        public string Fabricante { get; set; }
        [ForeignKey("Tipo")]
        public int TipoId { get; set; }
        public virtual Tipo Tipo { get; set; }
        [ForeignKey("Inscricao")]
        public int InscricaoId { get; set; }
        public virtual Inscricao Inscricao { get; set; }
        [ForeignKey("SubCategoria")]
        public int SubCategoriaId { get; set; }
        public virtual SubCategoria SubCategoria { get; set; }
        public string Filipeta { get; set; }
    }

    public class Tipo
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Tipo do Kit")]
        public string Descricao { get; set; }
    }

    public class Nivel
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Descrição")]
        public string Descricao { get; set; }
    }

    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Descrição")]
        public string Descricao { get; set; }
    }

    public class SubCategoria
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("SubCategoria")]
        public string Descricao { get; set; }
        [ForeignKey("Categoria")]
        public int CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }
    }

    public class Escala
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Escala")]
        public string Descricao { get; set; }
    }
}