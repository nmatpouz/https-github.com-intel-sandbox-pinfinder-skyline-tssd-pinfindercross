public class MockApiService
{
    // Simulate checking existence of LotNumber and Operation
    public Task<bool> CheckExistenceAsync(string lotNumber, string operation)
    {
        // Mock response: LotNumber "123" and Operation "A" exist
        if (lotNumber == "ABC1234" && operation == "1234")
        {
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    // Simulate fetching summaries for a given LotNumber and Operation
    public Task<List<string>> GetSummariesAsync(string lotNumber, string operation)
    {
        // Mock response for LotNumber "123" and Operation "A"
        if (lotNumber == "ABC1234" && operation == "1234")
        {
            return Task.FromResult(new List<string> { "1A.gz", "2B.gz" });
        }
        return Task.FromResult(new List<string>());
    }
}