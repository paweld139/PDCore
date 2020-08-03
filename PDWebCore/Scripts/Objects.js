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

function KeyValuePair(key, value) {
    this.Key = key;
    this.Value = value;
}

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
