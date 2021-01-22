//funcion que carga el calendario
function GetEventsOnPageLoad() {
    $('#calendar').fullCalendar({
        locale: 'es',
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        buttonText: {
            today: 'Hoy',
            month: 'Mes',
            week: 'Semana',
            day: 'Día'
        },
        defaultView: 'agendaWeek',
        selectable: true,
        height: 'parent',
        events: function (start, end, timezone, callback) {
            $.ajax({
                type: "GET",
                contentType: " application/json",
                url: "/Turno/GetEventData",
                dataType: "JSON",
                success: function (turnos) {
                   events = PushEvents(turnos);
                   callback(events);
                }
            })
        },
        dayClick: function (date, jsEvent, view) {
            if (view.name == 'month') {
                $('#calendar').fullCalendar('changeView', 'agendaDay', date);
            } else {
                // leemos las fechas de inicio de evento y hoy
                var evento = moment(date).format();
                var now = moment();
                // si el inicio de evento ocurre hoy o en el futuro mostramos el modal
                if (now.isBefore(evento)) {
                    showModal('Turno Nuevo', CreaTurno(date), null);
                }
                // si no, mostramos una alerta de error
                else {
                    Swal.fire(
                        'Error',
                        'No se puede crear un turno para una fecha pasada',
                        'warning'
                    )
                }
            }
        },
        eventRender: function eventRender(event) {
            
            var index = $('#Profesionales').val(); 

            if (index != 'all') {
            index = parseInt($('#Profesionales').val()); 
            }
            return ['all', event.Id_Medico].indexOf(index) >= 0
        },
        nextDayThreshold: '00:00:00',
        editable: false,
        droppable: false,
        allDay: false,
        nowIndicator: true,
        eventClick: function (EventId) {
            GetEventDetailByEventId(EventId.Id_Turno);
        },
        eventDrop: function(info) {
            console.log(info);
            UpdateEventDetails(info.Id_Turno, info.Fecha_Inicio.toISOString(), info.Fecha_Fin.toISOString());
        },
        eventResize: function (info) {
            UpdateEventDetails(info.Id_Turno, info.Fecha_Inicio.toISOString(), info.Fecha_Fin.toISOString());
        },
        weekends: true
    })
}

function showModal(title, body, isEventDetail) {
    $("#MyPopup .modal-title").html(title);
}

function GetEventDetailByEventId(eventinfo) {
  
    var object = {};
    object.Id_Turno = eventinfo;
    $.ajax({
        type: "GET",
        url: 'GetEventDetailByEventId',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: object,
        success: function (eventdetails) {
            showModal('Turno', eventdetails, true);
        }
    });
}

function showModal(title, body, isEventDetail) {
    $("#MyPopup .modal-title").html(title);
    if (isEventDetail == null) {
        $("#MyPopup .modal-body").html(body);
        $("#MyPopup").modal("show");
    }
    else {
        //datos a mostrar en el modal con los detalles del evento

        //El body es pasado como JSON entonces lo convierto a objeto del tipo Turno
        var turno = JSON.parse(body);
        //traigo el paciente y profesional por Id en formato JSON
        var pacienteJs = traePaciente(turno.Id_Paciente);
        var medicoJs = traeMedico(turno.Id_Medico);
        //Convierto los JSON a objeto
        var paciente = JSON.parse(pacienteJs);
        var medico = JSON.parse(medicoJs);
        //Titulo del modal
        var eventDetail = 'Paciente: ' + paciente.Apellido + ', ' + paciente.Nombre + '<div class="self-align: right" style="float:right"><button class="btn"><a href="/Paciente/Details/' + paciente.Id_Paciente + '">Ver Paciente</a></div></br>';
        //informacion que va al modal
        var eventInfo = 'Profesional: ' + medico.Apellido + ', ' + medico.Nombre + '</br>';
        var eventStart = 'Fecha y Hora: ' + moment(turno.Fecha_Inicio).format("DD-MM-YY HH:mm") + '</br><div class="self-align: right"><button class="btn"><a href="/Turno/Edit/' + turno.Id_Turno + '">Editar</a></button> &nbsp; <button class="btn" onclick="eliminaTurno(' + turno.Id_Turno + ')">Eliminar</button></div>';
        $("#MyPopup .modal-body").html(eventDetail + eventInfo + eventStart);
        $("#MyPopup").modal("show");
    }
}

function UpdateEventDetails(eventId, startDate, endDate) {
    var object = {};
    object.EventId = eventId;
    object.StartDate = startDate;
    object.EndDate = endDate;
    $.ajax ({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Turno/UpdateEventDetails",
        dataType: "JSON",
        data: JSON.stringify(object)
    });
}

// Funcion que trae un JSON de UN profesional a partir del Id del mismo
function traeMedico(Id_Medico) {
    var object = {};
    var med = null;
    object.Id_Medico = Id_Medico;
    $.ajax({
        async: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Medico/TraeMedicoPorId",
        dataType: "json",
        data: object,
        success: function (medico) {
            med = medico;
        }
    });
    return med;
}

// Funcion que trae un JSON de los profesionales
function traeMedicos() {
    var meds = null;
    $.ajax({
        async: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Medico/TraeMedicos",
        dataType: "json",
        success: function (medicos) {
            meds = medicos;
        }
    });
    return meds;
}

// Funcion que trae un JSON de UN paciente a partir del Id del mismo
function traePaciente(Id_Paciente) {
    var object = {};
    object.Id_Paciente = Id_Paciente;
    var pcte = null;
    $.ajax({
        async: false,
        type: "GET",
        url: "/Paciente/TraePacientePorId",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: object,
        success: function (paciente) {
            pcte = paciente;
        }
    });
    return pcte;
}

//Funcion que trae un JSON de todos los pacientes
function traePacientes() {
    var pctes = null;
    $.ajax({
        async: false,
        type: "GET",
        url: "/Paciente/TraePacientes",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pacientes) {
            pctes = pacientes;
        }
    });
    return pctes;
}

//funcion que crea el html para crear un turno. Este HTML va como parametro de una funcion que levanta un modal
function CreaTurno(date) {
    //formateo la fecha
    var fecha = moment(date).format();
    //traigo la lista de pacientes en forma de Json
    var pacientesJs = traePacientes();
    //paso el Json de los pacientes a una lista de objetos de los pacientes
    var pacientes = JSON.parse(pacientesJs);
    //traigo la lista de profesionales en forma de Json
    var medicosJs = traeMedicos();
    //paso el Json de los profesionales a una lista de objetos de los profesionales 
    var medicos = JSON.parse(medicosJs);
    //inicializo las variables que voy a usar en los options
    var htmlOptMeds = '';
    var htmlOptPctes = '';
    //HTMLinicial hasta el select de medicos
    var html = '<form method="post" action="/Turno/Create"><table class="text-left" id = "tablaCrea"><tbody><tr style="padding-bottom: 15px"><td style="padding-bottom: 15px"><label>Fecha del Turno</label><input class="form-control" type="datetime-local" style="padding-bottom: 15px" name="Fecha_Inicio" value="'+ fecha +'" required /></td></tr><tr style="padding-bottom: 15px; padding-top: 15px"><td><div class="form-group"><select class="form-control custom-select" name="Id_Medico" required><option selected="">Asignar turno a:</option>';
    // le agrego el select de los profesionales
    $.each(medicos, function (index, value) {
        htmlOptMeds = htmlOptMeds + '<option value="' + value.Id_Medico + '">' + value.Apellido + ',' + value.Nombre +'</option>';
    });
    // cierro el select y sumo el html hasta el select de los pacientes
    html = html + htmlOptMeds + '</select></div></td></tr><tr><td><div class="form-group"><select class="form-control custom-select" name="Id_Paciente" required><option selected="">Paciente:</option>';
    //select de los pacientes
    $.each(pacientes, function (index, value) {
        htmlOptPctes = htmlOptPctes + '<option value="' + value.Id_Paciente + '">' + value.Apellido + ',' + value.Nombre +'</option>';
    });
    //cierro select y completo loque queda del html y cierro el form
    html = html + htmlOptPctes + '</select></div></td></tr></tbody></table ><button class="btn" onClick="confirmacion()">Enviar Datos</button></form >';
    //devuelvo el html completo
    return html;
}

//completa el select de profesionales para filtrar
$(document).ready(function () {
    $("#Profesionales").ready(function () {
        $.ajax({
            url: '/Medico/TraeMedicos', 
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                //parseo los profesionales, de  JSON a una lista de medicos
                var meds = JSON.parse(response);
                var colors = JSON.parse(traeColores());
                $("#Profesionales").empty();
                $("#Profesionales").append('<option selected value="all">Todos: </option>');
                $.each(meds, function (index, value) {
                    $("#Profesionales").append('<option value="' + value.Id_Medico + '">' + value.Apellido + ',' + value.Nombre + '</option>');
                });
            }
        });
    });
});

// Filtro de profesionales
$('#Profesionales').on('change', function () {
    select = $(this).val();
    console.log(select);
    $('#calendar').fullCalendar('rerenderEvents');
});

//Trae colores en formato json
function traeColores() {
    var colores = null;
    $.ajax({
        async: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Turno/TraeColores",
        dataType: "json",
        success: function (colors) {
            colores = colors;
        }
    });
    return colores;
}

// Trae turnos asignados al medico pasado por id en formato json
function TraeTurnosPorIdMedico(Id_Medico) {
    var turnos = null;
    var object = {};
    object.Id_Medico = Id_Medico;
    var events = [];
    //traigo todos los turnos que estan asociados al medico que paso por Id
    $.ajax({
        async: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Turno/TraeTurnosPorIdMedico",
        dataType: "JSON",
        data: object,
        success: function (trns) {
            turnos = trns;
        }

    });

    // A los turnos los mapeo con la funcion PushEvents
    events = PushEvents(turnos);
    //Retorno el array de los turnos, asociados al medico que paso pr Id, ya mapeados listos para renderizar
    return events;
}

//trae todos los turnos en formato json
function TraeTurnos() {
    var turnos = null;
    $.ajax({
        async: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Turno/GetEventData",
        dataType: "json",
        success: function (data) {
            turnos = data;
        }
    });
    return turnos;
}

//funcion que pushea los eventos a un arreglo
function PushEvents(eventosEntrada) {
    var events = [];
    //en turnos tengo la lista de eventos en JSON
    // traigo los medicos y los colores que le voy a asignar a cada uno
    var medicosJs = traeMedicos();
    var medicos = JSON.parse(medicosJs);
    var coloresJs = traeColores();
    var colores = JSON.parse(coloresJs);
    if ((typeof eventosEntrada) != "object") {
        eventosEntrada = JSON.parse(eventosEntrada);
    }
    $.each(eventosEntrada, function (i, data) {
        var bgc = '';
        // Si la fecha es anterior a la de ahora, le asigno color gris al turno
        if (moment().isAfter(data.Fecha_Fin)) {
            bgc = '#c4c4c4';
        }
        else {
            //Le asigno a cada medico un color, de la base de datos
            $.each(medicos, function (indx, m) {
                if (data.Id_Medico == m.Id_Medico) {
                    //Hago el mod 50 porque tengo 50 colores cargados en la base de datos. 
                    //A partir del medico numero 50 vuelvo a repetir colores
                    bgc = colores[indx%50].Hex;
                }
            })
        }
        var pacienteJs = traePaciente(data.Id_Paciente);
        var paciente = JSON.parse(pacienteJs);
        //Mapeo los eventos y los paso al arreglo events 
        events.push(
            {
                title: paciente.Apellido + ', ' + paciente.Nombre,
                start: moment(data.Fecha_Inicio).format("YYYY-MM-DD HH:mm:ss"),
                end: moment(data.Fecha_Fin).format("YYYY-MM-DD HH:mm:ss"),
                backgroundColor: bgc, borderColor: 'rgba(0,0,0,0.1)',
                Id_Turno: data.Id_Turno,
                Id_Paciente: data.Id_Paciente,
                Id_Medico: data.Id_Medico
            });
    });
    //Devuelvo los eventos mapeados
    return events
}

//ELIMINA TURNO - Funcion para llamar al modal de confirmacion
function eliminaTurno(Id_Turno) {
    Swal.fire({
        title: 'Esta seguro que desea cancelar el turno',
        text: "Una vez cancelado, no se puede revertir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, cancelarlo!',
    }).then((result) => {
        if (result.value) {
            $.ajax({
                async: false,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Turno/Delete/" + Id_Turno,
                success: Swal.fire({
                    text: 'Deleted!',
                    title: 'Your file has been deleted.',
                    icon: 'success',
                }).then(function () {
                    window.location.reload(true);
                })
            });
        }
    })
}

//DATOS GUARDADOS - Funcion que levanta modal de datos guardados
function confirmacion() {
    const Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: false,
        onOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    })
    Toast.fire({
        icon: 'success',
        title: 'Datos cargados exitosamente',
        onClose: location.href = 'Turno/Index',
    }).then(function(){
        location.href = 'Turno/Index';
    })
}