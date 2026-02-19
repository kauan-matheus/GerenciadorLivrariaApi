using GerenciadorLivraria.Model;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorLivraria.Controllers;

[ApiController]
[Route("api/books")]
public class LivroController : ControllerBase
{
    //to criando a lista de livros para simular um banco de dados, deixando static para manter os dados
    private static readonly List<Livro> livros = new();

    private static readonly List<string> genres = new()
    {
        "Ficção", "Não Ficção", "Fantasia", "Romance",
        "Mistério", "Terror", "Aventura", "História", "Biografia"
    };

    [HttpPost]
    public IActionResult Post([FromBody] Livro livro)
    {
        bool livroDuplicado = false;

        //foreach pra cada l de livros verificando se existe titulo e autor iguais, se sim, livroDuplicado recebe true e sai do loop
        foreach (var l in livros)
        {
            if (l.Title == livro.Title && l.Author == livro.Author)
            {
                livroDuplicado = true;
                break;
            }
        }

        if (livroDuplicado)
            return Conflict("Livro já existe");

        if (livro.Price < 0)
            return BadRequest("Preço não pode ser negativo");

        if (livro.Stock < 0)
            return BadRequest("Estoque não pode ser negativo");

        //esse .Contains verifica se algo existe em alguma lista ou qlqr outra coisa
        if (!genres.Contains(livro.Genre))
            return BadRequest("Gênero não existe");

        //isso aqui e pra gerar um id unico para cada livro, usando Guid.NewGuid() que gera um id aleatorio
        livro.Id = Guid.NewGuid();

        livros.Add(livro);

        return CreatedAtAction(nameof(GetById), new { id = livro.Id }, livro);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        //aqui so retorno todos os livros msm
        return Ok(livros);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        //aqui eu to procurando o livro com o id que veio na url, usando um foreach
        //pra cada livro da lista de livros, se o id do livro for igual ao id que veio na url
        //eu salvo esse livro na variavel livro e saio do loop
        Livro? livro = null;

        foreach (var l in livros)
        {
            if (l.Id == id)
            {
                livro = l;
                break;
            }
        }

        if (livro is null)
            return NotFound("Livro não encontrado");

        return Ok(livro);
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, [FromBody] Livro livroAtualizado)
    {
        //aqui eu to procurando o livro com o id que veio na url, usando um foreach
        Livro? livro = null;

        //pra cada livro da lista de livros, se o id do livro for igual ao id que veio na url
        foreach (var l in livros)
        {
            if (l.Id == id)
            {
                livro = l;
                break;
            }
        }

        if (livro == null)
            return NotFound("Livro não encontrado");

        if (livroAtualizado.Price < 0)
            return BadRequest("Preço não pode ser negativo");

        if (livroAtualizado.Stock < 0)
            return BadRequest("Estoque não pode ser negativo");

        if (!genres.Contains(livroAtualizado.Genre))
            return BadRequest("Gênero não existe");

        //aqui eu to atualizando os dados do livro encontrado com os dados do livro atualizado que veio no corpo da req
        livro.Title = livroAtualizado.Title;
        livro.Author = livroAtualizado.Author;
        livro.Genre = livroAtualizado.Genre;
        livro.Price = livroAtualizado.Price;
        livro.Stock = livroAtualizado.Stock;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        Livro? livro = null;

        foreach (var l in livros)
        {
            if (l.Id == id)
            {
                livro = l;
                break;
            }
        }

        if (livro == null)
            return NotFound("Livro não encontrado");

        //aqui eu to removendo o livro encontrado da lista de livros usando o metodo .Remove() 
        livros.Remove(livro);

        return NoContent();
    }
}
