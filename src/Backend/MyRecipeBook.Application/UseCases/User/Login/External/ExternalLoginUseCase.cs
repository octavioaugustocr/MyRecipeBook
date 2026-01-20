
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Tokens;

namespace MyRecipeBook.Application.UseCases.User.Login.External
{
    public class ExternalLoginUseCase : IExternalLoginUseCase
    {
        private readonly IUserReadOnlyRepository _repositoryRead;
        private readonly IUserWriteOnlyRepository _repositoryWrite;
        private readonly IUnitOfWork _unitOfWork;
        private IAccessTokenGenerator _accessTokenGenerator;

        public ExternalLoginUseCase(IUserReadOnlyRepository repositoryRead, IUserWriteOnlyRepository repositoryWrite, IUnitOfWork unitOfWork, IAccessTokenGenerator accessTokenGenerator)
        {
            _repositoryRead = repositoryRead;
            _repositoryWrite = repositoryWrite;
            _unitOfWork = unitOfWork;
            _accessTokenGenerator = accessTokenGenerator;
        }

        public async Task<string> Execute(string name, string email)
        {
            var user = await _repositoryRead.GetByEmail(email);
            
            if (user is null)
            {
                user = new Domain.Entities.User
                {
                    Name = name,
                    Email = email,
                    Password = "-"
                };

                await _repositoryWrite.Add(user);
                await _unitOfWork.Commit();
            }

            return _accessTokenGenerator.Generate(user.UserIdentifier);
        }
    }
}
