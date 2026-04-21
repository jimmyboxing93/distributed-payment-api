#pragma warning disable SKEXP0050
using AIFinancialService.Models;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Text;


namespace AIFinancialService.Services
{
	public class KnowledgeService
	{

		private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingSerivce;
		private readonly VectorStoreCollection<Guid, PolicyRecord> _collection;

		public KnowledgeService(IEmbeddingGenerator<string, Embedding<float>> embeddingSerivce, VectorStore vectorStore)
		{
			_embeddingSerivce = embeddingSerivce;
			_collection = vectorStore.GetCollection<Guid, PolicyRecord>("bank_policies");
		}

		public async Task IngestPolicyAsync(string fullText)
		{
			await _collection.EnsureCollectionExistsAsync();

			var chunks = TextChunker.SplitPlainTextParagraphs(new List<string> { fullText }, 500);

			foreach (var chunk in chunks)
			{
				// Generate embedding using the new Microsoft.Extensions.AI standard
				var embeddings = await _embeddingSerivce.GenerateAsync(new List<string> { chunk });

				var record = new PolicyRecord
				{
					Id = Guid.NewGuid(),
					Content = chunk,
					Embedding = embeddings[0].Vector
				};
				await _collection.UpsertAsync(record);
			}
		}

		public async Task<string> SearchPoliciesAsync(string query)
		{
			// 1. Turn the user's question into a 768-dimension vector
			var queryEmbedding = await _embeddingSerivce.GenerateAsync(new List<string> { query });
			var queryVector = queryEmbedding[0].Vector;

			// Configure options
			var searchOptions = new VectorSearchOptions <PolicyRecord>
			{
				IncludeVectors = false
			};

			// Search
			var searchResult = _collection.SearchAsync(queryVector, top: 2, searchOptions);

			// Extract the data
			var results = new List<string>();

			await foreach (var result in searchResult) 
			{
				results.Add(result.Record.Content);
			}

			return string.Join("\n", results);
		}
	}
}
