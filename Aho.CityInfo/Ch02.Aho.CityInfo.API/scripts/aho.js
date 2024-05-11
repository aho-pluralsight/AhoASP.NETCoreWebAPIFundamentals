function jolifying() {
    var element = document.getElementById("pipeContent");

    console.info(element.textContent);

    if (element) {
        let parts = element.textContent.split('P').filter(x => x != '');
        let htmlContent = '<div id="paveDansLaMare"><h2 class="title">ASPNET Core \'API\' request/response pipeline in action!</h2></div>';
        htmlContent += '<div>';
        let midIn = parts.findIndex(pa => pa.includes('00'));
        let displayTag = 'p';

        parts.forEach((part, i) => {
            let boxId = String(i.toString()).padStart(2, '0');
            if (i < midIn) {
                htmlContent += `<div class="element box-${boxId}"><${displayTag} class="displayTag outbound">P${part}</${displayTag}>`;
            }
            if (i == midIn) {
                htmlContent += `<div class="element box-${boxId}"><${displayTag} class="displayTag bullseye"><mark>P${part}</mark></${displayTag}></div>`;
            }
            if (i > midIn) {
                htmlContent += `<${displayTag} class="displayTag return">P${part}</${displayTag}></div>`;
            }
        });
        htmlContent += '</div>';
        element.innerHTML = htmlContent;
    } else {
        alert("This shit is not working yo!");
        console.error("Element with ID 'pipeContent' not found.");
    }
}

window.addEventListener("load", function () {
    console.info(
        "AHO is having fun!",
    );
});
