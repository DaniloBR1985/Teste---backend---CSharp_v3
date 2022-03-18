using MoedaConsole.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.Globalization;
using System.Reflection;

namespace MoedaConsole
{
    class Program
    {
        static List<TimeSpan> listTempoToal = new List<TimeSpan>();
        static List<DadosMoeda> listDadosMoeda = new List<DadosMoeda>();
        static List<DadosCotacao> listDadosCotacao = new List<DadosCotacao>();
        public enum Cod_cotacao
        {
            AFN = 66,
            AL = 49,
            ANG = 33,
            ARS = 3,
            AWG = 6,
            BOB = 56,
            BYN = 64,
            CAD = 25,
            CDF = 58,
            CLP = 16,
            COP = 37,
            CRC = 52,
            CUP = 8,
            CVE = 51,
            CZK = 29,
            DJF = 36,
            DZD = 54,
            EGP = 12,
            EUR = 20,
            FJD = 38,
            GBP = 22,
            GEL = 48,
            GIP = 18,
            HTG = 63,
            ILS = 40,
            IRR = 17,
            ISK = 11,
            JPY = 9,
            KES = 21,
            KMF = 19,
            LBP = 42,
            LSL = 4,
            MGA = 35,
            MGB = 26,
            MMK = 69,
            MRO = 53,
            MRU = 15,
            MUR = 7,
            MXN = 41,
            MZN = 43,
            NIO = 23,
            NOK = 62,
            OMR = 34,
            PEN = 45,
            PGK = 2,
            PHP = 24,
            RON = 5,
            SAR = 44,
            SBD = 32,
            SGD = 70,
            SLL = 10,
            SOS = 61,
            SSP = 47,
            SZL = 55,
            THB = 39,
            TRY = 13,
            TTD = 67,
            UGX = 59,
            USD = 1,
            UYU = 46,
            VES = 68,
            VUV = 57,
            WST = 28,
            XAF = 30,
            XAU = 60,
            XDR = 27,
            XOF = 14,
            XPF = 50,
            ZAR = 65,
            ZWL = 31
        }
        static void Main(string[] args)
        {
            string path = @"~/webIRMA/Content/dist/img/avatarPadrao.png";



            DateTime proximoCiclo = DateTime.Now;
            while (true)
            {
                if (DateTime.Now > proximoCiclo)
                {
                    proximoCiclo = DateTime.Now.AddMinutes(2);
                    RunAsync().Wait();
                }
                
            }
            
        }

        static void lerDadosMoeda()
        {
            listDadosMoeda = File.ReadAllLines(@"../../../../DadosMoeda.csv")
                .Select(a => a.Split(';')).Where(a => !a.Contains("ID_MOEDA"))
                .Select(c => new DadosMoeda()
                {
                    moeda = c[0],
                    data_ref = DateTime.Parse(c[1])
                })
                .ToList();
        }
        static void lerDadosCotacao()
        {
            listDadosCotacao = File.ReadAllLines(@"../../../../DadosCotacao.csv")
                .Select(a => a.Split(';')).Where(a => !a.Contains("vlr_cotacao"))
                .Select(c => new DadosCotacao()
                {
                    vlr_cotacao = float.Parse(c[0]),
                    cod_cotacao = int.Parse(c[1]),
                    //moeda = Enum.Parse(typeof(Cod_cotacao), c[2]),
                    dat_cotacao = DateTime.Parse(c[2])
                })
                .ToList();
        }
        static async Task RunAsync()
        {
            DateTime dataInicial = DateTime.Now;
            lerDadosMoeda();
            lerDadosCotacao();
            using (var client = new HttpClient())
            {
                List<Moeda> listMoedas = new List<Moeda>();
                client.BaseAddress = new System.Uri("https://localhost:44363/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("Moeda/GetItemFila");
                if (response.IsSuccessStatusCode)
                {  //GET
                    listMoedas = await response.Content.ReadAsAsync<List<Moeda>>();
                    if (listMoedas != null)
                    {
                        var teste = Enum.Parse(typeof(Cod_cotacao), listDadosCotacao[9281].cod_cotacao.ToString());
                        var resultado =
                        from moedas in listMoedas
                        join dadosMoeda in listDadosMoeda on moedas.moeda equals dadosMoeda.moeda
                        join dadosCotacao in listDadosCotacao on moedas.moeda equals Enum.Parse(typeof(Cod_cotacao), dadosCotacao.cod_cotacao.ToString()).ToString()
                        where dadosMoeda.data_ref >= moedas.data_inicio && dadosMoeda.data_ref <= moedas.data_fim && dadosMoeda.data_ref == dadosCotacao.dat_cotacao
                        select new Resultado
                        {
                            moeda = dadosMoeda.moeda,
                            data_ref = dadosMoeda.data_ref.ToString("dd/MM/yyyy"),
                            vlr_cotacao = dadosCotacao.vlr_cotacao
                        };
                        if (resultado != null)
                        {
                            using (var writer = new StreamWriter(@"../../../../Resultado_ " + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv")) 
                            using (var csv = new CsvWriter(writer, new CultureInfo("pt-BR", true)))
                            {
                                csv.WriteRecords(resultado);
                            }


                        }

                    }
                }
                DateTime dataFinal = DateTime.Now;
                TimeSpan tempoTotal = dataFinal - dataInicial;
                listTempoToal.Add(tempoTotal);

            }
        }
    }
}
