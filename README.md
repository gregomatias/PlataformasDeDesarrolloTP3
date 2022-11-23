"# TP3-PlataformasDeDesarrollo"

Integrantes:

Alan Riva, Nicolas Villegas, Luis Duarte Carvhalosa, Natalia Belen Espinosa, Patricia Belen Cabrera, Matias Grego

TP3: https://github.com/gregomatias/PlataformasDeDesarrolloTP3

Se adapto todo el TP que veniamos haciendo a Entity Framework
Para probar la aplicación se crearon usuarios de prueba con HasData.
Usuario admin: us.: 444 Pass: 444







TP2: https://github.com/gregomatias/TP1PlataformasDeDesarrollo
General:

Se adapto todos los metodos para que funcionen con la base de datos.
Se Implementaron las solapas de Tarjeta de Crédito y plazo fijo y funcionalidades faltantes.

En el mismo incluimos una carpeta  scriptDB/base.sql que contiene el script que crea la base de datos con registros de prueba.

El usuario Admin es: Us: 123 Pass: 123

Admin:



Se implementaron 2 Solapas de Admin. Estas solo se pueden ver si se loguea con el dicho usuario.
-	Solapa Traslado:

Boton Agregar Titular: Permite asociar una caja de ahorro a otro usuario.

Boton Quitar Titular: Permite quitar una caja de ahorro a un usuario, siempre y cuando no sea el unico que la tenga.

-	Solapa Usuarios:

Boton Desbloquear:Permite desbloquear un usuario y resetearle el contador de iungresos.

Boton Eliminar: Permite elimiar todos los objetos del usuario, a excepcion de las cajas de ahorro compartidas con otro usuario.


TP1:   https://github.com/gregomatias/TP1PlataformasDeDesarrollo
1- Al abri la aplicación, el usuario visualiza una pantalla de login donde podrá:
-	Ingresar usuario y contraseña para iniciar sesión.
-   Registrarse en la aplicación ingresando sus datos personales a través de un formulario.
Consideración: tiene 3 intentos de logueo, en caso de llegar al máximo de intentos fallidos, se bloquea el usuario.

2- Al ingresar al sistema, nos muestra un menú con distintas pestañas que abarcan una funcionalidad específica. El usuario puede seleccionar la que desee.
-	Caja de ahorro: nos muestra en pantalla las cajas de ahorro que tiene el usuario que ingreso y el saldo de cada una. También se podrá crear una nueva caja de ahorro desde el botón crear caja.
Se piensa a futuro en un perfil administrador que pueda dar de baja las cajas de ahorro.
-     Movimientos: Al seleccionar una de las cajas de ahorro del usuario, podremos consultar los movimientos de la misma. Los movimientos pueden ser depósitos, retiros, transferencias emitidas, transferencias recibidas y pagos.
Se podrá filtrar por fecha, importe y detalle.
-     Transacciones: Se debe seleccionar una caja de ahorro del combo y se habilitará la posibilidad de retirar y depositar fondos.
Al realizar un retiro se realizará un débito en la cuenta del cliente que se podrá consultar en la consulta de movimientos.
Al realizar un deposito se realizará un crédito en la cuenta del cliente que se podrá consultar en la consulta de movimientos.
Consideraciones: para retirar el cliente debe tener saldo suficiente.
-     Transferencias: Se listarán todas las cajas de ahorro del usuario para el oriden de los fondos y se habilitará el ingreso del CBU destino. El CBU puede ser propio o de otro cliente del mismo banco.
La transferencia generá un moviendo de débito sobre la cuenta del cliente y una acreditación sobre la cuenta del cliente destino.
Consideraciones: Para realizar la transferencia de fondos, el cliente debe contar con saldo suficiente requerido por la operación.
-     Pagos: el cliente podrá ingresar pagos a realizar seleccionando el método de pago deseado (Tarjeta de crédito o caja de ahorro). El pago nace pendiente al ingresarse.
Los pagos pendientes se listarán y se dará la posibilidad de confirmarlos. Al confirmarlos se podrá visualizar en la consulta de movimientos y se realizará un débito en el medio de pago seleccionado. El pago pasará al estado confirmado.
Los pagos confirmados se listarán y se dará la posibilidad de eliminarlos. La eliminación del mismo no implica el reintegro de fondos.

3- Para finalizar la sesión, el usuario debe seleccionar el cierra de ventana del lateral derecho en la parte superior. Una vez hecho esto, la aplicación lo llevará a la pantalla de logueo nuevamente.

La pestaña plazo fijo, y tarjetas serán implementadas en versiones posteriores.
