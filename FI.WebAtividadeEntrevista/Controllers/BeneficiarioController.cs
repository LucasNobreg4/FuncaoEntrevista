using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using FI.AtividadeEntrevista.Utils;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FI.WebAtividadeEntrevista.Controllers
{
    public class BeneficiarioController : Controller
    {
        private readonly BoBeneficiario _boBeneficiario;

        public BeneficiarioController()
        {
            _boBeneficiario = new BoBeneficiario();
        }

        [HttpGet]
        public JsonResult Listar(long idCliente)
        {
            try
            {
                var beneficiarios = _boBeneficiario.ListarPorCliente(idCliente);
                return Json(new { Result = "OK", Records = beneficiarios }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                Response.StatusCode = 500;
                return Json(new { Result = "ERROR", Message = "Erro ao listar beneficiários." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}