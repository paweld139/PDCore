function Config(path) {
    this.path = path;
}

function InitializeConfig(config) {
    return new objectHider(config);
}

function SetMainConfig(config) {
    MainConfig = InitializeConfig(config);
}

function DialogFormField(id, name, minLength, maxLength, isEditor, validateRegexp, regexp, regexpError) {
    this.Id = id;
    this.Name = name;
    this.MinLength = minLength;
    this.MaxLength = maxLength;
    this.ValidateRegexp = validateRegexp;
    this.Regexp = regexp;
    this.RegexpError = regexpError;
    this.IsEditor = isEditor;
}

function TextEditor(instance, obj) {
    var self = this;

    self.Instance = instance;

    self.Obj = obj;

    self.Content = function () {
        if (self.Instance != null) {
            return self.Instance.getData();
        }

        return null;
    };

    self.IsEmpty = function () {
        return (self.Content().length == 0);
    };

    self.toJSON = function () {
        self.Obj.Content = self.Content();
        return self.Obj;
    };
}

function InitializeTextEditor(elementId, observable) {
    const self = this;

    const instance = CKEDITOR.replace(elementId, { language: 'en' });

    function getContent() { //Private member
        if (instance !== null) {
            return instance.getData();
        }

        return null;
    };

    self.updateElement = function (sneaky) { //Public member. Gdy bezpośrednio to zawsze głośno.
        if (observable) {
            let content = getContent();

            if (!sneaky) {
                observable(content); //Aktualizuję observable, która aktualizuje przy okazji pole
            }
            else {
                observable.sneakyUpdate(content);
            }
        }
        else {
            instance.updateElement();
        }
    }

    self.clear = function () {
        instance.setData("");
    }

    instance.on('blur', function () {
        self.updateElement();
    });
}

function OnLoad(onLoad) {
    let self = this;

    self.Loaded = false;

    self.Action = onLoad;

    self.Execute = function (callback) {
        if (!self.Loaded) {
            if (IsCallback(callback)) {
                self.Action(callback);
            }
            else {
                self.Action();
            }

            self.Loaded = true;
        }
    };
}

//class KeyValuePair {
//    constructor(key, value) {
//        this.key = key;
//        this.value = value;
//    }

//    //set(key, value) {
//    //    this.key = key;
//    //    this.value = value;
//    //}

//    *[Symbol.iterator]() {
//        for (let key in this) {
//            yield [key, this[key]] // yield [key, value] pair
//        }
//    }
//}

//class Storage {
//    constructor(pKey, pValue) {
//        this.key = pKey;
//        this.value = pValue;
//        this.map = new Map([[pKey, pValue]]);
//    }

//    //set(pKey, pValue) {
//    //    this.map.set(pKey, pValue);
//    //}

//    get(pKey) {
//        var result = this.map.get(pKey);
//        return result;
//    }
//}

var ObjectInfo = {
    Initialise: function (a) {
        $.each(a, function (index, o) {
            o.infos = [0, 0, 0, 0, 0, 0, 0];

            o.info = ko.observable('');
        });
    },

    UpdateUI: function (oA) {
        $.each(oA, function (key, value) {
            value.info("(" + value.infos.join(", ") + ")");
        });
    },

    UpdateUIElement: function (value) {
        value.info("(" + value.infos.join(", ") + ")");
    },

    Clear: function (oA) {
        $.each(oA, function (key, value) {
            value.infos.setAll(0);
        });
    },

    ClearElement: function (value) {
        value.infos.setAll(0);
    }
};

function Confirm() {
    this.Message;
    this.Result;
}

class Point {
    constructor(x, y) {
        this.x = x;
        this.y = y;
    }

    static distance(a, b) {
        const dx = a.x - b.x;
        const dy = a.y - b.y;

        return Math.sqrt(dx * dx + dy * dy);
    }
}

var UserModel = function () {
    var self = this;
    self.isLoggedIn = ko.observable("");
    self.userId = ko.observable("");
    self.roles = ko.observableArray([]);
    self.employeeId = ko.observable("");
    self.contrahentId = ko.observable("");


    // function to check if role passed is in array
    self.hasRole = function (roleName) {

        for (i = 0; 1 < self.roles.length; i++) {
            if (self.roles[i] == roleName)
                return true
        }
        return false;
    }
};

var UserData = new UserModel();