// JavaScript source code

//TODO: Completar funcion validarEnviarCore - debe validar que si Envio al core es true los parametros firstname , lastname - crc3e_rfc contengan dato
//de otra manera mandar un error y no dejar guardar el formulario, publicar

//TODO: Crear/subir este JS en la solucion https://org3baeeaf5.crm.dynamics.com/tools/solution/edit.aspx?id=f3a64b4c-3a1f-ee11-9cbe-0022482b6eda&fromSave=True
// registrar JS en la entidad contacto y la funcion 'validarEnviarCore' en el evento OnSave

//let formContext = executionContext.getFormContext();

//TODO: logica...

//Paso 1: Obtener campos de CRM firstname, lastname. crc3e_rfc
// https://carldesouza.com/get-and-set-field-values-using-formcontext-and-javascript-with-dynamics-365/

function validarEnviarCore(executionContext) {

    var formContext = executionContext.getFormContext();

    //Get Values
    var nombre = formContext.getAttribute("firstname").getValue();
    var apellido = formContext.getAttribute("lastname").getValue();
    var rfc = formContext.getAttribute("crc3e_rfc").getValue();
    var core = formContext.getAttribute("crc3e_envioalcore").getValue();



    //Paso 2: validar que tengan datos
    if (nombre === null || apellido === null || rfc === null && core === true) {
        Xrm.Navigation.openErrorDialog({ errorCode: 400, message: "Para crear el registro en el core son obligatorios los campos RFC , Nombre Y Apellidos" }).then(
            function (success) {
                console.log(success);
            },
            function (error) {
                console.log(error);
            });
        executionContext.getEventArgs().preventDefault();
    }
}
        ////Set Values
        //formContext.getAttribute("firstname").setValue(nombre);
        //formContext.getAttribute("lastname").setValue(apellido);
        //formContext.getAttribute("crc3e_rfc").setValue(rfc);

        // Alert new value
        //nombre = formContext.getAttribute("firstname").getValue();
        //alert(nombre);
        //apellido = formContext.getAttribute("lastname").getValue();
        //alert(lastname);
        //rfc = formContext.getAttribute("crc3e_rfc");
        //alert(rfc);
/*}*/

    //function validarEnviarCore(executionContext) {

    ////Paso 2: validar que tengan datos
    //if (nombre === "firstname" && apellido === "lastname" &&
    //    rfc === "crc3e_rfc") {
    //    alert(`El usuario llamado ${nombre} ${apellido} con RFC ${rfc} ha sido guardado.`);
    //    return true;
    //}

    ////Paso 3: el resultado del Paso2 sea en una variable o en una condicion del Paso 2 no  no cumplir esa condicion mandar en mensaje de error
    ////https://carldesouza.com/showing-an-error-through-javascript-in-power-apps-and-dynamics-365/
    //else {
    //    Xrm.Navigation.openErrorDialog({ errorCode: 1234, details: "Para crear el registro en el core son obligatorios los campos RFC , Nombre Y Apellidos" }).then(
    //        function (success) {
    //            console.log(success);
    //        },
    //        function (error) {
    //            console.log(error);
    //        });
    //    }
    //}