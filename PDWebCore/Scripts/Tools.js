function SetHintButton(hintButton, text, top, addToTitle, elementToAdd, position) {

    if (typeof top === "undefined") top = 0;
    if (typeof addToTitle === "undefined") addToTitle = false;
    if (typeof elementToAdd === "undefined") elementToAdd = null;
    if (typeof (position) === "undefined") position = 'right';
    if (IsUndefined(text)) text = $(hintButton).text();

    if (!$(hintButton).hasClass("helper")) {
        $(hintButton).addClass('helper').append($('<span>').addClass('fa fa-question-circle ml-2'));

        if (addToTitle) {
            $('.title').append(hintButton);
        }

        if (!IsUndefinedOrNull(elementToAdd)) {
            $(elementToAdd).append(hintButton);
        }
    }

    tooltip.info($(hintButton).get(0), text, position);
}

function Tag(tag, content) {
    var element = '<' + tag + '>' + content + '</' + tag + '>';
    return element;
}

const br = '<br />'


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

function arrayExist(array, predicate, predicateOwner) {
    let result = false;

    for (let i = 0, j = array.length; i < j; i++) {
        if (predicate.call(predicateOwner, array[i])) {
            result = true;
        }
    }
    return result;
}

Date.prototype.addSeconds = function (s) {
    this.setSeconds(this.getSeconds() + s);
    return this;
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

Date.prototype.addMonths = function (months) {
    var date = new Date(this.valueOf());
    date.setMonth(date.getMonth() + months);
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
        var folders = $(".folder:visible:not(#" + element + ")");

        if (folders.length) {
            $(".folder:visible:not(#" + element + ")").toggle("drop", {}, 250, function () {
                $('#' + element).toggle("drop", { direction: "right" }, 250);
            });
        }
        else {
            $('#' + element).show();
        }
    }
    else {
        $(".folder:visible:not(#" + element + ")").hide();
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

function SetHash(formId, hash, id, condition, before) {
    $("#" + formId + " #" + id).click(function (event) {
        event.preventDefault();

        if (before) {
            before();
        }

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

function getTimezoneOffset() {
    let d = new Date()
    return d.getTimezoneOffset();
}

//Default headers
//$.ajaxSetup({ 
//    headers: { 'x-my-custom-header': 'some value' }
//});

//Set header for requests
$.ajaxSetup({
    beforeSend: function (xhr) {
        xhr.setRequestHeader('x-timezone-offset', getTimezoneOffset());
    }
});

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
            dataType: 'json',
            headers: app ?
                {
                    'Authorization': 'Bearer ' + app.dataModel.getAccessToken(),
                } : undefined,
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
                if (app && xhr.status === 401) {
                    app.reauthorize();
                }
                else if (onError) {
                    onError(xhr.responseJSON);
                }
                else {
                    alert(xhr.status + ' - ' + thrownError + ' ' + xhr.responseJSON.message);
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

Array.prototype.isEmpty = function () {
    return IsUndefinedOrNull(this) || this.length === 0;
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
    update: function (element, valueAccessor, allBindingsAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());

        var allBindings = allBindingsAccessor(),
            duration = allBindings.flashDuration || 3000;

        if (value) {
            $(element).stop().hide().text(value).fadeIn(function () {
                clearTimeout($(element).data("timeout"));
                $(element).data("timeout", setTimeout(function () {
                    $(element).fadeOut();
                    valueAccessor()(null);
                }, duration));
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

ko.bindingHandlers.jqButtonWithOptions = {
    init: function (element) {
        $(element).button(); // Turns the element into a jQuery UI button
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor(),
            options = ko.utils.extend(value.options || {}, ko.bindingHandlers.jqDialog.defaultOptions);
        // Here we just update the "disabled" state, but you could update other properties too
        $(element).button("option", "disabled", value.enable === false);
        //$(element).button("option", "label", value.enable ? "Finished" : "Um, Not Gunna Happen");
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

ko.bindingHandlers.fadeVisibleWithDuration = {
    init: function (element, valueAccessor) {
        // Start visible/invisible according to initial value
        var shouldDisplay = valueAccessor();
        $(element).toggle(shouldDisplay);
    },
    update: function (element, valueAccessor, allBindingsAccessor) {
        // On update, fade in/out
        var shouldDisplay = valueAccessor(),
            allBindings = allBindingsAccessor(),
            duration = allBindings.fadeDuration || 500; // 500ms is default duration unless otherwise specified

        shouldDisplay ? $(element).fadeIn(duration) : $(element).fadeOut(duration);
    }
};

ko.bindingHandlers.slideVisible = {
    init: function (element, valueAccessor) {
        // Start visible/invisible according to initial value
        //var shouldDisplay = valueAccessor();
        $(element).toggle(valueAccessor());
    },
    update: function (element, valueAccessor, allBindingsAccessor) {
        var
            // First get the latest data that we're bound to
            value = valueAccessor(),
            // Now get the other bindings in the same data-bind attr
            allBindings = allBindingsAccessor(),

            // Next, whether or not the supplied model property is observable, get its current value
            valueUnwrapped = ko.utils.unwrapObservable(value),

            // 400ms is default duration unless otherwise specified
            duration = allBindings.slideDuration || 400;

        // Now manipulate the DOM element
        if (valueUnwrapped == true) {
            $(element).slideDown(duration); // Make the element visible
        } else {
            $(element).slideUp(duration); // Make the element invisible
        }
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

Array.prototype.first = function () {
    return this[0];
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

function IsUndefinedOrNull(o) {
    return IsUndefined(o) || o === null;
}

function HasType(o, type) {
    return (typeof o === type);
}

function IsFunction(o) {
    return HasType(o, "function");
}

function IsCallback(o) {
    return IsFunction(o);
}

var sortType = {
    asc: "asc",
    desc: "desc"
};

const capitalize = (str, lower = false) =>
    (lower ? str.toLowerCase() : str).replace(/(?:^|\s|["'([{])+\S/g, match => match.toUpperCase());
;

function getUTCOffset() {
    var hrs = -(new Date().getTimezoneOffset() / 60);
    return hrs;
}

function parseISOString(s) {
    var b = s.split(/\D+/);
    return new Date(Date.UTC(b[0], --b[1], b[2], b[3], b[4], b[5], b[6]));
}

function parseISOStringNoLocal(s) {
    let date = parseISOString(s);

    return new Date(date.getTime() + (date.getTimezoneOffset() * 60000));
}

function isoFormatDMY(d) {
    function pad(n) { return (n < 10 ? '0' : '') + n }
    return pad(d.getUTCDate()) + '/' + pad(d.getUTCMonth() + 1) + '/' + d.getUTCFullYear();
}

ko.bindingHandlers.foreachprop = {
    transformObject: function (obj) {
        var properties = [];
        ko.utils.objectForEach(obj, function (key, value) {
            properties.push({ key: key, value: value });
        });
        return properties;
    },
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var properties = ko.pureComputed(function () {
            var obj = ko.utils.unwrapObservable(valueAccessor());
            return ko.bindingHandlers.foreachprop.transformObject(obj);
        });
        ko.applyBindingsToNode(element, { foreach: properties }, bindingContext);
        return { controlsDescendantBindings: true };
    }
};

function makeIterator(array) {
    var nextIndex = 0;

    return {
        next: function () {
            return nextIndex < array.length ?
                { value: array[nextIndex++], done: false } :
                { done: true };
        }
    };
}

function* idMaker() {
    var index = 0;
    while (true)
        yield index++;
}

function* gen() {
    yield* ['a', 'b', 'c'];
}

function* fibonacci() {
    var fn1 = 0;
    var fn2 = 1;
    while (true) {
        var current = fn1;
        fn1 = fn2;
        fn2 = current + fn1;
        var reset = yield current;
        if (reset) {
            fn1 = 0;
            fn2 = 1;
        }
    }
}

function getAsync(url) {
    return new Promise((resolve, reject) => {
        const xhr = new XMLHttpRequest();
        xhr.open("GET", url);
        xhr.onload = () => resolve(xhr.responseText);
        xhr.onerror = () => reject(xhr.statusText);
        xhr.send();
    });
}

function imgLoad(url) {
    // Create new promise with the Promise() constructor;
    // This has as its argument a function
    // with two parameters, resolve and reject
    return new Promise(function (resolve, reject) {
        // Standard XHR to load an image
        var request = new XMLHttpRequest();
        request.open('GET', url);
        request.responseType = 'blob';
        // When the request loads, check whether it was successful
        request.onload = function () {
            if (request.status === 200) {
                // If successful, resolve the promise by passing back the request response
                resolve(request.response);
            } else {
                // If it fails, reject the promise with a error message
                reject(Error('Image didn\'t load successfully; error code:' + request.statusText));
            }
        };
        request.onerror = function () {
            // Also deal with the case when the entire request fails to begin with
            // This is probably a network error, so reject the promise with an appropriate message
            reject(Error('There was a network error.'));
        };
        // Send the request
        request.send();
    });
}

function isIterable(obj) {
    if (obj === null) {
        return false;
    }
    return typeof obj[Symbol.iterator] === 'function';
}

function fromISODateToLocaleDateString(input) {
    return parseISOString(input).toLocaleDateString()
}

function fromISODateToLocaleString(input) {
    return parseISOString(input).toLocaleString();
}

function fromMJDateToLocaleDateString(input) {
    return parseMicrosoftJSONDate(input).toLocaleDateString()
}

function dateConverter(collectionOrObject, datePropertyNameOrNames, dateSelector, dateSelectorOwner) {
    if (!isIterable(datePropertyNameOrNames)) {
        datePropertyNameOrNames = [datePropertyNameOrNames];
    }

    if (!isIterable(collectionOrObject)) {
        for (datePropertyName of datePropertyNameOrNames) {
            let date = collectionOrObject[datePropertyName];

            if (date) {
                collectionOrObject[datePropertyName] = dateSelector.call(dateSelectorOwner, date);
            }
        }
    }
    else {
        $.each(collectionOrObject, function () {
            for (datePropertyName of datePropertyNameOrNames) {
                let date = this[datePropertyName];

                if (date) {
                    this[datePropertyName] = dateSelector.call(dateSelectorOwner, date);
                }
            }
        });
    }

    return collectionOrObject;
}

function fromISODateToLocaleStringConverter(collectionOrObject, ...datePropertyNameOrNames) {
    return dateConverter(collectionOrObject, datePropertyNameOrNames, fromISODateToLocaleString);
}

function exportToCsv(filename, rows) {
    var processRow = function (row) {
        var finalVal = '';
        for (var j = 0; j < row.length; j++) {
            var innerValue = row[j] === null ? '' : row[j].toString();
            if (row[j] instanceof Date) {
                innerValue = row[j].toLocaleString();
            };
            var result = innerValue.replace(/"/g, '""');
            if (result.search(/("|,|\n)/g) >= 0)
                result = '"' + result + '"';
            if (j > 0)
                finalVal += ',';
            finalVal += result;
        }
        return finalVal + '\n';
    };

    var csvFile = '\uFEFF';
    for (var i = 0; i < rows.length; i++) {
        csvFile += processRow(rows[i]);
    }

    var blob = new Blob([csvFile], { type: 'text/csv;charset=utf-8;' });
    if (navigator.msSaveBlob) { // IE 10+
        navigator.msSaveBlob(blob, filename);
    } else {
        var link = document.createElement("a");
        if (link.download !== undefined) { // feature detection
            // Browsers that support HTML5 download attribute
            var url = URL.createObjectURL(blob);
            link.setAttribute("href", url);
            link.setAttribute("download", filename);
            link.style.visibility = 'hidden';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }
    }
}

function* objectsToRowsGenerator(array) {
    if (!array.isEmpty()) {
        yield Object.keys(array.first());

        for (let element of array) {
            yield Object.values(element);
        }
    }
}

function objectsToRows(array) {
    return [...objectsToRowsGenerator(array)];
}

// Array-like object (arguments) to Array
function argumentsToArray() {
    return Array.from(arguments);
}

Number.prototype.getHalf = function () {
    return this.valueOf() / 2;
};

Number.prototype.toLocaleCurrencyString = function (currency) {
    // Create our number formatter.
    let currencyFormatter = new Intl.NumberFormat(undefined, { //niezdefiniowana kultura, czyli aktualna
        style: 'currency',
        currency: currency //waluta jest obowiązkowa, bo w jednej kulturze może być wiele walut, np. w zależności od regionu
    });

    return currencyFormatter.format(this);
}

ko.utils.stringStartsWith = function (string, startsWith) {
    string = string || "";

    if (startsWith.length > string.length)
        return false;

    return string.substring(0, startsWith.length) === startsWith;
};

/**
 * Konwersja łańcucha znaków z ceną na liczbę zmiennoprzecinkową
 * @param {any} value String z ceną
 */
function currencyStringToFloat(value) {
    value = parseFloat(value.replace(/[^.\d]/g, "")); // Wyszukuje w ciągu znaków wszystkie wystąpienia inne od kropki i cyfry i zamienia jest na pusty string
    // Następnie następuje konwersa do float - liczby zmiennoprzecinkowej
    value = isNaN(value) ? 0 : value; // Jeśli zwrócona wartość nie jest liczbą, to zostaje przypisane 0, w przeciwnym razie zwrócona wartość

    return value; // Następuje zwrócenie obliczonej wartości
}

Array.prototype.objectInArray = function (searchFor, property) {
    var retVal = false;

    $.each(this, function (_index, item) {
        if (Object.prototype.hasOwnProperty.call(item, property)) {
            if (ko.unwrap(item[property]) === searchFor) {
                retVal = item[property];
            }
        }
    });

    return retVal;
};

ko.utils.unwrapFunction = function (func) {
    if (IsFunction(o)) {
        return func;
    }
    else {
        return ko.utils.unwrapFunction(func());
    }
};

//replaces single and double 'smart' quotes users commonly paste in from word into textareas and textboxes with normal text equivalents
//USAGE:
//data-bind="replaceWordChars:true
//also works with valueUpdate:'keyup' if you want"

ko.bindingHandlers.replaceWordChars = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var bindingValue = ko.utils.unwrapFunction(valueAccessor);

        if (bindingValue) {
            //update DOM - not sure why I should need to do this, but just updating viewModel doesn't always update DOM correctly for me
            $(element).val(removeMSWordChars(allBindingsAccessor().value()));
            allBindingsAccessor().value($(element).val()); //update viewModel
        }
    }
}

String.prototype.removeCharAt = function (i) {
    var tmp = this.split(''); // convert to an array
    tmp.splice(i - 1, 1); // remove 1 element from the array (adjusting for non-zero-indexed counts)
    return tmp.join(''); // reconstruct the string
}

ko.bindingHandlers.icon = {
    update: function (element, valueAccessor) {
        var icon = ko.unwrap(valueAccessor());
        $(element).html(icons[icon]);
    }
}

ko.bindingHandlers.jqDialog = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var model = ko.utils.unwrapObservable(valueAccessor()),
            options = ko.utils.extend(model.options || {}, ko.bindingHandlers.jqDialog.defaultOptions);

        //setup our buttons
        options.buttons = {
            "Accept": model.accept.bind(viewModel, viewModel),
            "Cancel": model.cancel.bind(viewModel, viewModel)
        };

        //initialize the dialog
        $(element).dialog(options);

        //handle disposal
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).dialog("destroy");
        });
    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        $(element).dialog(ko.utils.unwrapObservable(value.open) ? "open" : "close");

        if (value.title) {
            var title = value.title();
            if (title) {
                $(element).dialog("option", "title", title);
            }
        }
        //handle positioning
        if (value.position) {
            var target = value.position();
            if (target) {
                var pos = $(target).position();
                $(element).dialog("option", "position", [pos.left + $(target).width(), pos.top + $(target).height()]);
            }
        }
    },
    defaultOptions: {
        autoOpen: false,
        resizable: false,
        modal: true
    }
};

Array.prototype.randomIndex = function () {
    return Math.floor(Math.random() * this.length);
}

Array.prototype.randomElement = function () {
    const randomIndex = this.randomIndex();

    return this[randomIndex];
};

function getTimezoneName2() {
    tmSummer = new Date(Date.UTC(2005, 6, 30, 0, 0, 0, 0));
    so = -1 * tmSummer.getTimezoneOffset();
    tmWinter = new Date(Date.UTC(2005, 12, 30, 0, 0, 0, 0));
    wo = -1 * tmWinter.getTimezoneOffset();

    if (-660 == so && -660 == wo) return 'Pacific/Midway';
    if (-600 == so && -600 == wo) return 'Pacific/Tahiti';
    if (-570 == so && -570 == wo) return 'Pacific/Marquesas';
    if (-540 == so && -600 == wo) return 'America/Adak';
    if (-540 == so && -540 == wo) return 'Pacific/Gambier';
    if (-480 == so && -540 == wo) return 'US/Alaska';
    if (-480 == so && -480 == wo) return 'Pacific/Pitcairn';
    if (-420 == so && -480 == wo) return 'US/Pacific';
    if (-420 == so && -420 == wo) return 'US/Arizona';
    if (-360 == so && -420 == wo) return 'US/Mountain';
    if (-360 == so && -360 == wo) return 'America/Guatemala';
    if (-360 == so && -300 == wo) return 'Pacific/Easter';
    if (-300 == so && -360 == wo) return 'US/Central';
    if (-300 == so && -300 == wo) return 'America/Bogota';
    if (-240 == so && -300 == wo) return 'US/Eastern';
    if (-240 == so && -240 == wo) return 'America/Caracas';
    if (-240 == so && -180 == wo) return 'America/Santiago';
    if (-180 == so && -240 == wo) return 'Canada/Atlantic';
    if (-180 == so && -180 == wo) return 'America/Montevideo';
    if (-180 == so && -120 == wo) return 'America/Sao_Paulo';
    if (-150 == so && -210 == wo) return 'America/St_Johns';
    if (-120 == so && -180 == wo) return 'America/Godthab';
    if (-120 == so && -120 == wo) return 'America/Noronha';
    if (-60 == so && -60 == wo) return 'Atlantic/Cape_Verde';
    if (0 == so && -60 == wo) return 'Atlantic/Azores';
    if (0 == so && 0 == wo) return 'Africa/Casablanca';
    if (60 == so && 0 == wo) return 'Europe/London';
    if (60 == so && 60 == wo) return 'Africa/Algiers';
    if (60 == so && 120 == wo) return 'Africa/Windhoek';
    if (120 == so && 60 == wo) return 'Europe/Amsterdam';
    if (120 == so && 120 == wo) return 'Africa/Harare';
    if (180 == so && 120 == wo) return 'Europe/Athens';
    if (180 == so && 180 == wo) return 'Africa/Nairobi';
    if (240 == so && 180 == wo) return 'Europe/Moscow';
    if (240 == so && 240 == wo) return 'Asia/Dubai';
    if (270 == so && 210 == wo) return 'Asia/Tehran';
    if (270 == so && 270 == wo) return 'Asia/Kabul';
    if (300 == so && 240 == wo) return 'Asia/Baku';
    if (300 == so && 300 == wo) return 'Asia/Karachi';
    if (330 == so && 330 == wo) return 'Asia/Calcutta';
    if (345 == so && 345 == wo) return 'Asia/Katmandu';
    if (360 == so && 300 == wo) return 'Asia/Yekaterinburg';
    if (360 == so && 360 == wo) return 'Asia/Colombo';
    if (390 == so && 390 == wo) return 'Asia/Rangoon';
    if (420 == so && 360 == wo) return 'Asia/Almaty';
    if (420 == so && 420 == wo) return 'Asia/Bangkok';
    if (480 == so && 420 == wo) return 'Asia/Krasnoyarsk';
    if (480 == so && 480 == wo) return 'Australia/Perth';
    if (540 == so && 480 == wo) return 'Asia/Irkutsk';
    if (540 == so && 540 == wo) return 'Asia/Tokyo';
    if (570 == so && 570 == wo) return 'Australia/Darwin';
    if (570 == so && 630 == wo) return 'Australia/Adelaide';
    if (600 == so && 540 == wo) return 'Asia/Yakutsk';
    if (600 == so && 600 == wo) return 'Australia/Brisbane';
    if (600 == so && 660 == wo) return 'Australia/Sydney';
    if (630 == so && 660 == wo) return 'Australia/Lord_Howe';
    if (660 == so && 600 == wo) return 'Asia/Vladivostok';
    if (660 == so && 660 == wo) return 'Pacific/Guadalcanal';
    if (690 == so && 690 == wo) return 'Pacific/Norfolk';
    if (720 == so && 660 == wo) return 'Asia/Magadan';
    if (720 == so && 720 == wo) return 'Pacific/Fiji';
    if (720 == so && 780 == wo) return 'Pacific/Auckland';
    if (765 == so && 825 == wo) return 'Pacific/Chatham';
    if (780 == so && 780 == wo) return 'Pacific/Enderbury'
    if (840 == so && 840 == wo) return 'Pacific/Kiritimati';
    return 'US/Pacific';
}

function getTimezoneName() {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
}

function setTimezoneCookie() {

    var timezone_cookie = "timezoneOffset";

    // if the timezone cookie does not exist create one.
    if (!getCookie(timezone_cookie)) {

        // check if the browser supports cookie
        var test_cookie = 'test cookie';
        setCookie(test_cookie, true);

        // browser supports cookie
        if (getCookie(test_cookie)) {

            // delete the test cookie
            setCookie(test_cookie, null);

            // create a new cookie 
            setCookie(timezone_cookie, new Date().getTimezoneOffset());

            // re-load the page
            location.reload();
        }
    }
    // if the current timezone and the one stored in cookie are different
    // then store the new timezone in the cookie and refresh the page.
    else {

        var storedOffset = parseInt(getCookie(timezone_cookie));
        var currentOffset = new Date().getTimezoneOffset();

        // user may have changed the timezone
        if (storedOffset !== currentOffset) {
            setCookie(timezone_cookie, new Date().getTimezoneOffset());
            location.reload();
        }
    }
}

function setAnchorForRetrievingTimezone(elementId) {
    var el = document.getElementById(elementId);
    //Wed Feb 04 2015 18:37:55 GMT-1000 (Hawaiian Standard Time)                    
    var href = el.href + "?JsTime=" + new Date().toString();
    el.href = href;
}

function parseMicrosoftJSONDate(jsonDate) {
    return new Date(parseInt(jsonDate.substr(6)));
}

function parseMicrosoftJSONDate2(s) {
    return new Date(parseFloat(/Date\(([^)]+)\)/.exec(s)[1]));
}

ko.bindingHandlers.fromMJDate = {
    init: function (element, valueAccessor) {
        let value = valueAccessor();

        if (!IsUndefinedOrNull(value)) {
            element.innerHTML = fromMJDateToLocaleDateString(valueAccessor());
        }
    }
};

ko.bindingHandlers.fromISODate = {
    init: function (element, valueAccessor) {
        let value = valueAccessor();

        if (!IsUndefinedOrNull(value)) {
            element.innerHTML = fromISODateToLocaleDateString(valueAccessor());
        }
    }
};

ko.bindingHandlers.fromISODateTime = {
    init: function (element, valueAccessor) {
        let value = valueAccessor();

        if (!IsUndefinedOrNull(value)) {
            element.innerHTML = fromISODateToLocaleString(valueAccessor());
        }
    }
};

function flatten(o) {
    return Object.keys(o).reduce(function (r, k) {
        return r.concat(k, object[k]);
    }, []);
}

ko.bindingHandlers.foreachprop = {
    transformObject: function (obj) {
        var properties = [];
        ko.utils.objectForEach(obj, function (key, value) {
            properties.push({ key: key, value: value });
        });
        return properties;
    },
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var properties = ko.pureComputed(function () {
            var obj = ko.utils.unwrapObservable(valueAccessor());
            return ko.bindingHandlers.foreachprop.transformObject(obj);
        });
        ko.applyBindingsToNode(element, { foreach: properties }, bindingContext);
        return { controlsDescendantBindings: true };
    }
};

ko.bindingHandlers.randomOrder = {
    init: function (elem, valueAccessor) {
        // Build an array of child elements
        var child = ko.virtualElements.firstChild(elem),
            childElems = [];
        while (child) {
            childElems.push(child);
            child = ko.virtualElements.nextSibling(child);
        }

        // Remove them all, then put them back in a random order
        ko.virtualElements.emptyNode(elem);
        while (childElems.length) {
            var randomIndex = Math.floor(Math.random() * childElems.length),
                chosenChild = childElems.splice(randomIndex, 1);
            ko.virtualElements.prepend(elem, chosenChild[0]);
        }
    }
};

ko.extenders.defaultIfNull = function (target, defaultValue) {
    var result = ko.computed({
        read: target,
        write: function (newValue) {
            if (!newValue) {
                target(defaultValue);
            } else {
                target(newValue);
            }
        }
    });

    result(target());

    return result;
};

ko.extenders.withDefault = function (target, defaultValue) {
    target.subscribe(function (input) {
        if (!input) target(defaultValue)
    });
    return target
};

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

function DisplayErrors(errors) {
    for (var i = 0; i < errors.length; i++) {
        $("<label for='" + errors[i].Key + "' class='error'></label>")
            .html(errors[i].Value[0]).appendTo($("input#" + errors[i].Key).parent());
    }
}

function getValidationSummary() {
    var el = $(".validation-summary-errors");
    if (el.length == 0) {
        $(".title-separator").after("<div><ul class='validation-summary-errors ui-state-error'></ul></div>");
        el = $(".validation-summary-errors");
    }
    return el;
}

function getResponseValidationObject(response) {
    if (response && response.Tag && response.Tag == "ValidationError")
        return response;
    return null;
}

function CheckValidationErrorResponse(response, form, summaryElement) {
    var data = getResponseValidationObject(response);
    if (!data) return;

    var list = summaryElement || getValidationSummary();
    list.html('');
    $.each(data.State, function (i, item) {
        list.append("<li>" + item.Errors.join("</li><li>") + "</li>");
        if (form && item.Name.length > 0)
            $(form).find("*[name='" + item.Name + "']").addClass("ui-state-error");
    });
}

function makeUL(array) {
    if (!isIterable(array)) {
        return array;
    }

    if (array.length === 1) {
        return array[0];
    }

    // Create the list element:
    var list = document.createElement('ul');

    for (var i = 0; i < array.length; i++) {
        // Create the list item:
        var item = document.createElement('li');

        // Set its contents:
        item.appendChild(document.createTextNode(array[i]));

        // Add it to the list:
        list.appendChild(item);
    }

    // Finally, return the constructed list:
    return list;
}

function DisplayModelStateErrors(response, errorTarget) {
    for (key in response) {
        let errors = makeUL(response[key]);

        if (key.includes('model.')) {
            $("span[data-valmsg-for='" + key.split('model.')[1] + "']").html(errors);
        }
        else {
            if (errorTarget) {
                errorTarget(errors)
            }
            else {
                $(".model-state-errors").html(errors);
            }
        }
    }
}

function objectifyForm(formArray) {
    //serialize data function
    var returnArray = {};
    for (var i = 0; i < formArray.length; i++) {
        returnArray[formArray[i]['name']] = formArray[i]['value'];
    }
    return returnArray;
}

function scrollTop() {
    window.scrollTo(0, 0);
}

(function () {
    if (typeof Object.defineProperty === 'function') {
        try { Object.defineProperty(Array.prototype, 'sortBy', { value: sb }); } catch (e) { }
    }
    if (!Array.prototype.sortBy) Array.prototype.sortBy = sb;

    function sb(f) {
        for (var i = this.length; i;) {
            var o = this[--i];
            this[i] = [].concat(f.call(o, o, i), o);
        }
        this.sort(function (a, b) {
            for (var i = 0, len = a.length; i < len; ++i) {
                if (a[i] != b[i]) return a[i] < b[i] ? -1 : 1;
            }
            return 0;
        });
        for (var i = this.length; i;) {
            this[--i] = this[i][this[i].length - 1];
        }
        return this;
    }
})();

ko.bindingHandlers.selectPicker = {
    after: ['options'],   /* KO 3.0 feature to ensure binding execution order */
    init: function (element, valueAccessor, allBindingsAccessor) {
        var $element = $(element);
        $element.addClass('selectpicker').selectpicker();

        var doRefresh = function () {
            $element.selectpicker('refresh');
        }, subscriptions = [];

        // KO 3 requires subscriptions instead of relying on this binding's update
        // function firing when any other binding on the element is updated.

        // Add them to a subscription array so we can remove them when KO
        // tears down the element.  Otherwise you will have a resource leak.
        var addSubscription = function (bindingKey) {
            var targetObs = allBindingsAccessor.get(bindingKey);

            if (targetObs && ko.isObservable(targetObs)) {
                subscriptions.push(targetObs.subscribe(doRefresh));
            }
        };

        addSubscription('options');
        addSubscription('value');           // Single
        addSubscription('selectedOptions'); // Multiple

        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            while (subscriptions.length) {
                subscriptions.pop().dispose();
            }
        });
    },
    update: function (element, valueAccessor, allBindingsAccessor) {
    }
};