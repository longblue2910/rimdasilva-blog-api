using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Post.Domain.AggregatesModel.UserAggregate;
using Post.Domain.Exceptions;

namespace Post.Api.Applications.Commands.User;

public class CreateUserCommandHandler(IUserRepository repository, IMapper mapper, UserManager<ApplicationUser> userManager) : IRequestHandler<CreateUserCommand, bool>
{ 
    private readonly IUserRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Kiểm tra xem người dùng đã tồn tại hay chưa
        if (!(await _repository.IsUserValidForCreation(request.Username))) 
            throw new PostDomainException("Người dùng đã tồn tại.");

        var user = _mapper.Map<ApplicationUser>(request);
        user.UserType = (UserType)request.UserType;

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            string errMsg = string.Join(",", result.Errors.Select(x => x.Description));
            throw new PostDomainException(errMsg);
        }

        return true;
    }
}
