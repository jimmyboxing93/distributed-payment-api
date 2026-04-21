using Microsoft.Extensions.VectorData;

namespace AIFinancialService.Models
{
	public class PolicyRecord
	{
		[VectorStoreKey]
		public Guid Id { get; set; } = Guid.NewGuid();

		[VectorStoreData]
		public string Content { get; set; }

		[VectorStoreVector(3072)]
		public ReadOnlyMemory<float> Embedding { get; set; }

	}
}
