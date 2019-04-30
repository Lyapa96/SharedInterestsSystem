
export function toCsvContent(headers, data) {
        const separator = ";";
        const dataForCsv = data.map(values =>
            values.map(value => {
                let needEscapeString = false;
                if (
                    value.indexOf('"') !== -1 ||
                    value.indexOf("\r\n") !== -1 ||
                    value.indexOf("\n") !== -1 ||
                    value.indexOf(separator) !== -1
                ) {
                    needEscapeString = true;
                    value = value.replace(new RegExp('"', "g"), '""');
                }
                return needEscapeString ? `"${value}"` : value;
            })
        );

        return [headers, ...dataForCsv].reduce(
            (result, row) => result + row.join(separator) + "\r\n",
            ""
        );
}
