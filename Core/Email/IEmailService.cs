﻿using System.Threading.Tasks;

namespace Core.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}