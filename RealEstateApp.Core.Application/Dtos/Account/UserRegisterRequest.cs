using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace RealEstateApp.Core.Application.Dtos.Account
{
    /// <summary>
    /// Parámetros para la creación del usuario
    /// </summary>
    public class UserRegisterRequest
    {
        [SwaggerParameter(Description = "El correo electronico del usuario")]
        public string Email { get; set; }
        [SwaggerParameter(Description = "El nombre de usuario")]
        public string Username { get; set; }
        [SwaggerParameter(Description = "El nombre del usuario")]
        public string FirstName { get; set; }
        [SwaggerParameter(Description = "El apellido del usuario")]
        public string LastName { get; set; }
        [SwaggerParameter(Description = "El documento de identidad del usuario")]
        public string DocumentId { get; set; }
        [SwaggerParameter(Description = "El numero de telefono del usuario")]
        public string PhoneNumber { get; set; }
        [SwaggerParameter(Description = "La contraseña del usuario")]
        public string? Password { get; set; }
        [SwaggerParameter(Description = "La confirmacion de la contraseña del usuario")]
        public string? ConfirmPassword { get; set; }
        [SwaggerParameter(Description = "El rol del usuario")]
        public string Role { get; set; }
        [SwaggerParameter(Description = "Link de la foto de perfil del usuario")]
        public string? UserImagePath { get; set; }
        public IFormFile? UserImage { get; set; }


    }
}
