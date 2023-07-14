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
    public class Plugin101Update : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        { // Tres tristes tigres
            // Parangaricutimiricuaro
            // Obtain the tracing service

            ITracingService tracingService =
            (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.  
            if (context.PostEntityImages.Contains("postimage"))
            {
                // Obtain the target entity from the input parameters.  
                Entity postImageUpdate = context.PostEntityImages["postimage"] as Entity;

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
                    string folio = null;
                    bool core = false;

                    //Inicializando
                    if (postImageUpdate.Contains("crc3e_envioalcore"))
                    {
                        core = postImageUpdate.GetAttributeValue<bool>("crc3e_envioalcore");
                    }
                    if (postImageUpdate.Contains("crc3e_folio"))
                    {
                        folio = postImageUpdate.GetAttributeValue<string>("crc3e_folio");
                    }
                    if (core == true && folio == null)
                    {
                        if (postImageUpdate.Contains("crc3e_rfc"))
                        {
                            rfc = postImageUpdate.GetAttributeValue<string>("crc3e_rfc");
                        }
                        if (postImageUpdate.Contains("firstname"))
                        {
                            nombre = postImageUpdate.GetAttributeValue<string>("firstname");
                        }
                        //Con operacion ternario                 
                        apellido = !string.IsNullOrEmpty(postImageUpdate.GetAttributeValue<string>("lastname")) ? postImageUpdate.GetAttributeValue<string>("lastname") : "NO LASTNAME";

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

                        //postImageUpdate["crc3e_folio"] = objresponse.folio;
                        //postImageUpdate["description"] = objresponse.interpretacion;

                        Entity contact = new Entity(postImageUpdate.LogicalName, postImageUpdate.Id);
                        contact["description"] = objresponse.interpretacion;
                        contact["crc3e_folio"] = objresponse.folio;
            
                        service.Update(contact);
                    }
                    //service.Retrieve("contact", contact.Id,new Microsoft.Xrm.Sdk.Query.ColumnSet("rs_rfc","firstname"));
                    ////AAAAAA
                    ////
                    ////BBBBBBBBB
                    ////BBBBBBBBB
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
