function generatePdfFromHtml(htmlContent) {
    const pdf = new jsPDF();
    pdf.html(htmlContent, {
        callback: function (pdf) {
            pdf.save('example.pdf');
        },
        x: 10,
        y: 10
    });
}
