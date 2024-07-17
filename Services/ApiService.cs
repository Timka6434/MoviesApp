using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MoviesApp.Models
{
    class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiToken = "d8f95174-e175-42d9-97fc-627323d633f4";

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiToken);
        }

        public async Task<string> GetApiDataAsync(string url)
        {
            try
            {
                // Вывод заголовков для отладки
                Debug.WriteLine("Request Headers:");
                foreach (var header in _httpClient.DefaultRequestHeaders)
                {
                    Debug.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }

                Debug.WriteLine($"Request URL: {url}");

                HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);

                // Вывод отладочной информации о статусе ответа
                Debug.WriteLine($"Response Status Code: {(int)responseMessage.StatusCode} {responseMessage.ReasonPhrase}");

                responseMessage.EnsureSuccessStatusCode(); // Ответ в случае ошибки, выводит в консоль
                string responseData = await responseMessage.Content.ReadAsStringAsync();
                return responseData;
            }
            catch (HttpRequestException httpRequestException)
            {
                Debug.WriteLine($"HttpRequestException: {httpRequestException.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка получения данных из API: {ex.Message}");
                return null;
            }
        }
    }
}
