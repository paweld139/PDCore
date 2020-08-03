function SortUtil(columns, data) {
    var self = this;

    self.columns = columns;
    self.data = data;

    self.descending = "fa fa-arrow-down";

    self.ascending = "fa fa-arrow-up";
   

    self.sortClick = function (column) {
        try {
            self.clearColumnStates(column);

            if (column.state() === "" || column.state() === self.descending) {
                column.state(self.ascending);
            }
            else {
                column.state(self.descending);
            }

            switch (column.type) {
                case "number":
                    self.numberSort(column);
                    break;
                case "date":
                    self.dateSort(column);
                    break;
                case "object":
                    self.objectSort(column);
                    break;
                case "string":
                default:
                    self.stringSort(column);
                    break;
            }
        }
        catch (err) {
            alert(err);
        }
    };

    self.clearColumnStates = function (selectedColumn) {
        var otherColumns = self.columns().filter(function (col) {
            return col != selectedColumn;
        });
        for (var i = 0; i < otherColumns.length; i++) {
            otherColumns[i].state("");
        }
    };

    self.stringSort = function (column) {
        self.data(self.data().sort(function (a, b) {

            var taskA = ko.utils.unwrapObservable(a[column.property]), taskB = ko.utils.unwrapObservable(b[column.property]);
            if (taskA < taskB) {
                return (column.state() === self.ascending) ? -1 : 1;
            }
            else if (taskA > taskB) {
                return (column.state() === self.ascending) ? 1 : -1;
            }
            else {
                return 0;
            }
        }));
    };
}