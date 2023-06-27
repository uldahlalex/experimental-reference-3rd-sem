using System.ComponentModel.DataAnnotations;
using api.ActionFilters;
using api.Helpers;
using api.TransferObjects;
using Infrastructure.DataModels;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace api.Endpoints;

public class AuthenticationEndpoints : ControllerBase
{
    private readonly ResponseHelper _response;
    private readonly AuthenticationService _auth;

    public AuthenticationEndpoints(
        ResponseHelper response,
        AuthenticationService auth)
    {
        _response = response;
        _auth = auth;
    }

    [HttpPost]
    [Route("/api/register")]
    [ValidateModelFilter]
    public ResponseDto Register([FromBody] RegisterOrLoginRequestDto dto)
    {
        var result = _auth.Register(dto.Email, dto.Password);
        return _response.Success(HttpContext, 201, "Register successful",
            _auth.IssueToken(_auth.DefaultKeyValuesForJwtPayload(result)));
    }

    [HttpPost]
    [Route("/api/login")]
    [ValidateModelFilter]
    public ResponseDto Login([FromBody] RegisterOrLoginRequestDto dto)
    {
        EndUser result = _auth.LogIn(dto.Email, dto.Password);
        return _response.Success(HttpContext, 200, "Login successful",
            _auth.IssueToken(_auth.DefaultKeyValuesForJwtPayload(result)));
    }

    [HttpDelete]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [Route("/api/deleteSignedInUser")]
    public ResponseDto DeleteMe()
    {
        EndUser endUser = (HttpContext.Items["user"] as EndUser)!;

        _auth.DeleteUserById(endUser.EndUserId);
        return _response.Success(HttpContext, 201, "User has been deleted");
    }


    [HttpGet]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [Route("/api/verifyTokenIntegrity")]
    public ResponseDto VerifyTokenIntegrity()
    {
        EndUser endUser = (HttpContext.Items["user"] as EndUser)!;

        return _response.Success(HttpContext, 200, "Token is OK",
            _auth.IssueToken(_auth.DefaultKeyValuesForJwtPayload(endUser)));
    }


    [HttpPut]
    [ValidateModelFilter]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [Route("/api/avatar")]
    public ResponseDto UpdateAvatar([FromBody] AvatarRequestDto avatarRequestDto)
    {
        EndUser endUser = (HttpContext.Items["user"] as EndUser)!;

        _auth.UpdateAvatarForUser(endUser.EndUserId, avatarRequestDto.PravatarId);
        return _response.Success(HttpContext, 201, "Avatar has been updated");

    }


    
}