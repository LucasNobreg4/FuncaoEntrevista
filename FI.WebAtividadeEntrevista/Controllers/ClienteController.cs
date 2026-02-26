using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using FI.AtividadeEntrevista.Utils;
using FI.WebAtividadeEntrevista.Mappers;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        private readonly BoCliente _boCliente;
        private readonly BoBeneficiario _boBeneficiario;

        public ClienteController()
        {
            _boCliente = new BoCliente();
            _boBeneficiario = new BoBeneficiario();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Incluir")]
        public JsonResult IncluirSemBeneficiarios(ClienteModel model)
        {
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();
                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            if (!UtilCPF.ValidarCPF(model.CPF))
            {
                Response.StatusCode = 400;
                return Json("CPF inválido. Verifique o número digitado.");
            }

            if (_boCliente.VerificarExistencia(model.CPF))
            {
                Response.StatusCode = 400;
                return Json("CPF já cadastrado no sistema.");
            }

            try
            {
                model.Id = _boCliente.Incluir(ClienteMapper.ParaEntidade(model));
                return Json(new { Mensagem = "Cadastro efetuado com sucesso", Id = model.Id });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json("Erro ao cadastrar cliente. Contate o suporte.");
            }
        }

        [HttpPost]
        public JsonResult IncluirComBeneficiarios(ClienteModel model, List<BeneficiarioModel> Beneficiarios)
        {
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();
                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            if (!UtilCPF.ValidarCPF(model.CPF))
            {
                Response.StatusCode = 400;
                return Json("CPF inválido. Verifique o número digitado.");
            }

            if (_boCliente.VerificarExistencia(model.CPF))
            {
                Response.StatusCode = 400;
                return Json("CPF já cadastrado no sistema.");
            }

            try
            {
                model.Id = _boCliente.Incluir(ClienteMapper.ParaEntidade(model));

                if (Beneficiarios != null && Beneficiarios.Count > 0)
                {
                    var cpfsProcessados = new HashSet<string>();

                    foreach (var beneficiario in Beneficiarios)
                    {
                        if (!UtilCPF.ValidarCPF(beneficiario.CPF))
                        {
                            Response.StatusCode = 400;
                            return Json($"CPF do beneficiário {beneficiario.Nome} é inválido.");
                        }

                        var cpfSemFormatacao = UtilCPF.RemoverFormatacao(beneficiario.CPF);

                        if (cpfsProcessados.Contains(cpfSemFormatacao))
                        {
                            Response.StatusCode = 400;
                            return Json($"CPF {beneficiario.CPF} está duplicado na lista de beneficiários.");
                        }

                        long? idClienteDoCPF = _boBeneficiario.ConsultarClientePorCPFBeneficiario(cpfSemFormatacao);
                        if (idClienteDoCPF.HasValue && idClienteDoCPF.Value != model.Id)
                        {
                            Response.StatusCode = 400;
                            return Json($"CPF {beneficiario.CPF} já está cadastrado como beneficiário de outro cliente.");
                        }

                        cpfsProcessados.Add(cpfSemFormatacao);
                        beneficiario.IDCliente = model.Id;
                        _boBeneficiario.Incluir(BeneficiarioMapper.ParaEntidade(beneficiario));
                    }
                }

                return Json(new { Mensagem = "Cadastro efetuado com sucesso", Id = model.Id });
            }
            catch
            {
                Response.StatusCode = 500;
                return Json("Erro ao cadastrar cliente. Contate o suporte.");
            }
        }

        [HttpPost]
        [ActionName("Alterar")]
        public JsonResult AlterarSemBeneficiarios(ClienteModel model)
        {
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();
                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            if (!UtilCPF.ValidarCPF(model.CPF))
            {
                Response.StatusCode = 400;
                return Json("CPF inválido. Verifique o número digitado.");
            }

            Cliente clienteAtual = _boCliente.Consultar(model.Id);

            string cpfAtualSemFormatacao = UtilCPF.RemoverFormatacao(clienteAtual.CPF);
            string cpfNovoSemFormatacao = UtilCPF.RemoverFormatacao(model.CPF);

            if (cpfAtualSemFormatacao != cpfNovoSemFormatacao && 
                _boCliente.VerificarExistencia(cpfNovoSemFormatacao))
            {
                Response.StatusCode = 400;
                return Json("CPF já cadastrado para outro cliente.");
            }

            try
            {
                _boCliente.Alterar(ClienteMapper.ParaEntidade(model));
                return Json("Cadastro alterado com sucesso");
            }
            catch
            {
                Response.StatusCode = 500;
                return Json("Erro ao alterar cliente. Contate o suporte.");
            }
        }

        [HttpPost]
        public JsonResult AlterarComBeneficiarios(ClienteModel model, List<BeneficiarioModel> Beneficiarios)
        {
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();
                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            if (!UtilCPF.ValidarCPF(model.CPF))
            {
                Response.StatusCode = 400;
                return Json("CPF inválido. Verifique o número digitado.");
            }

            Cliente clienteAtual = _boCliente.Consultar(model.Id);

            string cpfAtualSemFormatacao = UtilCPF.RemoverFormatacao(clienteAtual.CPF);
            string cpfNovoSemFormatacao = UtilCPF.RemoverFormatacao(model.CPF);

            if (cpfAtualSemFormatacao != cpfNovoSemFormatacao && 
                _boCliente.VerificarExistencia(cpfNovoSemFormatacao))
            {
                Response.StatusCode = 400;
                return Json("CPF já cadastrado para outro cliente.");
            }

            try
            {
                _boCliente.Alterar(ClienteMapper.ParaEntidade(model));

                List<Beneficiario> beneficiariosAtuais = _boBeneficiario.ListarPorCliente(model.Id);

                foreach (var beneficiarioAtual in beneficiariosAtuais)
                {
                    if (Beneficiarios == null || !Beneficiarios.Any(b => b.Id == beneficiarioAtual.Id))
                    {
                        _boBeneficiario.Excluir(beneficiarioAtual.Id);
                    }
                }

                if (Beneficiarios != null && Beneficiarios.Count > 0)
                {
                    foreach (var beneficiario in Beneficiarios)
                    {
                        if (!UtilCPF.ValidarCPF(beneficiario.CPF))
                        {
                            Response.StatusCode = 400;
                            return Json($"CPF do beneficiário {beneficiario.Nome} é inválido.");
                        }

                        var cpfSemFormatacao = UtilCPF.RemoverFormatacao(beneficiario.CPF);

                        bool isDuplicado = beneficiariosAtuais.Any(ba => 
                            UtilCPF.RemoverFormatacao(ba.CPF) == cpfSemFormatacao && 
                            ba.Id != beneficiario.Id
                        );

                        if (isDuplicado)
                        {
                            Response.StatusCode = 400;
                            return Json($"CPF do beneficiário {beneficiario.Nome} já está cadastrado para este cliente.");
                        }

                        beneficiario.IDCliente = model.Id;

                        if (beneficiario.Id > 0)
                        {
                            _boBeneficiario.Alterar(BeneficiarioMapper.ParaEntidade(beneficiario));
                        }
                        else
                        {
                            _boBeneficiario.Incluir(BeneficiarioMapper.ParaEntidade(beneficiario));
                        }
                    }
                }

                return Json("Cadastro alterado com sucesso");
            }
            catch
            {
                Response.StatusCode = 500;
                return Json("Erro ao alterar cliente. Contate o suporte.");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            Cliente cliente = _boCliente.Consultar(id);
            ClienteModel model = ClienteMapper.ParaModel(cliente);
            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = _boCliente.Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}