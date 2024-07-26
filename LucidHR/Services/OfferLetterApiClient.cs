using LucidHR.Models;
using LucidHR.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LucidHR.Controllers
{
    public class OfferLetterApiClient : IOfferLetterApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OfferLetterApiClient> _logger;

        public OfferLetterApiClient(HttpClient httpClient, ILogger<OfferLetterApiClient> logger)
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            });

            _logger = logger;
        }

        public async Task<(bool IsSuccess, string Content)> GenerateOfferLetterAsync(OfferLetterViewModel model)
        {
            try
            {
                _logger.LogInformation("Sending request to generate offer letter for {@Model}", model);
                var response = await _httpClient.PostAsJsonAsync("https://localhost:44383/api/OfferLetter", model);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Offer letter generated successfully");
                    return (true, content);
                }
                else
                {
                    _logger.LogWarning("Failed to generate offer letter. Status code: {StatusCode}", response.StatusCode);
                    return (false, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calling the offer letter API");
                return (false, null);
            }
        }
        //public async Task<bool> StorePdfAsync(string email, byte[] pdfBytes)
        //{
        //    try
        //    {
        //        var content = new MultipartFormDataContent();
        //        content.Add(new ByteArrayContent(pdfBytes), "pdf", "OfferLetter.pdf");
        //        content.Add(new StringContent(email), "email");

        //        var response = await _httpClient.PostAsync("https://localhost:44383/api/OfferLetter/StorePdf", content);
        //        return response.IsSuccessStatusCode;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while storing the PDF");
        //        return false;
        //    }
        //}
        public async Task<byte[]> GetOfferLetterByEmailAsync(string email)
        {
            var response = await _httpClient.GetAsync($"https://localhost:44383/api/OfferLetter/GetOfferLetterByEmail?email={Uri.EscapeDataString(email)}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }

            _logger.LogWarning("Failed to get offer letter. Status code: {StatusCode}", response.StatusCode);
            return null;
        }
    }
}
