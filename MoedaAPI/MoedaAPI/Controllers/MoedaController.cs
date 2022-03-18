using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoedaAPI.Model;
using System.Web;
using Microsoft.AspNetCore.Http;


namespace MoedaAPI.Controllers
{
    //

    [ApiController]
    [Route("[controller]")]
    public class MoedaController : Controller
    {

        Stack<List<Moeda>> stackListMoedas = new Stack<List<Moeda>>();
        [HttpPost]
        [Route("AddItemFila")]
        public void AddItemFila(List<Moeda> listMoedas)
        {
            //ISession session[]
            //HttpContext.Session.["stackListMoedas"] = stackListMoedas;
            //window
            //HttpContext.Session.("SessionNome", listMoedas);
            //var session = HttpContext.Current.Session;
            //if (HttpContext.Current.Session[""] == null)
            try
            {
                stackListMoedas.Push(listMoedas);
            }
            catch (Exception e)
            {
                
            }
            
        }

        [HttpGet]
        [Route("GetItemFila")]
        public JsonResult GetItemFila()
        {
            List<Moeda> listMoeda1 = new List<Moeda>();
            List<Moeda> listMoeda2 = new List<Moeda>();
            Moeda moeda1 = new Moeda();
            moeda1.moeda = "USD";
            moeda1.data_inicio = DateTime.Parse("2010 - 01 - 01");
            moeda1.data_fim = DateTime.Parse("2010 - 12 - 01");
            listMoeda1.Add(moeda1);
            Moeda moeda2 = new Moeda();
            moeda2.moeda = "EUR";
            moeda2.data_inicio = DateTime.Parse("2020-01-01");
            moeda2.data_fim = DateTime.Parse("2020 - 12 - 01");
            listMoeda1.Add(moeda2);
            Moeda moeda3 = new Moeda();
            moeda3.moeda = "JPY";
            moeda3.data_inicio = DateTime.Parse("2000-03-11");
            moeda3.data_fim = DateTime.Parse("2000-03-30");
            listMoeda1.Add(moeda3);
            stackListMoedas.Push(listMoeda1);
            List<Moeda> resultado = new List<Moeda>();
            if (stackListMoedas.Count == 0)
            {
                resultado = null;
            }
            else
            {
                resultado = stackListMoedas.Pop();
            }            
            return Json(resultado);
        }
    }
}
