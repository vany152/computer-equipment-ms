function collectSpecificationsIntoCollector() {
    let specificationsArray = {};
    let specificationSections = document.querySelectorAll('[id^="newSpecificationSection-"]');
    Array.prototype.forEach.call(specificationSections,
        (section) => {
            let specification = getSpecificationParametersFromSection(section);
            if (specification.type.trim() !== "") {
                addSpecification(specification, specificationsArray);
            }
        });

    document.getElementById("specificationsCollector").value = JSON.stringify(specificationsArray);
}

function getSpecificationParametersFromSection(section) {
    let sectionCounter = section.id.replace("newSpecificationSection-", "");

    let specificationType = document.getElementById(`specificationTypeInput-${sectionCounter}`).value;
    let specificationValue = document.getElementById(`specificationValueInput-${sectionCounter}`).value;

    return { type: specificationType, value: specificationValue };
}

function addSpecification(specification, specificationsArray) {
    specificationsArray[specification.type] = specification.value;
}



function addSpecificationSection() {
    let newSpecificationSection = document.createElement("div");
    newSpecificationSection.style.cssText = "display: flex; width: 100%; margin-top: 10px";
    let currentCounterValue = Counter.value++;
    newSpecificationSection.id = `newSpecificationSection-${currentCounterValue}`;

    newSpecificationSection.innerHTML =
        `<div class="form-floating" style="flex-grow: 1; flex-basis: 45%">
            <input type="text" id="specificationTypeInput-${currentCounterValue}" class="form-control">
            <label for="specificationTypeInput-${currentCounterValue}">Тип спецификации</label>
        </div>
        <div class="form-floating" style="flex-grow: 1; flex-basis: 45%; margin-left: 10px">
            <input type="text" id="specificationValueInput-${currentCounterValue}" class="form-control">
            <label for="specificationValueInput-${currentCounterValue}">Значение</label>
        </div>
        <button type="button" class="btn btn-outline-dark btn-sm" onclick="removeSpecificationSection('${newSpecificationSection.id}')" style="flex-grow: 1; flex-basis: 10%; margin-left: 10px">Удалить</button>
        `;

    document.getElementById("specificationsSection").appendChild(newSpecificationSection);
}

function removeSpecificationSection(elementId) {
    document.getElementById(elementId).remove();
}

class Counter {
    static value = 1;
}