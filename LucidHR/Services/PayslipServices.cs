using Newtonsoft.Json;

namespace LucidHR.Services
{
    public class PayslipService
    {
        private readonly HttpClient _httpClient;

        public PayslipService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<byte[]> GeneratePayslipAsync(int uemail, string month, int year)
        {
            var response = await _httpClient.GetAsync($"https://localhost:44383/api/Payslip/generate?email={uemail}&month={month}&year={year}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            return null;
        }
        public async Task<List<string>> GetPayslipByEmailAsync(string email)
        {
            var response = await _httpClient.GetAsync($"https://localhost:44383/api/Payslip/GetpayslipByemailAlL?email={email}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<string>>(content);
            }

            return null;
        }
    }

}
