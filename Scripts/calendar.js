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
        dateClick: function (date) {
            showModal('Turno Nuevo', CreaTurno(date), null)},
        height: 'parent',
        events: function (start, end, timezone, callback) {
            $.ajax({
                type: "GET",
                contentType: " application/json",
                url: "/Turno/GetEventData",
                dataType: "JSON",
                success: function (data) {
                    var events = [];
                    $.each(data, function (i, data) {
                        var bgc = '';
                        if (moment().isAfter(data.Fecha_Fin)) {
                            bgc = '#c4c4c4';
                        }
                        else {
                            bgc = '#4287f5';
                        }
                        events.push(
                            {
                                Title: data.Id_Paciente,
                                start: moment(data.Fecha_Inicio).format("YYYY-MM-DD HH:mm:ss"),
                                end: moment(data.Fecha_Fin).format("YYYY-MM-DD HH:mm:ss"),
                                backgroundColor: bgc, borderColor: 'black',
                                Id_Turno: data.Id_Turno
                            });
                    });
                    callback(events);
                }
            })
        },
        dayClick: function (date, jsEvent, view) {
            if (view.name == 'month') {
                $('#calendar').fullCalendar('changeView', 'agendaDay', date);
            } else {
                showModal('Turno Nuevo', CreaTurno(date), null)
            }},
        nextDayThreshold: '00:00:00',
        editable: false,
        droppable: false,
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
        }
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
        var turno = JSON.parse(body);
        var pacienteJs = traePaciente(turno.Id_Paciente);
        var medicoJs = traeMedico(turno.Id_Medico);
        var paciente = JSON.parse(pacienteJs);
        var medico = JSON.parse(medicoJs);
        var eventDetail = 'Paciente: ' + paciente.Apellido + ', ' + paciente.Nombre + '</br>';
        var eventInfo = 'Profesional: ' + medico.Apellido + ', ' + medico.Nombre + '</br>';
        var eventStart = 'Fecha y Hora: ' + moment(turno.Fecha_Inicio).format("DD-MM-YY HH:mm") + '</br>';
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
function CreaTurno(date) {
    var fecha = moment(date).format();
    /*if (!fecha.getHours().toString()) {
       date.setHours(1);
        date.setMinutes(00);
    }*/

   
    var pacientesJs = traePacientes();
    var pacientes = JSON.parse(pacientesJs);
    var medicosJs = traeMedicos();
    var medicos = JSON.parse(medicosJs);
    var htmlOptMeds = '';
    var htmlOptPctes = '';
    //HTMLinicial hasta el select de medicos
    var html = '<form method="post" action="/Turno/Create"><table class="text-left" id = "tablaCrea"><tbody><tr style="padding-bottom: 15px"><td style="padding-bottom: 15px"><label>Fecha del Turno</label><input class="form-control" type="datetime-local" style="padding-bottom: 15px" name="Fecha_Inicio" value="'+ fecha +'" required /></td></tr><tr style="padding-bottom: 15px; padding-top: 15px"><td><div class="form-group"><select class="form-control custom-select" name="Id_Medico"><option selected="">Asignar turno a:</option>';
    $.each(medicos, function (index, value) {
        htmlOptMeds = htmlOptMeds + '<option value="' + value.Id_Medico + '">' + value.Apellido + ',' + value.Nombre +'</option>';
    });
    html = html + htmlOptMeds + '</select></div></td></tr><tr><td><div class="form-group"><select class="form-control custom-select" name="Id_Paciente"><option selected="">Paciente:</option>';
    $.each(pacientes, function (index, value) {
        htmlOptPctes = htmlOptPctes + '<option value="' + value.Id_Paciente + '">' + value.Apellido + ',' + value.Nombre +'</option>';
    });
    html = html + htmlOptPctes + '</select></div></td></tr></tbody></table ><button class="btn">Enviar Datos</button></form >';

    return html;
}