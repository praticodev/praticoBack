using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pratico.Api.Data;
using Pratico.Api.Extensions;
using Pratico.Api.ViewModels;
using Pratico.Business.Services;
using Pratico.Business.Utils;
using Pratico.Data.Repository;
using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Api.Controllers
{
    [Route("api/Auth")]    
    public class AuthController : MainController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;
        private readonly ICondominioRepository _condominioRepository;
        private readonly IUsuarioCondominioService _usuarioCondominioService;
        private readonly IPessoaService _pessoaService;
        private readonly IPessoaUsuarioService _pessoaUsuario;
        private readonly IMapper _mapper;
        private readonly IProprietarioImovelService _proprietarioImovelService;
        private readonly IImovelService _imovelService;
        private readonly IUsuarioSistemaService _usuarioSistemaService;
        private readonly IOperadorRepository _operadorRepository;
        private readonly IOperadorService _operadorService;
        private readonly IEmailService _emailService;
        public AuthController(INotificador notificador,
                              SignInManager<ApplicationUser> signInManager,
                              UserManager<ApplicationUser> userManager,
                              IOptions<AppSettings> appSettings,
                              IUser user, ILogger<AuthController> logger,
                              ICondominioRepository condominioRepository,
                              IPessoaService pessoaService,
                              IPessoaUsuarioService pessoaUsuario,
                              IMapper mapper,
                              IProprietarioImovelService proprietarioImovelService,
                              IUsuarioCondominioService usuarioCondominioService,
                              IImovelService imovelService, 
                              IUsuarioSistemaService usuarioSistemaService,
                              IOperadorRepository operadorRepository,
                              IOperadorService operadorService,
                              IEmailService emailService) : base(notificador, user)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _appSettings = appSettings.Value;
            _condominioRepository = condominioRepository;
            _usuarioCondominioService = usuarioCondominioService;
            _pessoaService = pessoaService;
            _pessoaUsuario = pessoaUsuario;
            _proprietarioImovelService = proprietarioImovelService;
            _mapper = mapper;
            _imovelService = imovelService;
            _usuarioSistemaService = usuarioSistemaService;
            _operadorRepository = operadorRepository;
            _operadorService = operadorService;
            _emailService = emailService;
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var condominio = await _condominioRepository.ObterCondominioPorCodigo(Criptografia.RecuperarHashCodCondominio(registerUser.CodCondominio));
            if (condominio != null && condominio.Ativo == true)
            {
                var user = new ApplicationUser
                {
                    UserName = registerUser.Email,
                    Email = registerUser.Email,
                    EmailConfirmed = true,
                    CodCondominio = Criptografia.RecuperarHashCodCondominio(registerUser.CodCondominio)
                };

                var result = await _userManager.CreateAsync(user, registerUser.Password);
                if (result.Succeeded)
                {
                    ApplicationUser usuario = null;
                    usuario = await _userManager.FindByEmailAsync(registerUser.Email);
                    Claim claim = new Claim("Condomino", "Cadastra,Edita,Visualiza");

                    Pessoa pessoa = await _pessoaService.ObterPorEmail(registerUser.Email, condominio.Id);

                    Guid usuarioId = new Guid(usuario.Id);
                    UsuarioCondominio usuarioMorador = new UsuarioCondominio()
                    {
                        CondominioId = condominio.Id,
                        UsuarioId = usuarioId
                    };
                    PessoaUsuario pessoaUsuario = new PessoaUsuario()
                    {
                        PessoaId = pessoa.Id,
                        UsuarioId = usuarioId
                    };
                    await _usuarioCondominioService.Adicionar(usuarioMorador);
                    await _pessoaUsuario.Adicionar(pessoaUsuario);
                    await _userManager.AddClaimAsync(usuario, claim);
                    await _userManager.AddToRoleAsync(usuario, "Condomino");
                    return CustomResponse(usuario.Id);
                }
                foreach (var error in result.Errors)
                {
                    NotificarErro(error.Description);
                }
            }

            return CustomResponse(registerUser);
        }

        [HttpPost("novo-colaborador")]
        public async Task<ActionResult> Colaborador(RegisterOperadorViewModel registerUser)
        {   
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new ApplicationUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true,
                CodCondominio = registerUser.CodCondominio
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                ApplicationUser usuario = null;
                var condominio = await _condominioRepository.ObterCondominioPorCodigo(registerUser.CodCondominio);
                if (condominio != null)
                {
                    var operador = _mapper.Map<Operador>(registerUser);
                    operador.CondominioId = condominio.Id;
                    operador = await _operadorService.Adicionar(operador);

                    usuario = await _userManager.FindByEmailAsync(registerUser.Email);
                    Claim claim = new Claim("Operador", "Cadastra,Edita,Visualiza");

                    Guid usuarioId = new Guid(usuario.Id);
                    UsuarioCondominio usuarioMorador = new UsuarioCondominio()
                    {
                        CondominioId = condominio.Id,
                        UsuarioId = usuarioId
                    };
                    await _usuarioCondominioService.Adicionar(usuarioMorador);
                    await _userManager.AddClaimAsync(usuario, claim);
                    try
                    {
                        await _userManager.AddToRoleAsync(usuario, "Operador");
                    }
                    catch (Exception e)
                    {

                        throw e;
                    }
                }
                return CustomResponse(usuario.Id);
            }
            foreach (var error in result.Errors)
            {
                NotificarErro(error.Description);
            }

            return CustomResponse(registerUser);
        }


        [HttpPost("nova-conta-usuario")]
        public async Task<ActionResult> RegistrarUsurioSistema(RegisterUserSistemaViewModel registerUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new ApplicationUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true,
                CodCondominio = registerUser.CodCondominio
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                ApplicationUser usuario = null;

                string perfil = string.Empty;
                string permissoes = string.Empty;
                usuario = await _userManager.FindByEmailAsync(registerUser.Email);
                switch (registerUser.Perfil)
                {
                    case 1:
                        perfil = "Administrador";
                        permissoes = "Cadastra,Edita,Visualiza,Exclui";
                        await _userManager.AddToRoleAsync(usuario, "Administrador");
                        break;
                    case 2:
                        perfil = "Operador";
                        permissoes = "Cadastra,Edita,Visualiza";
                        await _userManager.AddToRoleAsync(usuario, "Operador");
                        break;

                }

                Claim claim = new Claim(perfil, permissoes);
                Guid usuarioId = new Guid(usuario.Id);
                UsuarioSistema usuarioSistema = new UsuarioSistema()
                {
                    CondominioId = registerUser.CondominioId,
                    Nome = registerUser.Nome,
                    Perfil = registerUser.Perfil,
                    UsuarioId = usuarioId
                }; ;
                await _usuarioSistemaService.Adicionar(usuarioSistema);
                await _userManager.AddClaimAsync(usuario, claim);
                return CustomResponse(usuario.Id);
            }
            foreach (var error in result.Errors)
            {
                NotificarErro(error.Description);
            }

            return CustomResponse(registerUser);
        }

        [HttpGet("mostra")]
        public async Task<ActionResult> Mostra()
        {
            return CustomResponse("abriu");
        }

        [HttpPost("entrar")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            Guid id = Guid.NewGuid();
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.Succeeded)
            {
                _logger.LogInformation("Usuario " + loginUser.Email + " logado com sucesso");
                return CustomResponse(await GerarJwt(loginUser.Email, loginUser.Morador));
            }
            if (result.IsLockedOut)
            {
                NotificarErro("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse(loginUser);
            }

            NotificarErro("Usuário ou Senha incorretos");
            return CustomResponse(loginUser);
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPost("redefinir-senha")]
        public async Task<ActionResult> RedefinirSenha(RedefinirSenhaViewModel redefinirSenha)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var email = AppUser.GetUserEmail();
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                NotificarErro("Usuário não encontrado.");
                return CustomResponse(redefinirSenha);
            }

            var result = await _userManager.ChangePasswordAsync(user, redefinirSenha.SenhaAtual, redefinirSenha.NovaSenha);

            if (result.Succeeded)
            {
                return CustomResponse(new { mensagem = "Senha redefinida com sucesso." });
            }

            foreach (var error in result.Errors)
            {
                NotificarErro(error.Description);
            }

            return CustomResponse(redefinirSenha);
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost("enviar-codigo-redefinicao-senha")]
        public async Task<ActionResult> EnviarCodigoRedefinicaoSenha(SolicitarRedefinicaoSenhaViewModel solicitarRedefinicaoSenha)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = await _userManager.FindByEmailAsync(solicitarRedefinicaoSenha.Email);

            if (user == null)
            {
                return CustomResponse(new { mensagem = "Se o e-mail estiver cadastrado, o código será enviado." });
            }

            var codigo = System.Security.Cryptography.RandomNumberGenerator.GetInt32(0, 1000000).ToString("D6");
            await _userManager.SetAuthenticationTokenAsync(user, "Pratico.Api", "PasswordResetCode", codigo);

            var result = await _emailService.EnviaCodigoRedefinicaoSenha(user.Email, codigo);

            if (!result.StartsWith("Mensagem enviada"))
            {
                NotificarErro(result);
                return CustomResponse(solicitarRedefinicaoSenha);
            }

            return CustomResponse(new { mensagem = "Se o e-mail estiver cadastrado, o código será enviado." });
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost("validar-codigo-redefinicao-senha")]
        public async Task<ActionResult> ValidarCodigoRedefinicaoSenha(ValidarCodigoRedefinicaoSenhaViewModel validarCodigoRedefinicaoSenha)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = await _userManager.FindByEmailAsync(validarCodigoRedefinicaoSenha.Email);

            if (user == null)
            {
                NotificarErro("Código inválido.");
                return CustomResponse(validarCodigoRedefinicaoSenha);
            }

            var codigo = await _userManager.GetAuthenticationTokenAsync(user, "Pratico.Api", "PasswordResetCode");

            if (string.IsNullOrEmpty(codigo) || codigo != validarCodigoRedefinicaoSenha.Codigo)
            {
                NotificarErro("Código inválido.");
                return CustomResponse(validarCodigoRedefinicaoSenha);
            }

            return CustomResponse(new { mensagem = "Código validado com sucesso." });
        }

        private async Task<LoginResponseViewModel> GerarJwt(string email, bool usuario)
        {
            var guid = Guid.NewGuid().ToString();
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);//aqui
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in userRoles)
                claims.Add(new Claim("role", userRole));
            

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);
            var usuarioCondomino = await _pessoaUsuario.ObterPorUsuario(user.Id, user.CodCondominio);//lalala
            bool validado = false;
            string imovel = string.Empty;

            var condominio = await _condominioRepository.ObterCondominioPorCodigo(user.CodCondominio);

            LoginResponseViewModel response = null;

            if (usuarioCondomino != null)
            {
                if (!usuarioCondomino.PessoaId.ToString().Equals("00000000-0000-0000-0000-000000000000"))
                    validado = await _proprietarioImovelService.ObterProprietarioValidado(usuarioCondomino.PessoaId);

                var imovelValidar = await _imovelService.ObterImovelPorMorador(usuarioCondomino.PessoaId);
                if (!imovelValidar.ToString().Equals("00000000-0000-0000-0000-000000000000"))
                    imovel = imovelValidar.ToString();

                response = new LoginResponseViewModel
                {
                    AccessToken = encodedToken,
                    ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                    UserToken = new UserTokenViewModel
                    {
                        //Id = user.Id,
                        Email = user.Email,
                        Validado = validado,
                        Imovel = imovel,
                        CodCondominio = user.CodCondominio,
                        NomeCondominio = condominio.NomeFantasia,
                        Pessoa = !usuarioCondomino.PessoaId.ToString().Equals("00000000-0000-0000-0000-000000000000") ? usuarioCondomino.PessoaId.ToString() : "",
                        Claims = claims.Select(c => new ClaimViewModel { Type = c.Type, Value = c.Value })
                    }
                };
            }
            else
            {                
                var colaborador = await _usuarioSistemaService.Buscar(user.Id, condominio.Id);
                response = new LoginResponseViewModel
                {
                    AccessToken = encodedToken,
                    ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                    UserToken = new UserTokenViewModel
                    {
                        //Id = user.Id,
                        Email = user.Email,
                        Validado = validado,
                        Imovel = imovel,
                        CodCondominio = user.CodCondominio,
                        NomeCondominio = condominio.NomeFantasia,
                        Pessoa = colaborador != null ? colaborador.UsuarioId.ToString() : "",
                        Claims = claims.Select(c => new ClaimViewModel { Type = c.Type, Value = c.Value })
                    }
                };
            }
            return response;
        }
        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        [HttpPost("UsuarioMorador")]
        public async Task<ActionResult> UsuarioMorador(UsuarioMoradorViewModel usuario)
        {
            var pessoa = await _pessoaService.Adicionar(_mapper.Map<Proprietario>(usuario));
            PessoaUsuario usuarioGravado = null;
            if (pessoa != null)
            {
                PessoaUsuario pessoaUsuario = new PessoaUsuario
                {
                    PessoaId = pessoa.Id,
                    UsuarioId = usuario.Entidade
                };
                usuarioGravado = await _pessoaUsuario.Adicionar(pessoaUsuario);
            }
            return CustomResponse(usuarioGravado);
        }
    }
}
