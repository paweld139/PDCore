/**
 * Main ViewModel
 * */
function CommonViewModel() {

    //#region Default

    var self = this;

    self.folders = null;

    //#endregion


    //#region AppointmentType

    self.SelectedAppointmentTypeId = ko.observable();

    //#endregion


    //#region Jquery Validator settings

    $.validator.setDefaults({ ignore: ":hidden" });

    jQuery.validator.addMethod("enforcetrue", function (value, element, param) {
        return element.checked;
    });

    jQuery.validator.unobtrusive.adapters.addBool("enforcetrue");

    //#endregion


    //#region Comments

    //self.ValidateTerm = function () {
    //    var res = $('#mainForm').validate().element('#Term_Date');

    //    if (res) {
    //        SendRequest(requestType.POST, "/Scheduler/ValidateTerm",
    //            { doctorId: $("#Appointment_DoctorId").val(), examinationId: self.SelectedExaminationId(), date: $("#Term_Date").val() }, null, null,
    //            function (data) {
    //                RefreshUnobtrusiveValidator("mainForm");
    //                SetHash("mainForm", null, "patient-btn", function () { $("form").submit(); return false; });
    //                $("#Term_Date").val(data.from);
    //                $("#Term_DateTo").val(data.to);
    //                GoToFolder("patient");
    //            }, null, "patient");
    //    }
    //};

    //#endregion


    //#region PatientView

    self.GetPatientView = new OnLoad(function () {
        SendRequest(requestType.POST, "/Scheduler/GetPatientPartialView", null, null, null, function () {
            RefreshUnobtrusiveValidator("mainForm");
            SetHash("mainForm", null, "patient-btn", function () { $("form").submit(); return false; });
            GoToFolder("patient");
        }, null, "patient");
    });

    self.GoToPatientView = function () {
        var res = $('#mainForm').validate().element('#Term_Date');

        if (res) {
            if (self.GetPatientView.Loaded) {
                GoToFolder("patient");
            }

            self.GetPatientView.Execute();
        }
    };

    //#endregion


    //#region Validate

    self.IsExamination = function () {
        return (self.SelectedAppointmentTypeId() > 1);
    };

    self.ValidateTerm = function (date) {
        SendRequest(requestType.POST, "/Scheduler/ValidateTerm",
            { doctorId: $("#Appointment_DoctorId").val(), examinationId: self.SelectedExaminationId(), date: date }, null, null,
            function (data) {
                $("#Term_Date").val(data.from);
                $("#Term_DateTo").val(data.to);
            }, null, null, "term-info");
    };

    self.ValidateExamination = function () {
        if (IsNegative(self.SelectedExaminationId) && self.IsExamination()) {
            alert("Nie wybrano badania");

            return false;
        }

        return true;
    };

    self.ValidateCode = function () {
        SendRequest(requestType.POST, "/Scheduler/ValidateCode", getFormData($("#mainForm")), null, null, function () {
            GoToFolder("information");
        }, null, "information");
    };

    //#endregion


    //#region Initialize

    self.InitiateHashButtons = function () {
        if (IsPositive(self.SelectedSpecialityId)) {
            self.SelectedAppointmentTypeId.subscribe(
                function (newValue) {
                    self.SelectedSpecialityId.notifySubscribers(self.SelectedSpecialityId());

                    if (newValue === "3") {
                        self.GetExaminations(-1, newValue);
                    }
                });

            self.SelectedExaminationId.subscribe(function (examinationId) {
                self.GetDoctors(examinationId);
            });

            SetHash("mainForm", null, "appointment-btn", function () {
                if (!self.ValidateExamination()) {
                    return false;
                }

                self.GoToPatientView();

                return false;
            });
        }
        else {
            self.SelectedExaminationId.subscribe(function (examinationId) {
                if (self.SelectedAppointmentTypeId() === "3") {
                    self.GetSpecialities(self.SelectedCategoryId(), examinationId);
                }
                else {
                    self.GetDoctors(examinationId);
                }
            });

            self.SelectedAppointmentTypeId.subscribeChanged(function (newValue, oldValue) {
                if (newValue === "3") {
                    $("#specialization-div").before($("#examination-div"));

                    self.GetExaminations(-1, newValue);
                }
                else if (oldValue === "3") {
                    $("#specialization-div").after($("#examination-div"));
                }

                if (newValue == "1" || newValue == "2") {
                    self.SelectedCategoryId(-1);;
                }
            });

            SetHash("mainForm", "term", "appointment-btn", function () {
                return self.ValidateExamination();
            });

            SetHash("mainForm", null, "term-btn", function () {
                self.GoToPatientView();

                return false;
            });
        }
    };

    self.InitiateElements = function () {
        $("#datepicker").datepicker({
            changeMonth: true,
            changeYear: true,
            onSelect: function (_dateText) {
                self.ValidateTerm($(this).datepicker("getDate").toDateTime());
            }
        });

        $("a.btn").click(goBack);


        $("#mainForm").submit(function () {
            SendRequest(requestType.POST, "/Scheduler/Index", getFormData($(this)), null, null, function () {
                RefreshUnobtrusiveValidator("mainForm");
                SetHash("mainForm", null, "validation-btn", function () { self.ValidateCode(); return false; });
                GoToFolder("validation");
            }, null, "validation");

            return false;
        });

        self.InitiateHashButtons();
    };

    self.OnLoad = new OnLoad(function () {
        self.folders = ["appointment", "term", "patient", "validation", "information"];

        self.InitiateElements();

        GoToFolder("appointment");
    });

    self.Initialize = function () {
        self.OnLoad.Execute();
    };

    //#endregion


    //#region Specialities

    self.specialities = ko.observableArray();

    self.SelectedSpecialityId = ko.observable($("#Appointment_SpecializationId").val());

    self.SelectedSpecialityId.subscribe(function (specialityId) {
        let appointmentTypeId = self.SelectedAppointmentTypeId();

        if (self.IsExamination() && appointmentTypeId !== "3") {
            self.GetExaminations(specialityId, appointmentTypeId);
        }
        else {
            if (appointmentTypeId === "3") {
                self.GetDoctors(self.SelectedExaminationId());
            }
            else {
                self.GetDoctors(null);
            }
        }
    });

    self.GetSpecialities = function (categoryId, examinationId) {
        let selectedAppointmentTypeId = self.SelectedAppointmentTypeId();

        if (categoryId == -1 || (selectedAppointmentTypeId === "3" && examinationId === "-1")) {
            self.specialities(null);
        }

        SendRequest(requestType.GET, "/Scheduler/GetSpecialities", { categoryId: categoryId, examinationId: examinationId },
            function () {
                return (categoryId > -1 && (selectedAppointmentTypeId !== "3" || examinationId !== "-1"));
            }, null,
            function (data) {
                self.specialities(data);
            });
    };

    //#endregion


    //#region Examinations

    self.examinations = ko.observableArray();

    self.SelectedExaminationId = ko.observable();

    self.CanShowExamination = ko.computed(function () {
        return self.IsExamination();
    });

    self.GetExaminations = function (specialityId, appointmentTypeId) {
        if (specialityId == -1 && appointmentTypeId != 3) {
            self.examinations(null);
        }

        SendRequest(requestType.GET, "/Scheduler/GetExaminations", { specialityId: specialityId, appointmentTypeId: appointmentTypeId },
            function () {
                return ((specialityId > -1 || appointmentTypeId == 3) && self.IsExamination());
            },
            function (data) {
                if (data.IsError) {
                    self.examinations(null);
                }
            },
            function (data) {
                self.examinations(data);
            });
    };

    //#endregion


    //#region Doctors

    self.doctors = ko.observableArray();

    self.GetDoctors = function (examinationId) {
        var specialityId = self.SelectedSpecialityId();
        var appointmentTypeId = self.SelectedAppointmentTypeId();

        if (((appointmentTypeId <= 1 || appointmentTypeId == 3) && !IsPositive(specialityId)) || !IsPositive(examinationId)) {
            self.doctors(null);
        }

        SendRequest(requestType.GET, "/Scheduler/GetDoctors", { examinationId: examinationId, specialityId: specialityId },
            function () {
                return (((appointmentTypeId <= 1 || (appointmentTypeId == 3 && IsPositive(examinationId))) && IsPositive(specialityId)) || (IsPositive(examinationId) && appointmentTypeId != 3));
            },
            function (data) {
                if (!IsUndefined(data) && data.IsError) {
                    self.doctors(null);
                }
            },
            function (data) {
                self.doctors(data);
            });
    };

    //#endregion


    //#region Category

    self.SelectedCategoryId = ko.observable();

    self.SelectedCategoryId.subscribe(function (categoryId) {
        if (self.SelectedAppointmentTypeId() !== "3") {
            self.GetSpecialities(categoryId);
        }
    });

    //#endregion
}

//#region Default

var MainViewModel =
{
    CommonVM: new CommonViewModel()
};

ko.applyBindings(MainViewModel);

function RunSammy() {
    SetMainConfig(new Config(""));

    MainViewModel.CommonVM.Initialize();

    Sammy(function () {
        let cVM = MainViewModel.CommonVM;

        this.get('#:folder', function () {
            let currentFolder = this.params.folder;

            if (cVM.folders.includes(currentFolder)) {
                ActivateFolder(currentFolder);

                //SetProgressBar(Round((100 / (cVM.folders.length - 1)) * cVM.folders.indexOf(this.params.folder), 0));
            }
            else {
                GoToFolder(cVM.folders[0]);
            }
        });
    }).run();
}

//#endregion


