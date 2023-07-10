// JavaScript source code

//TODO: Completar funcion validarEnviarCore - debe validar que si Envio al core es true los parametros firstname , lastname - crc3e_rfc contengan dato
//de otra manera mandar un error y no dejar guardar el formulario, publicar

//TODO: Crear/subir este JS en la solucion https://org3baeeaf5.crm.dynamics.com/tools/solution/edit.aspx?id=f3a64b4c-3a1f-ee11-9cbe-0022482b6eda&fromSave=True
// registrar JS en la entidad contacto y la funcion 'validarEnviarCore' en el evento OnSave

function validarEnviarCore(executionContext)
{
    let formContext = executionContext.getFormContext();

    //TODO: logica...

    //Paso 1: Obtener campos de CRM firstname, lastname. crc3e_rfc
    // https://carldesouza.com/get-and-set-field-values-using-formcontext-and-javascript-with-dynamics-365/

    //Paso 2: validar que tengan datos

    //Paso 3: el resultado del Paso2 sea en una variable o en una condicion del Paso 2 no  no cumplir esa condicion mandar en mensaje de error
    //https://carldesouza.com/showing-an-error-through-javascript-in-power-apps-and-dynamics-365/

}