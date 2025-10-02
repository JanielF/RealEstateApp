using Swashbuckle.AspNetCore.Annotations;

namespace RealEstateApp.Core.Application.Dtos.Account
{
    /// <summary>
    /// Parámetros para realizar la autenticacion del usuario
    /// </summary> 
    public class AuthenticationRequest
    {
        [SwaggerParameter(Description = "El correo electronico o nombre del usuario")]
        public string Input { get; set; }
        [SwaggerParameter(Description = "La constraseña del usuario")]
        public string Password { get; set; }
    }
}
