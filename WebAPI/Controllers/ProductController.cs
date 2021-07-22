using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork<Product> _products;


        public ProductController(IUnitOfWork<Product> products)
        {
            _products = products;
        }


        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _products.Entity.GetAll();
        }

        [Authorize]
        [HttpGet("{id}")]    
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetProductById(int id) 
        {
            Product product = await _products.Entity.GetByID(id);

            if (product != null)
                return Ok(product);

            return NotFound();
        }

        [HttpPost]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateProduct(Product product)
        {
            await _products.Entity.Create(product);

            await _products.Save();

            return CreatedAtAction(nameof(GetProductById), new { id = product.ID }, product);
        }

        [Authorize]
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(int id, Product product)
        {
            if (id != product.ID)
                return BadRequest();

            _products.Entity.UpdateByID(id, product);

            try
            {
                await _products.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            if (await _products.Entity.DeleteByID(id) == null)
                return NotFound();

            await _products.Save();

            return NoContent();
        }
    }
}
