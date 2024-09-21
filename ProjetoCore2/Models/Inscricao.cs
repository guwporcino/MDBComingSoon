using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ProjetoCore2.Validations;

namespace ProjetoCore2.Models
{
    public class Inscricao
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Pessoa")]
        public int PessoaId { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public List<Modelo> Modelos { get; set; }
    }

    public class Pessoa
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Nome completo")]
        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        public string Nome { get; set; }

        [DisplayName("E-mail")]
        [Required(ErrorMessage = "O campo E-mail é obrigatório")]
        public string Email { get; set; }

        [DisplayName("Cpf")]
        [Required(ErrorMessage = "O campo CPF é obrigatório")]
        [CpfValidation(ErrorMessage = "CPF inválido")]
        public string Cpf { get; set; }

        [DisplayName("Celular com DDD")]
        [Required(ErrorMessage = "O campo Telefone é obrigatório")]
        [TelefoneValidation(ErrorMessage = "Celular inválido")]
        public string Telefone { get; set; }

        [ForeignKey("Nivel")]
        [DisplayName("Nível do modelista")]
        [Required(ErrorMessage = "O campo Nível do modelista é obrigatório")]
        public int NivelId { get; set; }
        public virtual Nivel Nivel { get; set; }
    }

    public class Modelo
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Descrição do modelo")]
        [Required(ErrorMessage = "O campo Descrição do modelo é obrigatório")]
        public string Descricao { get; set; }

        [ForeignKey("Escala")]
        [Required(ErrorMessage = "O campo Escala é obrigatório")]
        public int EscalaId { get; set; }
        public virtual Escala Escala { get; set; }
        [Required(ErrorMessage = "O campo Fabricante é obrigatório")]
        public string Fabricante { get; set; }

        [ForeignKey("Tipo")]
        [Required(ErrorMessage = "O campo Tipo é obrigatório")]
        public int TipoId { get; set; }
        public virtual Tipo Tipo { get; set; }

        [ForeignKey("Inscricao")]
        public int InscricaoId { get; set; }

        [ForeignKey("SubCategoria")]
        [Required(ErrorMessage = "O campo SubCategoria é obrigatório")]
        public int SubCategoriaId { get; set; }
        public virtual SubCategoria SubCategoria { get; set; }

        public string Filipeta { get; set; }
    }

    public class Tipo
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Tipo do Kit")]
        [Required(ErrorMessage = "O campo Tipo do Kit é obrigatório")]
        public string Descricao { get; set; }
    }

    public class Nivel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Descrição é obrigatório")]
        public string Descricao { get; set; }
    }

    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Descrição é obrigatório")]
        public string Descricao { get; set; }
    }

    public class SubCategoria
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("SubCategoria")]
        public string Descricao { get; set; }

        [ForeignKey("Categoria")]
        [Required(ErrorMessage = "O campo Categoria é obrigatório")]
        public int CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }
    }

    public class Escala
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("SubCategoria")]
        [Required(ErrorMessage = "O campo Descrição é obrigatório")]
        public string Descricao { get; set; }
    }
}