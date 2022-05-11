using Microsoft.Extensions.Configuration;
using MvcSegundoExamenAWSAgus.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MvcSegundoExamenAWSAgus.Services
{
    public class ServiceConciertos
    {
        private string UrlApi;
        private MediaTypeWithQualityHeaderValue Header;

        public ServiceConciertos(IConfiguration configuration)
        {
            this.UrlApi = configuration.GetValue<string>("ApiUrls:ApiConciertosAWS");
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }

        private async Task<T> CallApi<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string url = this.UrlApi + request;
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<List<Evento>> GetEventosPorCategAsync(int id)
        {
            string request = "/GetEventosPorCategoria/"+id;
            List<Evento> eventos = await this.CallApi<List<Evento>>(request);
            return eventos;
        }

        public async Task<List<Evento>> GetEventosAsync()
        {
            string request = "/GetEventos";
            List<Evento> eventos = await this.CallApi<List<Evento>>(request);
            return eventos;
        }

        public async Task<Evento> FindEventoAsync(int id)
        {
            string request = "/FindEvento/" + id;
            Evento evento = await this.CallApi<Evento>(request);
            return evento;
        }

        public async Task<List<CategoriaEvento>> GetCategoriasAsync()
        {
            string request = "/GetCategoriaEvento";
            List<CategoriaEvento> categoriaeventos = await this.CallApi<List<CategoriaEvento>>(request);
            return categoriaeventos;
        }

        public async void CambiarCategoriaEvento(int categoria, int evento)
        {
            string request = "/CambiarEventoCategoria/"+evento+"/"+categoria;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string url = this.UrlApi + request;
                HttpResponseMessage response = await client.PutAsync(url,null);
            }
        }

        public async Task<string> GetNombreCategoria(int idcategoria)
        {
            string request = "/GetNombreCategoria/" + idcategoria;
            string nombre = await this.CallApi<string>(request);
            return nombre;
        }

        public async void DeleteEvento(int idevento)
        {
            string request = "/DeleteEvento/"+idevento;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string url = this.UrlApi + request;
                HttpResponseMessage response = await client.DeleteAsync(url);
            }
        }

        public async void NuevoEvento(Evento evento)
        {
            string request = "/InsertarEvento";
            string url = UrlApi + request;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string json = JsonConvert.SerializeObject(evento);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
            }
        }
    }
}
