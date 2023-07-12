using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace PL101.plugin
{
    public class Plugin101 : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the tracing service

            ITracingService tracingService =
            (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity contact = (Entity)context.InputParameters["Target"];

                // Obtain the organization service reference which you will need for  
                // web service calls.  
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    // Paso 1: Declarar variables "RFC" y "firstname" y "lastname"
                    string rfc = null;
                    string nombre = null;
                    string apellido = null;
                    bool core = false;

                    //Inicializando
                    if (contact.Contains("crc3e_envioalcore"))
                    {
                        core = contact.GetAttributeValue<bool>("crc3e_envioalcore");
                    }
                    if (core == true)
                    {
                        if (contact.Contains("crc3e_rfc"))
                        {
                            rfc = contact.GetAttributeValue<string>("crc3e_rfc");
                        }
                        if (contact.Contains("firstname"))
                        {
                            nombre = contact.GetAttributeValue<string>("firstname");
                        }
                        //Con operacion ternario                 
                        apellido = !string.IsNullOrEmpty(contact.GetAttributeValue<string>("lastname")) ? contact.GetAttributeValue<string>("lastname") : "NO LASTNAME";

                        // Paso 2: Construir el request con las variables del Paso 1
                        string host;
                        host = "https://servicioclientesfederacion.azurewebsites.net";
                        string recurso;
                        recurso = "/api/altacliente";
                        string endpoint = host + recurso;

                        string request;
                        string nombreCompleto;
                        nombreCompleto = $"{nombre} {apellido}";

                        request = GenerarJSONRequest(nombreCompleto, rfc);

                        var data = new StringContent(request, Encoding.UTF8, "application/json");

                        // Paso 3: Mandar el request con las variables
                        HttpClient cliente = new HttpClient();
                        var message = cliente.PostAsync(endpoint, data).Result;
                        //HttpResponseMessage response =

                        // Paso 4: Obtener respuesta del request
                        string responseString = null;

                        if (message.IsSuccessStatusCode)
                        {
                            responseString = message.Content.ReadAsStringAsync().Result;
                        }
                        // Paso 5: Mapear datos en CRM
                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        ResponseClass objresponse = serializer.Deserialize<ResponseClass>(responseString);

                        contact["crc3e_folio"] = objresponse.folio;
                        contact["description"] = objresponse.interpretacion;
                    }
                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in FollowUpPlugin.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("FollowUpPlugin: {0}", ex.ToString());

                    throw new InvalidPluginExecutionException(ex.Message);
                }
            }
        }

        // Paso 2A: Generar body del request
        private string GenerarJSONRequest(string fullname, string rfc)
        {
            string template;
            template = $@"
                {{
                    ""fullname"": ""{fullname}"",
                    ""rfc"" : ""{rfc}""
                }}
            ";

            return template;
        }
    }
}
