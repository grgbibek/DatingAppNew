using API.Data;
using API.DTOs;
using API.Entities;
using API.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers(){
        var users = await _userRepository.GetUsersAsync();
        var userToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
        return Ok(userToReturn);
    } 

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username){
        return await _userRepository.GetMemberAsync(username);
    } 
}
