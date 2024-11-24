let pageNumber = 1;
const pageSize = 10;
function createTable(jsonData, tableid) {
    var table = $("<table>").addClass("table table-striped table-bordered").attr("id", tableid);;

    var thead = $("<thead>");
    var tbody = $("<tbody>");
    var tfoot = $("<tfoot>");

    jsonData.forEach(function (row) {
        var tr = $("<tr>").addClass(row.row_class);

        for (var key in row) {
            if (key.startsWith("col") && !isNaN(key.substring(3))) {
                var columnIndex = key.substring(3);
                var colType = row["col" + columnIndex + "_type"] || "text";
                var colClass = row["col" + columnIndex + "_class"] || "";

                if (row.row_type === "head") {
                    tr.append($("<th>").addClass(colClass).text(row[key]));
                } else if (row.row_type === "body") {
                    if (colType === "html") {
                        tr.append($("<td>").addClass(colClass).html(row[key]));
                    } else {
                        tr.append($("<td>").addClass(colClass).text(row[key]));
                    }
                } else if (row.row_type === "foot") {
                    tr.append($("<td>").addClass(colClass).text(row[key]));
                }
            }
        }

        if (row.row_type === "head") {
            thead.append(tr);
        } else if (row.row_type === "body") {
            tbody.append(tr);
        } else if (row.row_type === "foot") {
            tfoot.append(tr);
        }
    });

    if (thead.children().length) table.append(thead);
    if (tbody.children().length) table.append(tbody);
    if (tfoot.children().length) table.append(tfoot);

    return table;
}


function callApi(url, data) {
    return $.ajax({
        url: url,
        type: data ? "POST" : "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: data ? JSON.stringify(data) : null
    });
}

function exportToExcel(tableid, filename) {
    const table = document.getElementById(tableid);
    const wb = XLSX.utils.table_to_book(table, { sheet: "Sheet1" });
    XLSX.writeFile(wb, filename + '.xlsx');
}

function exportToPDF(tableid, filename) {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();
    doc.autoTable({ html: '#' + tableid });
    doc.save(filename + '.pdf');
}