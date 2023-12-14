using AddressBook.Domain.Abstractions.Helpers;
using AddressBook.Domain.Abstractions.Services;
using AddressBook.Domain.Abstractions.Wrappers;
using AddressBook.Domain.Enums;
using AddressBook.Domain.Models;
using AddressBook.Domain.Pagination;
using AddressBook.Domain.Parameters;
using Microsoft.Extensions.Configuration;

namespace AddressBook.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly IJsonSerializerHelper _jsonSerializerHelper;
        private readonly string? _seed;
        private readonly string? _results;

        public UserService(IHttpClientWrapper httpClient, IConfiguration configuration, IJsonSerializerHelper jsonSerializerHelper)
        {
            _seed = configuration.GetSection("ApiSettings:Seed").Value;
            _results = configuration.GetSection("ApiSettings:Results").Value;
            _jsonSerializerHelper = jsonSerializerHelper;
            _httpClient = httpClient;
        }

        private async Task<List<User>> GetFilteredUsersAsync(QueryParameters queryParameters)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"?seed={_seed}&results={_results}");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Failed to fetch user data. Status code: " + response.StatusCode);
            }

            var userResponse = await _jsonSerializerHelper.DeserializeHttpContentAsync<UserResponse>(response.Content);

            return userResponse is null ? new List<User>() : FilterUsersByParameters(userResponse.Results, queryParameters);
        }

        public async Task<PaginatedResult<User>> GetPaginatedUsersAsync(PagingInfo pagingInfo, QueryParameters parameters)
        {
            var filteredUsers = await GetFilteredUsersAsync(parameters);

            return filteredUsers.Paginate(pagingInfo.Page, pagingInfo.Size);
        }

        private static List<User> FilterUsersByParameters(List<User> users, QueryParameters queryParameters)
        {
            users = FilterUsersByGender(users, queryParameters.Gender);
            users = FilterUsersByName(users, queryParameters.Name);
            users = FilterUsersByCountry(users, queryParameters.Country);
            users = FilterUsersByCity(users, queryParameters.City);

            return users;
        }

        private static List<User> FilterUsersByGender(List<User> users, GenderEnum? gender)
        {
            return gender is null ? users 
                : users.Where(u => u.Gender == gender.ToString()).ToList();
        }

        private static List<User> FilterUsersByName(List<User> users, string? name)
        {
            return name is null ? users
                : users.Where(u => (u.Name.First + " " + u.Name.Last).ToLower().Contains(name.ToLower())).ToList();
        }

        private static List<User> FilterUsersByCountry(List<User> users, string? country)
        {
            return country is null ? users 
                : users.Where(u => u.Location.Country.ToLower().Contains(country.ToLower())).ToList();
        }

        private static List<User> FilterUsersByCity(List<User> users, string? city)
        {
            return city is null ? users
             : users.Where(u => u.Location.City.ToLower().Contains(city.ToLower())).ToList();
        }
    }
}
