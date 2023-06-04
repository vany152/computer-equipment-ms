function collectComponentIdsIntoCollector() {
    let componentCheckboxes = document.querySelectorAll('[id^="checkboxComponent-"]');
    let checkedComponentIds = selectCheckedComponentIds(componentCheckboxes);

    let collector = document.getElementById("componentIdsJsonCollector");
    collector.value = JSON.stringify(checkedComponentIds);

    if (checkedComponentIds.length > 0) {
        let collectorNotEmptyFlag = document.getElementById("componentIdsJsonCollectorNotEmpty");
        collectorNotEmptyFlag.checked = true;
    }
}

function selectCheckedComponentIds(componentCheckboxes) {
    let ids = [];
    Array.prototype.forEach.call(componentCheckboxes,
        (checkbox) => {
            if (checkbox.checked === true) {
                let id = checkbox.id.replace("checkboxComponent-", "");
                ids.push(Number(id))
            }
        });

    return ids;
}

function checkComponentsSelection() {
    let collectorNotEmptyFlag = document.getElementById("componentIdsJsonCollectorNotEmpty");

    let componentsSelected = collectorNotEmptyFlag.checked
    if (!componentsSelected) {
        alert("Должен быть выбран хотя бы один компонент!");
    }

    return componentsSelected;
}