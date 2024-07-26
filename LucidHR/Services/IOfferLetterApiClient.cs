using LucidHR.Models;

namespace LucidHR.Services
{
    public interface IOfferLetterApiClient
    {
        Task<(bool IsSuccess, string Content)> GenerateOfferLetterAsync(OfferLetterViewModel model);
        //Task<bool> StorePdfAsync(string email, byte[] pdfBytes);
        Task<byte[]> GetOfferLetterByEmailAsync(string email);
    }
}