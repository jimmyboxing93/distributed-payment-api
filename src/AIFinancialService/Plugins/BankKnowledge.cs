using System.ComponentModel;
using Microsoft.SemanticKernel;
using AIFinancialService.Services;

namespace AIFinancialService.Plugins
{
	public class BankKnowledge
	{
		private readonly KnowledgeService _knowledgeSerivce;

		public BankKnowledge(KnowledgeService knowledgeSerivce) 
		{
			_knowledgeSerivce = knowledgeSerivce;
		}

		[KernelFunction("search_bank_policies")]
		[Description("Searches the bank's internal policy manual for information on fees, account rules, limits, and regulations.")]
		public async Task<string> SearchPolicies(
		[Description("The specific policy topic or question to research")] string query) 
		{
			return await _knowledgeSerivce.SearchPoliciesAsync(query);
		}
	}
}
