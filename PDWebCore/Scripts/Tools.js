function SetHintButton(hintButton, text, top, addToTitle, elementToAdd, position) {

    if (typeof top === "undefined") top = 0;
    if (typeof addToTitle === "undefined") addToTitle = false;
    if (typeof elementToAdd === "undefined") elementToAdd = null;
    if (typeof (position) === "undefined") position = 'right';

    $(hintButton).addClass('helper').append($('<span>').addClass('glyphicon glyphicon-question-sign'));

    if (addToTitle) {
        $('.title').append(hintButton);
    }

    if (elementToAdd != null) {
        $(elementToAdd).append(hintButton);
    }

    tooltip.info($(hintButton).get(0), text, position);
}

function Round(n, k) {
    var factor = Math.pow(10, k);
    return Math.round(n * factor) / factor;
}

function objectHider(obj) {
    this.getObject = function () { return obj; };
}

function goBack() {
    window.history.back();
}

String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};

function arrayFirstIndexOf(array, predicate, predicateOwner) {
    for (var i = 0, j = array.length; i < j; i++) {
        if (predicate.call(predicateOwner, array[i])) {
            return i;
        }
    }
    return -1;
}

function arraySumBy(array, predicate, predicateOwner) {
    var sum = 0;

    for (var i = 0, j = array.length; i < j; i++) {
        sum += predicate.call(predicateOwner, array[i])
    }

    return sum;
}

function arrayLengthOf(array, predicate, predicateOwner) {
    var length = 0;

    for (var i = 0, j = array.length; i < j; i++) {
        if (predicate.call(predicateOwner, array[i])) {
            return length++;
        }
    }
    return length;
}

Date.prototype.addHours = function (h) {
    this.setTime(this.getTime() + (h * 60 * 60 * 1000));
    return this;
}

Date.prototype.toDateTime = function () {
    return this.getFullYear() + "-" + ("0" + (this.getMonth() + 1)).slice(-2) + "-" +
        ("0" + this.getDate()).slice(-2) + " " + ("0" + this.getHours()).slice(-2) + ":" + ("0" + this.getMinutes()).slice(-2)
}

Date.prototype.toDate = function () {
    return this.getFullYear() + "-" + ("0" + (this.getMonth() + 1)).slice(-2) + "-" +
        ("0" + this.getDate()).slice(-2)
}

Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
}

function ShowOrHidePanel(event) {
    var target = event.target;
    if (target.className != 'panel-heading') {
        event.stopPropagation();
        return;
    }

    if (target.nextElementSibling == null) {
        event.stopPropagation();
        return;
    }

    if (target.nextElementSibling.style.display != 'none')
        $(target).next().slideUp("slow", function () {
            $(target).parent().attr('class', 'panel-success');
            $(target).children().eq(2).attr('class', 'glyphicon glyphicon-chevron-down');
        });
    else {
        $(target).next().slideDown("slow", function () {
            $(target).parent().attr('class', 'panel panel-success');
            $(target).children().eq(2).attr('class', 'glyphicon glyphicon-chevron-up');
        });
    }

    event.stopPropagation();
}

var emailRegex = /^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/;
var positiveNumberRegex = /^\d*[1-9]\d*$/;

function createDialog(dialogFormFields, validateTipsId, openButtonId, submitFunction, dialogId, hasHidden, afterClose, afterOpen, height, width) {
    height = typeof height === 'undefined' ? 'auto' : height;
    width = typeof width === 'undefined' ? 'auto' : width;

    var dialog, form,

        allFields = $([]),
        tips = $("#" + validateTipsId),
        editors = dialogFormFields.filter(function (element) { return element.IsEditor == true; });


    function updateTips(t) {
        tips
            .text(t)
            .addClass("ui-state-highlight");
        setTimeout(function () {
            tips.removeClass("ui-state-highlight", 1500);
        }, 500);
    }

    function checkLengthSub(o, min, max) {
        var result;

        if (!o[0].id.includes('cke_')) {
            result = o.val().length > max || o.val().length < min;
        }
        else {
            o = editors.find(function (element) { return element.Id == o[0].id; });
            result = o.EditorInstance.getData().length > max || o.EditorInstance.getData().length < min;
        }

        return result;
    }

    function checkLength(o, n, min, max) {
        if (checkLengthSub(o, min, max)) {
            o.addClass("ui-state-error");
            updateTips(n + " musi mieć długość pomiędzy " +
                min + " i " + max + ".");
            return false;
        } else {
            return true;
        }
    }

    function checkRegexp(o, regexp, n) {
        if (!(regexp.test(o.val()))) {
            o.addClass("ui-state-error");
            updateTips(n);
            return false;
        } else {
            return true;
        }
    }

    function submit() {
        if (allFields.length == 0) {
            for (var i = 0; i < dialogFormFields.length; i++) {
                allFields = allFields.add($("#" + dialogFormFields[i].Id));
            }
        }

        var valid = true;
        allFields.removeClass("ui-state-error");

        var el;

        for (var i = 0; i < dialogFormFields.length; i++) {
            el = dialogFormFields[i];

            if (el.ValidateRegexp) {
                valid = valid && checkRegexp($("#" + el.Id), el.Regexp, el.RegexpError);
            }
            else {
                valid = valid && checkLength($("#" + el.Id), el.Name, el.MinLength, el.MaxLength);
            }
        }

        if (valid) {
            submitFunction();

            dialog.dialog("close");

            if (afterClose != null) {
                afterClose();
            }
        }
        return valid;
    }

    dialog = $("#" + dialogId).dialog({
        autoOpen: false,
        height: height,
        width: width,
        modal: true,
        buttons: {
            Wyślij: submit,
            Zamknij: function () {
                if (confirm("Czy na pewno chcesz zamknąć?"))
                    dialog.dialog("close");
            }
        },
        close: function () {
            form[0].reset();
            allFields.removeClass("ui-state-error");
        }
    });

    form = dialog.find("form").on("submit", function (event) {
        event.preventDefault();
        submit();
    });


    $(form).on('reset', function () {
        if (hasHidden) {
            $("input[type='hidden']", $(this)).val(0);
        }

        if (typeof (editors) !== 'undefined' && editors.length > 0) {
            for (var i = 0; i < editors.length; i++) {
                editors[i].EditorInstance.setData('');
            }
        }

        tips.text('');
    });

    $("#" + openButtonId).button().on("click", function (e) {
        dialog.dialog("open");

        if (typeof (editors) !== 'undefined' && editors.length > 0) {
            for (var i = 0; i < editors.length; i++) {
                if (!CKEDITOR.instances[editors[i].Id.replace('cke_', '')]) {
                    editors[i].EditorInstance = CKEDITOR.replace(editors[i].Id.replace('cke_', ''), {
                        language: 'pl'
                    });
                }
            }
        }

        if (afterOpen != null) {
            afterOpen();
        }

        e.stopPropagation();
    });
}

//function HandleError(data) {
//    if (typeof data.responseJSON == 'string' && data.responseJSON.startsWith('error_')) {
//        var errors = data.responseJSON.replace('error_', '');

//        alert("Błędy:\n" + errors);

//        return true;
//    }

//    return false;
//}

function HandleConfirm(data) {
    if (data.IsConfirm) {
        return confirm(data.Message);
    }

    return true;
}

function HandleError(data) {
    if (data.IsError) {

        alert("Błędy:\n" + data.Message);

        return true;
    }

    return false;
}

function IsError(requestT, data) {
    if (requestT == requestType.GET) {
        return (typeof data === 'string' && data.startsWith('error_'));
    }
    else {
        return (typeof data.responseJSON === 'string' && data.responseJSON.startsWith('error_'));
    }
}

/**
 * Aktywuje daną zakładkę
 * @param {any} element Id zakładki do aktywowania
 * @param {any} withDrop Czy włączyć animację?
 */
function ActivateFolder(element, withDrop) {
    if (withDrop) {
        var folders = $(".folder:visible");

        if (folders.length) {
            $(".folder:visible").toggle("drop", {}, 250, function () {
                $('#' + element).toggle("drop", { direction: "right" }, 250);
            });
        }
        else {
            $('#' + element).show();
        }
    }
    else {
        $(".folder:visible").hide();
        $('#' + element).show();
    }
}

function SetLoading(res) {
    if (res) {
        $('#overlay').show();
        $('#loading').show();
    }
    else {
        $('#overlay').hide();
        $('#loading').hide();
    }
}

var requestType = {
    POST: 'POST',
    GET: 'GET',
    PUT: 'PUT',
    DELETE: 'DELETE'
};

//function SetHashButton(formId, hash, eq, condition) {
//    SetHash(formId, hash, eq, condition, "button");
//}

//function SetHashLink(formId, hash, eq, condition) {
//    SetHash(formId, hash, eq, condition, "a");
//}

function SetHash(formId, hash, id, condition) {
    $("#" + formId + " #" + id).click(function (event) {
        event.preventDefault();
        if ($("#" + formId).valid()) {
            if (!condition || condition()) {
                GoToFolder(hash);
            }
        }
    });
}

function IsPositive(o) {
    o = ko.utils.unwrapObservable(o);

    return (!IsUndefined(o) && o !== null && o > 0);
}

function IsNegative(o) {
    o = ko.utils.unwrapObservable(o);

    return (IsUndefined(o) || o === null);
}

function GoToFolder(name) {
    location.hash = "#" + name;
}

function RefreshUnobtrusiveValidator(formId) {
    var form = $("#" + formId);

    form.removeData('validator');

    form.removeData('unobtrusiveValidation');

    $.validator.unobtrusive.parse(form);
}

function GetPath() {
    if (MainConfig)
        return GetMainConfigElement("path");
    else
        return "";
}

function GetMainConfigElement(elementName) {
    return MainConfig.getObject()[elementName];
}

function SendRequest(requestT, url, params, condition, onComplete, onSuccess, onError, replaceTargetId, messageTargetId) {
    if (!condition || condition()) {
        SetLoading(true);

        var errorHandler = HandleError;

        url = GetPath() + url;

        //        console.log(params);

        //        console.log(ko.toJSON(params));

        $.ajax({
            method: requestT,
            data: params,
            url: url,
            contentType: "application/json; charset=utf-8",
            headers: app ? { 'Authorization': 'Bearer ' + app.dataModel.getAccessToken() } : undefined,
            success: function (data) {
                if (onSuccess && !errorHandler(data) && HandleConfirm(data)) {
                    if (replaceTargetId) {
                        $("#" + replaceTargetId).html(data.View);
                    }

                    if (messageTargetId) {
                        $("#" + messageTargetId).text(data.Message);
                    }

                    onSuccess(data);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status + ' - ' + thrownError + ' ' + xhr.responseJSON.message);

                if (app && xhr.status === 401) {
                    app.reauthorize();
                }

                if (onError) {
                    onError(data);
                }
            },
            complete: function (data) {
                if (onComplete) {
                    onComplete(data.responseJSON);
                }
                else if (!onComplete && !onSuccess && !onError) {
                    errorHandler(data.responseJSON);
                }

                SetLoading(false);
            }
        });
    }
}

var MainConfig = null;

var tooltip = {
    /**
    * Make a Tooltip
    **/
    make: function make(target, content) {
        var orientation = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : "right"; var type = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : "help";
        return new Tooltip({
            target: target,
            content: content,
            classes: "tooltip " + type + "-" + orientation,
            position: orientation + " middle"
        });

    },

    /**
    * Help tooltip
    **/
    help: function help(t, c) {
        var o = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : "right";
        return this.make(t, c, o, "help");
    },

    /**
    * Info Tooltip
    **/
    info: function info(t, c) {
        var o = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : "right";
        return this.make(t, c, o, "info");
    }
};

function getFormData($form) {
    var formData = $form.serializeArray();

    //formData.push({ name: "X-Requested-With", value: "XMLHttpRequest" });

    //    var data = $form.serializeArray();

    //    data.push({ name: "X-Requested-With", value: "XMLHttpRequest" });

    //    var formdata = new FormData();

    //    $.each(data, function (i, v) {
    //        formdata.append(v.name, v.value);
    //    });

    return formData;
}

function OnAjaxSubmitCompleted(data, onSuccess) {
    data = data.responseJSON;

    var errorHandler = HandleError;

    if (!errorHandler(data) && HandleConfirm(data)) {
        if (onSuccess) {
            onSuccess(data.Data);
        }
    }
    else {
        return false;
    }
}

function auto_grow(element) {
    element.style.height = "5px";
    element.style.height = (element.scrollHeight) + "px";
}

Array.prototype.clone = function () {
    return JSON.parse(JSON.stringify(this));
};

//control visibility, give element focus, and select the contents (in order)
ko.bindingHandlers.visibleAndSelect = {
    update: function (element, valueAccessor) {
        ko.bindingHandlers.visible.update(element, valueAccessor);
        if (valueAccessor()) {
            setTimeout(function () {
                $(element).find("input").focus().select();
            }, 0); //new tasks are not in DOM yet
        }
    }
};

ko.bindingHandlers.flash = {
    init: function (element) {
        $(element).hide();
    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        if (value) {
            $(element).stop().hide().text(value).fadeIn(function () {
                clearTimeout($(element).data("timeout"));
                $(element).data("timeout", setTimeout(function () {
                    $(element).fadeOut();
                    valueAccessor()(null);
                }, 3000));
            });
        }
    },
    timeout: null
};

ko.bindingHandlers.jqButton = {
    init: function (element) {
        $(element).button(); // Turns the element into a jQuery UI button
    },
    update: function (element, valueAccessor) {
        var currentValue = valueAccessor();
        // Here we just update the "disabled" state, but you could update other properties too
        $(element).button("option", "disabled", currentValue.enable === false);
    }
};

ko.bindingHandlers.fadeVisible = {
    init: function (element, valueAccessor) {
        // Start visible/invisible according to initial value
        var shouldDisplay = valueAccessor();
        $(element).toggle(shouldDisplay);
    },
    update: function (element, valueAccessor) {
        // On update, fade in/out
        var shouldDisplay = valueAccessor();
        shouldDisplay ? $(element).fadeIn() : $(element).fadeOut();
    }
};

ko.bindingHandlers.starRating = {
    init: function (element, valueAccessor) {
        $(element).addClass("starRating");
        for (var i = 0; i < 5; i++)
            $("<span>").appendTo(element);

        // Handle mouse events on the stars
        $("span", element).each(function (index) {
            $(this).hover(
                function () { $(this).prevAll().add(this).addClass("hoverChosen") },
                function () { $(this).prevAll().add(this).removeClass("hoverChosen") }
            ).click(function () {
                var observable = valueAccessor();  // Get the associated observable
                observable(index + 1);               // Write the new rating to it
            });
        });
    },
    update: function (element, valueAccessor) {
        // Give the first x stars the "chosen" class, where x <= rating
        var observable = valueAccessor();
        $("span", element).each(function (index) {
            $(this).toggleClass("chosen", index < observable());
        });
    }
};


ko.subscribable.fn.subscribeChanged = function (callback) {
    let oldValue;

    this.subscribe(function (_oldValue) {
        oldValue = _oldValue;
    }, this, 'beforeChange');

    this.subscribe(function (newValue) {
        callback(newValue, oldValue);
    });
};

const hideFirstOption = function (option, item) {
    if (item.Key === 0) {
        ko.applyBindingsToNode(option, { visible: false }, item);
    }
};

const contextMenuClicked = function (_e, event) {
    event.preventDefault();
};

Array.prototype.setAll = function (v) {
    var i, n = this.length;
    for (i = 0; i < n; ++i) {
        this[i] = v;
    }
};

Array.prototype.insert = function (index, item) {
    this.splice(index, 0, item);
};

Array.prototype.last = function () {
    return this[this.length - 1];
};

function SetProgressBar(value) {
    $(".progress-bar").width(value + "%").html(value + "%");
}

function ScrollInto(scrollingDiv, elementInDiv, position) {
    if (typeof position === "undefined") {
        position = 0;
    }

    if (typeof elementInDiv !== "undefined") {
        var elInDiv = $(elementInDiv);

        if (elInDiv.length) {
            $(scrollingDiv).animate({ scrollTop: $(scrollingDiv).scrollTop() + (elInDiv.offset().top - $(scrollingDiv).offset().top) + position });
        }
    }
    else {
        $(scrollingDiv).animate({ scrollTop: position });
    }
}

function IsUndefined(o) {
    return (typeof o === "undefined");
}

function HasType(o, type) {
    return (typeof o === type);
}

function IsCallback(o) {
    return HasType(o, "function");
}

var sortType = {
    asc: "asc",
    desc: "desc"
};

const capitalize = (str, lower = false) =>
    (lower ? str.toLowerCase() : str).replace(/(?:^|\s|["'([{])+\S/g, match => match.toUpperCase());
;

function GetUTCOffset() {
    var hrs = -(new Date().getTimezoneOffset() / 60);
    return hrs;
}
