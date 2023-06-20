function collectConfigurationIdsWithDiscountsIntoCollector() {
    let configurationCheckboxes = document.querySelectorAll('[id^="checkboxConfiguration-"]');
    let discountInputs = document.querySelectorAll('[id^="discountInput-"]');

    let configurationIdsWithDiscounts = selectConfigurationIdsWithDiscounts(configurationCheckboxes, discountInputs);

    let collector = document.getElementById("configurationIdsWithDiscountsJsonCollector");
    collector.value = JSON.stringify(configurationIdsWithDiscounts);

    let collectorNotEmptyFlag = document.getElementById("configurationIdsWithDiscountsJsonCollectorNotEmpty");
    collectorNotEmptyFlag.checked = Object.keys(configurationIdsWithDiscounts).length > 0;
}

function selectConfigurationIdsWithDiscounts(configurationCheckboxes, discountInputs) {
    let checkboxIds = getCheckboxIds(configurationCheckboxes);
    let checkboxesArray = Array.from(configurationCheckboxes);
    let discountInputsArray = Array.from(discountInputs);

    let configurationIdsWithDiscounts = {};
    for (let i = 0; i < checkboxIds.length; ++i) {
        if (checkboxesArray[i].checked === true) {
            configurationIdsWithDiscounts[checkboxIds[i]] = Number(discountInputsArray[i].value);
        }
    }

    return configurationIdsWithDiscounts;
}

function getCheckboxIds(configurationCheckboxes) {
    let ids = [];
    Array.prototype.forEach.call(
        configurationCheckboxes,
        (checkbox) => {
            let compositeId = checkbox.id;
            let pureId = compositeId.replace("checkboxConfiguration-", "");
            ids.push(pureId)
        });

    return ids;
}

function checkConfigurationsSelection() {
    let collectorNotEmptyFlag = document.getElementById("configurationIdsWithDiscountsJsonCollectorNotEmpty");

    let configurationsSelected = collectorNotEmptyFlag.checked
    if (!configurationsSelected) {
        alert("Должна быть выбрана хотя бы одна конфигурация!");
    }

    return configurationsSelected;
}