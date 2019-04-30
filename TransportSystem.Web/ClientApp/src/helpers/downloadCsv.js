export const downloadCsv = (content, name) => {
    const BOM = "\uFEFF";
    const blob = new Blob([BOM + content], {
        type: "text/csv;charset=UTF-8;"
    });
    const link = document.createElement("a");
    const url = URL.createObjectURL(blob);
    link.setAttribute("href", url);
    link.setAttribute("download", `${name || "untitled"}.csv`);
    link.style.visibility = "hidden";
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};
