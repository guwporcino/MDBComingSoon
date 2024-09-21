using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoCore2.Models
{
    [Table("Posts")]
    public class BlogEntry
    {
        [Key]
        public int Id { get; set; }
        public string Conteudo { get; set; }
    }
}
