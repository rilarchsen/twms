﻿namespace TimeWorkedManagementSystem.DTOs;

public class EditUserRequest
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}