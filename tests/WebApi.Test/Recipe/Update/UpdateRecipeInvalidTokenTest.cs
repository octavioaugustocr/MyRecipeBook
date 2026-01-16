using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using System.Net;

namespace WebApi.Test.Recipe.Update
{
    public class UpdateRecipeInvalidTokenTest : MyRecipeBookClassFixture
    {
        private const string METHOD = "recipe";

        public UpdateRecipeInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        {   
        }

        [Fact]
        public async Task Error_Token_Invalid()
        {
            var id = IdEncripterBuilder.Build().Encode(1);

            var request = RequestRecipeJsonBuilder.Build();

            var response = await DoPut(method: $"{METHOD}/{id}", request: request, token: "tokenInvalid");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Without_Token()
        {
            var id = IdEncripterBuilder.Build().Encode(1);

            var request = RequestRecipeJsonBuilder.Build();

            var response = await DoPut(method: $"{METHOD}/{id}", request: request, token: string.Empty);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Token_With_User_NotFound()
        {
            var id = IdEncripterBuilder.Build().Encode(1);

            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var request = RequestRecipeJsonBuilder.Build();

            var response = await DoPut(method: $"{METHOD}/{id}", request: request, token: "tokenInvalid");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
