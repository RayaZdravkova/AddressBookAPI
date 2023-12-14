using AddressBook.Domain.Abstractions.Helpers;
using AddressBook.Domain.Abstractions.Wrappers;
using AddressBook.Domain.Enums;
using AddressBook.Domain.Models;
using AddressBook.Domain.Pagination;
using AddressBook.Domain.Parameters;
using AddressBook.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Xunit;

namespace AddressBook.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IHttpClientWrapper> _httpClientMock;
        private readonly Mock<IConfiguration> _configuration;
        private readonly Mock<IJsonSerializerHelper> _jsonSerializerWrapper;

        public UserServiceTests()
        {
            _configuration = new Mock<IConfiguration>();
            _jsonSerializerWrapper = new Mock<IJsonSerializerHelper>();
            _httpClientMock = new Mock<IHttpClientWrapper>();

            _configuration.Setup(c => c.GetSection("ApiSettings:Seed").Value).Returns("addressbook");
            _configuration.Setup(c => c.GetSection("ApiSettings:Results").Value).Returns("100");
        }

        [Fact]
        public async Task GetPaginatedUsersAsync_PassValidPagingInfo_ReturnPaginatedUsers()
        {
            var usersJson = "users json";

            HttpResponseMessage response = new();
            response.Content = new StringContent(usersJson);
            response.StatusCode = HttpStatusCode.OK;
            _httpClientMock.Setup(c => c.GetAsync(It.IsAny<string>())).ReturnsAsync(response);

            UserResponse userResponse = new UserResponse()
            {
                Results = new List<User>() 
                {
                    new User()
                    {
                        Gender = GenderEnum.female.ToString(),
                        Name = new Name()
                        {
                            Title = "Mr",
                            First = "Todor",
                            Last = "Petrov"
                        },
                        Nat = "NL",
                        Email = "t.petrov@mail.com",
                        Phone = "1234567890",
                        Location = new Location()
                        {
                            City = "Sofia",
                            Country = "Bulgaria",
                        }
                    },
                    new User()
                    {
                        Gender = GenderEnum.female.ToString(),
                        Name = new Name()
                        {
                            Title = "Mr",
                            First = "Ivan",
                            Last = "Petrov"
                        },
                        Nat = "CA",
                        Email = "i.petrov@mail.com",
                        Phone = "1234567890",
                        Location = new Location()
                        {
                            City = "Sofia",
                            Country = "Bulgaria",
                        }
                    },
                    new User()
                    {
                        Gender = GenderEnum.female.ToString(),
                        Name = new Name()
                        {
                            Title = "Mrs",
                            First = "Aleksandra",
                            Last = "Ivanova"
                        },
                        Nat = "CA",
                        Email = "ivanova.a@mail.com",
                        Phone = "1234567890",
                        Location = new Location()
                        {
                            City = "Sofia",
                            Country = "Bulgaria",
                        }
                    }
                }
            };

            _jsonSerializerWrapper.Setup(j => j.DeserializeHttpContentAsync<UserResponse>(response.Content)).ReturnsAsync(userResponse);
            PagingInfo pagingInfo = new PagingInfo();
            pagingInfo.Page = 0;
            pagingInfo.Size = 2;
            QueryParameters queryParameters = new QueryParameters();

            PaginatedResult<User> expectedResult = new PaginatedResult<User>(userResponse.Results.Take(pagingInfo.Size).ToList(),
                userResponse.Results.Count, pagingInfo.Page, pagingInfo.Size);

            var userService = new UserService(_httpClientMock.Object, _configuration.Object, _jsonSerializerWrapper.Object);
            var actual = await userService.GetPaginatedUsersAsync(pagingInfo, queryParameters);

            Assert.False(actual.Empty);
            for (int i = 0; i < expectedResult.Content.Count; i++)
            {
                Assert.Equal(expectedResult.Content[i].Gender, actual.Content[i].Gender);
                Assert.Equal(expectedResult.Content[i].Name.First, actual.Content[i].Name.First);
                Assert.Equal(expectedResult.Content[i].Email, actual.Content[i].Email);
                Assert.Equal(expectedResult.Content[i].Phone, actual.Content[i].Phone);
                Assert.Equal(expectedResult.Content[i].Location.Country, actual.Content[i].Location.Country);
                Assert.Equal(expectedResult.Content[i].Location.City, actual.Content[i].Location.City);
            }
            Assert.Equal(expectedResult.Size, actual.Size);
            Assert.Equal(expectedResult.Number, actual.Number);
            Assert.Equal(expectedResult.TotalPages, actual.TotalPages);
        }

        [Fact]
        public async Task GetPaginatedUsersAsync_PassValidFilterData_ReturnFilteredUsers()
        {
            var usersJson = "users json";

            HttpResponseMessage response = new();
            response.Content = new StringContent(usersJson);
            response.StatusCode = HttpStatusCode.OK;
            _httpClientMock.Setup(c => c.GetAsync(It.IsAny<string>())).ReturnsAsync(response);

            UserResponse userResponse = new UserResponse()
            {
                Results = new List<User>()
                {
                    new User()
                    {
                        Gender = GenderEnum.female.ToString(),
                        Name = new Name()
                        {
                            Title = "Mr",
                            First = "Todor",
                            Last = "Petrov"
                        },
                        Nat = "NL",
                        Email = "t.petrov@mail.com",
                        Phone = "1234567890",
                        Location = new Location()
                        {
                            City = "Sofia",
                            Country = "Bulgaria",
                        }
                    },
                    new User()
                    {
                        Gender = GenderEnum.male.ToString(),
                        Name = new Name()
                        {
                            Title = "Mr",
                            First = "Ivan",
                            Last = "Petrov"
                        },
                        Nat = "CA",
                        Email = "i.petrov@mail.com",
                        Phone = "1234567890",
                        Location = new Location()
                        {
                            Country = "Canada"
                        }
                    },
                    new User()
                    {
                        Gender = GenderEnum.female.ToString(),
                        Name = new Name()
                        {
                            Title = "Mrs",
                            First = "Aleksandra",
                            Last = "Ivanova"
                        },
                        Nat = "BG",
                        Email = "ivanova.a@mail.com",
                        Phone = "1234567890",
                        Location = new Location()
                        {
                            City = "Sofia",
                            Country = "Bulgaria",
                        }
                    },
                    new User()
                    {
                        Gender = GenderEnum.male.ToString(),
                        Name = new Name()
                        {
                            Title = "Mr",
                            First = "Ramon",
                            Last = "Thompson"
                        },
                        Nat = "BG",
                        Email = "ramon@mail.com",
                        Phone = "1234567890",
                        Location = new Location()
                        {
                            City = "Sofia",
                            Country = "Bulgaria",
                        }
                    }
                }
            };

            UserResponse expectedUserResponse = new UserResponse()
            {
                Results = new List<User>()
                {
                    new User()
                    {
                        Gender = GenderEnum.male.ToString(),
                        Name = new Name()
                        {
                            Title = "Mr",
                            First = "Ramon",
                            Last = "Thompson"
                        },
                        Nat = "BG",
                        Email = "ramon@mail.com",
                        Phone = "1234567890",
                        Location = new Location()
                        {
                            City = "Sofia",
                            Country = "Bulgaria",
                        }
                    }
                }
            };

            _jsonSerializerWrapper.Setup(j => j.DeserializeHttpContentAsync<UserResponse>(response.Content)).ReturnsAsync(userResponse);
            PagingInfo pagingInfo = new PagingInfo();
            pagingInfo.Page = 0;
            pagingInfo.Size = 100;
            QueryParameters queryParameters = new QueryParameters() { Gender = GenderEnum.male, Name = "Ramon" };

            PaginatedResult<User> expectedResult = new PaginatedResult<User>(expectedUserResponse.Results, userResponse.Results.Count, pagingInfo.Page, pagingInfo.Size);

            var userService = new UserService(_httpClientMock.Object, _configuration.Object, _jsonSerializerWrapper.Object);
            var actual = await userService.GetPaginatedUsersAsync(pagingInfo, queryParameters);

            Assert.False(actual.Empty);
            for (int i = 0; i < expectedResult.Content.Count; i++)
            {
                Assert.Equal(expectedResult.Content[i].Gender, actual.Content[i].Gender);
                Assert.Equal(expectedResult.Content[i].Name.First, actual.Content[i].Name.First);
                Assert.Equal(expectedResult.Content[i].Email, actual.Content[i].Email);
                Assert.Equal(expectedResult.Content[i].Phone, actual.Content[i].Phone);
                Assert.Equal(expectedResult.Content[i].Location.Country, actual.Content[i].Location.Country);
                Assert.Equal(expectedResult.Content[i].Location.City, actual.Content[i].Location.City);
            }
        }

        [Fact]
        public async Task GetPaginatedUsersAsync_PassNegativePageNumberAndSize_ThrowsValidationException()
        {
            var usersJson = "users json";

            HttpResponseMessage response = new();
            response.Content = new StringContent(usersJson);
            response.StatusCode = HttpStatusCode.OK;
            _httpClientMock.Setup(c => c.GetAsync(It.IsAny<string>())).ReturnsAsync(response);

            UserResponse userResponse = new UserResponse()
            {
                Results = new List<User>()
                {
                    new User()
                    {
                        Gender = GenderEnum.female.ToString(),
                        Name = new Name()
                        {
                            Title = "Mr",
                            First = "Todor",
                            Last = "Petrov"
                        },
                        Nat = "NL",
                        Email = "t.petrov@mail.com",
                        Phone = "1234567890",
                        Location = new Location()
                        {
                            City = "Sofia",
                            Country = "Bulgaria",
                        }
                    },
                    new User()
                    {
                        Gender = GenderEnum.female.ToString(),
                        Name = new Name()
                        {
                            Title = "Mr",
                            First = "Ivan",
                            Last = "Petrov"
                        },
                        Nat = "CA",
                        Email = "i.petrov@mail.com",
                        Phone = "1234567890",
                        Location = new Location()
                        {
                            City = "Sofia",
                            Country = "Bulgaria",
                        }
                    },
                    new User()
                    {
                        Gender = GenderEnum.female.ToString(),
                        Name = new Name()
                        {
                            Title = "Mrs",
                            First = "Aleksandra",
                            Last = "Ivanova"
                        },
                        Nat = "CA",
                        Email = "ivanova.a@mail.com",
                        Phone = "1234567890",
                        Location = new Location()
                        {
                            City = "Sofia",
                            Country = "Bulgaria",
                        }
                    }
                }
            };

            _jsonSerializerWrapper.Setup(j => j.DeserializeHttpContentAsync<UserResponse>(response.Content)).ReturnsAsync(userResponse);
            PagingInfo pagingInfo = new PagingInfo();
            pagingInfo.Page = -1;
            pagingInfo.Size = -2;
            QueryParameters queryParameters = new QueryParameters();

            PaginatedResult<User> expectedResult = new PaginatedResult<User>(userResponse.Results.Take(pagingInfo.Size).ToList(),
                userResponse.Results.Count, pagingInfo.Page, pagingInfo.Size);

            var userService = new UserService(_httpClientMock.Object, _configuration.Object, _jsonSerializerWrapper.Object);

            await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await userService.GetPaginatedUsersAsync(pagingInfo, queryParameters);
            });
        }

        [Fact]
        public async Task GetPaginatedUsersAsync_ApiCallReturnsErrorStatusCode_ThrowsHttpRequestException()
        {
            var usersJson = "users json";

            HttpResponseMessage response = new();
            response.Content = new StringContent(usersJson);
            response.StatusCode = HttpStatusCode.BadRequest;
            _httpClientMock.Setup(c => c.GetAsync(It.IsAny<string>())).ReturnsAsync(response);

            PagingInfo pagingInfo = new PagingInfo();
            QueryParameters queryParameters = new QueryParameters();

            var userService = new UserService(_httpClientMock.Object, _configuration.Object, _jsonSerializerWrapper.Object);

            await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await userService.GetPaginatedUsersAsync(pagingInfo, queryParameters);
            });
        }

        [Fact]
        public async Task GetPaginatedUsersAsync_NotExistingUsers_ReturnEmptyPaginatedResult()
        {
            var usersJson = "users json";

            HttpResponseMessage response = new();
            response.Content = new StringContent(usersJson);
            response.StatusCode = HttpStatusCode.OK;
            _httpClientMock.Setup(c => c.GetAsync(It.IsAny<string>())).ReturnsAsync(response);

            UserResponse userResponse = new UserResponse()
            {
                Results = new List<User>()
            };

            _jsonSerializerWrapper.Setup(j => j.DeserializeHttpContentAsync<UserResponse>(response.Content)).ReturnsAsync(userResponse);
            PagingInfo pagingInfo = new PagingInfo();
            pagingInfo.Page = 1;
            pagingInfo.Size = 10;
            QueryParameters queryParameters = new QueryParameters();

            PaginatedResult<User> expectedResult = new PaginatedResult<User>(userResponse.Results.Take(pagingInfo.Size).ToList(),
                userResponse.Results.Count, pagingInfo.Page, pagingInfo.Size);

            var userService = new UserService(_httpClientMock.Object, _configuration.Object, _jsonSerializerWrapper.Object);
            var actual = await userService.GetPaginatedUsersAsync(pagingInfo, queryParameters);

            Assert.True(actual.Empty);
            Assert.Empty(actual.Content);
            Assert.Equal(expectedResult.TotalPages, actual.TotalPages);
        }

        [Fact]
        public async Task GetPaginatedUsersAsync_PassUnexistingPage_ReturnEmptyPaginatedResult()
        {
            var usersJson = "users json";

            HttpResponseMessage response = new();
            response.Content = new StringContent(usersJson);
            response.StatusCode = HttpStatusCode.OK;
            _httpClientMock.Setup(c => c.GetAsync(It.IsAny<string>())).ReturnsAsync(response);

            UserResponse userResponse = new UserResponse()
            {
                Results = new List<User>()
            };

            _jsonSerializerWrapper.Setup(j => j.DeserializeHttpContentAsync<UserResponse>(response.Content)).ReturnsAsync(userResponse);
            PagingInfo pagingInfo = new PagingInfo();
            pagingInfo.Page = 1000;
            pagingInfo.Size = 1000;
            QueryParameters queryParameters = new QueryParameters();

            PaginatedResult<User> expectedResult = new PaginatedResult<User>(userResponse.Results.Take(pagingInfo.Size).ToList(),
                userResponse.Results.Count, pagingInfo.Page, pagingInfo.Size);

            var userService = new UserService(_httpClientMock.Object, _configuration.Object, _jsonSerializerWrapper.Object);
            var actual = await userService.GetPaginatedUsersAsync(pagingInfo, queryParameters);

            Assert.True(actual.Empty);
            Assert.Empty(actual.Content);
        }

    }
}
