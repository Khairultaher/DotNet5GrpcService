using Grpc.Net.Client;
using GrpcServer.Protos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GrpcWebClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
		private readonly GrpcChannel channel;
		public ProductController()
		{
			channel = GrpcChannel.ForAddress("https://localhost:5001");
		}

		[HttpGet]
		public List<Product> GetAll()
		{
			var client = new ProductServiceContact.ProductServiceContactClient(channel);
			return client.GetAll(new Empty()).Products.ToList();
		}

		[HttpGet("{id}", Name = "GetProduct")]
		public IActionResult GetById(int id)
		{
			var client = new ProductServiceContact.ProductServiceContactClient(channel);
			var product = client.Get(new ProductId { Id = id });
			if (product == null)
			{
				return NotFound();
			}
			return new ObjectResult(product);
		}

		[HttpPost]
		public IActionResult Post([FromBody] Product product)
		{
			var client = new ProductServiceContact.ProductServiceContactClient(channel);
			var createdProduct = client.Insert(product);

			return CreatedAtRoute("GetProduct", new { id = createdProduct.ProductId }, createdProduct);
		}
		
		[HttpPut]
		public IActionResult Put([FromBody] Product product)
		{
			var client = new ProductServiceContact.ProductServiceContactClient(channel);
			var udpatedProduct = client.Update(product);
			if (udpatedProduct == null)
			{
				return NotFound();
			}
			return NoContent();
		}
		
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var client = new ProductServiceContact.ProductServiceContactClient(channel);
			client.Delete(new ProductId { Id = id });
			return new ObjectResult(id);
		}

	}
}
