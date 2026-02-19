using System.ComponentModel.DataAnnotations;

namespace GerenciadorLivraria.Model;

public class Livro
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [StringLength(100, MinimumLength = 2)]
    public string Title { get; set; } = String.Empty;
    [StringLength(100, MinimumLength = 2)]
    public string Author { get; set; } = String.Empty;
    public string Genre { get; set; } = String.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
